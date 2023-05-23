using System.Windows;
using System.Windows.Input;
using HrHelper.Classes;
using Wpf.Ui.Controls;

namespace HrHelper.Windows
{
    public partial class Main_win : Window
    {
        public Main_win()
        {
            InitializeComponent();

            if (!PhotoFolder.CheckPhotoFolder())
                PhotoFolder.CreatePhotoFolder();

            frameMain.Navigate(new Pages.Main_page());
            Settings.mainFrame = frameMain;
        }
        //User Menu buttons
        #region User Menu
        private void person_but_Click(object sender, RoutedEventArgs e) => frameMain.Navigate(new Pages.Main_page());                          
        private void settings_but_Click(object sender, RoutedEventArgs e) => frameMain.Navigate(new Pages.Settings_page());
        private void vacancy_but_Click(object sender, RoutedEventArgs e) => frameMain.Navigate(new Pages.Vacancy_page());
        private void exit_but_Click(object sender, RoutedEventArgs e)
        {
            Authorization_win win = new Authorization_win();
            win.Show();
            this.Close();
        }
        #endregion
    }
}
