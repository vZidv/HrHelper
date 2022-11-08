using HrHelper.Classes;
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
            using (HrHelperDatabaseContext db = new HrHelperDatabaseContext())
            {
                Vacancy vacancy = new Vacancy() { JobTitle = jobTitle_tb.Text };
                db.Vacancies.Add(vacancy);
                db.SaveChanges();
            }
            MyMessageBox.Show("Внимание", "Вакансия успешно добавлена.",MyMessageBoxOptions.Ok);
            this.Close();
        }
    }
}
