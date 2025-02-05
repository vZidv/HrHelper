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
    /// Interaction logic for UserEdit_page.xaml
    /// </summary>
    public partial class UserEdit_page : Page
    {
        AuthorizationUser user { get; set; }
        public UserEdit_page(AuthorizationUser user)
        {
            InitializeComponent();

            this.user = user;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            LoadUserTypeComboBox();
            LoadUser(user);
        }
        // Функция длярузки данных пользователя в форму редактирования
        private void LoadUser(AuthorizationUser user)
        {
            // Заполняем поля формы данными пользователя
            login_tb.Text = user.Login;
            password_tb.Text = user.Password;
            userType_cb.Text = user.UserType.Type;
        }

        // Функция для загрузки типов пользователей в выпадающий список
        private void LoadUserTypeComboBox()
        {
            // Создаем контст базы данных
            using (HrHelperDatabaseContext db = new HrHelperDatabaseContext())
            {
                // Загружаем типы пользователей в выпадающий список        userType_cb.ItemsSource = db.UserTypes.ToArray();

                // Устанавливаем отображаемый путь для элементов выпадающего списка
                userType_cb.DisplayMemberPath = "Type";
            }
        }

        // Обработчик нажатия кнопки "Сохранить изменения"
        private void editUser_but_Click(object sender, RoutedEventArgs e)
        {
            // Создаем список контролов, которые должны быть заполнены
            List<Control> controls = new List<Control>() { login_tb, password_tb, userType_cb };

            // Проверяем, заполнены ли обязательные поля
            if (Classes.CheckValue.CheckElementNullValue(controls) == true)
            {
                // Если не заполнены, выводим сообщение об ошибке и выходим из функции
                MyMessageBox.Show("Ошибка", "Пожалуйста, заполните обязательные поля", true);
                return;
            }

            // Обновляем данные пользователя
            user.Login = login_tb.Text;
            user.Password = password_tb.Text;
            user.UserTypeId = (userType_cb.SelectedItem as UserType).Id;

            // Создаем контекст базы данных
            using (HrHelperDatabaseContext db = new HrHelperDatabaseContext())
            {
                // Обновляем данные пользователя в базе данных
                db.AuthorizationUsers.Update(user);
                db.SaveChanges();
            }

            // Выводим сообщение об успешном обновлении данных пользователя
            MyMessageBox.Show("Внимание", "Данные успешно обновлены!");

            // Закрываем окно редактирования пользователя
            Settings.mainWindow.Close();
        }

        // Обработчик нажатия кнопки "Не сохранять"
        private void dontSave_but_Click(object sender, RoutedEventArgs e)
        {
            // Закрываем окно редактирования пользователя без сохранения изменений
            Settings.mainWindow.Close();
        }
    }
}
