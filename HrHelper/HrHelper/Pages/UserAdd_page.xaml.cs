using HrHelper.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace HrHelper.Pages
{
    public partial class UserAdd_page : Page
    {
        public UserAdd_page()
        {
            InitializeComponent();
            
        }
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            LoadUserTypeComboBox();
        }

        // Функция для загрузки типов пользователей в выпадающий список
        private void LoadUserTypeComboBox()
        {
            // Создаем контекст базы данных
            using (HrHelperDatabaseContext db = new HrHelperDatabaseContext())
            {
                // Загружаем типы пользователей в выпадающий список
                userType_cb.ItemsSource = db.UserTypes.ToArray();

                // Устанавливаем отображаемый путь для элементов выпадающего списка
                userType_cb.DisplayMemberPath = "Type";
            }
        }

        // Обработчик нажатия кнопки "Добавить пользователя"
        private void addUser_but_Click(object sender, RoutedEventArgs e)
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

            // Создаем нового пользователя
            AuthorizationUser user = new AuthorizationUser()
            {
                Login = login_tb.Text,
                Password = password_tb.Text,
                UserTypeId = (userType_cb.SelectedItem as UserType).Id
            };

            // Создаем контекст базы данных
            using (HrHelperDatabaseContext db = new HrHelperDatabaseContext())
            {
                // Добавляем пользователя в базу данных
                db.AuthorizationUsers.Add(user);
                db.SaveChanges();
            }

            // Выводим сообщение об успешном добавлении пользователя
            Classes.MyMessageBox.Show("Внимание", "Пользователь добавлен!");

            // Закрываем окно добавления пользователя
            Settings.mainWindow.Close();
        }


    }
}
