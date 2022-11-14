using HrHelper.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.EntityFrameworkCore;

namespace HrHelper.Pages
{
    /// <summary>
    /// Interaction logic for Vacancy_page.xaml
    /// </summary>
    public partial class Vacancy_page : Page
    {
        Vacancy? vacancyNow;
        public Vacancy_page()
        {
            InitializeComponent();

            LoadVacancy();
            LoadStatusComobox();
        }

        private void LoadVacancy()
        {
            Vacancy[] vacancies;
            using (var db = new HrHelperDatabaseContext())
                vacancies = db.Vacancies.ToArray();

            foreach (var vacnci in vacancies)
            {
                Button button = new Button() { Content = vacnci.JobTitle, Style = (Style)Application.Current.Resources["defaultBut"] };
                button.Click += ChooseVacancy_but_Click;
                vacancy_sp.Children.Add(button);
            }
        }
        private void LoadStatusComobox()
        {
            SummaryStatus[] statuses;
            using (HrHelperDatabaseContext db = new HrHelperDatabaseContext())
                statuses = db.SummaryStatuses.ToArray();

            foreach (SummaryStatus status in statuses)
            {
                status_cb.Items.Add(status.Status);
            }
        }
        private void vacancyAdd_but_Click(object sender, RoutedEventArgs e)
        {
            Windows.VacancyAdd_win win = new Windows.VacancyAdd_win();
            win.Show();
        }

        private void ChooseVacancy_but_Click(object sender, RoutedEventArgs e)
        {
            var but = sender as Button;

            SummaryForVacancy[] summaryForVacancy;
            using (HrHelperDatabaseContext db = new HrHelperDatabaseContext())
            {
                vacancyNow = db.Vacancies.Where(o => o.JobTitle == but.Content.ToString()).First();

                summaryForVacancy = db.SummaryForVacancies.Where(o => o.JobId == vacancyNow.Id).Include(o => o.Summary).ToArray();
            }

            List<Summary>? summaries = new List<Summary>();
            for (int i = 0; i < summaryForVacancy.Length;)
            {
                summaries.Add(summaryForVacancy[i].Summary);
                i++;
            }
            summary_dg.ItemsSource = summaries;


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

        private void status_cb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            summary_dg.ItemsSource = null;
            using (HrHelperDatabaseContext db = new HrHelperDatabaseContext())
            {
                SummaryForVacancy[] summaryForVacancy = db.SummaryForVacancies.Where(o => o.JobId == vacancyNow.Id).ToArray();
                List<Summary>? summaries = new List<Summary>();

                foreach (var summaryFor in summaryForVacancy)
                {
                    Summary summary = db.Summaries.Where(o => o.Id == summaryFor.SummaryId).First();

                    summary.Status = db.SummaryStatuses.Where(o => o.Id == summary.StatusId).First();
                    if (status_cb.SelectedValue.ToString() == summary.Status.Status.ToString())
                    {
                        summaries.Add(summary);
                    }
                }
                summary_dg.ItemsSource = summaries.ToArray();
            }
        }

    }
}
