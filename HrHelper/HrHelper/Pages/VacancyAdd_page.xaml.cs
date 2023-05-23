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
    /// Interaction logic for VacancyAdd_page.xaml
    /// </summary>
    public partial class VacancyAdd_page : Page
    {
        public VacancyAdd_page()
        {
            InitializeComponent();
        }


        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            LoadBusynessComboBox();
        }
        private void LoadBusynessComboBox()
        {
            using (HrHelperDatabaseContext db = new HrHelperDatabaseContext())
            {
                busyness_cb.ItemsSource = db.Busynesses.ToArray();
                busyness_cb.DisplayMemberPath = "Type";
            }
        }

        private void minSalary_tb_PreviewTextInput(object sender, TextCompositionEventArgs e) => OnlyNumberInput(e);
        private void maxSalary_tb_PreviewTextInput(object sender, TextCompositionEventArgs e) => OnlyNumberInput(e);


        private void OnlyNumberInput(TextCompositionEventArgs e)
        {
            if (!char.IsDigit(e.Text, e.Text.Length - 1))
            {
                e.Handled = true;
            }
        }

        private void addVacancy_but_Click(object sender, RoutedEventArgs e)
        {
            Vacancy vacancy = new Vacancy()
            {
                JobTitle = jobTitle_tb.Text,
                Description = description_tb.Text,
                Skills= skills_tb.Text,
                BusynessId = (busyness_cb.SelectedItem as Busyness).Id,
                MaxSalary =Convert.ToDecimal( maxSalary_tb.Text),
                MinSalary = Convert.ToDecimal(minSalary_tb.Text)

            };
            using (HrHelperDatabaseContext db = new HrHelperDatabaseContext())
            {
                db.Vacancies.Add(vacancy);
                db.SaveChanges();
            }

            MyMessageBox.Show("Внимание", "Вакансия добавлена!");
            Settings.mainWindow.Close();
        }
    }
}
