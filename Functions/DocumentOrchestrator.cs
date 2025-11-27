using Microsoft.Azure.Functions.Worker;
using Microsoft.DurableTask;
using Microsoft.Extensions.Logging;
using AzureFunctions.Models;
using AzureFunctions_Triggers.Models;

public class DocumentOrchestrator
{
    private readonly ILogger _logger;

    public DocumentOrchestrator(ILoggerFactory loggerFactory)
    {
        _logger = loggerFactory.CreateLogger<DocumentOrchestrator>();
    }

    [Function(nameof(DocumentOrchestrator))]
    public async Task<DocumentMetadata> Run([OrchestrationTrigger] TaskOrchestrationContext ctx)
    {
        var metadata = ctx.GetInput<DocumentMetadata>();
        metadata = await ctx.CallActivityAsync<DocumentMetadata>("BlobUploadActivity", metadata);
        var verificationResult = await ctx.CallActivityAsync<VerificationResult>("VerifyDocumentActivity", metadata);
        metadata.Status = verificationResult.Status;
        metadata.Email = verificationResult.Email;
        metadata.EmployeeName = verificationResult.EmployeeName;
        await ctx.CallActivityAsync("SaveMetadataActivity", metadata);
        await ctx.CallActivityAsync("SendEmailActivity", verificationResult);
        _logger.LogInformation("Document workflow completed");
        return metadata;
    }
}
