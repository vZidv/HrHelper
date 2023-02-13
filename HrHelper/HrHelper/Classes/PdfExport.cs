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

using Word = Microsoft.Office.Interop.Word;
using System.Threading;
using System.Windows;

namespace HrHelper.Classes
{
    static class PdfExport
    {
        public static void SummaryExport(Dictionary<string, string> items, Summary summary)
        {
            Microsoft.Win32.SaveFileDialog dialog = new Microsoft.Win32.SaveFileDialog();
            dialog.Filter = "All files (*.*)|*.*";
            dialog.FileName = $"{summary.LastName} {summary.FirstName} {summary.Patronymic}";
            dialog.ShowDialog();

            string savePath = dialog.FileName;
            dialog.FileName = Directory.GetCurrentDirectory() + $"\\Word\\ {summary.LastName} {summary.FirstName} {summary.Patronymic}";
            
            MessageBox.Show(dialog.FileName);
            WordExport.ExportSummary(items, summary, dialog);
            ConvertDOCtoPDF(savePath + ".pdf", dialog.FileName + ".docx");

            File.Delete($"{dialog.FileName}.docx");

        }

        /// <summary>
        /// Conver File Docx to Pdf
        /// </summary>
        /// <param name="filenamePdf">Pdf file name be like "Document.pdf"</param>
        /// <param name="filenameDocx">Pdf file name be like "Document.doxc"</param>
        public static void ConvertDOCtoPDF(string filenamePdf, string filenameDocx)
        {

            object misValue = System.Reflection.Missing.Value;
            string PATH_APP_PDF = $"{filenamePdf}";            
            var WORD = new Word.Application();
            Word.Document doc = WORD.Documents.Open($"{filenameDocx}");
            doc.Activate();          
            doc.SaveAs2(@PATH_APP_PDF, Word.WdSaveFormat.wdFormatPDF, misValue, misValue, misValue,
            misValue, misValue, misValue, misValue, misValue, misValue, misValue);

            doc.Close();
            WORD.Quit();            
        }
    }
}
