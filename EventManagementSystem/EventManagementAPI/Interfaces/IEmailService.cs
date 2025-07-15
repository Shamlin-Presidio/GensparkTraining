using MailKit.Net.Smtp;
using MimeKit;
using MimeKit.Text;
namespace EventManagementAPI.Interfaces;

public interface IEmailService
{
    Task SendEmailAsync(string to, string subject, string htmlBody, byte[]? pdfAttachment = null, string? filename = null);
}