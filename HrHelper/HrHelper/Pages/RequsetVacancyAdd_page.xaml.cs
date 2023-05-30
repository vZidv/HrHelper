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
    /// Interaction logic for RequsetVacancyAdd_page.xaml
    /// </summary>
    public partial class RequsetVacancyAdd_page : Page
    {
        public RequsetVacancyAdd_page()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            LoadBusynessComboBox();
            login_tb.Text = Settings.currentUser.Login;
        }
        private void LoadBusynessComboBox()
        {
            using (HrHelperDatabaseContext db = new HrHelperDatabaseContext())
            {
                busyness_cb.ItemsSource = db.Busynesses.ToArray();
                busyness_cb.DisplayMemberPath = "Type";
            }
        }

        private void addRequestVacancy_but_Click(object sender, RoutedEventArgs e)
        {
            VacancyRequest vacancyRequest = new VacancyRequest()
            {
                Name = jobTitle_tb.Text,
                Description = description_tb.Text,
                Skills = skills_tb.Text,
                BusynessId = (busyness_cb.SelectedItem as Busyness).Id,
                Department = department_tb.Text,
                UserId = Settings.currentUser.Id

            };
            using (HrHelperDatabaseContext db = new HrHelperDatabaseContext())
            {
                db.VacancyRequests.Add(vacancyRequest);
                db.SaveChanges();
            }

            MyMessageBox.Show("Внимание", "Ваш запрос добавлен!");
            Settings.mainWindow.Close();
        }
    }
}
