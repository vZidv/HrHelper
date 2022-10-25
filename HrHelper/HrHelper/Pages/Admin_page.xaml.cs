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
    /// Interaction logic for Admin_page.xaml
    /// </summary>
    public partial class Admin_page : Page
    {       
        public Admin_page()
        {
            InitializeComponent();

            LoadDataGrid();
        }

        void LoadDataGrid()
        {
            AuthorizationUser[] users;
            using (HrHelperDatabaseContext db = new HrHelperDatabaseContext())
            {
                users = db.AuthorizationUsers.ToArray();

                foreach (var user in users)
                {
                    user.TypeNavigation = db.UserTypes.Where(o => o.Id == user.Type).First();
                }               
            }
            users_dg.ItemsSource = users;      
        }

        private void userDelete_but_Click(object sender, RoutedEventArgs e)
        {
            if (!Classes.MyMessageBox.Show("Внимание", "Вы точно хотите удалить пользователя без возможности восстановления?", Classes.MyMessageBoxOptions.YesNo))
                return;


            int r = users_dg.SelectedIndex;
            string? id = null;
            for (int i = 0; i < 2;)
            {
                switch (i)
                {
                    case 0:
                        TextBlock itemL = users_dg.Columns[i].GetCellContent(users_dg.Items[r]) as TextBlock;
                        id = itemL.Text;
                        break;
                }
                i++;
            }
            using (HrHelperDatabaseContext db = new HrHelperDatabaseContext())
            {
                AuthorizationUser user = db.AuthorizationUsers.Where(o => o.Id == Convert.ToInt32(id)).First();
                db.AuthorizationUsers.Remove(user);
                db.SaveChanges();
            }
            LoadDataGrid();
            Classes.MyMessageBox.Show("Внимание", "Пользователь удален!");
        }

        private void userAdd_bt_Click(object sender, RoutedEventArgs e) => Classes.Settings.mainFrame.Navigate(new Pages.UserAdd_page());
    }
}
