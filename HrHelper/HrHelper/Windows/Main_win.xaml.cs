using System.Windows;
using System.Windows.Input;
using HrHelper.Classes;

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
        //Toolbar Buttons
        #region Toolbar Buttons
        private void minWind_but_Click(object sender, RoutedEventArgs e) => WindowState = WindowState.Minimized;
        private void close_but_Click(object sender, RoutedEventArgs e) => Application.Current.Shutdown();
        private void toolBar_grid_MouseDown(object sender, MouseButtonEventArgs e) => DragMove();
        private void maxWind_but_Click(object sender, RoutedEventArgs e)
        {
            if (WindowState == System.Windows.WindowState.Maximized)
                WindowState = System.Windows.WindowState.Normal;
            else
                WindowState = System.Windows.WindowState.Maximized;
        }
        #endregion

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
