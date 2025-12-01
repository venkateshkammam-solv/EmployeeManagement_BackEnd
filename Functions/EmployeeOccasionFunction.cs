
using Microsoft.Azure.Functions.Worker;
using AzureFunctionPet.Repositories;

using Shared.Services;
using AzureFunctionPet.Constants;

namespace AzureFunctionPet.Functions
{
    public class TimerSummaryFunction
    {
        private readonly DataLog _dataLog;
        private readonly EmailService _emailService;
        private readonly IEmployeeRepository _repository;

        public TimerSummaryFunction(IEmployeeRepository repository, DataLog dataLog, EmailService emailService)
        {
            _repository = repository;
            _dataLog = dataLog;
            _emailService = emailService;
        }

        [Function("TimerSummaryFunction")]
        public async Task Run([TimerTrigger("0 56 10 * * *")] TimerInfo timerInfo)
        {
            await _dataLog.LogInfoAsync($"Birthday check started at: {DateTime.UtcNow}");

            try
            {
                var employees = await _repository.GetAllEmployeesAsync();
                var today = DateTime.UtcNow.Date;

                var birthdayEmployees = employees?
                    .Where(e =>
                        e.DateOfBirth.Day == today.Day &&
                        e.DateOfBirth.Month == today.Month
                    ).ToList();

                await _dataLog.LogInfoAsync($"Birthday employees found: {birthdayEmployees?.Count ?? 0}");

                if (birthdayEmployees == null || birthdayEmployees.Count == 0)
                {
                    await _dataLog.LogInfoAsync("No birthdays today.");
                    return;
                }

                foreach (var emp in birthdayEmployees)
                {
                    string emailBody = EmailTemplates.BirthdayWishes.Replace("{{Name}}", emp.FullName);

                    await _emailService.SendEmailAsync(
                        emp.Email,
                        "Happy Birthday",
                        emailBody
                    );

                    await _dataLog.LogInfoAsync($"Birthday email sent to {emp.FullName} ({emp.Email})");
                }

                await _dataLog.LogInfoAsync("All birthday emails sent successfully.");
            }
            catch (Exception ex)
            {
                await _dataLog.LogErrorAsync("Error occurred during birthday email process", ex);
            }
        }
    }
}
