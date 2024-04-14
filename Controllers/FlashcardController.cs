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
        // Text or image only
        bool isTextOnly = image == null || type == "text-only";
        bool isImageOnly = label == "" || type == "image-only";

        // Portrait
        PageSize pageSize = PageSizes.A4.Portrait();
        int maxImageHeight = 500;
        int labelFontSize = 75;

        // Portrait, two pages OR text only? Font sizes should be larger
        if (type == "two-pages" || isTextOnly) {
            labelFontSize = 90;
        }

        // Portrait, two pages OR image only? Images should be larger
        if (type == "two-pages" || isImageOnly) {
            maxImageHeight = 800;
        }

        // Landscape
        if (orientation == "landscape") {
            pageSize = PageSizes.A4.Landscape();
            labelFontSize = 60;
            maxImageHeight = 325;

            // If landscape, text only, font should be larger
            if (type == "two-pages" || isTextOnly) {
                labelFontSize = 100;
            }

            // If landscape, image only, image should be larger
            if (type == "two-pages" || isImageOnly) {
                maxImageHeight = 400;
            }
        }

        // Lined font
        string fontFile = "wwwroot/fonts/nara-penmanship-lined.ttf"; 
        string fontName = "Nara Penmanship Line P";

        // Unlined font
        if (font == "unlined") {
            fontFile = "wwwroot/fonts/we-can-unlined.otf";
            fontName = "HandwritingWeCan";
        }

        Console.WriteLine("maxImageHeight");
        Console.WriteLine(maxImageHeight);

        Document document = Document.Create(container =>
        {
            container.Page(page =>
            {
                // Font setup
                FontManager.RegisterFont(System.IO.File.OpenRead(fontFile));

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
                    .Column(x => {
                        if (image != null && type != "text-only") {
                            long fileSize = image.Length;
                            string fileType = image.ContentType;
                            if (fileSize > 0)
                            {
                                using (var stream = new MemoryStream())
                                {
                                    image.CopyTo(stream);
                                    var bytes = stream.ToArray();
                                    x.Item().ShowOnce().Image(bytes, ImageScaling.FitArea);
                                }
                            }
                        }
                    });

                // Label
                // If it is "text-only" OR if there's no image, show the label at the center
                if (isTextOnly) {
                    page.Content()
                        .AlignCenter()
                        .AlignMiddle()
                        .PaddingVertical(1, Unit.Centimetre)
                        .Column(x => {
                            x.Item().Text(label).FontSize(labelFontSize).FontFamily(fontName);
                        });
                }

                // Else if it's "image and text", show the label at the bottom
                else if (type == "image-text") {
                    page.Content()
                        .AlignCenter()
                        .AlignMiddle()
                        .PaddingVertical(1, Unit.Centimetre)
                        .Column(x => {
                            x.Item().Text(label).FontSize(labelFontSize).FontFamily(fontName);
                        });
                }

                // Else if it is "1st page: Image, 2nd page: Text", do a pagebreak first before adding the label at the center
                else if (type == "two-pages") {
                    page.Content()
                        .AlignCenter()
                        .AlignMiddle()
                        .PaddingVertical(1, Unit.Centimetre)
                        .Column(x => {
                            x.Item().PageBreak();
                            x.Item().Text(label).FontSize(labelFontSize).FontFamily(fontName);
                        });
                }

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
