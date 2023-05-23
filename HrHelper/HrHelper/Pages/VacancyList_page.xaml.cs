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
    /// Interaction logic for VacancyList_page.xaml
    /// </summary>
    public partial class VacancyList_page : Page
    {
        public VacancyList_page()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            LoadDataGrid();
        }
        private void LoadDataGrid()
        {
            using (HrHelperDatabaseContext db = new HrHelperDatabaseContext())
            {
                Vacancy[] vacancy = db.Vacancies.Include(o => o.Busyness).ToArray();
                vacancy_dg.ItemsSource = vacancy;
            }
            RowCountUpdate();
        }
        private void RowCountUpdate() => allClients_tblock.Text = $"Всего - {vacancy_dg.Items.Count}";

        private void vacancyAdd_but_Click(object sender, RoutedEventArgs e) 
        {
            new MinWin_win(new VacancyAdd_page()).ShowDialog();
            LoadDataGrid();
        }

        private void openVacancy_button_Click(object sender, RoutedEventArgs e)
        {
            Vacancy vacancy = vacancy_dg.SelectedItem as Vacancy;
            new MinWin_win(new Vacancy_page(vacancy)).ShowDialog();
            LoadDataGrid();
        }
    }
}
