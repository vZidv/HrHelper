using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
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
    /// Interaction logic for SummaryAdd_page.xaml
    /// </summary>
    public partial class SummaryAdd_page : Page
    {
        string? photoPath;
        string photoFormat;
        int status;

        public SummaryAdd_page()
        {
            InitializeComponent();

            LoadStatusComboBox();
        }
        private void LoadStatusComboBox()
        {
            using (HrHelperDatabaseContext db = new HrHelperDatabaseContext())
            {
                SummaryStatus[] statuses = db.SummaryStatuses.ToArray();
                foreach (SummaryStatus stat in statuses)
                {
                    status_cb.Items.Add(stat.Status);
                }
                
            }
        }

        private void summaryAdd_bt_Click(object sender, RoutedEventArgs e)
        {
            //date
            DateTime date = Convert.ToDateTime(birthday_dateP.SelectedDate);
            date.ToString("yyyy-MM-dd");

            
            using (HrHelperDatabaseContext db = new HrHelperDatabaseContext())
            {
                SummaryStatus[] statuses = db.SummaryStatuses.ToArray();
                foreach (SummaryStatus stat in statuses)
                {
                    if (stat.Status == status_cb.Text)
                        status = stat.Id;
                }

            }


            Summary summary = new Summary()
            {
                FirstName = name_tb.Text,
                LastName = lastname_tb.Text,
                Patronymic = patronymic_tb.Text,
                Gender = gender_cb.Text,
                Birthday = date,
                Phone = phone_tb.Text,
                Email = email_tb.Text,
                Address = address_tb.Text,
                Town = town_tb.Text,
                Specialization = specialization_tb.Text,
                JobTitle = jobTitle_tb.Text,
                Status = status,
                //BusynessId
                Education = education_tb.Text,
                PhotoId = CreatePhoto(),          
                Comments = commnet_tb.Text
                
            };
            using (HrHelperDatabaseContext db = new HrHelperDatabaseContext())
            {
                db.Add(summary);
                db.SaveChanges();
            }
        }
        int? CreatePhoto()
        {
            if (photoPath == string.Empty)
                return null;

            string path = Classes.PhotoFolder.AddPhoto(photoPath, $"{name_tb.Text} {lastname_tb.Text} {patronymic_tb.Text}", photoFormat);
            Photo photo = new Photo() { Path = path };

            using (HrHelperDatabaseContext db = new HrHelperDatabaseContext())
            {
                db.Add(photo);
                db.SaveChanges();
            }          
            return photo.Id;
        }
        private void changePhoto_bt_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dialog = new Microsoft.Win32.OpenFileDialog();
            dialog.DefaultExt = ".png";
            dialog.Filter = "Image(*.jpg,*.png)|*.jpg;*.png|JPG Files(*.jpg)|*.jpg|PNG|*.png";

            Nullable<bool> result = dialog.ShowDialog();
            photoPath = dialog.FileName;
            photoFormat = new FileInfo(photoPath).Extension;
            photo_image.ImageSource = new BitmapImage(new Uri(dialog.FileName));
        }
    }
}
