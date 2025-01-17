using Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using System.Net.Mail;
using System.Net;

public class EmailService : IEmailService
{
    private readonly string? _smtpServer;
    private readonly int _smtpPort;
    private readonly string? _senderEmail;
    private readonly string? _senderPassword;

    public EmailService(IConfiguration configuration)
    {
        var smtpSettings = configuration.GetSection("SmtpSettings");
        _smtpServer = smtpSettings["Server"];
        _smtpPort = int.Parse(smtpSettings["Port"] ?? "587");
        _senderEmail = smtpSettings["SenderEmail"];
        _senderPassword = smtpSettings["SenderPassword"];
    }

    public async Task SendEmailAsync(string recipientEmail, string subject, string body)
    {
        if (string.IsNullOrEmpty(_smtpServer) || string.IsNullOrEmpty(_senderEmail) || string.IsNullOrEmpty(_senderPassword))
        {
            throw new Exception("SMTP yapılandırması eksik.");
        }

        var mail = new MailMessage
        {
            From = new MailAddress(_senderEmail),
            Subject = subject,
            Body = body,
            IsBodyHtml = true
        };
        mail.To.Add(recipientEmail);

        using (var smtpClient = new SmtpClient(_smtpServer, _smtpPort))
        {
            smtpClient.Credentials = new NetworkCredential(_senderEmail, _senderPassword);
            smtpClient.EnableSsl = true;

            await smtpClient.SendMailAsync(mail);
        }
    }
}
