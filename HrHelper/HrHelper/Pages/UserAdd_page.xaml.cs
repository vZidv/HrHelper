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

        private void LoadUserTypeComboBox()
        {
            using (HrHelperDatabaseContext db = new HrHelperDatabaseContext())
            {
                userType_cb.ItemsSource = db.UserTypes.ToArray();
                userType_cb.DisplayMemberPath = "Type";
            }
        }

        private void addUser_but_Click(object sender, RoutedEventArgs e)
        {
            List<Control> controls = new List<Control>() { login_tb, password_tb, userType_cb };

            if (Classes.CheckValue.CheckElementNullValue(controls) == true)
            {
                MyMessageBox.Show("Ошибка", "Пожалуйста, заполните обязательные поля", true);
                return;
            }

            AuthorizationUser user = new AuthorizationUser()
            {
                Login = login_tb.Text,
                Password = password_tb.Text,
                UserTypeId = (userType_cb.SelectedItem as UserType).Id
            };

            using (HrHelperDatabaseContext db = new HrHelperDatabaseContext())
            {
                db.AuthorizationUsers.Add(user);
                db.SaveChanges();
            }
            Classes.MyMessageBox.Show("Внимание", "Пользователь добавлен!");
            Settings.mainWindow.Close();
        }


    }
}
