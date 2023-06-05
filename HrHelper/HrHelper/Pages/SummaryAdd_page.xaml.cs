using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using HrHelper.Classes;
using System.Text.RegularExpressions;

namespace HrHelper.Pages
{
    public partial class SummaryAdd_page : Page
    {
        string? photoPath = null;
        string photoFormat;

        int status;
        int? busyness;
        int? education;
        int vacancy;

        public SummaryAdd_page()
        {
            InitializeComponent();

            LoadComboBoxes();
        }

        public void LoadComboBoxes()
        {
            LoadStatusComboBox();
            LoadBusynnesComboBox();
            LoadEducationComboBox();
            LoadJobTitleComboBox();
            LoadGenderCombobox();
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
        // Метод LoadJobTitleComboBox загружает список вакансий в комбобокс
        private void LoadJobTitleComboBox()
        {
            using (HrHelperDatabaseContext db = new HrHelperDatabaseContext())
            {
                Vacancy[] vacancies = db.Vacancies.ToArray();
                foreach (Vacancy vacancy in vacancies)
                {
                    jobTitle_cb.Items.Add(vacancy.JobTitle);
                }
            }
        }

        // Метод LoadGenderCombobox загружает список полов в комбобокс
        private void LoadGenderCombobox()
        {
            using (HrHelperDatabaseContext db = new HrHelperDatabaseContext())
            {
                gender_cb.ItemsSource = db.Genders.ToArray();
                gender_cb.DisplayMemberPath = "Name";
            }
        }

        // Метод LoadBusynnesComboBox загружает список уровней занятости в комбобокс
        private void LoadBusynnesComboBox()
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

        // Метод LoadEducationComboBox загружает список образований в комбобокс
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


        /// <summary>
        /// Add new Summary 
        /// </summary>
        private void summaryAdd_bt_Click(object sender, RoutedEventArgs e)
        {
            List<Control> controls = new List<Control>() {firstName_tb,lastName_tb,gender_cb,birthday_datePicker,status_cb};

            if (Classes.CheckValue.CheckElementNullValue(controls) == true)
            {
                MyMessageBox.Show("Ошибка", "Пожалуйста, заполните обязательные поля", true);
                return;
            }

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

                if(status == 0)
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

            SummaryContact contacts = new SummaryContact()
            {
                Phone = phone_tb.Text,
                Email = email_tb.Text,
                OtherContacts = contactsOther_tb.Text
            };
            using (HrHelperDatabaseContext db = new HrHelperDatabaseContext())
            {
                db.Add(contacts);
                db.SaveChanges();
            }

            Summary summary = new Summary()
            {
                //ФИО
                FirstName = firstName_tb.Text,
                LastName = lastName_tb.Text,
                Patronymic = patronymic_tb.Text,
                
                GenderId = (gender_cb.SelectedItem as Gender).Id,
                
                Birthday = date,

                ContactsId = contacts.Id,
                //Address
                Address = address_tb.Text,
                Town = town_tb.Text,

                BusynessId = busyness,

                PhotoId = CreatePhoto(),

                Comments = comments_tb.Text,

                StatusId = status,
                //Last Company 
                LastCompany = lastCompany_tb.Text,
                LastJobTitle = lastJobTitle_tb.Text,
                //Education
                EducationId = education,
                EducationInstution = educationInstution_tb.Text,
                AboutYourself = aboutYourself_tblock.Text
            };
            using (HrHelperDatabaseContext db = new HrHelperDatabaseContext())
            {
                db.Add(summary);
                db.SaveChanges();
            }
            try
            {
                SummaryForVacancy summaryFor = new SummaryForVacancy()
                {
                    SummaryId = summary.Id,
                    VacancyId = vacancy
                };
                using (HrHelperDatabaseContext db = new HrHelperDatabaseContext())
                {
                    db.Add(summaryFor);
                    db.SaveChanges();
                }
            }
            catch
            {}

            Classes.MyMessageBox.Show("Внимание", "Пользователь добавлен.");
            Classes.Settings.mainFrame.Navigate(new SummaryList_page());
        }
        int? CreatePhoto()
        {
            if (photoPath == null || photoPath == String.Empty)
                return null;

            string path = Classes.PhotoFolder.AddPhoto(photoPath, $"{firstName_tb.Text} {lastName_tb.Text} {patronymic_tb.Text}", photoFormat);
            Photo photo = new Photo() { Path = path };

            using (HrHelperDatabaseContext db = new HrHelperDatabaseContext())
            {
                db.Add(photo);
                db.SaveChanges();
            }
            return photo.Id;
        }

        /// <summary>
        /// Change Photo person
        /// </summary>
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
            catch { photoPath = null; }
        }

        private void dontSave_but_Click(object sender, RoutedEventArgs e)
        {
            Settings.mainFrame.GoBack();
        }

        private void phone_tb_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
