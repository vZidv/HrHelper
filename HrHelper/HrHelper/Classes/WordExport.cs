using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Threading.Tasks;
using Word = Microsoft.Office.Interop.Word;
using System.Windows.Media.Imaging;
using Microsoft.Office.Interop.Word;
using System.Windows.Controls;
using Microsoft.Office.Interop.Excel;
using System.Diagnostics;
using System.Reflection.Metadata;

namespace HrHelper.Classes
{
    public class WordExport
    {

        public bool ExportSummary(Dictionary<string, string> items, string filleName, string photo)
        {
            Word.Application app = null;

            try
            {

                FileInfo fileInfo = new FileInfo(filleName);
                app = new Word.Application();

                Object file = fileInfo.FullName;
                object missing = Type.Missing;

                Word.Document document = app.Documents.Open(file);
                if (photo != null)
                    AddPhoto(document, photo);

                foreach (var item in items)
                {
                    Word.Find find = app.Selection.Find;
                    find.Text = item.Key;
                    find.Replacement.Text = item.Value;

                    Object wrap = Word.WdFindWrap.wdFindContinue;
                    object replace = Word.WdReplace.wdReplaceAll;

                    find.Execute(FindText: Type.Missing,
                        MatchCase: false,
                        MatchWholeWord: false,
                        MatchWildcards: false,
                        MatchSoundsLike: false,
                        MatchAllWordForms: false,
                        Forward: false,
                        Wrap: wrap,
                        Format: false,
                        ReplaceWith: missing, Replace: replace);
                }

                Object newFileName = Path.Combine(fileInfo.DirectoryName, "Test" + fileInfo.Name);
                app.ActiveDocument.SaveAs2(newFileName);
                app.ActiveDocument.Close();
                app.Quit();
                return true;
            }
            catch (Exception ex) { MyMessageBox.Show("Ошибка", ex.Message, true); }
            finally
            {
                if (app != null)
                    app.Quit();
            }

            return false;
        }
        public void AddPhoto(Word.Document document, string photo)
        {
            object f = false;
            object t = true;
            object left = Type.Missing;
            object top = Type.Missing;
            //object width = 300;
            //object height = 300;
            object range = Type.Missing;
            Microsoft.Office.Interop.Word.WdWrapType wrapp = Microsoft.Office.Interop.Word.WdWrapType.wdWrapSquare;
            document.Shapes.AddPicture("F:\\Артём\\Проекты и их материалы\\HrHelper\\Programm\\HrHelper\\HrHelper\\HrHelper\\bin\\Debug\\net6.0-windows" + photo,
                ref f, ref t, ref left, ref top, ref range).WrapFormat.Type = wrapp;
        }
    }
}
