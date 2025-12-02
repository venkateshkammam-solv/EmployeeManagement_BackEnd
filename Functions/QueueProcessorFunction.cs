using System;
using System.Text.Json;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using AzureFunctions_Triggers.Models;
using AzureFunctionPet.Constants;

namespace AzureFunctionPet.Functions
{
    public class QueueProcessorFunction
    {
        private readonly EmailService _emailService;
        private readonly ILogger<QueueProcessorFunction> _logger;

        public QueueProcessorFunction(EmailService emailService, ILogger<QueueProcessorFunction> logger)
        {
            _emailService = emailService;
            _logger = logger;
        }

        [Function("QueueProcessor")]
        public async Task Run(
            [QueueTrigger("employee-events", Connection = "AzureWebJobsStorage")] string queueMessage,
            FunctionContext context)
        {
            string employeeName = "Unknown";

            try
            {
                _logger.LogInformation("Queue message received.");

                // Deserializing queue payload
                var employeeData = JsonSerializer.Deserialize<EmployeeDetailsDto>(
                    queueMessage,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                if (employeeData == null)
                {
                    _logger.LogWarning("Queue message could not be deserialized to EmployeeDetailsDto.");
                    return;
                }

                employeeName = employeeData.FullName;

                _logger.LogInformation("Processing onboarding workflow.");

                //onboarding email
                var subject = $"Welcome to the Company, {employeeName}!";
                var body = EmailTemplates.EmployeeOnboarding
                    .Replace("{{EmployeeName}}", employeeName)
                    .Replace("{{EmployeeCode}}", employeeData.id)
                    .Replace("{{Department}}", employeeData.Department)
                    .Replace("{{DateOfJoining}}", employeeData.DateOfJoining.ToString("yyyy-MM-dd"));

                await _emailService.SendEmailAsync(employeeData.Email, subject, body);

                _logger.LogInformation("Onboarding email sent successfully.");
            }
            catch (JsonException jsonEx)
            {
                _logger.LogError(jsonEx, "Failed to deserialize queue message.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while processing the queue message.");
            }
        }
    }
}
