using SendGrid;
using SendGrid.Helpers.Mail;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

public class EmailService
{
    private readonly ILogger<EmailService> _logger;
    private readonly string _sendGridApiKey;
    public EmailService(ILogger<EmailService> logger, IConfiguration config)
    {
        _logger = logger;

        _sendGridApiKey = config["SendGridApiKey"] ?? Environment.GetEnvironmentVariable("SendGridApiKey");
    }

    public async Task SendEmailAsync(string toEmail, string subject, string body)
    {
        var client = new SendGridClient(_sendGridApiKey);
        var from = new EmailAddress("kammamvenky111@gmail.com", "Venkatesh Kammam");
        var to = new EmailAddress(toEmail);
        var msg = MailHelper.CreateSingleEmail(from, to, subject, body, body);
        var response = await client.SendEmailAsync(msg);
    }
}
