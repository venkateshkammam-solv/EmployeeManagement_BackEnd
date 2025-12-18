using AzureFunctions_Triggers.Services.Interfaces;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Azure.Storage.Blobs;
using Microsoft.Extensions.Logging;
using System.Net;
using MyAzureFunctionApp.Shared;
using AzureFunctions_Triggers.Shared.Constants;
namespace AzureFunctions_Triggers.Functions
{
    public class DocumentVerificationStatus
    {
        private readonly ILogger<DocumentVerificationStatus> _logger;
        private readonly IDocumentService _documentService;
        private readonly BlobServiceClient _blobClient;

        public DocumentVerificationStatus(
            IDocumentService documentService,
            ILogger<DocumentVerificationStatus> logger,
            BlobServiceClient blobClient)
        {
            _documentService = documentService;
            _logger = logger;
            _blobClient = blobClient;
        }

        [Function("DocumentVerificationStatus")]
        public async Task<HttpResponseData> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get")] HttpRequestData req)
        {
            var documents = await _documentService.GetAllEmployeeDocuments();

            foreach (var doc in documents)
            {
                if (string.IsNullOrEmpty(doc.UploadedBlobUrl))
                    continue;

                try
                {
                    var blobUri = new Uri(doc.UploadedBlobUrl);
                    string containerName = blobUri.Segments[2].Trim('/');
                    string blobName = blobUri.Segments[3];
                    var container = _blobClient.GetBlobContainerClient(containerName);
                    var blobClient = container.GetBlobClient(blobName);

                    if (!await blobClient.ExistsAsync())
                    {
                        _logger.LogWarning(Messages.BlobNotFoundMsg);
                        doc.base64String = null;
                        continue;
                    }

                    var download = await blobClient.DownloadContentAsync();
                    byte[] bytes = download.Value.Content.ToArray();

                    string mime = GetMimeType(doc.FileName);
                    doc.base64String = $"data:{mime};base64,{Convert.ToBase64String(bytes)}";
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, Messages.FileToBase64Msg);
                    doc.base64String = null;
                }
            }

            var response = req.CreateResponse(HttpStatusCode.OK);
            await response.WriteAsJsonAsync(documents);
            return response;
        }

 
        private string GetMimeType(string fileName)
        {
            var ext = Path.GetExtension(fileName).ToLowerInvariant();

            return ext switch
            {
                ".jpg" or ".jpeg" => "image/jpeg",
                ".png" => "image/png",
                ".gif" => "image/gif",
                ".pdf" => "application/pdf",
                _ => "application/octet-stream"
            };
        }
    }
}
