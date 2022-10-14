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
                summary.Busyness = db.Busynesses.Where(o => o.Id == summary.BusynessId).First();

                name_tb.Text = summary.FirstName;
                lastname_tb.Text = summary.LastName;
                patronymic_tb.Text = summary.Patronymic;
                Photo photo = db.Photos.Where(o => o.Id == summary.Id).First();
                photo_image.ImageSource = new BitmapImage(new Uri(photo.Path));
                birthdayDate_tBlock.Text = "Дата рождениия: " + summary.Birthday.ToString("yyyy.MM.dd");
                age_tBlock.Text = "Возраст: " + GetAge(summary).ToString();
                gender_tBlock.Text = "Пол: " + summary.Gender;
                phone_tb.Text = "+" + summary.Phone.ToString();
                email_tb.Text = summary.Email;
                town_tb.Text = summary.Town;
                address_tb.Text = summary.Address;

                jobTitle_tb.Text = summary.JobTitle;
                specialization_tb.Text = summary.Specialization;
                busyness_tb.Text = summary.Busyness.Type;
                education_tb.Text = summary.Education;
                commnet_tb.Text = summary.Comments;
            }
        }
        int GetAge(Summary summary)
        {
            int year = (DateTime.Now.Year - summary.Birthday.Year);
            if (DateTime.Now.Month < summary.Birthday.Month)
                year--;
            else if (DateTime.Now.Month == summary.Birthday.Month)
                if (DateTime.Now.Day < summary.Birthday.Day)
                    year--;

            return year;
        }
    }
}
