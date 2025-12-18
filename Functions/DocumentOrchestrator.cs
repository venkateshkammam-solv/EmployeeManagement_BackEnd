using Microsoft.Azure.Functions.Worker;
using Microsoft.DurableTask;
using Microsoft.Extensions.Logging;
using AzureFunctions.Models;
using AzureFunctions_Triggers.Models;
using MyAzureFunctionApp.Shared;
using AzureFunctions_Triggers.Shared.Constants;

public class DocumentOrchestrator
{
    private readonly ILogger _logger;

    public DocumentOrchestrator(ILoggerFactory loggerFactory)
    {
        _logger = loggerFactory.CreateLogger<DocumentOrchestrator>();
    }

    [Function(nameof(DocumentOrchestrator))]
    public async Task<DocumentMetadata> Run(
        [OrchestrationTrigger] TaskOrchestrationContext ctx)
    {
        var metadata = ctx.GetInput<DocumentMetadata>();

        metadata.InstanceId = ctx.InstanceId;

        metadata = await ctx.CallActivityAsync<DocumentMetadata>("BlobUploadActivity", metadata);
        metadata.Status = "Pending";
        metadata.UploadedOn = ctx.CurrentUtcDateTime;

        await ctx.CallActivityAsync("SaveMetadataActivity", metadata);

        _logger.LogInformation(Messages.DocumentIsPendingForHumanApprovalMsg);

        var approvalResult = await ctx.WaitForExternalEvent<ApprovalResult>("DocumentApprovalEvent");
        metadata.Status = approvalResult.IsApproved ? "Approved" : "Rejected";
        metadata.ReviewedBy = approvalResult.ReviewedBy;
        metadata.ReviewerComments = approvalResult.Comments;
        metadata.ReviewedOn = ctx.CurrentUtcDateTime;

        await ctx.CallActivityAsync("SaveMetadataActivity", metadata);

        await ctx.CallActivityAsync("SendEmailActivity", new EmailPayload
        {
            Email = metadata.Email,
            EmployeeName = metadata.EmployeeName,
            Status = metadata.Status
        });

        _logger.LogInformation(Messages.DocumentWorkFlowCmpltMsg);
        return metadata;
    }
}
