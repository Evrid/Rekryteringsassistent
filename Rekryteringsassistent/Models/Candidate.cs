using DocumentFormat.OpenXml.Packaging;
using Microsoft.CodeAnalysis.Diagnostics;
using System.Text;
using UglyToad.PdfPig;

namespace Rekryteringsassistent.Models;

public class Candidate
{
    public int? Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string? Email { get; set; }
    public byte[]? CV_PDF { get; set; }
    public byte[]? CV_Word { get; set; }

    public string? CV_Text { get; set; }
    public AIAnalysisResult? AnalysisResult { get; set; }

    public void ConvertPdfToText()
    {
        using (var pdf = PdfDocument.Open(this.CV_PDF))
        {
            StringBuilder text = new StringBuilder();
            for (var i = 1; i <= pdf.NumberOfPages; i++)
            {
                var page = pdf.GetPage(i);
                text.AppendLine(page.Text);
            }
            this.CV_Text = text.ToString();
        }
    }

    public void ConvertWordToText()
    {
        using (var stream = new MemoryStream(this.CV_Word))
        {
            using (var doc = WordprocessingDocument.Open(stream, false))
            {
                var body = doc.MainDocumentPart.Document.Body;
                this.CV_Text = body.InnerText;
            }
        }
    }
}