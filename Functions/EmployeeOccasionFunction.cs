using Microsoft.Azure.Functions.Worker;
using AzureFunctionPet.Repositories;
using AzureFunctionPet.Constants;
using Microsoft.Extensions.Logging;

namespace AzureFunctionPet.Functions
{
    public class EmployeeOcassion
    {
        private readonly EmailService _emailService;
        private readonly IEmployeeRepository _repository;
        private readonly ILogger<EmployeeOcassion> _logger;

        public EmployeeOcassion(
            IEmployeeRepository repository,
            ILogger<EmployeeOcassion> logger,
            EmailService emailService)
        {
            _repository = repository;
            _logger = logger;
            _emailService = emailService;
        }

        [Function("EmployeeOcassion")]
        public async Task Run([TimerTrigger("0 56 10 * * *")] TimerInfo timerInfo)
        {
            _logger.LogInformation($"Birthday check started at: {DateTime.UtcNow}");

            try
            {
                var employees = await _repository.GetAllEmployeesAsync();
                var today = DateTime.UtcNow.Date;

                var birthdayEmployees = employees?
                    .Where(e => e.DateOfBirth.Day == today.Day &&
                                e.DateOfBirth.Month == today.Month)
                    .ToList();

                _logger.LogInformation($"Birthday employees found: {birthdayEmployees?.Count ?? 0}");

                if (birthdayEmployees == null || birthdayEmployees.Count == 0)
                {
                    _logger.LogInformation("No birthdays today.");
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

                    _logger.LogInformation($"Birthday email sent to: {emp.Email}");
                }

                _logger.LogInformation("All birthday emails sent successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred during birthday email process");
            }
        }
    }
}
