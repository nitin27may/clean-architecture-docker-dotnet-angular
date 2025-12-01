namespace Contact.Domain.Interfaces;
public interface IEmailService
{
    Task SendEmailAsync(string to, string subject, string body);
}
