using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using toolfox.Models;
using toolfox.Models.ViewModels;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System.IO;
using Microsoft.Extensions.Primitives;
using QuestPDF.Drawing;
using HarfBuzzSharp;
using System.Net;

namespace toolfox.Controllers;

public class FlashcardController : Controller
{
    private readonly ILogger<FlashcardController> _logger;

    public FlashcardController(ILogger<FlashcardController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        // View() matches the name of the function, that is, "Index"...
        // ...to the filename within Views, that is, "Index.cshtml"
        return View();
    }

    [HttpPost]
    public IActionResult Generate(IFormFile image, string label, string font, string type, string orientation, string notes)
    {
        PageSize pageSize = orientation == "portrait" ? PageSizes.A4.Portrait() : PageSizes.A4.Landscape();
        int maxImageHeight = orientation == "portrait" ? 500 : 325;
        int labelFontSize = orientation == "portrait" ? 60 : 40;

        Document document = Document.Create(container =>
        {
            container.Page(page =>
            {
                // Font setup

                // Page setup
                page.Size(pageSize);
                page.Margin(2, Unit.Centimetre);
                page.PageColor(Colors.White);
                page.DefaultTextStyle(x => x.FontSize(20));

                // Image
                page.Header()
                    .AlignMiddle()
                    .AlignCenter()
                    .MaxHeight(maxImageHeight)
                    .Column(x =>
                    {
                        if (image != null) {
                            long fileSize = image.Length;
                            string fileType = image.ContentType;
                            if (fileSize > 0)
                            {
                                using (var stream = new MemoryStream())
                                {
                                    image.CopyTo(stream);
                                    var bytes = stream.ToArray();
                                    x.Item().Image(bytes, ImageScaling.FitArea);
                                }
                            }
                        }
                    });

                // Label
                page.Content()
                    .AlignCenter()
                    .AlignBottom()
                    .PaddingVertical(1, Unit.Centimetre)
                    .Column(x =>
                    {
                        x.Item().Text(label).FontSize(labelFontSize);
                    });

                // Notes
                page.Footer()
                    .AlignCenter()
                    .Text(notes)
                    .FontSize(15).FontColor(Colors.Grey.Medium);
            });
        });
        byte[] pdfBytes = document.GeneratePdf();
        MemoryStream ms = new MemoryStream(pdfBytes);

        FileStreamResult fileStreamResult = new FileStreamResult(ms, "application/pdf");
        fileStreamResult.FileDownloadName = "Sample.pdf";
        return fileStreamResult;
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
