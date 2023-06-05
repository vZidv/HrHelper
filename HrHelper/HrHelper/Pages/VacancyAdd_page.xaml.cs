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
            // Проверяем, что вакансия не равна null
            if (vacancy == null)
                return;

            // Заполняем поля формы данными из объекта вакансии
            jobTitle_tb.Text = vacancy.JobTitle;
            description_tb.Text = vacancy.Description;
            skills_tb.Text = vacancy.Skills;
            busyness_cb.Text = vacancy.Busyness.Type;

            // Проверяем, что поля для зарплаты не равны null, и заполняем их данными из объекта вакансии
            if (minSalary_tb != null)
                minSalary_tb.Text = vacancy.MinSalary.ToString();

            if (maxSalary_tb != null)
                maxSalary_tb.Text = vacancy.MaxSalary.ToString();
        }

        // Обработчик события загрузки страницы
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            // Загружаем список типов занятости в комбобокс
            LoadBusynessComboBox();
        }

        // Функция для загрузки списка типов занятости в комбобокс
        private void LoadBusynessComboBox()
        {
            // Создаем контекст базы данных
            using (HrHelperDatabaseContext db = new HrHelperDatabaseContext())
            {
                // Заполняем комбобокс данными из базы данных
                busyness_cb.ItemsSource = db.Busynesses.ToArray();
                busyness_cb.DisplayMemberPath = "Type";
            }
        }

        // Обработчики событий ввода текста в поля для зарплаты
        private void minSalary_tb_PreviewTextInput(object sender, TextCompositionEventArgs e) => OnlyNumberInput(e);
        private void maxSalary_tb_PreviewTextInput(object sender, TextCompositionEventArgs e) => OnlyNumberInput(e);

        // Функция для проверки, что введенный текст является числом
        private void OnlyNumberInput(TextCompositionEventArgs e)
        {
            if (!char.IsDigit(e.Text, e.Text.Length - 1))
            {
                e.Handled = true;
            }
        }

        // Обработчик нажатия кнопки "Добавить вакансию"
        private void addVacancy_but_Click(object sender, RoutedEventArgs e)
        {
            // Создаем список контролов, которые должны быть заполнены
            List<Control> controls = new List<Control>() { jobTitle_tb, busyness_cb, maxSalary_tb, minSalary_tb };

            // Проверяем, что все обязательные поля заполнены
            if (Classes.CheckValue.CheckElementNullValue(controls) == true)
            {
                MyMessageBox.Show("Ошибка", "Пожалуйста, заполните обязательные поля", true);
                return;
            }

            // Получаем выбранный тип занятости
            Busyness busyness = busyness_cb.SelectedItem as Busyness;

            // Если тип занятости не выбран, ищем его в базе данных
            if (busyness == null)
            {
                using (HrHelperDatabaseContext db = new HrHelperDatabaseContext())
                {
                    busyness = db.Busynesses.Where(o => o.Type == busyness_cb.Text).First();
                }
            }

            // Создаем объект вакансии
            Vacancy vacancy = new Vacancy()
            {
                JobTitle = jobTitle_tb.Text,
                Description = description_tb.Text,
                Skills = skills_tb.Text,
                BusynessId = busyness.Id,
                MaxSalary = Convert.ToDecimal(maxSalary_tb.Text),
                MinSalary = Convert.ToDecimal(minSalary_tb.Text)

            };

            // Добавляем вакансию в базу данных
            using (HrHelperDatabaseContext db = new HrHelperDatabaseContext())
            {
                db.Vacancies.Add(vacancy);
                db.SaveChanges();
            }

            // Выводим сообщение об успешном добавлении вакансии
            MyMessageBox.Show("Внимание", "Вакансия добавлена!");

            // Закрываем окно
            Settings.mainWindow.Close();
        }
    }
}
