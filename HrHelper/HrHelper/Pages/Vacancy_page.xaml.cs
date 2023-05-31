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
    /// Interaction logic for Vacancy_page.xaml
    /// </summary>
    public partial class Vacancy_page : Page
    {
        Vacancy vacancy { get; set; }
        public Vacancy_page(Vacancy vacancy)
        {
            InitializeComponent();

            this.vacancy = vacancy;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            LoadVacancy(vacancy);
        }
        private void LoadVacancy(Vacancy vacancy)
        {
            jobTitle_tb.Text = vacancy.JobTitle;
            description_tb.Text = vacancy.Description;
            skills_tb.Text = vacancy.Skills;
            busyness_tb.Text = vacancy.Busyness.Type;
            minSalary_tb.Text = vacancy.MinSalary.ToString();
            maxSalary_tb.Text = vacancy.MaxSalary.ToString();
        }

        private void deleteVacancy_but_Click(object sender, RoutedEventArgs e)
        {
            if (MyMessageBox.Show("Внимание", "Вы точно хотите удалить эту вакансию?", MyMessageBoxOptions.YesNo) == false)
                return;

            using(HrHelperDatabaseContext db = new HrHelperDatabaseContext())
            {
                db.Vacancies.Remove(vacancy);
                db.SaveChanges();
            }
            MyMessageBox.Show("Внимание", "Вакансия успешно удалена!");
            Settings.mainWindow.Close();
        }

        private void editVacancy_but_Click(object sender, RoutedEventArgs e)
        {
            Settings.mainFrame.Navigate(new VacancyEdit_page(vacancy));
        }
    }
}
