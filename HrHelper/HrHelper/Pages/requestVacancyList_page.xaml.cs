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
    /// Interaction logic for requestVacancy_page.xaml
    /// </summary>
    public partial class RequestVacancyList_page : Page
    {
        public RequestVacancyList_page()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            LoadDataGrid();
            ConfigureUserWindow(Settings.currentUser);
        }

        // Метод ConfigureUserWindow настраивает окно пользователя в зависимости от его типа
        private void ConfigureUserWindow(AuthorizationUser user)
        {
            switch (user.UserType.Type)
            {
                case "admin":
                    requestVacancyAdd_but.Visibility = Visibility.Visible;
                    requestVacancyAdd_but.IsEnabled = true;
                    break;
                case "user":
                    requestVacancyAdd_but.Visibility = Visibility.Hidden;
                    requestVacancyAdd_but.IsEnabled = false;
                    break;
                case "client":
                    requestVacancyAdd_but.Visibility = Visibility.Visible;
                    requestVacancyAdd_but.IsEnabled = true;
                    break;
            }
        }

        // Метод LoadDataGrid загружает данные в таблицу vacancy_dg из базы данных
        private void LoadDataGrid()
        {
            using (HrHelperDatabaseContext db = new HrHelperDatabaseContext())
            {
                VacancyRequest[] vacancyRequests = db.VacancyRequests.Include(o => o.Busyness).Include(o => o.User).ToArray();
                vacancy_dg.ItemsSource = vacancyRequests;
            }
            RowCountUpdate();
        }

        // Метод RowCountUpdate обновляет количество строк в таблице vacancy_dg
        private void RowCountUpdate() => allClients_tblock.Text = $"Всего - {vacancy_dg.Items.Count}";

        // Обработчик события нажатия кнопки requestVacancyAdd_but
        private void requestVacancyAdd_but_Click(object sender, RoutedEventArgs e)
        {
            // Открывает окно MinWin_win с страницей RequsetVacancyAdd_page и ожидает закрытия окна
            new MinWin_win(new Pages.RequsetVacancyAdd_page()).ShowDialog();
            LoadDataGrid();
        }

        // Обработчик события нажатия кнопки delete_button
        private void delete_button_Click(object sender, RoutedEventArgs e)
        {
            // Получает выбранный элемент таблицы vacancy_dg
            VacancyRequest vacancyRequest = vacancy_dg.SelectedItem as VacancyRequest;
            // Проверяет, хочет ли пользователь удалить выбранный элемент
            if (MyMessageBox.Show("Внимание", "Вы точно хотите удалить этот запрос?", MyMessageBoxOptions.YesNo) == false)
                return;

            // Удаляет выбранный элемент из базы данных
            using (HrHelperDatabaseContext db = new HrHelperDatabaseContext())
            {
                db.VacancyRequests.Remove(vacancyRequest);
                db.SaveChanges();
            }

            // Показывает сообщение об успешном удалении элемента и обновляет таблицу vacancy_dg
            MyMessageBox.Show("Внимание", "Запрос успешно удален!");
            LoadDataGrid();
        }

        // Обработчик события нажатия кнопки openRequestVacancy_button
        private void openRequestVacancy_button_Click(object sender, RoutedEventArgs e)
        {
            // Получает выбранный элемент таблицы vacancy_dg
            VacancyRequest vacancyRequest = vacancy_dg.SelectedItem as VacancyRequest;

            // Открывает окно MinWin_win с страницей RequestVacancy_page и ожидает закрытия окна
            new MinWin_win(new Pages.RequestVacancy_page(vacancyRequest)).ShowDialog();
            LoadDataGrid();
        }

        private void search_tb_SelectionChanged(object sender, RoutedEventArgs e)
        {
            using (HrHelperDatabaseContext db = new HrHelperDatabaseContext())
            {
                // Фильтруем данные
                VacancyRequest[] vacancies = db.VacancyRequests.Where(o =>
                EF.Functions.Like(o.JobTitle, $"%{search_tb.Text}%")).Include(o => o.Busyness).Include(o => o.User).ToArray();

                // Обновляем таблицу с данными 
                vacancy_dg.ItemsSource = vacancies;

                // Обновляем количество строк в таблице
                RowCountUpdate();
            }
        }
    }
}
