using Microsoft.EntityFrameworkCore;
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

            
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            LoadDataGrid();
            RowCountUpdate();
        }

        private void LoadDataGrid()
        {
            using (HrHelperDatabaseContext db = new HrHelperDatabaseContext())
            {
                Summary[] summaries = db.Summaries.Include(o => o.Status).ToArray();
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
            //ПЕРЕДЕЛАТЬ
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


        private void summaryAdd_bt_Click(object sender, RoutedEventArgs e) => Classes.Settings.mainFrame.Navigate(new Pages.SummaryAdd_page());

        private void search_tb_SelectionChanged(object sender, RoutedEventArgs e)
        {
            using (HrHelperDatabaseContext db = new HrHelperDatabaseContext())
            {
                Summary[] clients = db.Summaries.Where(o =>
                EF.Functions.Like(o.FirstName, $"%{search_tb.Text}%") ||
                EF.Functions.Like(o.LastName, $"%{search_tb.Text}%") ||
                EF.Functions.Like(o.Patronymic, $"%{search_tb.Text}%")).ToArray();
                summary_dg.ItemsSource = clients;

                RowCountUpdate();
            }
        }

        private void RowCountUpdate() => allClients_tblock.Text = $"Всего - {summary_dg.Items.Count}";

    }
}