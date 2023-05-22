using HrHelper.Classes;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace HrHelper.Pages
{
    public partial class SummaryEdit_page : Page
    {
        Summary summary;

        string? photoPath = null;
        string photoFormat;

        int status;
        int? busyness;
        int? education;
        int vacancy;
        public SummaryEdit_page(Summary summary)
        {
            InitializeComponent();
                this.summary = summary;
            LoadComboBoxes();
            LoadSummary();
        }
        //LoadComboBoxes
        #region LoadComboBoxes
        private void LoadComboBoxes()
        {
            LoadStatusComboBox();
            LoadJobTitleComboBox();
            LoadbussynesComboBox();
            LoadEducationComboBox();
        }
        private void LoadEducationComboBox()
        {
            using (HrHelperDatabaseContext db = new HrHelperDatabaseContext())
            {
                Education[] educations = db.Educations.ToArray();
                foreach (Education education in educations)
                {
                    education_cb.Items.Add(education.EducationName);
                }

            }
        }
        private void LoadStatusComboBox()
        {
            using (HrHelperDatabaseContext db = new HrHelperDatabaseContext())
            {
                SummaryStatus[] statuses = db.SummaryStatuses.ToArray();
                foreach (SummaryStatus status in statuses)
                {
                    status_cb.Items.Add(status.Status);
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
                    bussyness_cb.Items.Add(busyness.Type);
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
                    jobTitle_cb.Items.Add(vacancy.JobTitle);
                }

                SummaryForVacancy? summaryFor = null;
                try
                {
                    summaryFor = db.SummaryForVacancies.Where(o => o.SummaryId == summary.Id).First();
                }
                catch { }

                if (summaryFor == null)
                    jobTitle_cb.Text = String.Empty;
                else
                    jobTitle_cb.Text = db.Vacancies.Where(o => o.Id == summaryFor.JobId).First().JobTitle;
            }
        }
        #endregion
        private void LoadSummary()
        {
            // Name
            firstName_tb.Text = summary.FirstName;
            lastName_tb.Text = summary.LastName;
            patronymic_tb.Text = summary.Patronymic;

            // Photo
            if (summary.Photo != null)
            {
                photoPath = Classes.PhotoFolder.ProjectPath() + summary.Photo.Path;
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
                photoPath = summary.Photo.Path;
            }

            // Contacts
            phone_tb.Text = summary.Contacts.Phone;
            email_tb.Text = summary.Contacts.Email;
            //skype_tb.Text = summary.Contacts.Skype;


            birthday_datePicker.Text = summary.Birthday.ToString("dd.MM.yyyy");
            gender_cb.Text = summary.Gender;

            town_tb.Text = summary.Town;
            address_tb.Text = summary.Address;

            if (summary.Busyness != null)
                bussyness_cb.Text = summary.Busyness.Type;

            educationInstution_tb.Text = summary.EducationInstution;
            if (summary.Education != null)
                education_cb.Text = summary.Education.EducationName;


            comments_tb.Text = summary.Comments;
            status_cb.Text = summary.Status.Status;

            lastCompany_tb.Text = summary.LastCompany;
            lastJobTitle_tb.Text = summary.LastJobTitle;
        }
        private void save_but_Click(object sender, RoutedEventArgs e)
        {
            if (!CheckCorrectInputData())
                return;

            DateTime date = Convert.ToDateTime(birthday_datePicker.SelectedDate);
            date.ToString("yyyy-MM-dd");

            using (HrHelperDatabaseContext db = new HrHelperDatabaseContext())
            {
                SummaryStatus[] statuses = db.SummaryStatuses.ToArray();
                foreach (SummaryStatus stat in statuses)
                {
                    if (stat.Status == status_cb.Text)
                        status = stat.Id;
                }

                if (status == 0)
                    status = 4;

                try
                {
                    Busyness[] busynesses = db.Busynesses.ToArray();
                    foreach (Busyness busynes in busynesses)
                    {
                        if (busynes.Type == bussyness_cb.Text)
                            busyness = busynes.Id;
                    }
                }
                catch
                {
                    busyness = null;
                }

                Education[] educations = db.Educations.ToArray();
                foreach (Education education in educations)
                {
                    if (education.EducationName == education_cb.Text)
                        this.education = education.Id;
                }

                Vacancy[] vacancies = db.Vacancies.ToArray();
                foreach (Vacancy vacancy in vacancies)
                {
                    if (vacancy.JobTitle == jobTitle_cb.Text)
                        this.vacancy = vacancy.Id;
                }

            }

            //SummaryContact contacts = new SummaryContact()
            //{
            //    Phone = phone_tb.Text,
            //    Email = email_tb.Text,
            //    Skype = skype_tb.Text
            //};
            using (HrHelperDatabaseContext db = new HrHelperDatabaseContext())
            {
                //db.Add(contacts);
                db.SaveChanges();
            }

            //Summary newSummary = new Summary()
            //{
            //    Id = summary.Id,
            //    FirstName = firstName_tb.Text,
            //    LastName = lastName_tb.Text,
            //    Patronymic = patronymic_tb.Text,

            //    Gender = gender_cb.Text,
            //    Birthday = date,

            //    ContactsId = contacts.Id,
            //    Address = address_tb.Text,
            //    Town = town_tb.Text,

            //    BusynessId = busyness,
            //    PhotoId = CreatePhoto(),
            //    Comments = comments_tb.Text,
            //    StatusId = status,
            //    LastCompany = lastCompany_tb.Text,
            //    LastJobTitle = lastJobTitle_tb.Text,
            //    EducationId = education,
            //    EducationInstution = educationInstution_tb.Text
            //};
            using (HrHelperDatabaseContext db = new HrHelperDatabaseContext())
            {
                //db.Update(newSummary);
                db.SaveChanges();
            }
            try
            {
                SummaryForVacancy summaryFor = new SummaryForVacancy()
                {
                    SummaryId = summary.Id,
                    JobId = vacancy
                };
                using (HrHelperDatabaseContext db = new HrHelperDatabaseContext())
                {
                    //db.Add(newSummary);
                    db.SaveChanges();
                }
            }
            catch
            { }

            Classes.MyMessageBox.Show("Внимание", "Пользователь обновлен.");
            Classes.Settings.mainFrame.Navigate(new Summary_page(summary.Id));
        }
        bool CheckCorrectInputData()
        {
            //if true,then all good 

            //Проверка Имя,Фамилия
            TextBox[] fullname = new TextBox[] { firstName_tb, lastName_tb };
            if (!CheckTextBoxForEmpty(fullname, "Поле имя или фамилия пустое!"))
                return false;
            else if (birthday_datePicker.Text == string.Empty)
            {
                birthday_datePicker.BorderBrush = (SolidColorBrush)Application.Current.Resources["Dnd"];
                MyMessageBox.Show("Ошибка", "Не указана дата рождения!", true);
                return false;
            }
            return true;
        }
        int? CreatePhoto()
        {
            if (photoPath == null || photoPath == String.Empty)
                return null;
            if (summary.Photo != null)
                if (photoPath == summary.Photo.Path)
                    return summary.Photo.Id;

            string path = Classes.PhotoFolder.AddPhoto(photoPath, $"{firstName_tb.Text} {lastName_tb.Text} {patronymic_tb.Text}", photoFormat);
            Photo photo = new Photo() { Path = path };

            using (HrHelperDatabaseContext db = new HrHelperDatabaseContext())
            {
                db.Add(photo);
                db.SaveChanges();
            }
            return photo.Id;
        }
        bool CheckTextBoxForEmpty(TextBox[] textBoxes, string errorMessage)
        {
            //if true,then all good 
            bool result = true;
            foreach (TextBox textBox in textBoxes)
            {
                if (textBox.Text == string.Empty)
                {
                    textBox.BorderBrush = (SolidColorBrush)Application.Current.Resources["Dnd"];
                    result = false;
                }
            }
            if (!result)
                MyMessageBox.Show("Ошибка", errorMessage, true);

            return result;
        }

        private void changePhoto_bt_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Microsoft.Win32.OpenFileDialog dialog = new Microsoft.Win32.OpenFileDialog();
                dialog.DefaultExt = ".png";
                dialog.Filter = "Image(*.jpg,*.png)|*.jpg;*.png|JPG Files(*.jpg)|*.jpg|PNG|*.png";

                Nullable<bool> result = dialog.ShowDialog();
                photoPath = dialog.FileName;
                photoFormat = new FileInfo(photoPath).Extension;
                photo_image.ImageSource = new BitmapImage(new Uri(dialog.FileName));
            }
            catch { photoPath = summary.Photo.Path; };
        }
    }
}
