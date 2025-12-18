using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Azure.Storage.Blobs;
using Azure.Storage.Sas;
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
        // Get or create the container
        var container = _blobClient.GetBlobContainerClient(_containerName);
        await container.CreateIfNotExistsAsync();

        // Sanitize file name
        var sanitized = SanitizeFileName(metadata.FileName);

        // Generate unique blob name
        var blobName = $"{DateTime.UtcNow:yyyyMMddHHmmss}_{Guid.NewGuid():N}_{sanitized}";

        // Get blob client
        var blob = container.GetBlobClient(blobName);

        // Upload file
        using var fileStream = File.OpenRead(metadata.TempFilePath);
        await blob.UploadAsync(fileStream, overwrite: false);

        metadata.UploadedBlobUrl = GenerateBlobSasUri(blob, TimeSpan.FromHours(1));

        _logger.LogInformation("Uploaded blob: {Blob}", blobName);
        return metadata;
    }

    private static string SanitizeFileName(string fileName)
    {
        foreach (var c in Path.GetInvalidFileNameChars())
            fileName = fileName.Replace(c, '_');

        return fileName.Replace(" ", "_");
    }

    private string GenerateBlobSasUri(BlobClient blobClient, TimeSpan validDuration)
    {
        if (!blobClient.CanGenerateSasUri)
        {
            _logger.LogWarning("BlobClient cannot generate SAS URI. Returning blob URI directly.");
            return blobClient.Uri.ToString();
        }

        var sasBuilder = new BlobSasBuilder
        {
            BlobContainerName = blobClient.BlobContainerName,
            BlobName = blobClient.Name,
            Resource = "b",
            ExpiresOn = DateTimeOffset.UtcNow.Add(validDuration)
        };

        sasBuilder.SetPermissions(BlobSasPermissions.Read);
        Uri sasUri = blobClient.GenerateSasUri(sasBuilder);

        return sasUri.ToString();
    }
}
