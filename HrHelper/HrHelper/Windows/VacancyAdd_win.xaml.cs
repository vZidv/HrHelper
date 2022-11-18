using HrHelper.Classes;
using HrHelper.Pages;
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
using System.Windows.Shapes;

namespace HrHelper.Windows
{
    /// <summary>
    /// Interaction logic for VacancyAdd_win.xaml
    /// </summary>
    public partial class VacancyAdd_win : Window
    {
        public VacancyAdd_win()
        {
            InitializeComponent();
        }

        private void vacancyAdd_but_Click(object sender, RoutedEventArgs e)
        {
            Vacancy vacancy = new Vacancy() { JobTitle = jobTitle_tb.Text };

            using (HrHelperDatabaseContext db = new HrHelperDatabaseContext())
            {                
                db.Vacancies.Add(vacancy);
                db.SaveChanges();
            }
            MyMessageBox.Show("Внимание", "Вакансия успешно добавлена.",MyMessageBoxOptions.Ok);

            Vacancy_page vacancy_Page = new Vacancy_page();
            Settings.mainFrame.Navigate(vacancy_Page);

            this.Close();
        }

        #region Toolbar buttons
        private void toolBar_grid_MouseDown(object sender, MouseButtonEventArgs e) => DragMove();

        private void close_but_Click(object sender, RoutedEventArgs e) => this.Close();

        private void minWind_but_Click(object sender, RoutedEventArgs e) => this.WindowState = WindowState.Minimized;
        #endregion
    }
}
