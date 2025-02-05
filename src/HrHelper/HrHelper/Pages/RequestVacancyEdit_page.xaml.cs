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
    /// Interaction logic for RequestVacancyEdit_page.xaml
    /// </summary>
    public partial class RequestVacancyEdit_page : Page
    {
        VacancyRequest vacancyRequest { get; set; }
        public RequestVacancyEdit_page(VacancyRequest vacancyRequest)
        {
            InitializeComponent();
            this.vacancyRequest = vacancyRequest;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            LoadBusynessComboBox();
            LoadRequestVacancy();
        }
        // Загружаем список типов занятости в комбобокс
        private void LoadBusynessComboBox()
        {
            using (HrHelperDatabaseContext db = new HrHelperDatabaseContext())
            {
                busyness_cb.ItemsSource = db.Busynesses.ToArray();
                busyness_cb.DisplayMemberPath = "Type";
            }
        }

        // Загружаем данные запроса в соответствующие поля
        private void LoadRequestVacancy()
        {
            jobTitle_tb.Text = vacancyRequest.JobTitle;
            description_tb.Text = vacancyRequest.Description;
            busyness_cb.Text = vacancyRequest.Busyness.Type;
            department_tb.Text = vacancyRequest.Department;
            login_tb.Text = vacancyRequest.User.Login;
            skills_tb.Text = vacancyRequest.Skills;
        }

        // Обработчик кнопки "Не сохранять запрос"
        private void dontSaveRequestVacancy_but_Click(object sender, RoutedEventArgs e)
        {
            Classes.Settings.mainFrame.GoBack();
        }

        // Обработчик кнопки "Сохранить запрос"
        private void saveRequestVacancy_but_Click(object sender, RoutedEventArgs e)
        {
            // Проверяем, заполнены ли обязательные поля
            List<Control> controls = new List<Control>() { jobTitle_tb, busyness_cb, department_tb };
            if (Classes.CheckValue.CheckElementNullValue(controls) == true)
            {
                MyMessageBox.Show("Ошибка", "Пожалуйста, заполните обязательные поля", true);
                return;
            }

            // Обновляем данные запроса в базе данных
            vacancyRequest.JobTitle = jobTitle_tb.Text;
            vacancyRequest.Description = description_tb.Text;
            vacancyRequest.Skills = skills_tb.Text;
            vacancyRequest.BusynessId = (busyness_cb.SelectedItem as Busyness).Id;
            vacancyRequest.Department = department_tb.Text;
            vacancyRequest.UserId = Settings.currentUser.Id;

            using (HrHelperDatabaseContext db = new HrHelperDatabaseContext())
            {
                db.VacancyRequests.Update(vacancyRequest);
                db.SaveChanges();
            }

            // Показываем сообщение об успешном обновлении и возвращаемся на предыдущую страницу
            MyMessageBox.Show("Внимание", "Ваш запрос обновлен!");
            Settings.mainFrame.GoBack();
        }
    }
}
