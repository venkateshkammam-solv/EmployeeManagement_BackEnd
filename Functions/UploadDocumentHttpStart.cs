using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Net.Http.Headers;
using AzureFunctions.Models;
using Microsoft.DurableTask.Client;
using System.Net;
using Shared.Services;
using AzureFunctions_Triggers.Shared.Constants;
using AzureFunctions_Triggers.Shared.Services;

public class UploadDocumentHttpStart
{
    private readonly ILogger _logger;
    private readonly IdGenerator _codeGenerator;

    public UploadDocumentHttpStart(ILoggerFactory loggerFactory, IdGenerator codeGenerator)
    {
        _logger = loggerFactory.CreateLogger<UploadDocumentHttpStart>();
        _codeGenerator = codeGenerator;
    }

    [Function("UploadDocument_HttpStart")]
    public async Task<HttpResponseData> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequestData req,
        [DurableClient] DurableTaskClient client)
    {
        try
        {
            ValidateContentType(req, out var boundary);

            var (metadata, fileName, _) =
                await ParseMultipartDataAsync(req, boundary);

            if (string.IsNullOrEmpty(fileName))
            {
                return await HttpResponseHelper.CreateErrorResponse(req, HttpStatusCode.BadRequest, Messages.NoFileUploaded);
            }

            metadata.id = await _codeGenerator.GenerateidAsync("doc");

            string instanceId = await client.ScheduleNewOrchestrationInstanceAsync("DocumentOrchestrator", metadata);

            _logger.LogInformation(Messages.DocumentOrchestratorStarted, metadata.id,instanceId);

            return await CreateSuccessResponse(req,metadata.id, instanceId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, Messages.UploadDocumentError);
            return await HttpResponseHelper.CreateErrorResponse(req, HttpStatusCode.InternalServerError, ex.Message);
        }
    }

    private void ValidateContentType(HttpRequestData req, out string boundary)
    {
        if (!req.Headers.TryGetValues("Content-Type", out var contentTypes))
            throw new InvalidOperationException(Messages.MissingContentTypeHeader);

          var contentType = contentTypes.First();
          var mediaType = MediaTypeHeaderValue.Parse(contentType);

        if (string.IsNullOrEmpty(mediaType.Boundary.Value))
            throw new InvalidOperationException(Messages.MissingMultipartBoundary);

        boundary = HeaderUtilities.RemoveQuotes(mediaType.Boundary).Value!;

        if (!contentType.StartsWith("multipart/form-data", StringComparison.OrdinalIgnoreCase))
            throw new InvalidOperationException(Messages.InvalidMultipartContentType);
    }

    private async Task<(DocumentMetadata metadata, string fileName, string tempFilePath)>
        ParseMultipartDataAsync(HttpRequestData req, string boundary)
    {
        var reader = new MultipartReader(boundary, req.Body);

        MultipartSection? section;
        string? fileName = null;
        string tempFilePath = string.Empty;
        var formFields = new Dictionary<string, string>();

        while ((section = await reader.ReadNextSectionAsync()) != null)
        {
            var contentDisposition = ContentDispositionHeaderValue.Parse(section.ContentDisposition);

            if (contentDisposition.DispositionType.Equals("form-data") &&
                !string.IsNullOrEmpty(contentDisposition.FileName.Value))
            {
                fileName = SanitizeFileName(contentDisposition.FileName.Value);

                tempFilePath = Path.Combine(Path.GetTempPath(), fileName);
                using var fileStream = File.Create(tempFilePath);
                await section.Body.CopyToAsync(fileStream);
            }
            else
            {
                using var readerStream = new StreamReader(section.Body);

                formFields[contentDisposition.Name.Value!] =
                    await readerStream.ReadToEndAsync();
            }
        }

        var metadata = new DocumentMetadata
        {
            EmployeeId = formFields.GetValueOrDefault("employeeId") ?? string.Empty,
            EmployeeName = formFields.GetValueOrDefault("employeeName") ?? string.Empty,
            Email = formFields.GetValueOrDefault("email") ?? string.Empty,
            DocumentType = formFields.GetValueOrDefault("documentType"),
            Description = formFields.GetValueOrDefault("description"),
            Department = formFields.GetValueOrDefault("department"),
            EffectiveDate = formFields.GetValueOrDefault("effectiveDate"),
            FileName = fileName!,
            TempFilePath = tempFilePath,
            Status = "Pending",
            UploadedOn = DateTime.UtcNow,
            type = "Document"
        };

        return (metadata, fileName!, tempFilePath);
    }

    private string SanitizeFileName(string fileName)
    {
        fileName = fileName.Replace("\"", "").Trim();
        foreach (var c in Path.GetInvalidFileNameChars())
            fileName = fileName.Replace(c, '_');

        return fileName;
    }

    private async Task<HttpResponseData> CreateSuccessResponse(HttpRequestData req, string documentId, string instanceId)
    {
        var response = req.CreateResponse(HttpStatusCode.Accepted);
        await response.WriteAsJsonAsync(new
        {
            documentId,
            instanceId,
            message = Messages.DocumentWorkflowStarted
        });
        return response;
    }
}
