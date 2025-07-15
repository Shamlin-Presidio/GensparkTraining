using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace EventManagementAPI.Misc;
public class PdfGenerator
{
    public static byte[] GenerateEventRegistrationPdf(string username, string eventTitle, string description, DateTime start, DateTime end)
    {
        QuestPDF.Settings.License = LicenseType.Community;
        var doc = Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.Margin(2, Unit.Centimetre);
                // page.Content().Column(col =>
                // {
                //     col.Item().Text($"Event Registration Confirmation").FontSize(20).Bold();
                //     col.Item().Text($"Name: {username}");
                //     col.Item().Text($"Event: {eventTitle}");
                //     col.Item().Text($"Description: {description}");
                //     col.Item().Text($"Starts At: {start}");
                //     col.Item().Text($"Ends At: {end}");
                // });
                 page.Content().Column(col =>
                {
                    col.Item().Text("Event Registration Confirmation").FontSize(20).Bold();
                    col.Item().Text($"Name: {username ?? "N/A"}");
                    col.Item().Text($"Event: {eventTitle ?? "N/A"}");
                    col.Item().Text($"Description: {description ?? "N/A"}");
                    col.Item().Text($"Starts At: {start:yyyy-MM-dd HH:mm}");
                    col.Item().Text($"Ends At: {end:yyyy-MM-dd HH:mm}");
                });
            });
        });

        return doc.GeneratePdf();
    }
}
