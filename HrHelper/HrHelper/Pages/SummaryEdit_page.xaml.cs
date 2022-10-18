using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
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
        int idSummary { get; }
        string? imagePathNow;

        public SummaryEdit_page(int idSummary)
        {
            this.idSummary = idSummary;

            InitializeComponent();
            LoadSummary(idSummary);
        }

        void LoadSummary(int id)
        {
            using (var db = new HrHelperDatabaseContext())
            {
                Summary summary = db.Summaries.Where(o => o.Id == id).First();
                summary.Busyness = db.Busynesses.Where(o => o.Id == summary.BusynessId).First();

                name_tb.Text = summary.FirstName;
                lastname_tb.Text = summary.LastName;
                patronymic_tb.Text = summary.Patronymic;
                Photo photo = db.Photos.Where(o => o.Id == summary.Id).First();
                
                 using(Stream stream = File.OpenRead(photo.Path))
                {
                    BitmapImage bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.StreamSource = stream;
                    bitmap.CacheOption = BitmapCacheOption.OnLoad;
                    bitmap.EndInit();
                    bitmap.Freeze();
                    photo_image.ImageSource = bitmap;
                }
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

                commnet_tb.Visibility = Visibility.Hidden;
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

        private void openComment_toggleBt_Checked(object sender, RoutedEventArgs e)
        {
            if (openComment_toggleBt.IsChecked == true)
                commnet_tb.Visibility = Visibility.Visible;
            else
                commnet_tb.Visibility = Visibility.Hidden;
        }

        private void changePhoto_bt_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dialog = new Microsoft.Win32.OpenFileDialog();
            dialog.DefaultExt = ".png";
            dialog.Filter = "Image(*.jpg,*.png)|*.jpg;*.png|JPG Files(*.jpg)|*.jpg|PNG|*.png";

            Nullable<bool> result = dialog.ShowDialog();

            if (result == true)
            {
                imagePathNow = dialog.FileName;                        
                photo_image.ImageSource = new BitmapImage(new Uri(imagePathNow));

                
                using (var db = new HrHelperDatabaseContext())
                {
                    Summary summary = db.Summaries.Where(o => o.Id == idSummary).First();
                    Photo photo = db.Photos.Where(o => o.Id == summary.PhotoId).First();

                    string fileFormat = new FileInfo(imagePathNow).Extension;

                    string? photoPath = Classes.PhotoFolder.AddPhoto(imagePathNow, $"{name_tb.Text} {lastname_tb.Text} {patronymic_tb.Text}", fileFormat);
                    photo.Path = photoPath;
                    db.SaveChanges();
                }
                LoadSummary(idSummary);
            }           
        }

        private void save_bt_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
