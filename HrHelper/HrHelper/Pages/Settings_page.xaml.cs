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
    /// Interaction logic for Settings_page.xaml
    /// </summary>
    public partial class Settings_page : Page
    {
        public Settings_page()
        {
            InitializeComponent();
        }

        private void deleteDb_but_Click(object sender, RoutedEventArgs e)
        {
            if (!Classes.MyMessageBox.Show("Внимание!", "Вы собираетесь удалить базу данных! " +
                "После удаления вся информация из базы данных будет удалена без возможности восстановления, программа автоматически закроется. Вы уверены, что хотите это сделать?", Classes.MyMessageBoxOptions.YesNo))
                return;  
            using (HrHelperDatabaseContext db = new HrHelperDatabaseContext())
                db.Database.EnsureDeleted();
            Classes.MyMessageBox.Show("Внимание!", "База данных удалина!");

            Application.Current.Shutdown();
        }
    }
}
