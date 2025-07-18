namespace OrderApp.Web;

using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Options;
using OrderApp.Core.MailSeting;


public class MailService
{
    private readonly SmtpSettings _smtpSettings;

    public MailService(IOptions<SmtpSettings> smtpSettings)
    {
        _smtpSettings = smtpSettings.Value;
    }

    public async Task SendEmailAsync(string to, string subject, string body)
    {
        if (string.IsNullOrWhiteSpace(to))
            throw new ArgumentException("Recipient email address cannot be empty.", nameof(to));

        var mail = new MailMessage
        {
            From = new MailAddress(_smtpSettings.From),
            Subject = subject,
            Body = body,
            IsBodyHtml = true
        };
        mail.To.Add(new MailAddress(to));

        using var smtp = new SmtpClient
        {
            Host = _smtpSettings.Host,
            Port = _smtpSettings.Port,
            EnableSsl = true,
            Credentials = new NetworkCredential(
                _smtpSettings.Username,
                _smtpSettings.Password
            )
        };

        await smtp.SendMailAsync(mail);
    }
}

