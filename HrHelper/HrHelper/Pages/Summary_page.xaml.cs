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
    /// Interaction logic for Summary_page.xaml
    /// </summary>
    public partial class Summary_page : Page
    {
        int idSummary { get; }
        string? imagePathNow;

        public Summary_page(int summaryId)
        {
            idSummary = summaryId;

            InitializeComponent();
            LoadSummary(idSummary);
        }

        void LoadSummary(int id)
        {
            using (var db = new HrHelperDatabaseContext())
            {
                Summary summary = db.Summaries.Where(o => o.Id == id).First();
                //if (summary.BusynessId != null)
                //{
                //    summary.Busyness = db.Busynesses.Where(o => o.Id == summary.BusynessId).First();
                //    busyness_tb.Text = summary.Busyness.Type;
                //}
                fullname_tblock.Text = $"{summary.LastName} {summary.FirstName} {summary.Patronymic}";

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

                dateBirthdayAge_tblock.Text = summary.Birthday.ToString("dd.MM.yyyy")+$"({GetAge(summary)} годиков)";
                gender_tblock.Text = summary.Gender;
                //phone_tb.Text = "+" + summary.Phone.ToString();
                //email_tb.Text = summary.Email;
                town_tblock.Text = summary.Town;
                address_tblock.Text = summary.Address;

                //jobTitle_tb.Text = summary.JobTitle;
                //specialization_tb.Text = summary.Specialization;
                //education_tb.Text = summary.Education;
                comments_tb.Text = summary.Comments;

                SummaryStatus status = db.SummaryStatuses.Where(o => o.Id == summary.Status).First();
                SetStatus(status);
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
            void SetStatus(SummaryStatus status)
            {
                status_tblock.Text = status.Status;
                string statusColor = null;
                switch(status.Status)
                {
                    case "Приглашен":
                        statusColor = "Online";
                        break;
                    case "Принят":
                        statusColor = "Balance";
                        break;
                    case "Отказ":
                        statusColor = "Dnd";
                        break;
                    case "Без Статуса":
                        statusColor = "Grey";
                        break;
                }
                status_border.Background = Application.Current.Resources[$"{statusColor}"] as SolidColorBrush;
            }
        }
    }
}
