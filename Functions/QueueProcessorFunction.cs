using System;
using System.Text.Json;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using AzureFunctions_Triggers.Models;
using AzureFunctions_Triggers.Shared.Constants;
using AzureFunctions_Triggers.Constants;

namespace AzureFunctions_Triggers.Functions
{
    public class QueueProcessorFunction
    {
        private readonly EmailService _emailService;
        private readonly ILogger<QueueProcessorFunction> _logger;

        public QueueProcessorFunction(EmailService emailService,ILogger<QueueProcessorFunction> logger)
        {
            _emailService = emailService;
            _logger = logger;
        }

        [Function("QueueProcessor")]
        public async Task Run([QueueTrigger("employee-events", Connection = "AzureWebJobsStorage")] string queueMessage, FunctionContext context)
        {
            string employeeName = string.Empty;

            try
            {
                _logger.LogInformation(Messages.QueueMessageReceived);

                var employeeData = JsonSerializer.Deserialize<EmployeeDetailsDto>(queueMessage, 
                   new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                if (employeeData == null)
                {
                    _logger.LogWarning(Messages.QueueDeserializationFailed);
                    return;
                }

                employeeName = employeeData.FullName;

                _logger.LogInformation(Messages.ProcessingOnboardingWorkflow);

                var subject = $"Welcome to the Company, {employeeName}!";
                var body = EmailTemplates.EmployeeOnboarding
                    .Replace("{{EmployeeName}}", employeeName)
                    .Replace("{{EmployeeCode}}", employeeData.id)
                    .Replace("{{Department}}", employeeData.Department)
                    .Replace("{{DateOfJoining}}", employeeData.DateOfJoining.ToString("yyyy-MM-dd"));
                await _emailService.SendEmailAsync(employeeData.Email, subject, body);

                _logger.LogInformation(Messages.OnboardingEmailSent);
            }
            catch (JsonException jsonEx)
            {
                _logger.LogError(jsonEx, Messages.QueueDeserializationError);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, Messages.QueueProcessingError);
            }
        }
    }
}
