using HrHelper.Classes;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
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

        }
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            LoadComboBoxes();
            LoadSummary();
        }
        //LoadComboBoxes
        #region LoadComboBoxes
        private void LoadComboBoxes()
        {
            LoadGenderCombobox();
            LoadStatusComboBox();
            LoadJobTitleComboBox();
            LoadbussynesComboBox();
            LoadEducationComboBox();
        }
        // Метод загрузки вариантов образования в выпадающий список
        private void LoadEducationComboBox()
        {
            // Создаем объект контекста базы данных
            using (HrHelperDatabaseContext db = new HrHelperDatabaseContext())
            {
                // Получаем список всех видов образования из базы данных
                Education[] educations = db.Educations.ToArray();

                // Добавляем каждый вид образования в выпадающий список
                foreach (Education education in educations)
                {
                    education_cb.Items.Add(education.EducationName);
                }
            }
        }

        // Метод загрузки вариантов статуса резюме в выпадающий список
        private void LoadStatusComboBox()
        {
            // Создаем объект контекста базы данных
            using (HrHelperDatabaseContext db = new HrHelperDatabaseContext())
            {
                // Получаем список всех статусов резюме из базы данных
                SummaryStatus[] statuses = db.SummaryStatuses.ToArray();

                // Добавляем каждый статус в выпадающий список
                foreach (SummaryStatus status in statuses)
                {
                    status_cb.Items.Add(status.Status);
                }
            }
        }

        // Метод загрузки вариантов видов деятельности в выпадающий список
        private void LoadbussynesComboBox()
        {
            // Создаем объект контекста базы данных
            using (HrHelperDatabaseContext db = new HrHelperDatabaseContext())
            {
                // Получаем список всех видов деятельности из базы данных
                Busyness[] busynesses = db.Busynesses.ToArray();

                // Добавляем каждый вид деятельности в выпадающий список
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
                    jobTitle_cb.Text = db.Vacancies.Where(o => o.Id == summaryFor.VacancyId).First().JobTitle;
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
            if (summary.Contacts != null)
            {
                phone_tb.Text = summary.Contacts.Phone;
                email_tb.Text = summary.Contacts.Email;
                contactsOther_tb.Text = summary.Contacts.OtherContacts;
            }


            birthday_datePicker.Text = summary.Birthday.ToString("dd.MM.yyyy");


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

            gender_cb.Text = summary.Gender.Name;
            aboutYourself_tblock.Text = summary.AboutYourself;
        }
        private void save_but_Click(object sender, RoutedEventArgs e)
        {
            // Проверяем, заполнены ли обязательные поля
            List<Control> controls = new List<Control>() { firstName_tb, lastName_tb, gender_cb, birthday_datePicker, status_cb };
            if (Classes.CheckValue.CheckElementNullValue(controls) == true)
            {
                MyMessageBox.Show("Ошибка", "Пожалуйста, заполните обязательные поля", true);
                return;
            }

            // Получаем дату рождения из выбранной даты и преобразуем ее в нужный формат
            DateTime date = Convert.ToDateTime(birthday_datePicker.SelectedDate);
            date.ToString("yyyy-MM-dd");

            // Получаем статус резюме из базы данных
            using (HrHelperDatabaseContext db = new HrHelperDatabaseContext())
            {
                SummaryStatus[] statuses = db.SummaryStatuses.ToArray();
                foreach (SummaryStatus stat in statuses)
                {
                    if (stat.Status == status_cb.Text)
                        status = stat.Id;
                }

                // Если статус не найден, устанавливаем значение по умолчанию
                if (status == 0)
                    status = 4;

                try
                {
                    // Получаем вид деятельности из базы данных
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

                // Получаем образование из базы данных
                Education[] educations = db.Educations.ToArray();
                foreach (Education education in educations)
                {
                    if (education.EducationName == education_cb.Text)
                        this.education = education.Id;
                }

                // Получаем вакансию из базы данных
                Vacancy[] vacancies = db.Vacancies.ToArray();
                foreach (Vacancy vacancy in vacancies)
                {
                    if (vacancy.JobTitle == jobTitle_cb.Text)
                        this.vacancy = vacancy.Id;
                }
            }

            // Обновляем контактную информацию в резюме
            SummaryContact contacts = summary.Contacts;
            contacts.Phone = phone_tb.Text;
            contacts.Email = email_tb.Text;
            contacts.OtherContacts = contactsOther_tb.Text;

            // Сохраняем изменения в базе данных
            using (HrHelperDatabaseContext db = new HrHelperDatabaseContext())
            {
                //db.Add(contacts);
                db.SaveChanges();
            }

            // Создаем новый объект резюме с обновленными данными
            Summary newSummary = new Summary()
            {
                Id = summary.Id,
                FirstName = firstName_tb.Text,
                LastName = lastName_tb.Text,
                Patronymic = patronymic_tb.Text,

                GenderId = (gender_cb.SelectedItem as Gender).Id,
                Birthday = date,

                ContactsId = contacts.Id,
                Address = address_tb.Text,
                Town = town_tb.Text,

                BusynessId = busyness,
                PhotoId = CreatePhoto(),
                Comments = comments_tb.Text,
                StatusId = status,
                LastCompany = lastCompany_tb.Text,
                LastJobTitle = lastJobTitle_tb.Text,
                EducationId = education,
                EducationInstution = educationInstution_tb.Text
            };

            // Сохраняем изменения в базе данных
            using (HrHelperDatabaseContext db = new HrHelperDatabaseContext())
            {
                db.Update(newSummary);
                db.SaveChanges();
            }

            try
            {
                // Создаем новый объект резюме для вакансии
                SummaryForVacancy summaryFor = new SummaryForVacancy()
                {
                    SummaryId = summary.Id,
                    VacancyId = vacancy
                };
                using (HrHelperDatabaseContext db = new HrHelperDatabaseContext())
                {
                    db.Add(newSummary);
                    db.SaveChanges();
                }
            }
            catch
            { }

            // Выводим сообщение об успешном сохранении изменений
            Classes.MyMessageBox.Show("Внимание", "Резюме обновлено.");

            // Переходим на страницу резюме
            Classes.Settings.mainFrame.Navigate(new Summary_page(summary.Id));
        }

        // Метод создания фотографии
        int? CreatePhoto()
        {
            // Если путь к фотографии не указан, возвращаем null
            if (photoPath == null || photoPath == String.Empty)
                return null;

            // Если фотография уже существует, возвращаем ее id
            if (summary.Photo != null)
                if (photoPath == summary.Photo.Path)
                    return summary.Photo.Id;

            // Создаем новый объект фотографии и сохраняем его в базе данных
            string path = Classes.PhotoFolder.AddPhoto(photoPath, $"{firstName_tb.Text} {lastName_tb.Text} {patronymic_tb.Text}", photoFormat);
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

        private void LoadGenderCombobox()
        {
            using (HrHelperDatabaseContext db = new HrHelperDatabaseContext())
            {
                gender_cb.ItemsSource = db.Genders.ToArray();
                gender_cb.DisplayMemberPath = "Name";
            }

        }

        private void phone_tb_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void dontSave_but_Click(object sender, RoutedEventArgs e)
        {
            Settings.mainFrame.Navigate(new Summary_page(summary.Id));
        }
    }
}
