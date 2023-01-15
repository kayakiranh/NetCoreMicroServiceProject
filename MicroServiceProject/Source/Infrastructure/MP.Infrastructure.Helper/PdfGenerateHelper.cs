using IronPdf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MP.Infrastructure.Helper
{
    public static class PdfGenerateHelper
    {
        public static void GeneratePDF()
        {
            HtmlToPdf renderer = new HtmlToPdf();
            renderer.RenderHtmlAsPdf("<h1>This is test file</h1>").SaveAs("Example.pdf");
        }
    }
}
