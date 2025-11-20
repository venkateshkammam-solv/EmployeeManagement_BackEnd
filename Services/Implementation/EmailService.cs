using SendGrid;
using SendGrid.Helpers.Mail;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

public class EmailService
{
    private readonly ILogger<EmailService> _logger;
    private readonly string _sendGridApiKey;
    private readonly string _fromEmail;
    private readonly string _setting;
    public EmailService(ILogger<EmailService> logger, IConfiguration config)
    {
        _logger = logger;

        _sendGridApiKey = config["SendGridApiKey"] ?? Environment.GetEnvironmentVariable("SendGridApiKey");
        _fromEmail = config["EmailSettings:FromEmail"];
    }

    public async Task SendEmailAsync(string toEmail, string subject, string body)
    {
        var client = new SendGridClient(_sendGridApiKey);
        var from = new EmailAddress(_fromEmail, "Venkatesh Kammam");
        var to = new EmailAddress(toEmail);
        var msg = MailHelper.CreateSingleEmail(from, to, subject, body, body);

        var response = await client.SendEmailAsync(msg);

        _logger.LogInformation($"Email sent to {toEmail}, Status: {response.StatusCode}");
    }
}
