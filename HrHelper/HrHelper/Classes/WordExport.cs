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
        static public string ProjectPath()
        {
            return Directory.GetCurrentDirectory();
        }

        public bool ExportSummary(Dictionary<string, string> items,Summary summary)
        {
            string filleName = ProjectPath() + "\\Word\\SummaryModel.docx";
            Word.Application app = null;

            try
            {
                FileInfo fileInfo = new FileInfo(filleName);
                app = new Word.Application();

                Microsoft.Win32.SaveFileDialog dialog = new Microsoft.Win32.SaveFileDialog();
                dialog.Filter = "All files (*.*)|*.*";
                dialog.FileName = $"{summary.LastName} {summary.FirstName} {summary.Patronymic}";
                dialog.ShowDialog();

                fileInfo.CopyTo(dialog.FileName + ".docx");

                MessageBox.Show(dialog.FileName);

                fileInfo = new FileInfo(dialog.FileName + ".docx");

                MessageBox.Show(fileInfo.FullName);
                
                string file = fileInfo.FullName;
                object missing = Type.Missing;

                Word.Document document = app.Documents.Open(file);
                if (summary.Photo != null)
                    AddPhoto(document, summary.Photo.Path);

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

                app.Visible = true;

                return true;
            }
            catch (Exception ex) 
            { 
                //MyMessageBox.Show("Ошибка", ex.Message, true);
                MessageBox.Show(ex.Message);
            }
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

            object range = Type.Missing;
            Microsoft.Office.Interop.Word.WdWrapType wrapp = Microsoft.Office.Interop.Word.WdWrapType.wdWrapSquare;
            document.Shapes.AddPicture(ProjectPath() + photo,
                ref f, ref t, ref left, ref top, ref range).WrapFormat.Type = wrapp;
        }
    }
}
