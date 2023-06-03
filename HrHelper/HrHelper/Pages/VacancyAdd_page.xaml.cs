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
        public Vacancy vacancy {get;set;}

        public VacancyAdd_page()
        {
            InitializeComponent();
        }
        public void LoadVacancyData(Vacancy vacancy)
        {
            if (vacancy == null)
                return;


            jobTitle_tb.Text = vacancy.JobTitle;
            description_tb.Text = vacancy.Description;
            skills_tb.Text = vacancy.Skills;
            busyness_cb.Text = vacancy.Busyness.Type;

            if (minSalary_tb != null)
                minSalary_tb.Text = vacancy.MinSalary.ToString();

            if (maxSalary_tb != null)
                maxSalary_tb.Text = vacancy.MaxSalary.ToString();           
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
            List<Control> controls = new List<Control>() {jobTitle_tb,busyness_cb,maxSalary_tb,minSalary_tb};
            if (Classes.CheckValue.CheckElementNullValue(controls) == true)
            {
                MyMessageBox.Show("Ошибка", "Пожалуйста, заполните обязательные поля", true);
                return;
            }

            Busyness busyness = busyness_cb.SelectedItem as Busyness;
            if(busyness == null)
            {
                using (HrHelperDatabaseContext db = new HrHelperDatabaseContext())
                {
                    busyness = db.Busynesses.Where(o => o.Type == busyness_cb.Text).First();
                }
            }
               
            Vacancy vacancy = new Vacancy()
            {
                JobTitle = jobTitle_tb.Text,
                Description = description_tb.Text,
                Skills= skills_tb.Text,
                BusynessId = busyness.Id,
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
