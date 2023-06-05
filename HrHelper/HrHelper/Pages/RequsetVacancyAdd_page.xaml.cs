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

        // Метод LoadBusynessComboBox загружает список должностей в ComboBox busyness_cb из базы данных
        private void LoadBusynessComboBox()
        {
            using (HrHelperDatabaseContext db = new HrHelperDatabaseContext())
            {
                busyness_cb.ItemsSource = db.Busynesses.ToArray();
                busyness_cb.DisplayMemberPath = "Type";
            }
        }

        // Обработчик события нажатия кнопки addRequestVacancy_but
        private void addRequestVacancy_but_Click(object sender, RoutedEventArgs e)
        {
            // Создает список элементов управления, которые должны быть заполнены пользователем
            List<Control> controls = new List<Control>() { jobTitle_tb, busyness_cb, department_tb };
            // Проверяет, заполнены ли обязательные поля
            if (Classes.CheckValue.CheckElementNullValue(controls) == true)
            {
                MyMessageBox.Show("Ошибка", "Пожалуйста, заполните обязательные поля", true);
                return;
            }

            // Создает новый объект VacancyRequest и заполняет его данными из формы
            VacancyRequest vacancyRequest = new VacancyRequest()
            {
                JobTitle = jobTitle_tb.Text,
                Description = description_tb.Text,
                Skills = skills_tb.Text,
                BusynessId = (busyness_cb.SelectedItem as Busyness).Id,
                Department = department_tb.Text,
                UserId = Settings.currentUser.Id
            };

            // Добавляет новый объект VacancyRequest в базу данных и сохраняет изменения
            using (HrHelperDatabaseContext db = new HrHelperDatabaseContext())
            {
                db.VacancyRequests.Add(vacancyRequest);
                db.SaveChanges();
            }

            // Показывает сообщение об успешном добавлении запроса и закрывает окно
            MyMessageBox.Show("Внимание", "Ваш запрос добавлен!");
            Settings.mainWindow.Close();
        }

       
    }
}
