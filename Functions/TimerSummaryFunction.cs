using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using AzureFunctionPet.Repositories;

namespace AzureFunctionPet.Functions
{
    //public class TimerSummaryFunction
    //{
    //    private readonly DataLog _dataLog;
    //    private readonly EmailService _emailService;
    //    private readonly ICosmosRepository _repository;

    //    public TimerSummaryFunction(ICosmosRepository repository, DataLog dataLog, EmailService emailService)
    //    {
    //        _repository = repository;
    //        _dataLog = dataLog;
    //        _emailService = emailService;
    //    }

    //    // CRON format: {second} {minute} {hour} {day} {month} {day-of-week}
    //    // "0 */5 * * * *" = every 5 minutes
    //    [Function("TimerSummaryFunction")]
    //    public async Task Run([TimerTrigger("0 */1 * * * *")] TimerInfo timerInfo)
    //    {
    //        await _dataLog.LogInfoAsync($"Timer triggered at: {DateTime.UtcNow}");

    //        try
    //        {
    //            // Fetch all events from Cosmos DB
    //            var events = await _repository.GetAllEmployeesAsync();
    //            var totalCount = events?.Count() ?? 0;

    //            await _dataLog.LogInfoAsync($"Total events found in Cosmos DB: {totalCount}");

    //            // Compose summary message
    //            string summary = $"Timer Summary Report - {DateTime.UtcNow}\n" +
    //                             $"Total events in database: {totalCount}";

    //            // Send email summary
    //            await _emailService.SendEmailAsync("", "Webhook System Summary", summary);

    //            await _dataLog.LogInfoAsync("Summary email sent successfully.");
    //        }
    //        catch (Exception ex)
    //        {
    //            await _dataLog.LogErrorAsync("Error occurred in TimerSummaryFunction", ex);
    //        }
    //    }
    //}
}
