using HrHelper.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Interaction logic for VacancyEdit_page.xaml
    /// </summary>
    public partial class VacancyEdit_page : Page
    {
        Vacancy vacancy { get; set; }
        public VacancyEdit_page(Vacancy vacancy)
        {
            InitializeComponent();
            this.vacancy = vacancy;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            LoadVacancy(vacancy);
            LoadBusynessComboBox();
        }
        private void LoadVacancy(Vacancy vacancy)
        {
            jobTitle_tb.Text = vacancy.JobTitle;
            description_tb.Text = vacancy.Description;
            skills_tb.Text = vacancy.Skills;
            busyness_cb.Text = vacancy.Busyness.Type;
            minSalary_tb.Text = vacancy.MinSalary.ToString();
            maxSalary_tb.Text = vacancy.MaxSalary.ToString();
        }

        private void minSalary_tb_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void maxSalary_tb_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void dontEditVacancy_but_Click(object sender, RoutedEventArgs e)
        {
            Settings.mainFrame.GoBack();
        }
        private void LoadBusynessComboBox()
        {
            using (HrHelperDatabaseContext db = new HrHelperDatabaseContext())
            {
                busyness_cb.ItemsSource = db.Busynesses.ToArray();
                busyness_cb.DisplayMemberPath = "Type";
            }
        }

        private void saveVacancy_but_Click(object sender, RoutedEventArgs e)
        {
            List<Control> controls = new List<Control>() { jobTitle_tb, busyness_cb, maxSalary_tb, minSalary_tb };
            if (Classes.CheckValue.CheckElementNullValue(controls) == true)
            {
                MyMessageBox.Show("Ошибка", "Пожалуйста, заполните обязательные поля", true);
                return;
            }

            Busyness busyness = busyness_cb.SelectedItem as Busyness;
            if (busyness == null)
            {
                using (HrHelperDatabaseContext db = new HrHelperDatabaseContext())
                {
                    busyness = db.Busynesses.Where(o => o.Type == busyness_cb.Text).First();
                }
            }


            vacancy.JobTitle = jobTitle_tb.Text;
            vacancy.Description = description_tb.Text;
            vacancy.Skills = skills_tb.Text;
            vacancy.BusynessId = busyness.Id;
            vacancy.MaxSalary = Convert.ToDecimal(maxSalary_tb.Text);
            vacancy.MinSalary = Convert.ToDecimal(minSalary_tb.Text);


            using (HrHelperDatabaseContext db = new HrHelperDatabaseContext())
            {
                db.Vacancies.Update(vacancy);
                db.SaveChanges();
            }

            MyMessageBox.Show("Внимание", "Вакансия Обновлена!");
            Settings.mainFrame.GoBack();
        }
    }
}
