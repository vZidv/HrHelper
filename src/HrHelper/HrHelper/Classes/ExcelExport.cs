using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using ex = Microsoft.Office.Interop.Excel;
using Microsoft.Office.Interop.Excel;
using System.Threading;

namespace HrHelper.Classes
{
    public static class ExcelExport
    {
        /// <summary>
        /// Экспорт данных из DataGrid в Excel
        /// </summary>
        ///param name="dataGrid">DataGrid, данные из которого нужно экспортировать</param>   
        public static void ExportSummaryList(DataGrid dataGrid)
        {
            Thread.Sleep(1000);
            // Создаем новый экземпляр приложения Excel
            ex.Application excel = new ex.Application();

            // Создаем новую книгу Excel
            Workbook workbook = excel.Workbooks.Add(Type.Missing);

            // Создаем новый лист Excel
            Worksheet worksheet = null;
            worksheet = workbook.Sheets["Лист1"];
            worksheet = workbook.ActiveSheet;

            // Заполняем ячейки Excel данными из DataGrid
            for (int i = 1; i <= dataGrid.Columns.Count-1; i++)
            {
                worksheet.Cells[1, i] = dataGrid.Columns[i - 1].Header;
            }

            for (int i = 0; i < dataGrid.Items.Count; i++)
            {
                for (int j = 0; j < dataGrid.Columns.Count-1; j++)
                {
                    TextBlock cellContent = dataGrid.Columns[j].GetCellContent(dataGrid.Items[i]) != null ? dataGrid.Columns[j].GetCellContent(dataGrid.Items[i]) as TextBlock : null;
                    if(cellContent != null)
                        worksheet.Cells[i+2, j+1] = cellContent.Text;
                }
            }

            // Сохраняем файл Excel
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Excel files (*.xlsx)|*.xlsx";
            if (saveFileDialog.ShowDialog() == true)
            {
                workbook.SaveAs(saveFileDialog.FileName);
                MyMessageBox.Show("Внимание","Файл успешно сохранен");
            }

            // Закрываем приложение Excel
            excel.Quit();
        }
    }
}
