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
    public partial class User_page : Page
    {
        public User_page()
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
                users_dg.ItemsSource = db.AuthorizationUsers.Include(o=>o.TypeNavigation).ToArray();
            }
            RowCountUpdate();
        }

        private void RowCountUpdate() => alluses_tblock.Text = $"Всего - {users_dg.Items.Count}";
    }
}
