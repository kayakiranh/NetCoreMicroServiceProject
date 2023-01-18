using IronPdf;
using System;

namespace MP.Infrastructure.Helper
{
    public static class PdfGenerateHelper
    {
        public static void GeneratePDF(int customerId, int creditCardId)
        {
            HtmlToPdf renderer = new HtmlToPdf();
            renderer.RenderHtmlAsPdf($"<h1>Customer : {customerId}</h1><br><h1>Credit Card : {creditCardId}</h1>").SaveAs($"{customerId}-{creditCardId}-{new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds()}.pdf");
        }
    }
}
