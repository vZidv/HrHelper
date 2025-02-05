using HrHelper.Classes;
using HrHelper.Windows;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace HrHelper.Pages
{
    /// <summary>
    /// Interaction logic for VacancyList_page.xaml
    /// </summary>
    public partial class VacancyList_page : Page
    {
        public VacancyList_page()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            LoadDataGrid();
        }
        // Функция для загрузки данных в таблицу
        private void LoadDataGrid()
        {
            // Создаем контекст базы данных
            using (HrHelperDatabaseContext db = new HrHelperDatabaseContext())
            {
                // Получаем список вакансий из базы данных, включая связанные данные
                Vacancy[] vacancy = db.Vacancies.Include(o => o.Busyness).ToArray();
                // Заполняем таблицу данными из списка вакансий
                vacancy_dg.ItemsSource = vacancy;
            }
            // Обновляем количество строк в таблице
            RowCountUpdate();
        }

        // Функция для обновления количества строк в таблице
        private void RowCountUpdate() => allClients_tblock.Text = $"Всего - {vacancy_dg.Items.Count}";

        // Обработчик нажатия кнопки "Добавить вакансию"
        private void vacancyAdd_but_Click(object sender, RoutedEventArgs e)
        {
            // Открываем окно добавления вакансии
            new MinWin_win(new VacancyAdd_page()).ShowDialog();
            // Обновляем таблицу
            LoadDataGrid();
        }

        // Обработчик нажатия кнопки "Открыть вакансию"
        private void openVacancy_button_Click(object sender, RoutedEventArgs e)
        {
            // Получаем выбранную вакансию из таблицы
            Vacancy vacancy = vacancy_dg.SelectedItem as Vacancy;
            // Открываем окно с информацией о вакансии
            new MinWin_win(new Vacancy_page(vacancy)).ShowDialog();

            // Обновляем таблицу
            LoadDataGrid();
        }

        // Обработчик нажатия кнопки "Удалить вакансию"
        private void delete_button_Click(object sender, RoutedEventArgs e)
        {
            // Получаем выбранную ваксию из таблицы
            Vacancy vacancy = vacancy_dg.SelectedItem as Vacancy;
            // Проверяем, точно ли пользователь хочет удалить вакансию
            if (MyMessageBox.Show("Внимание", "Вы точно хотите удалить эту вакансию?", MyMessageBoxOptions.YesNo) == false)
        return;

            // Создаем контексты данных
            using (HrHelperDatabaseContext db = new HrHelperDatabaseContext())
            {
                // Получаем список связанных данных для вакансии
                SummaryForVacancy[] summaryForVacancy = db.SummaryForVacancies.Where(o => o.VacancyId == vacancy.Id).ToArray();
                // Удаляем связанные данные
                db.SummaryForVacancies.RemoveRange(summaryForVacancy);
                // Удаляем вакансию
                db.Vacancies.Remove(vacancy);
                // Сохраняем изменения в базе данных
                db.SaveChanges();
            }

            // Выводим сообщение об успешном удалении вакансии
            MyMessageBox.Show("Внимание", "Вакансия успешно удалена!");
            // Обновляем таблицу
            LoadDataGrid();
        }

        private void search_tb_SelectionChanged(object sender, RoutedEventArgs e)
        {
            using (HrHelperDatabaseContext db = new HrHelperDatabaseContext())
            {
                // Фильтруем данные
                Vacancy[] vacancies = db.Vacancies.Where(o =>
                EF.Functions.Like(o.JobTitle, $"%{search_tb.Text}%")).Include(o => o.Busyness).ToArray();

                // Обновляем таблицу с данными 
                vacancy_dg.ItemsSource = vacancies;

                // Обновляем количество строк в таблице
                RowCountUpdate();
            }
        }
    }
    
}
