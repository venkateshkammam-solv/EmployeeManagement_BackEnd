using System;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using AzureFunctionPet.Models;
using Shared.Services;
using AzureFunctions_Triggers.Models;
using AzureFunctionPet.Constants;

namespace AzureFunctionPet.Functions
{
    public class QueueProcessorFunction
    {
        private readonly DataLog _dataLog;
        private readonly EmailService _emailService;

        public QueueProcessorFunction(DataLog dataLog, EmailService emailService)
        {
            _dataLog = dataLog;
            _emailService = emailService;
        }

        [Function("QueueProcessor")]
        public async Task Run([QueueTrigger("employee-events", Connection = "AzureWebJobsStorage")] string queueMessage, FunctionContext context)
        {
            var logger = context.GetLogger<QueueProcessorFunction>();
            string employeeName = "Unknown";

            try
            {
                await _dataLog.LogInfoAsync($"Queue message received: {queueMessage}");
                logger.LogInformation("Queue message received.");

                // Deserialize the message
                var employeeData = JsonSerializer.Deserialize<EmployeeDetailsDto>(
                    queueMessage,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
                );

                if (employeeData == null)
                {
                    var errorMsg = "Queue message could not be deserialized to EmployeeDetailsDto.";
                    logger.LogError(errorMsg);
                    await _dataLog.LogErrorAsync(errorMsg);
                    return;
                }

                employeeName = employeeData.FullName;
                await _dataLog.LogInfoAsync($"Processing onboarding for employee: {employeeName}");
                logger.LogInformation("Processing employee onboarding {@EmployeeData}", employeeData);
                var subject = $"Welcome to the Company, {employeeName}!";
                var body = EmailTemplates.EmployeeOnboarding
                    .Replace("{{EmployeeName}}", employeeName)
                    .Replace("{{EmployeeCode}}", employeeData.id)
                    .Replace("{{Department}}", employeeData.Department)
                    .Replace("{{DateOfJoining}}", employeeData.DateOfJoining.ToString("yyyy-MM-dd"));
                await _emailService.SendEmailAsync(employeeData.Email, subject, body);
                await _dataLog.LogInfoAsync($"Onboarding email sent to {employeeName} ({employeeData.Email})");
                logger.LogInformation("Onboarding email successfully sent to employee {EmployeeName}", employeeName);
            }
            catch (JsonException jsonEx)
            {
                logger.LogError(jsonEx, "Failed to deserialize queue message for employee: {EmployeeName}", employeeName);
                await _dataLog.LogErrorAsync($"JSON deserialization failed for employee: {employeeName}", jsonEx);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to process onboarding queue message for employee: {EmployeeName}", employeeName);
                await _dataLog.LogErrorAsync($"Error processing onboarding queue message for employee: {employeeName}", ex);
            }
        }
    }
}
