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
        // Функция для загрузки данных вакансии в форму
        private void LoadVacancy(Vacancy vacancy)
        {
            // Заполняем поля формы данными из объекта вакансии
            jobTitle_tb.Text = vacancy.JobTitle;
            description_tb.Text = vacancy.Description;
            skills_tb.Text = vacancy.Skills;
            busyness_tb.Text = vacancy.Busyness.Type;
            minSalary_tb.Text = vacancy.MinSalary.ToString();
            maxSalary_tb.Text = vacancy.MaxSalary.ToString();
        }

        // Обработчик нажатия кнопки "Удалить вакансию"
        private void deleteVacancy_but_Click(object sender, RoutedEventArgs e)
        {
            // Проверяем, точно ли пользователь хочет удалить выбранную вакансию
            if (MyMessageBox.Show("Внимание", "Вы точно хотите удалить эту вакансию?", MyMessageBoxOptions.YesNo) == false)
                return;

            // Создаем контекст базы данных
            using (HrHelperDatabaseContext db = new HrHelperDatabaseContext())
            {
                // Удаляем вакансию из базы данных
                db.Vacancies.Remove(vacancy);
                db.SaveChanges();
            }

            // Выводим сообщение об успешном удалении вакансии
            MyMessageBox.Show("Внимание", "Вакансия успешно удалена!");

            // Закрываем окно
            Settings.mainWindow.Close();
        }

        // Обработчик нажатия кнопки "Редактировать вакансию"
        private void editVacancy_but_Click(object sender, RoutedEventArgs e)
        {
            // Переходим на страницу редактирования вакансии и передаем выбранную вакансию в качестве параметра
            Settings.mainFrame.Navigate(new VacancyEdit_page(vacancy));
        }
    }
}
