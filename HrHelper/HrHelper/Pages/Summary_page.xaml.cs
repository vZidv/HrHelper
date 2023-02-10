using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using Microsoft.EntityFrameworkCore;
using HrHelper.Classes;
using ex = Microsoft.Office.Interop.Excel;


namespace HrHelper.Pages
{
    public partial class Summary_page : Page
    {
        int idSummary { get; }
        Summary summary;
        string? imagePathNow;

        public Summary_page(int summaryId)
        {
            idSummary = summaryId;

            InitializeComponent();

            LoadSummary(idSummary);
            LoadComboBoxes();
        }
        private void LoadComboBoxes()
        {
            LoadStatusComboBox();
            LoadJobTitleComboBox();
            LoadbussynesComboBox();
        }

        private void LoadSummary(int id)
        {
            using (var db = new HrHelperDatabaseContext())
            {
                summary = db.Summaries.Where(o => o.Id == id).
                 Include(r => r.Photo).Include(o => o.Contacts).Include(o => o.Busyness).Include(o => o.Status).Include(o => o.Education)
                 .First();
            }
            fullname_tblock.Text = $"{summary.LastName} {summary.FirstName} {summary.Patronymic}";

            if (summary.Photo != null)
            {
                string photoPath = Classes.PhotoFolder.ProjectPath() + summary.Photo.Path;
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

            dateBirthdayAge_tblock.Text = summary.Birthday.ToString("dd.MM.yyyy") + $"({GetAge(summary)} годиков)";
            gender_tblock.Text = summary.Gender;

            town_tblock.Text = summary.Town;
            address_tblock.Text = summary.Address;
            if (summary.Busyness != null)
                busynessChange_cb.Text = summary.Busyness.Type;

            educationInstution_tblock.Text = summary.EducationInstution;
            if (summary.Education != null)
                education_tblock.Text = summary.Education.EducationName;

            comments_tb.Text = summary.Comments;

            SetStatus(summary.Status);
            statusChange_cb.Text = summary.Status.Status;
        }
        void LoadContacts(Summary summary)
        {
            if (summary.ContactsId == null)
                return;

            using (var db = new HrHelperDatabaseContext())
                summary.Contacts = db.SummaryContacts.Where(o => o.Id == summary.ContactsId).First();

            if (summary.Contacts.Phone != null)
                ContactsAddView("Номер телефона", summary.Contacts.Phone);
            if (summary.Contacts.Email != null)
                ContactsAddView("Почта", summary.Contacts.Email);
            if (summary.Contacts.Skype != null)
                ContactsAddView("Skype", summary.Contacts.Skype);
        }
        void ContactsAddView(string titleContact, string contact)
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
        private void LoadbussynesComboBox()
        {
            using (HrHelperDatabaseContext db = new HrHelperDatabaseContext())
            {
                Busyness[] busynesses = db.Busynesses.ToArray();
                foreach (Busyness busyness in busynesses)
                {
                    busynessChange_cb.Items.Add(busyness.Type);
                }

            }
        }
        private void LoadJobTitleComboBox()
        {
            using (HrHelperDatabaseContext db = new HrHelperDatabaseContext())
            {
                Vacancy[] vacancies = db.Vacancies.ToArray();
                foreach (Vacancy vacancy in vacancies)
                {
                    jobTitleChange_cb.Items.Add(vacancy.JobTitle);
                }
                SummaryForVacancy? summaryFor = null;
                try
                {
                    summaryFor = db.SummaryForVacancies.Where(o => o.SummaryId == idSummary).First();
                }
                catch { }

                if (summaryFor == null)
                    jobTitleChange_cb.Text = String.Empty;
                else
                    jobTitleChange_cb.Text = db.Vacancies.Where(o => o.Id == summaryFor.JobId).First().JobTitle;
            }
        }

        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            using (HrHelperDatabaseContext db = new HrHelperDatabaseContext())
            {
                Summary summary = db.Summaries.Where(o => o.Id == idSummary).First();

                summary.Comments = comments_tb.Text;
                summary.StatusId = db.SummaryStatuses.Where(o => o.Status == status_tblock.Text).First().Id;
                if (busynessChange_cb.SelectedIndex != -1)
                    summary.BusynessId = db.Busynesses.Where(o => o.Type == busynessChange_cb.Text).First().Id;

                if (jobTitleChange_cb.Text != String.Empty)
                {
                    SummaryForVacancy summaryFor;

                    try
                    {
                        summaryFor = db.SummaryForVacancies.Where(o => o.SummaryId == idSummary).First();
                        summaryFor.JobId = db.Vacancies.Where(o => o.JobTitle == jobTitleChange_cb.Text).First().Id;
                        db.SummaryForVacancies.Update(summaryFor);
                    }
                    catch
                    {
                        summaryFor = new SummaryForVacancy()
                        {
                            JobId = db.Vacancies.Where(o => o.JobTitle == jobTitleChange_cb.Text).First().Id,
                            SummaryId = idSummary
                        };
                        db.SummaryForVacancies.Add(summaryFor);
                    }
                }
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

        private void jobTitleChange_cb_SelectionChanged(object sender, SelectionChangedEventArgs e) { }
        
        private void wordExport_Click(object sender, RoutedEventArgs e)
        {
            //Summary base data 
            var items = new Dictionary<string, string>
            {
                {"<Fullname>", fullname_tblock.Text},
                {"<Gender>", gender_tblock.Text},
                {"<Age>", GetAge(summary).ToString()},
                {"<BirthdayDate>",summary.Birthday.ToString("d MMMM yyyy")},
            };
            // Summary contacts
            if (summary.Contacts != null)
            {
                items.Add("<PhoneNumber>", summary.Contacts.Phone);
                items.Add("<Email>", summary.Contacts.Email);
                items.Add("<Skype>", summary.Contacts.Skype);
            }
            if (summary.Town != null)
            {
                items.Add("<Town>", town_tblock.Text);
                items.Add("<Address>", address_tblock.Text);
            }
            if (summary.Education != null)
                items.Add("<Education>", summary.Education.EducationName);
            if (summary.Busyness != null)
                items.Add("<Busyness>", busynessChange_cb.Text);


            items.Add("<JobTitle>", jobTitleChange_cb.Text);
            items.Add("<LastCompany>", summary.LastCompany);
            items.Add("<LastJobTitle>", summary.LastJobTitle);
            items.Add("<EducationInstution>", educationInstution_tblock.Text);
            items.Add("<Comments>", comments_tb.Text);

            string? photo = null;
            if (summary.Photo != null)
                photo = summary.Photo.Path;
            
            WordExport.ExportSummary(items,summary);
        }

        private void summaryEdit_but_Click(object sender, RoutedEventArgs e) => Settings.mainFrame.Navigate(new SummaryEdit_page(summary));

        private void pdfExport_but_Click(object sender, RoutedEventArgs e)
        {
            var items = new Dictionary<string, string>
            {
                {"<Fullname>", fullname_tblock.Text},
                {"<Gender>", gender_tblock.Text},
                {"<Age>", GetAge(summary).ToString()},
                {"<BirthdayDate>",summary.Birthday.ToString("d MMMM yyyy")},
            };
            // Summary contacts
            if (summary.Contacts != null)
            {
                items.Add("<PhoneNumber>", summary.Contacts.Phone);
                items.Add("<Email>", summary.Contacts.Email);
                items.Add("<Skype>", summary.Contacts.Skype);
            }
            if (summary.Town != null)
            {
                items.Add("<Town>", town_tblock.Text);
                items.Add("<Address>", address_tblock.Text);
            }
            if (summary.Education != null)
                items.Add("<Education>", summary.Education.EducationName);
            if (summary.Busyness != null)
                items.Add("<Busyness>", busynessChange_cb.Text);


            items.Add("<JobTitle>", jobTitleChange_cb.Text);
            items.Add("<LastCompany>", summary.LastCompany);
            items.Add("<LastJobTitle>", summary.LastJobTitle);
            items.Add("<EducationInstution>", educationInstution_tblock.Text);
            items.Add("<Comments>", comments_tb.Text);

            string? photo = null;
            if (summary.Photo != null)
                photo = summary.Photo.Path;

            PdfExport.SummaryExport(items,summary);
        }
     
    }
}
