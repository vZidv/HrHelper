﻿using System;
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
        int busyness;
        int education;

        public SummaryAdd_page()
        {
            InitializeComponent();

            LoadStatusComboBox();
            LoadBusynnesComboBox();
            LoadEducationComboBox();
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


        private void summaryAdd_bt_Click(object sender, RoutedEventArgs e)
        {
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

                Busyness[] busynesses = db.Busynesses.ToArray();
                foreach (Busyness busynes in busynesses)
                {
                    if (busynes.Type == bussyness_cb.Text)
                        busyness = busynes.Id;
                }

                Education[] educations = db.Educations.ToArray();
                foreach (Education education in educations)
                {
                    if (education.EducationName == education_cb.Text)
                        this.education = education.Id;
                }

            }
            SummaryContact contacts = new SummaryContact()
            {
                Phone = phone_tb.Text,
                Email = email_tb.Text,
                Skype = skype_tb.Text
            };
            using (HrHelperDatabaseContext db = new HrHelperDatabaseContext())
            {
                db.Add(contacts);
                db.SaveChanges();
            }

            Summary summary = new Summary()
            {
                FirstName = firstName_tb.Text,
                LastName = lastName_tb.Text,
                Patronymic = patronymic_tb.Text,
                Gender = gender_cb.Text,
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
            using (HrHelperDatabaseContext db = new HrHelperDatabaseContext())
            {
                db.Add(summary);
                db.SaveChanges();
            }

            Classes.MyMessageBox.Show("Внимание", "Пользователь добавлен.");
            Classes.Settings.mainFrame.Navigate(new Main_page());
        }
        int? CreatePhoto()
        {
            if (photoPath == string.Empty)
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
