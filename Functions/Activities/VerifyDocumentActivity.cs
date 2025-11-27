using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Extensions.DurableTask;
using Microsoft.Extensions.Logging;
using AzureFunctions_Triggers.Models;
using AzureFunctions.Models;

namespace AzureFunctions_Triggers.Functions.Activities
{
    public class VerifyDocumentActivity
    {
        private readonly ILogger<VerifyDocumentActivity> _logger;

        public VerifyDocumentActivity(ILogger<VerifyDocumentActivity> logger)
        {
            _logger = logger;
        }

        [Function(nameof(VerifyDocumentActivity))]
        public VerificationResult Run([ActivityTrigger] DocumentMetadata document)
        {
            _logger.LogInformation($"Verifying document of the Employee");

            bool verificationPassed = !string.IsNullOrEmpty(document.UploadedBlobUrl);

            var result = new VerificationResult
            {
                EmployeeId = document.EmployeeId,
                EmployeeName = document.EmployeeName,
                Email = document.Email ?? "",
                Status = verificationPassed ? "Verified" : "Failed",
                Message = verificationPassed ? "Document verified successfully" : "Document verification failed",
                BlobUrl = document.UploadedBlobUrl ?? "",
                VerifiedDate = DateTime.UtcNow
            };
            _logger.LogInformation($"Verification result for Employee: {result.Status}");

            return result;
        }
    }
}
