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
    /// Interaction logic for Main_page.xaml
    /// </summary>
    public partial class Main_page : Page
    {
        public Main_page()
        {
            InitializeComponent();

            LoadDataGrid();
        }

        void LoadDataGrid()
        {
            using(HrHelperDatabaseContext db = new HrHelperDatabaseContext())
            {
                Summary[] summaries = db.Summaries.ToArray();
                foreach(Summary summary in summaries)
                    summary.Busyness = db.Busynesses.Where(o => o.Id == summary.BusynessId).First();

                summary_dg.ItemsSource = summaries;
            }
        }

        private void settings_button_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
