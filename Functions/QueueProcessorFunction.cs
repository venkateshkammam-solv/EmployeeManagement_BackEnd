using System.Text.Json;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using AzureFunctionPet.Models;

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
        public async Task Run(
            [QueueTrigger("employee-events", Connection = "AzureWebJobsStorage")] string queueMessage)
        {
            await _dataLog.LogInfoAsync($"Queue message received: {queueMessage}");

            try
            {
                var eventData = JsonSerializer.Deserialize<WebhookEvent>(queueMessage,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                await _dataLog.LogInfoAsync($"Processing event: {eventData?.Id} - {eventData?.Source}");

                var subject = $"Webhook Event Processed_VKammam: {eventData?.Id}";
                var body = $"Event ID: {eventData?.Id}\nSource: {eventData?.Source}\nTimestamp: {DateTime.UtcNow}";
                await _emailService.SendEmailAsync("kammamvenky26@gmail.com", subject, body);

                await _dataLog.LogInfoAsync($"Email sent for Event: {eventData?.Id}");
            }
            catch (Exception ex)
            {
                await _dataLog.LogErrorAsync("Error processing queue message", ex);
            }
        }
    }
}
