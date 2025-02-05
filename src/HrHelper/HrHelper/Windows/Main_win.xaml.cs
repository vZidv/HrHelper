using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using HrHelper.Classes;
using Wpf.Ui.Controls;

namespace HrHelper.Windows
{
    public partial class Main_win : Window
    {
        public Frame frame
        {
            get { return frameMain; }

        }
        AuthorizationUser use { get; set; }


        //AuthorizationUser user
        public Main_win(AuthorizationUser user)
        {
            InitializeComponent();

            this.use = user;
            Settings.currentUser = user;

            ConfigureUserWindow(user);

            if (!PhotoFolder.CheckPhotoFolder())
                PhotoFolder.CreatePhotoFolder();

            
            Settings.mainFrame = frameMain;
            Settings.mainWindow = this;
        }
        //User Menu buttons
        #region User Menu
        private void requestVacancy_but_Click(object sender, RoutedEventArgs e) => frameMain.Navigate(new Pages.RequestVacancyList_page());
        private void person_but_Click(object sender, RoutedEventArgs e) => frameMain.Navigate(new Pages.SummaryList_page());                          
        private void settings_but_Click(object sender, RoutedEventArgs e) => frameMain.Navigate(new Pages.Settings_page());
        private void vacancy_but_Click(object sender, RoutedEventArgs e) => frameMain.Navigate(new Pages.VacancyList_page());
        private void users_but_Click(object sender, RoutedEventArgs e) => frameMain.Navigate(new Pages.UserList_page());
        private void settings_but_Click_1(object sender, RoutedEventArgs e) => frameMain.Navigate(new Pages.Settings_page());
        private void exit_but_Click(object sender, RoutedEventArgs e)
        {
            Authorization_win win = new Authorization_win();
            win.Show();
            this.Close();
        }

        #endregion
        private void ConfigureUserWindow(AuthorizationUser user)
        {
            switch(user.UserType.Type)
            {
                case "admin":
                    users_but.Visibility = Visibility.Visible;
                    frameMain.Navigate(new Pages.SummaryList_page());
                    break;
                case "user":
                    users_but.Visibility = Visibility.Hidden;
                    frameMain.Navigate(new Pages.SummaryList_page());
                    break;
                case "client":
                    frameMain.Navigate(new Pages.RequestVacancyList_page());
                    users_but.Visibility = Visibility.Hidden;
                    person_but.Visibility = Visibility.Hidden;
                    vacancy_but.Visibility = Visibility.Hidden;
                    break;

            }
        }


    }
}
