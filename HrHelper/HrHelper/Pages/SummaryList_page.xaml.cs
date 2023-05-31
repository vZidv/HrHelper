using HrHelper.Classes;
using Microsoft.EntityFrameworkCore;
//using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Media;
using ui = Wpf.Ui;

//using ex = Microsoft.Office.Interop.Excel;


namespace HrHelper.Pages
{
    public partial class SummaryList_page : Page
    {
        Vacancy selectVacancy { get; set; }
        List<Summary> selectedSummary = new List<Summary>();
        public SummaryList_page()
        {
            InitializeComponent();

        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            summary_dg.ItemsSource = LoadSummariesData();
            RowCountUpdate();
            LoadVacancyComboBox();
            excelButtonChangeOpacity();
        }
        private void LoadVacancyComboBox()
        {
            using (HrHelperDatabaseContext db = new HrHelperDatabaseContext())
            {
                List<Vacancy> vacancies = db.Vacancies.ToList();
                vacancies.Add(new Vacancy() { JobTitle = "Все вакансии" });
                vacancyChange_comboBox.ItemsSource = vacancies;
                vacancyChange_comboBox.DisplayMemberPath = "JobTitle";
            }
        }

        private Summary[] LoadSummariesData()
        {
            using (HrHelperDatabaseContext db = new HrHelperDatabaseContext())
                return db.Summaries.Include(o => o.Busyness).Include(o => o.Status).Include(o => o.SummaryForVacancies).Include(o => o.Contacts).ToArray();
        }
        private Summary[] SortSummaryData()
        {
            Summary[] summaries;
            using (HrHelperDatabaseContext db = new HrHelperDatabaseContext())
            {
                if (vacancyChange_comboBox.SelectedItem != null)
                {
                    if ((vacancyChange_comboBox.SelectedItem as Vacancy).JobTitle == "Все вакансии")
                    {
                        summaries = db.Summaries.ToArray();
                    }
                    else
                    {
                        summaries = db.Summaries.Where(o => o.SummaryForVacancies.Any(u => u.VacancyId == (vacancyChange_comboBox.SelectedItem as Vacancy).Id)).ToArray();
                    }
                    return summaries;
                }
            }

            return LoadSummariesData();
        }

        private void openSummary_button_Click(object sender, RoutedEventArgs e)
        {
            Summary summary = summary_dg.SelectedItem as Summary;
            Classes.Settings.mainFrame.Navigate(new Pages.Summary_page(Convert.ToInt32(summary.Id)));
        }

        private void summaryAdd_bt_Click(object sender, RoutedEventArgs e) => Classes.Settings.mainFrame.Navigate(new Pages.SummaryAdd_page());

        private void search_tb_SelectionChanged(object sender, RoutedEventArgs e)
        {
            //Доделать
            Summary[] summary = SortSummaryData();
            using (HrHelperDatabaseContext db = new HrHelperDatabaseContext())
            {
                summary = db.Summaries.Where(o =>
                EF.Functions.Like(o.FirstName, $"%{search_tb.Text}%") ||
                EF.Functions.Like(o.LastName, $"%{search_tb.Text}%") ||
                EF.Functions.Like(o.Patronymic, $"%{search_tb.Text}%")).ToArray();
                summary_dg.ItemsSource = summary;

                RowCountUpdate();
            }
        }

        private void RowCountUpdate() => allClients_tblock.Text = $"Всего - {summary_dg.Items.Count}";



        private void vacancyChange_comboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            summary_dg.ItemsSource = SortSummaryData();
            RowCountUpdate();
        }

        private void excelButtonChangeOpacity()
        {
            if (selectedSummary.Count > 0)
                excelExport_but.Opacity = 1;
            else
                excelExport_but.Opacity = 0.5;
        }
        private void excelExport_but_Click(object sender, RoutedEventArgs e)
        {
            if (selectedSummary.Count <= 0)
            {
                MyMessageBox.Show("Внимание", "Для того чтобы экспортировать данные в Excel, необходимо выбрать как минимум одну строку.");
                return;
            }

            DataGrid dataGrid = CopyDataGrid();
            dataGrid.AutoGenerateColumns = false;
            Data_grid.Children.Add(dataGrid);
            dataGrid.ItemsSource = selectedSummary;
            dataGrid.Visibility = Visibility.Hidden;
        }
        private DataGrid CopyDataGrid()
        {
            DataGrid newGrid = new DataGrid();

            // Копируем заголовки столбцов из существующего DataGrid

            for (int i = 0; i < summary_dg.Columns.Count; i++)
            {
                DataGridTextColumn column = summary_dg.Columns[i] as DataGridTextColumn;

                if (column == null)
                    continue;

                DataGridTextColumn newColumn = new DataGridTextColumn();

                newColumn.Header = column.Header != null ? (column.Header) : (null);
                newColumn.Binding = column.Binding;

                newGrid.Columns.Add(newColumn);
            }

            newGrid.Loaded += (sender, e) =>
            {
                newGrid.ItemsSource = selectedSummary;
                ExcelExport.ExportSummaryList(newGrid);
            };

            return newGrid;
        }

        private void selectSummary_cb_Click(object sender, RoutedEventArgs e)
        {
            CheckBox checkBox = (CheckBox)sender;
            if (checkBox.IsChecked == true)
            {
                selectedSummary.Add(summary_dg.SelectedItem as Summary);
            }
            else
            {
                selectedSummary.Remove(summary_dg.SelectedItem as Summary);
            }
            excelButtonChangeOpacity();
        }

        private void selectAll_cb_Click(object sender, RoutedEventArgs e)
        {
            CheckBox currentCheckB = (CheckBox)sender;
            if (currentCheckB.IsChecked == true)
            {
                foreach (var item in summary_dg.Items)
                {
                    selectedSummary.Add((Summary)item);

                    var row = summary_dg.ItemContainerGenerator.ContainerFromItem(item) as DataGridRow;
                    if (row != null)
                    {
                        var checkBox = FindVisualChild<CheckBox>(row);
                        if (checkBox != null && checkBox.Name == "selectSummary_cb")
                        {
                            checkBox.IsChecked = true;                            
                        }
                    }
                }

            }          
            else if (currentCheckB.IsChecked == false)
            {
                foreach (var item in summary_dg.Items)
                {
                    var row = summary_dg.ItemContainerGenerator.ContainerFromItem(item) as DataGridRow;
                    if (row != null)
                    {
                        var checkBox = FindVisualChild<CheckBox>(row);
                        if (checkBox != null && checkBox.Name == "selectSummary_cb")
                        {
                            checkBox.IsChecked = false;
                        }
                    }
                }
            }
            excelButtonChangeOpacity();
        }

        private T FindVisualChild<T>(DependencyObject obj) where T : DependencyObject
        {
            if (obj != null)
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(obj, i);
                    if (child != null && child is T)
                        return (T)child;
                    else
                    {
                        T childOfChild = FindVisualChild<T>(child);
                        if (childOfChild != null)
                            return childOfChild;
                    }
                }
            }
            return null;
        }
    }
}