using EventManagementAPI.Interfaces;
using MailKit.Net.Smtp;
using MimeKit;
using MimeKit.Text;

public class EmailService : IEmailService
{
    private readonly IConfiguration _config;

    public EmailService(IConfiguration config)
    {
        _config = config;
    }

    public async Task SendEmailAsync(string to, string subject, string htmlBody, byte[]? pdfAttachment = null, string? filename = null)
    {
        try
        {
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(_config["EmailSettings:From"]));
            email.To.Add(MailboxAddress.Parse(to));
            email.Subject = subject;

            var builder = new BodyBuilder { HtmlBody = htmlBody };

            if (pdfAttachment != null && filename != null)
                builder.Attachments.Add(filename, pdfAttachment, ContentType.Parse("application/pdf"));

            email.Body = builder.ToMessageBody();

            using var smtp = new SmtpClient();
            await smtp.ConnectAsync(
                _config["EmailSettings:SmtpServer"],
                int.Parse(_config["EmailSettings:Port"]),
                MailKit.Security.SecureSocketOptions.StartTls
            );

            await smtp.AuthenticateAsync(
                _config["EmailSettings:Username"],
                _config["EmailSettings:Password"]
            );

            await smtp.SendAsync(email);
            await smtp.DisconnectAsync(true);

            Console.WriteLine($"📧 Email sent to {to} with subject '{subject}'" +
                (pdfAttachment != null ? $" and PDF '{filename}' attached." : "."));
        }
        catch (Exception ex)
        {
            Console.WriteLine("Failed to send email: " + ex.Message);
        }
    }
}
