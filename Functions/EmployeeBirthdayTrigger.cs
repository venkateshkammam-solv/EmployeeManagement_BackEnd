using Microsoft.Azure.Functions.Worker;
using AzureFunctions_Triggers.Repositories;
using AzureFunctions_Triggers.Constants;
using Microsoft.Extensions.Logging;
using AzureFunctions_Triggers.Shared.Constants;

namespace AzureFunctions_Triggers.Functions
{
    public class EmployeeBirthdayTrigger
    {
        private readonly EmailService _emailService;
        private readonly IEmployeeRepository _repository;
        private readonly ILogger<EmployeeBirthdayTrigger> _logger;

        public EmployeeBirthdayTrigger(
            IEmployeeRepository repository,
            ILogger<EmployeeBirthdayTrigger> logger,
            EmailService emailService)
        {
            _repository = repository;
            _logger = logger;
            _emailService = emailService;
        }

        [Function("EmployeeBirthdayTrigger")]
        public async Task Run([TimerTrigger("0 56 10 * * *")] TimerInfo timerInfo)
        {

            try
            {
                var employees = await _repository.GetAllEmployeesAsync();
                var today = DateTime.UtcNow.Date;

                var birthdayEmployees = employees?
                    .Where(e => e.DateOfBirth.Day == today.Day &&
                                e.DateOfBirth.Month == today.Month)
                    .ToList();

                _logger.LogInformation(Messages.BirthdayEmployeeFoundMsg);

                if (birthdayEmployees == null || birthdayEmployees.Count == 0)
                {
                    _logger.LogInformation(Messages.BirthdayEmployeeNotFoundMsg);
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

                    _logger.LogInformation(Messages.BirthdayEmailSentMsg);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, Messages.BirthdayEmailSentErrorMsg);
            }
        }
    }
}
