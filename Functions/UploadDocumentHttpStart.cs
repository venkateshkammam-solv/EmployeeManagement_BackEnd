using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using AzureFunctions.Models;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.DurableTask.Client;

public class UploadDocumentHttpStart
{
    private readonly ILogger _logger;

    public UploadDocumentHttpStart(ILoggerFactory loggerFactory)
    {
        _logger = loggerFactory.CreateLogger<UploadDocumentHttpStart>();
    }

    [Function("UploadDocument_HttpStart")]
    public async Task<HttpResponseData> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequestData req,
        [DurableClient] DurableTaskClient client)  
    {
        var response = req.CreateResponse();

        try
        {
            // Check Content-Type
            if (!req.Headers.TryGetValues("Content-Type", out var contentTypes))
            {
                response.StatusCode = HttpStatusCode.BadRequest;
                await response.WriteStringAsync("Missing Content-Type header.");
                return response;
            }

            var contentType = contentTypes.First();
            if (!contentType.StartsWith("multipart/form-data"))
            {
                response.StatusCode = HttpStatusCode.BadRequest;
                await response.WriteStringAsync("Content-Type must be multipart/form-data.");
                return response;
            }

            // Extract boundary
            var boundaryIndex = contentType.IndexOf("boundary=", System.StringComparison.OrdinalIgnoreCase);
            if (boundaryIndex < 0)
            {
                response.StatusCode = HttpStatusCode.BadRequest;
                await response.WriteStringAsync("Boundary not found in Content-Type header.");
                return response;
            }

            var boundary = "--" + contentType.Substring(boundaryIndex + "boundary=".Length).Trim('"');

            // Prepare temp storage and variables
            string tempPath = Path.GetTempPath();
            string? fileName = null;
            string tempFilePath = "";
            var formFields = new Dictionary<string, string>();
            MemoryStream? currentFileStream = null;
            bool inFile = false;

            using var reader = new StreamReader(req.Body);
            string? line;

            while ((line = await reader.ReadLineAsync()) != null)
            {
                if (line.StartsWith(boundary))
                {
                    if (currentFileStream != null && fileName != null)
                    {
                        tempFilePath = Path.Combine(tempPath, fileName);
                        await File.WriteAllBytesAsync(tempFilePath, currentFileStream.ToArray());
                        currentFileStream.Dispose();
                        currentFileStream = null;
                        inFile = false;
                    }
                    continue;
                }

                if (line.StartsWith("Content-Disposition:", System.StringComparison.OrdinalIgnoreCase))
                {
                    var fileMatch = Regex.Match(line, "filename=\"(?<filename>.+?)\"");
                    var nameMatch = Regex.Match(line, "name=\"(?<name>.+?)\"");

                    if (fileMatch.Success)
                    {
                        fileName = fileMatch.Groups["filename"].Value;
                        currentFileStream = new MemoryStream();
                        inFile = true;
                        await reader.ReadLineAsync();
                        await reader.ReadLineAsync();
                    }
                    else if (nameMatch.Success)
                    {
                        // Form field section
                        var fieldName = nameMatch.Groups["name"].Value;
                        await reader.ReadLineAsync(); 
                        var fieldValue = await reader.ReadLineAsync() ?? "";
                        formFields[fieldName] = fieldValue;
                    }
                }
                else if (inFile)
                {
                    var bytes = Encoding.UTF8.GetBytes(line + "\r\n");
                    currentFileStream?.Write(bytes, 0, bytes.Length);
                }
            }

            if (string.IsNullOrWhiteSpace(fileName) || string.IsNullOrWhiteSpace(tempFilePath))
            {
                response.StatusCode = HttpStatusCode.BadRequest;
                await response.WriteStringAsync("No file uploaded.");
                return response;
            }

            var metadata = new DocumentMetadata
            {
                id = formFields.ContainsKey("id") ? formFields["id"] : "",
                EmployeeName = formFields.ContainsKey("employeeName") ? formFields["employeeName"] : "",
                EmployeeId = formFields.ContainsKey("employeeId") ? formFields["employeeId"] : "",
                Email = formFields.ContainsKey("email") ? formFields["email"] : "",
                FileName = fileName,
                TempFilePath = tempFilePath,
                DocumentType = formFields.ContainsKey("documentType") ? formFields["documentType"] : null,
                Description = formFields.ContainsKey("description") ? formFields["description"] : null,
                Department = formFields.ContainsKey("department") ? formFields["department"] : null,
                EffectiveDate = formFields.ContainsKey("effectiveDate") ? formFields["effectiveDate"] : null,
                UploadedOn = DateTime.UtcNow
            };

            // Start the orchestrator
            var instanceId = await client.ScheduleNewOrchestrationInstanceAsync(
                "DocumentOrchestrator",
                input: metadata
            );

            _logger.LogInformation("Orchestrator started with ID: {InstanceId}", instanceId);

            response.StatusCode = HttpStatusCode.Accepted;
            await response.WriteAsJsonAsync(new
            {
                instanceId,
                statusQueryGetUri = $"/runtime/webhooks/durabletask/instances/{instanceId}?taskHub=default&connection=Storage",
                message = "Orchestrator started successfully"
            });

            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to start orchestrator");
            response.StatusCode = HttpStatusCode.InternalServerError;
            await response.WriteStringAsync($"Error: {ex.Message}");
            return response;
        }
    }
}
