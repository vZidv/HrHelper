using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;
using PdfSharp;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using System.Diagnostics;
using System.Drawing.Configuration;
using System.Drawing;


namespace HrHelper.Classes
{
    static class PdfExport
    {
        public static void SummaryExport(Summary summary,int age)
        {
            PdfDocument document = new PdfDocument();
            document.Info.Title = "Test pdf Export!";

            PdfPage page = document.AddPage();

            XGraphics gfx = XGraphics.FromPdfPage(page);

            XFont font = new XFont("Arial", 25, XFontStyle.Bold);

            string fullname = $"{summary.LastName} {summary.FirstName} {summary.Patronymic}";
            string genderAge = $"{summary.Gender},{age} лет, родился {summary.Birthday}";

            gfx.DrawString(fullname, font, XBrushes.Black,
                    new XRect(0, 0, page.Width, page.Height), XStringFormat.TopCenter);
            font = new XFont("Arial", 9, XFontStyle.Regular);
            gfx.DrawString(genderAge, font, XBrushes.Black,
                    new XRect(0, 30, page.Width, page.Height), XStringFormat.TopCenter);


            string filename = $"{summary.LastName} {summary.FirstName} {summary.Patronymic}.pdf";
            document.Save(filename);
            MyMessageBox.Show("Сделано", "pdf создан!");
        }
    }
}
