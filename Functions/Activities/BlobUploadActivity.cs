using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Azure.Storage.Blobs;
using AzureFunctions.Models;

public class BlobUploadActivity
{
    private readonly BlobServiceClient _blobClient;
    private readonly ILogger _logger;
    private readonly string _containerName;

    public BlobUploadActivity(BlobServiceClient client, ILoggerFactory loggerFactory, string containerName)
    {
        _blobClient = client;
        _logger = loggerFactory.CreateLogger<BlobUploadActivity>();
        _containerName = containerName;
    }

    [Function("BlobUploadActivity")]
    public async Task<DocumentMetadata> Run([ActivityTrigger] DocumentMetadata metadata)
    {
        var container = _blobClient.GetBlobContainerClient(_containerName);
        await container.CreateIfNotExistsAsync();

        var sanitized = SanitizeFileName(metadata.FileName);
        var blobName = $"{DateTime.UtcNow:yyyyMMddHHmmss}_{Guid.NewGuid():N}_{sanitized}";
        var blob = container.GetBlobClient(blobName);

        using var fileStream = File.OpenRead(metadata.TempFilePath);
        await blob.UploadAsync(fileStream, overwrite: false);

        metadata.UploadedBlobUrl = blob.Uri.ToString();
        _logger.LogInformation("Uploaded blob: {Blob}", blobName);

        return metadata;
    }

    private static string SanitizeFileName(string fileName)
    {
        foreach (var c in System.IO.Path.GetInvalidFileNameChars())
        {
            fileName = fileName.Replace(c, '_');
        }
        return fileName.Replace(" ", "_");
    }
}
