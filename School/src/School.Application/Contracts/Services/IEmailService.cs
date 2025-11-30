namespace School.Application.Contracts.Services
{
    public interface IEmailService
    {
        Task SendHtmlEmailAsync(string recipientEmail, string subject, string htmlBody);
        Task SendHtmlEmailAsync(List<string> recipientEmails, string subject, string htmlBody);
    }
}

