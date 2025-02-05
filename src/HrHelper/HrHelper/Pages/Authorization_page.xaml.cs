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
            // Проверка на пустые поля логина и пароля
            if (login_tb.Text == String.Empty || password_tb.Password == String.Empty)
            {
                Classes.MyMessageBox.Show("Ошибка", "Поле логин или пароль пустое!", true);
                return;
            }
            try
            {
                AuthorizationUser user;

                // Получение пользователя из базы данных
                using (HrHelperDatabaseContext db = new HrHelperDatabaseContext())
                    user = db.AuthorizationUsers.Where(u => u.Login == login_tb.Text).Include(o => o.UserType).FirstOrDefault();

                // Проверка на существование пользователя и совпадение пароля
                if (user == null || password_tb.Password != user.Password)
                {
                    Classes.MyMessageBox.Show("Ошибка", "Неверный логин или пароль!", true);
                    return;
                }

                // Открытие главного окна и закрытие окна авторизации
                new Main_win(user).Show();
                authorization_win.Close();
            }
            catch (Exception ex)
            {
                MyMessageBox.Show("Ошибка", ex.Message);
            }
        }
    }


}

