using HrHelper.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Microsoft.EntityFrameworkCore;
using HrHelper.Classes;

namespace HrHelper.Pages
{
    public partial class Authorization_page : Page
    {
        public Window authorization_win;

        public Authorization_page() => InitializeComponent();

        private void Authorization_but_Click(object sender, RoutedEventArgs e)
        {         
            if (login_tb.Text == String.Empty || password_tb.Password == String.Empty)
            {
                Classes.MyMessageBox.Show("Ошибка","Поле логин или пароль пустое!",true);
                return;
            }
            try
            {
                AuthorizationUser user;

                using (HrHelperDatabaseContext db = new HrHelperDatabaseContext())
                    user = db.AuthorizationUsers.Where(u => u.Login == login_tb.Text).Include(o => o.TypeNavigation).FirstOrDefault();

                if (user == null || password_tb.Password != user.Password)
                {
                    Classes.MyMessageBox.Show("Ошибка", "Неверный логин или пароль!", true);
                    return;
                }
                if (password_tb.Password != user.Password)
                    Classes.MyMessageBox.Show("Ошибка", "Неверный пароль!", true);

                //new Main_win(user).Show();
                new Main_win(user).Show();
                authorization_win.Close();
            }
            catch(Exception ex)
            {
                MyMessageBox.Show("Ошибка",ex.Message);
            }
        }

  
    }
}
