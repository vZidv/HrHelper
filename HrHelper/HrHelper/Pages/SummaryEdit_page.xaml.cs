using System;
using System.Collections.Generic;
using System.IO;
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
    /// Interaction logic for SummaryEdit_page.xaml
    /// </summary>
    public partial class SummaryEdit_page : Page
    {
        public SummaryEdit_page()
        {
            InitializeComponent();

            LoadSummary();
        }

        void LoadSummary()
        {
            using (var db = new HrHelperDatabaseContext())
            {
                Summary summary = db.Summaries.Where(o => o.Id == 1).First();

                name_tb.Text = summary.FirstName;
                lastname_tb.Text = summary.LastName;
                patronymic_tb.Text = summary.Patronymic;
                Photo photo = db.Photos.Where(o => o.Id == summary.Id).First();
                photo_image.ImageSource = new BitmapImage(new Uri(photo.Path));
            }
        }
    }
}
