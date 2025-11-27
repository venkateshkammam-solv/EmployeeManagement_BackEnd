using System;
using System.Threading.Tasks;
using AzureFunctionPet.Constants;
using AzureFunctionPet.Models;
using AzureFunctions.Models;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Shared.Services;  

public class SendEmailActivity
{
    private readonly EmailService _emailService;
    private readonly ILogger<SendEmailActivity> _logger;

    public SendEmailActivity(EmailService emailService, ILogger<SendEmailActivity> logger)
    {
        _emailService = emailService;
        _logger = logger;
    }

    [Function("SendEmailActivity")]
    public async Task RunAsync(
        [ActivityTrigger] DocumentMetadata metadata)
    {
        try
        {
            string statusColor = metadata.Status == "Verified" ? "green" : "red";

            string emailBody = EmailTemplates.DocumentVerification
                .Replace("{{EmployeeName}}", metadata.EmployeeName)
                .Replace("{{EmployeeId}}", metadata.EmployeeId)
                .Replace("{{FileName}}", metadata.FileName)
                .Replace("{{Status}}", metadata.Status)
                .Replace("{{VerifiedDate}}", DateTime.UtcNow.ToString("dd MMM yyyy HH:mm tt"))
                .Replace("{{StatusColor}}", statusColor);

            await _emailService.SendEmailAsync(metadata.Email, subject: "Document Verification Status", body: emailBody);

            _logger.LogInformation($"Email successfully delivered to the Employee");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Failed to send email: {ex.Message}");
            throw;
        }
    }
}
