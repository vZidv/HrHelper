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
        private void LoadUser(AuthorizationUser user)
        {
            login_tb.Text = user.Login;
            password_tb.Text = user.Password;
            userType_cb.Text = user.TypeNavigation.Type;
        }
        private void LoadUserTypeComboBox()
        {
            using (HrHelperDatabaseContext db = new HrHelperDatabaseContext())
            {
                userType_cb.ItemsSource = db.UserTypes.ToArray();
                userType_cb.DisplayMemberPath = "Type";
            }
        }

        private void editUser_but_Click(object sender, RoutedEventArgs e)
        {
            user.Login = login_tb.Text;
            user.Password = password_tb.Text;
            user.Type = (userType_cb.SelectedItem as UserType).Id;

            using (HrHelperDatabaseContext db = new HrHelperDatabaseContext())
            {
                db.AuthorizationUsers.Update(user);
                db.SaveChanges();
            }
            MyMessageBox.Show("Внимание", "Данные успешно обновлены!");
            Settings.mainWindow.Close();
        }
    }
}
