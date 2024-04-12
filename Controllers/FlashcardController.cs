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
    public IActionResult Generate(string image)
    {
        string imageUrl = Request.Form["imageurl"];
        string imageFilename = Request.Form["image"];
        string label = Request.Form["label"];
        string font = Request.Form["font"];
        string type = Request.Form["type"];
        string orientation = Request.Form["orientation"];
        string notes = Request.Form["notes"];

        Document document = Document.Create(container =>
        {
            container.Page(page =>
            {
                // Font setup
                
                // Page setup
                page.Size(PageSizes.A4);
                page.Margin(2, Unit.Centimetre);
                page.PageColor(Colors.White);
                page.DefaultTextStyle(x => x.FontSize(20));
                // page.Header()
                //     .Text("Hello PDF!")
                //     .SemiBold().FontSize(36).FontColor(Colors.Blue.Medium);

                page.Content()
                    .PaddingVertical(1, Unit.Centimetre)
                    .Column(x =>
                    {
                        x.Spacing(20);

                        // x.Item().Image(Placeholders.Image(200, 100));
                        x.Item().Text(label);
                    });

                // page.Footer()
                //     .AlignCenter()
                //     .Text(x =>
                //     {
                //         x.Span("Page ");
                //         x.CurrentPageNumber();
                //     });
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
