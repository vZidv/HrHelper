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
        // Функция для сортировки данных кандидатов
        private Summary[] SortSummaryData()
        {
            Summary[] summaries;

            // Создаем контекст базы данных
            using (HrHelperDatabaseContext db = new HrHelperDatabaseContext())
            {
                // Если выбрана конкретная вакансия, то выбираем кандидатов, связанных с этой вакансией
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

            // Если вакансия не выбрана, то загружаем все данные кандидатов
            return LoadSummariesData();
        }

        // Обработчик нажатия кнопки "Открыть кандидата"
        private void openSummary_button_Click(object sender, RoutedEventArgs e)
        {
            // Получаем выбранный элемент таблицы
            Summary summary = summary_dg.SelectedItem as Summary;

            // Переходим на страницу с информацией о кандидате
            Classes.Settings.mainFrame.Navigate(new Pages.Summary_page(Convert.ToInt32(summary.Id)));
        }

        // Обработчик нажатия кнопки "Добавить кандидата"
        private void summaryAdd_bt_Click(object sender, RoutedEventArgs e) => Classes.Settings.mainFrame.Navigate(new Pages.SummaryAdd_page());

        // Обработчик изменения текста в поле поиска
        private void search_tb_SelectionChanged(object sender, RoutedEventArgs e)
        {
            // Получаем отсортированные данные кандидатов
            Summary[] summary = SortSummaryData();

            // Создаем контекст базы данных
            using (HrHelperDatabaseContext db = new HrHelperDatabaseContext())
            {
                // Фильтруем данные кандидатов по имени, фамилии и отчеству
                summary = db.Summaries.Where(o =>
                EF.Functions.Like(o.FirstName, $"%{search_tb.Text}%") ||
                EF.Functions.Like(o.LastName, $"%{search_tb.Text}%") ||
                EF.Functions.Like(o.Patronymic, $"%{search_tb.Text}%")).ToArray();

                // Обновляем таблицу с данными кандидатов
                summary_dg.ItemsSource = summary;

                // Обновляем количество строк в таблице
                RowCountUpdate();
            }
        }

        // Функция для обновления количества строк в таблице
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
            // Получаем текущий CheckBox
            CheckBox currentCheckB = (CheckBox)sender;

            // Если CheckBox выбран, то выбираем все элементы в таблице
            if (currentCheckB.IsChecked == true)
            {
                // Очищаем список выбранных элементов
                selectedSummary.Clear();

                // Проходим по всем элементам в таблице
                foreach (var item in summary_dg.Items)
                {
                    // Добавляем элемент в список выбранных элементов
                    selectedSummary.Add((Summary)item);

                    // Получаем строку таблицы
                    var row = summary_dg.ItemContainerGenerator.ContainerFromItem(item) as DataGridRow;

                    // Если строка не пустая, то находим CheckBox в строке и выбираем его
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
            // Если CheckBox не выбран, то снимаем выбор со всех элементов в таблице
            else if (currentCheckB.IsChecked == false)
            {
                // Очищаем список выбранных элементов
                selectedSummary.Clear();

                // Проходим по всем элемент в таблице
                foreach (var item in summary_dg.Items)
                {
                    // Получаем строку таблицы
                    var row = summary_dg.ItemContainerGenerator.ContainerFromItem(item) as DataGridRow;

                    // Если строка не пустая, то находим CheckBox в строке и снимаем его выбор
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

            // Обновляем прозрачность кнопки экспорта в Excel
            excelButtonChangeOpacity();
        }

        // Функция для поиска дочернего элемента заданного типа
        T FindVisualChild<T>(DependencyObject obj) where T : DependencyObject
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

        private void delete_button_Click(object sender, RoutedEventArgs e)
        {
            // Получаем выбранный элемент таблицы
            Summary summary = summary_dg.SelectedItem as Summary;

            // Показываем диалоговое окно для подтверждения удаления
            if (MyMessageBox.Show("Внимание", "Вы точно хотите удалить этого кандидата?", MyMessageBoxOptions.YesNo) == false)
                return;
            try
            {
                // Удаляем выбранный элемент из базы данных
                using (HrHelperDatabaseContext db = new HrHelperDatabaseContext())
                {
                    SummaryForVacancy[] summaryForVacancy = db.SummaryForVacancies.Where(o => o.SummaryId == summary.Id).ToArray();
                    if (summaryForVacancy.Length > 0)
                        db.SummaryForVacancies.RemoveRange(summaryForVacancy);
                    db.SaveChanges();
                    summary = db.Summaries.Where(o => o.Id == summary.Id).First();

                    db.Summaries.Remove(summary);
                    db.SaveChanges();
                }

                // Показываем сообщение об успешном удалении элемента
                MyMessageBox.Show("Внимание", "Кандидат успешно удален!");

                // Обновляем таблицу
                summary_dg.ItemsSource = LoadSummariesData();

                // Обновляем количество строк в таблице
                RowCountUpdate();
            }
            catch (Exception ex)
            {               
                MyMessageBox.Show("Ошибка", $"Не удалось удалить кандидата: {ex.Message}");
            }
        }
    }
}