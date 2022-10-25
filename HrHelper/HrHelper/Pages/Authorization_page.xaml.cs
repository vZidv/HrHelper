using HrHelper.Windows;
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
    /// Interaction logic for Authorization_page.xaml
    /// </summary>
    public partial class Authorization_page : Page
    {
        public Window authorization_win;

        public Authorization_page()
        {
            InitializeComponent();
        }

        private void Authorization_but_Click(object sender, RoutedEventArgs e)
        {         
            if (login_textbox.Text == String.Empty || password_pb.Password == String.Empty)
            {
                Classes.MyMessageBox.Show("Ошибка","Поле логин или пароль пустое!");
                return;
            }
            using (HrHelperDatabaseContext db = new HrHelperDatabaseContext())
            {
                AuthorizationUser user = db.AuthorizationUsers.Where(u => u.Login == login_textbox.Text).FirstOrDefault();

                if (user == null || password_pb.Password != user.Password)
                {
                    MessageBox.Show("Неверный логин или пароль!");
                    return;
                }
                if (password_pb.Password != user.Password)
                {
                    Classes.MyMessageBox.Show("Ошибка", "Неверный пароль!");
                }
                user.TypeNavigation = db.UserTypes.Where(o => o.Id == user.Type).First();

                switch (user.TypeNavigation.Type)
                {
                    case "admin":
                        Admin_win admin = new Admin_win();
                        admin.Show();
                        break;
                    case "user":
                        Main_win main_win = new Main_win();
                        main_win.Show();
                        break;
                }
                authorization_win.Close();
            }
        }

        private void password_pb_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (password_pb.Password.Length > 0)
                password_textblock.Visibility = Visibility.Collapsed;
            else
                password_textblock.Visibility = Visibility.Visible;
        }
    }
}
