using HrHelper.Classes;
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
    /// Interaction logic for RequestVacancy_page.xaml
    /// </summary>
    public partial class RequestVacancy_page : Page
    {
        VacancyRequest vacancyRequest { get; set; }
        public RequestVacancy_page(VacancyRequest vacancyRequest)
        {
            InitializeComponent();

            this.vacancyRequest = vacancyRequest;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            LoadRequestVacancy();
            ConfigureUserWindow(Settings.currentUser);
        }
        private void ConfigureUserWindow(AuthorizationUser user)
        {
            switch (user.UserType.Type)
            {
                case "admin":
                    buttonsForClient_st.Visibility = Visibility.Hidden;
                    buttonsForUser_st.Visibility = Visibility.Visible;

                    buttonsForClient_st.IsEnabled = false;
                    buttonsForUser_st.IsEnabled = true;
                    break;
                case "user":
                    buttonsForClient_st.Visibility = Visibility.Hidden;
                    buttonsForUser_st.Visibility = Visibility.Visible;

                    buttonsForClient_st.IsEnabled = false;
                    buttonsForUser_st.IsEnabled = true;
                    break;
                case "client":
                    if (vacancyRequest.UserId != Settings.currentUser.Id)
                    {
                        buttonsForClient_st.Visibility = Visibility.Hidden;
                        buttonsForUser_st.Visibility = Visibility.Hidden;

                        buttonsForClient_st.IsEnabled = false;
                        buttonsForUser_st.IsEnabled = false;
                        break;
                    }

                    buttonsForClient_st.Visibility = Visibility.Visible;
                    buttonsForUser_st.Visibility = Visibility.Hidden;

                    buttonsForClient_st.IsEnabled = true;
                    buttonsForUser_st.IsEnabled = false;
                    break;
            }
        }
        private void LoadRequestVacancy()
        {     
            jobTitle_tb.Text = vacancyRequest.JobTitle;
            description_tb.Text = vacancyRequest.Description;
            busyness_tb.Text = vacancyRequest.Busyness.Type;
            department_tb.Text = vacancyRequest.Department;
            login_tb.Text = vacancyRequest.User.Login;
            skills_tb.Text = vacancyRequest.Skills;
        }

        private void delete_but_Click(object sender, RoutedEventArgs e)
        {
            if (MyMessageBox.Show("Внимание", "Вы точно хотите удалить этот запрос?", MyMessageBoxOptions.YesNo) == false)
                return;

            using (HrHelperDatabaseContext db = new HrHelperDatabaseContext())
            {
                db.VacancyRequests.Remove(vacancyRequest);
                db.SaveChanges();
            }
            MyMessageBox.Show("Внимание", "Запрос успешно удалён!");
            Settings.mainWindow.Close();
        }

        private void vacancyCreate_but_Click(object sender, RoutedEventArgs e)
        {
            Vacancy vacancy;
            using (HrHelperDatabaseContext db = new HrHelperDatabaseContext())
            {
                vacancy = new Vacancy()
                {
                    JobTitle = jobTitle_tb.Text,
                    Description = description_tb.Text,
                    Skills = skills_tb.Text,
                    Busyness = db.Busynesses.Where(o => o.Type == busyness_tb.Text).First(),                   
                };
            }
            VacancyAdd_page vacancyAdd = new VacancyAdd_page();
            vacancyAdd.LoadVacancyData(vacancy);
            Settings.mainFrame.Navigate(vacancyAdd);

        }

        private void editRequestVacancy_but_Click(object sender, RoutedEventArgs e) => Settings.mainFrame.Navigate(new RequestVacancyEdit_page(vacancyRequest));



    }
}
