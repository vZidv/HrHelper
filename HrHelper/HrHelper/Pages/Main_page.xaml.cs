using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace HrHelper.Pages
{
    public partial class Main_page : Page
    {
        public Main_page()
        {
            InitializeComponent();

            LoadDataGrid();
        }

        private void LoadDataGrid()
        {
            using (HrHelperDatabaseContext db = new HrHelperDatabaseContext())
            {
                Summary[] summaries = db.Summaries.ToArray();
                foreach (Summary summary in summaries)
                {
                    if (summary.BusynessId != null)
                        summary.Busyness = db.Busynesses.Where(o => o.Id == summary.BusynessId).First();
                }
                summary_dg.ItemsSource = summaries;
            }
        }

        private void openSummary_button_Click(object sender, RoutedEventArgs e)
        {           

            int id = ChoosePersonId();
            Classes.Settings.mainFrame.Navigate(new Pages.Summary_page(Convert.ToInt32(id)));
        }

        private int ChoosePersonId()
        {
            int r = summary_dg.SelectedIndex;
            string id = null;

            for (int i = 0; i < 2;)
            {
                switch (i)
                {
                    case 0:
                        TextBlock itemL = summary_dg.Columns[i].GetCellContent(summary_dg.Items[r]) as TextBlock;
                        id = itemL.Text;
                        break;
                }
                i++;
            }
            return Convert.ToInt32(id);
        }

        public void ChangeSummaryForDataGrid(int status)
        {
            using (HrHelperDatabaseContext db = new HrHelperDatabaseContext())
                summary_dg.ItemsSource = db.Summaries.Where(o => o.StatusId == status).ToArray();     
            
        }
        private void summaryAdd_bt_Click(object sender, RoutedEventArgs e) => Classes.Settings.mainFrame.Navigate(new Pages.SummaryAdd_page());
        private void acceptSammury_but_Click(object sender, RoutedEventArgs e) => ChangeSummaryForDataGrid(3);
        private void refusal_but_Click(object sender, RoutedEventArgs e) => ChangeSummaryForDataGrid(2);
        private void invitedSummary_but_Click(object sender, RoutedEventArgs e) => ChangeSummaryForDataGrid(1);
        private void withoutStatusSummary_but_Click(object sender, RoutedEventArgs e) => ChangeSummaryForDataGrid(4);
    }
}