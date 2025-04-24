using Contact.Domain.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;

namespace Contact.Infrastructure.ExternalServices;

public class EmailService(IOptions<SmtpSettings> smtpSettings, ILogger<EmailService> logger) : IEmailService
{
    private readonly SmtpSettings _smtpSettings = smtpSettings.Value;

    public async Task SendEmailAsync(string to, string subject, string body)
    {
        try
        {
            using var client = new SmtpClient(_smtpSettings.SmtpServer, _smtpSettings.Port)
            {
                EnableSsl = _smtpSettings.EnableSsl,
                Credentials = new NetworkCredential(_smtpSettings.Username, _smtpSettings.Password)
            };

            var message = new MailMessage
            {
                From = new MailAddress(_smtpSettings.FromEmail),
                Subject = subject,
                Body = body,
                IsBodyHtml = true,
            };
            
            message.To.Add(new MailAddress(to));
            
            await client.SendMailAsync(message);
            logger.LogInformation("Email sent successfully to {Recipient}", to);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to send email to {Recipient}", to);
            throw;
        }
    }
}
