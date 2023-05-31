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
        private void LoadDataGrid()
        {
            using (HrHelperDatabaseContext db = new HrHelperDatabaseContext())
            {
                VacancyRequest[] vacancyRequests = db.VacancyRequests.Include(o => o.Busyness).Include(o => o.User).ToArray();
                vacancy_dg.ItemsSource = vacancyRequests;
            }
            RowCountUpdate();
        }
        private void RowCountUpdate() => allClients_tblock.Text = $"Всего - {vacancy_dg.Items.Count}";

        private void requestVacancyAdd_but_Click(object sender, RoutedEventArgs e)
        {
            new MinWin_win(new Pages.RequsetVacancyAdd_page()).ShowDialog();
            LoadDataGrid();
        }

        private void delete_button_Click(object sender, RoutedEventArgs e)
        {
            VacancyRequest vacancyRequest = vacancy_dg.SelectedItem as VacancyRequest;
            if (MyMessageBox.Show("Внимание", "Вы точно хотите удалить этот запрос?", MyMessageBoxOptions.YesNo) == false)
                return;

            using (HrHelperDatabaseContext db = new HrHelperDatabaseContext())
            {
                db.VacancyRequests.Remove(vacancyRequest);
                db.SaveChanges();
            }

            MyMessageBox.Show("Внимание", "Запрос успешно удален!");
            LoadDataGrid();
        }

        private void openRequestVacancy_button_Click(object sender, RoutedEventArgs e)
        {
            VacancyRequest vacancyRequest = vacancy_dg.SelectedItem as VacancyRequest;

            new MinWin_win(new Pages.RequestVacancy_page(vacancyRequest)).ShowDialog();
            LoadDataGrid();
        }
    }
}
