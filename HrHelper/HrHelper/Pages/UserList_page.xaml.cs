using HrHelper.Classes;
using HrHelper.Windows;
using Microsoft.EntityFrameworkCore;
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
    /// Interaction logic for User_page.xaml
    /// </summary>
    public partial class UserList_page : Page
    {
        public UserList_page()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            LoadDataGrid();
        }
        private void LoadDataGrid()
        {
            using(HrHelperDatabaseContext db = new HrHelperDatabaseContext())
            {
                users_dg.ItemsSource = db.AuthorizationUsers.Include(o=>o.UserType).ToArray();
            }
            RowCountUpdate();
        }

        private void RowCountUpdate() => alluses_tblock.Text = $"Всего - {users_dg.Items.Count}";

        private void userAdd_but_Click(object sender, RoutedEventArgs e)
        {
            MinWin_win win = new MinWin_win(new UserAdd_page());
            win.ShowDialog();
            LoadDataGrid();
        }

        private void delete_button_Click(object sender, RoutedEventArgs e)
        {
            AuthorizationUser user = users_dg.SelectedItem as AuthorizationUser;
            if (MyMessageBox.Show("Внимание", "Вы точно хотите удалить этого пользователя?", MyMessageBoxOptions.YesNo) == false)
                return;

            using (HrHelperDatabaseContext db = new HrHelperDatabaseContext())
            {
                db.AuthorizationUsers.Remove(user);
                db.SaveChanges();
            }
            MyMessageBox.Show("Внимание", "Пользователь успешно удален!");

            LoadDataGrid();
        }

        private void edit_button_Click(object sender, RoutedEventArgs e)
        {
            MinWin_win win = new MinWin_win(new UserEdit_page(users_dg.SelectedItem as AuthorizationUser));
            win.ShowDialog();
            LoadDataGrid();
        }
    }
}
