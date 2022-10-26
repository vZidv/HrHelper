using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Reflection;
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
using System.Xml.Linq;

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
            SummaryIsReadOnlyMode(true);
        }

        void LoadSummary(int id)
        {
            using (var db = new HrHelperDatabaseContext())
            {
                Summary summary = db.Summaries.Where(o => o.Id == id).First();
                if (summary.BusynessId != null)
                {
                    summary.Busyness = db.Busynesses.Where(o => o.Id == summary.BusynessId).First();
                    busyness_tb.Text = summary.Busyness.Type;
                }

                name_tb.Text = summary.FirstName;
                lastname_tb.Text = summary.LastName;
                patronymic_tb.Text = summary.Patronymic;                
                if (summary.PhotoId != null)
                {
                    Photo photo = db.Photos.Where(o => o.Id == summary.PhotoId).First();

                    string photoPath = Classes.PhotoFolder.ProjectPath() + photo.Path;
                    using (Stream stream = File.OpenRead(photoPath))
                    {
                        BitmapImage bitmap = new BitmapImage();
                        bitmap.BeginInit();
                        bitmap.StreamSource = stream;
                        bitmap.CacheOption = BitmapCacheOption.OnLoad;
                        bitmap.EndInit();
                        bitmap.Freeze();
                        photo_image.ImageSource = bitmap;
                    }
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
                    Photo photo;

                    if (summary.PhotoId != null)
                    {
                        photo = db.Photos.Where(o => o.Id == summary.PhotoId).First();
                    }
                    else
                        photo = new Photo();
                       

                    string fileFormat = new FileInfo(imagePathNow).Extension;

                    string? photoPath = Classes.PhotoFolder.AddPhoto(imagePathNow, $"{name_tb.Text} {lastname_tb.Text} {patronymic_tb.Text}", fileFormat);
                    photo.Path = photoPath;
                    if(summary.PhotoId == null)
                    {                        
                        db.Photos.Add(photo);
                        db.SaveChanges();
                        summary.PhotoId = photo.Id;
                    }
                        
                    db.SaveChanges();
                }
                LoadSummary(idSummary);
            }           
        }

        private void editSummary_bt_Click(object sender, RoutedEventArgs e)
        {
            SummaryIsReadOnlyMode(false);
        }
        private void SummaryIsReadOnlyMode(bool modeEdit)
        {
            name_tb.IsReadOnly = modeEdit;
            lastname_tb.IsReadOnly=modeEdit;
            patronymic_tb.IsReadOnly = modeEdit;
            phone_tb.IsReadOnly = modeEdit;
            email_tb.IsReadOnly = modeEdit;
            town_tb.IsReadOnly = modeEdit;
            address_tb.IsReadOnly = modeEdit;
            jobTitle_tb.IsReadOnly = modeEdit;
            specialization_tb.IsReadOnly = modeEdit;
            education_tb.IsReadOnly = modeEdit;
            commnet_tb.IsReadOnly = modeEdit;

            // age_tBlock.Text 
            // gender_tBlock.Text 
            // birthdayDate_tBlock
        }

        private void saveSummary_bt_Click(object sender, RoutedEventArgs e)
        {
            using (HrHelperDatabaseContext db = new HrHelperDatabaseContext())
            {
                Summary summary = db.Summaries.Where(o => o.Id == idSummary).First();

                summary.FirstName = name_tb.Text;
                summary.LastName = lastname_tb.Text;
                summary.Patronymic = patronymic_tb.Text;
                //summary.Gender = gender_cb.Text,
                //summary.Birthday = date,
                summary.Phone = phone_tb.Text;
                summary.Email = email_tb.Text;
                summary.Address = address_tb.Text;
                summary.Town = town_tb.Text;
                summary.Specialization = specialization_tb.Text;
                summary.JobTitle = jobTitle_tb.Text;
                //BusynessId
                summary.Education = education_tb.Text;
                //summary.PhotoId = CreatePhoto(),          
                summary.Comments = commnet_tb.Text;
                db.SaveChanges();
            }
        }
    }
}
