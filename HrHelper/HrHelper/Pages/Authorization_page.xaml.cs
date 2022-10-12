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
           if(login_textbox.Text == String.Empty || password_textbox.Text == String.Empty)
            {
                MessageBox.Show("Одно из полей пусто!");
                return;
            }
            using(HrHelperDatabaseContext db = new HrHelperDatabaseContext())
            {
                AuthorizationUser user = db.AuthorizationUsers.Where(u => u.Login == login_textbox.Text).FirstOrDefault();

                if(user == null)
                {
                    MessageBox.Show("Неверный логин или пароль!");
                    return;
                }
                if (password_textbox.Text == user.Password)
                {
                    Main_win main_win = new Main_win();
                    main_win.Show();
                    authorization_win.Close();              
                }
                    
            }
        }
    }
}
