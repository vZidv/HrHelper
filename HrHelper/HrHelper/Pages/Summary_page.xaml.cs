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
using System.Runtime.InteropServices;
using HrHelper.Classes;
using ex = Microsoft.Office.Interop.Excel;

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
            LoadStatusComboBox();
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

                LoadContacts(summary);

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

                SummaryStatus status = db.SummaryStatuses.Where(o => o.Id == summary.StatusId).First();
                SetStatus(status);
            }


        }
        void LoadContacts(Summary summary)
        {
            if (summary.ContactsId == null)
                return;
            using (var db = new HrHelperDatabaseContext())
            {
                summary.Contacts = db.SummaryContacts.Where(o => o.Id == summary.ContactsId).First();
            }

            if (summary.Contacts.Phone != null)
                ContactsAddView("Номер телефона",summary.Contacts.Phone);
             if (summary.Contacts.Email != null)
                ContactsAddView("Почта", summary.Contacts.Email);
             if (summary.Contacts.Skype != null)
                ContactsAddView("Skype", summary.Contacts.Skype);
        }
        void ContactsAddView(string titleContact,string contact)
        {
            StackPanel contact_sp = new StackPanel();
            contact_sp.Orientation = Orientation.Horizontal;

            TextBlock title_tb = new TextBlock();
            title_tb.Text = titleContact;
            title_tb.Style = (Style)Application.Current.Resources["defaultTextBlock"];

            TextBlock contact_tb = new TextBlock();
            contact_tb.Margin = new Thickness(10);
            contact_tb.Text = contact;
            contact_tb.Style = (Style)Application.Current.Resources["valueTextBlock"];

            contact_sp.Children.Add(title_tb);
            contact_sp.Children.Add(contact_tb);

            contacts_sp.Children.Add(contact_sp);
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
            switch (status.Status)
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
        private void LoadStatusComboBox()
        {
            using (HrHelperDatabaseContext db = new HrHelperDatabaseContext())
            {
                SummaryStatus[] statuses = db.SummaryStatuses.ToArray();
                foreach (SummaryStatus status in statuses)
                {
                    statusChange_cb.Items.Add(status.Status);
                }

            }
        }

        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            using (HrHelperDatabaseContext db = new HrHelperDatabaseContext())
            {
                Summary summary = db.Summaries.Where(o => o.Id == idSummary).First();

                summary.Comments = comments_tb.Text;
                summary.StatusId = db.SummaryStatuses.Where(o => o.Status == status_tblock.Text).First().Id;
                db.SaveChanges();
            }
        }

        private void statusChange_cb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SummaryStatus status = new SummaryStatus()
            {
                Status = statusChange_cb.SelectedValue.ToString()
            };

            SetStatus(status);
        }

        private void excelExport_but_Click(object sender, RoutedEventArgs e)
        {
            ex.Application exApp = new ex.Application();

            exApp.Workbooks.Add();
            ex.Worksheet wsh = (ex.Worksheet)exApp.ActiveSheet;
            Summary summary;
            using (HrHelperDatabaseContext db = new HrHelperDatabaseContext())
                summary = db.Summaries.Where(o => o.Id == idSummary).First();
            wsh.Cells[1, 1] = "Имя";
            wsh.Cells[1, 2] = "Фамилия";
            wsh.Cells[1, 3] = "Отчество";

            wsh.Cells[1, 4] = "Дата рождения";
            wsh.Cells[1, 5] = "Статус";
            wsh.Cells[1, 6] = "Комментарии";

            wsh.Cells[2, 1] = summary.FirstName;
            wsh.Cells[2, 2] = summary.LastName;
            wsh.Cells[2, 3] = summary.Patronymic;

            wsh.Cells[2, 4] = summary.Birthday;
            wsh.Cells[2, 5] = status_tblock.Text;
            wsh.Cells[2, 6] = summary.Comments;
            exApp.Visible = true;
        }
    }
}
