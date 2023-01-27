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
            LoadStatusComboBox();
        }

        private void LoadStatusComboBox()
        {
            using (HrHelperDatabaseContext db = new HrHelperDatabaseContext())
            {
                UserType[] types = db.UserTypes.ToArray();
                foreach (UserType type in types)
                {
                    userType_cb.Items.Add(type.Type);
                }
            }
        }

        private void userAdd_but_Click(object sender, RoutedEventArgs e)
        {
            if(login_tb.Text == String.Empty || password_tb.Text == String.Empty)
            {
                Classes.MyMessageBox.Show("Ошибка", "Одно из полей пустое!");
                return;
            }   
            
            int userType = 0;
            using (HrHelperDatabaseContext db = new HrHelperDatabaseContext())
            {
                if (userType_cb.SelectedIndex == -1)
                    userType = 2;
                else
                {
                    UserType[] types = db.UserTypes.ToArray();
                    foreach (UserType type in types)
                    {
                        if (type.Type == userType_cb.Text)
                            userType = type.Id;
                    }
                }              

                AuthorizationUser user = new AuthorizationUser()
                {
                    Login = login_tb.Text,
                    Password = password_tb.Text,
                    Type = userType
                };
                db.AuthorizationUsers.Add(user);
                db.SaveChanges();
            }
            Classes.MyMessageBox.Show("Внимание", "Пользователь добавлен!");
            Classes.Settings.mainFrame.Navigate(new Pages.Admin_page());
        }
    }
}
