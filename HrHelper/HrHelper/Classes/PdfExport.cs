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
        /// <summary>
        /// Экспорт резюме
        /// </summary>
        /// <param name="items">Словарь элементов</param>
        /// <param name="summary">Объект резюме</param>
        public static void SummaryExport(Dictionary<string, string> items, Summary summary)
        {
            // Создание диалогового окна для сохранения файла
            Microsoft.Win32.SaveFileDialog dialog = new Microsoft.Win32.SaveFileDialog();
            dialog.Filter = "All files (*.*)|*.*";
            dialog.FileName = $"{summary.LastName} {summary.FirstName} {summary.Patronymic}";
            dialog.ShowDialog();

            // Установка пути сохранения файла
            string savePath = dialog.FileName;
            dialog.FileName = Directory.GetCurrentDirectory() + $"\\Word\\ {summary.LastName} {summary.FirstName} {summary.Patronymic}";

            // Экспорт сводки в Word и конвертация в PDF
            WordExport.ExportSummary(items, summary, dialog);
            ConvertDOCtoPDF(savePath + ".pdf", dialog.FileName + ".docx");

            // Удаление временного файла .docx
            File.Delete($"{dialog.FileName}.docx");
        }

        /// <summary>
        /// Конвертация файла DOCX в PDF
        /// </summary>
        /// <param name="filenamePdf">Имя файла PDF, например "Document.pdf"</param>
        /// <param name="filenameDocx">Имя файла DOCX, например "Document.docx"</param>
        public static void ConvertDOCtoPDF(string filenamePdf, string filenameDocx)
        {
            object misValue = System.Reflection.Missing.Value;
            string PATH_APP_PDF = $"{filenamePdf}";

            // Создание экземпляра приложения Word
            var WORD = new Word.Application();

            // Открытие документа DOCX
            Word.Document doc = WORD.Documents.Open($"{filenameDocx}");
            doc.Activate();

            // Сохранение документа в формате PDF
            doc.SaveAs2(@PATH_APP_PDF, Word.WdSaveFormat.wdFormatPDF, misValue, misValue, misValue,
            misValue, misValue, misValue, misValue, misValue, misValue, misValue);

            // Закрытие документа и выход из приложения Word
            doc.Close();
            WORD.Quit();
        }
    }
}
