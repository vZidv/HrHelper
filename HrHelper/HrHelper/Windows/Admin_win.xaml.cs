using System.Windows;
using System.Windows.Input;

namespace HrHelper.Windows
{
    public partial class Admin_win : Window
    {
        public Admin_win()
        {
            InitializeComponent();

            frameMain.Navigate(new Pages.Admin_page());
            Classes.Settings.mainFrame = frameMain;
        }
        private void minWind_but_Click(object sender, RoutedEventArgs e) => WindowState = WindowState.Minimized;
        private void close_but_Click(object sender, RoutedEventArgs e) => Application.Current.Shutdown();
        private void toolBar_grid_MouseDown(object sender, MouseButtonEventArgs e) => DragMove();
        private void user_but_Click(object sender, RoutedEventArgs e) => frameMain.Navigate(new Pages.Admin_page());

        private void maxWind_but_Click(object sender, RoutedEventArgs e)
        {
            if (WindowState == System.Windows.WindowState.Maximized)
                WindowState = System.Windows.WindowState.Normal;
            else
                WindowState = System.Windows.WindowState.Maximized;
        }
        private void logOut_but_Click(object sender, RoutedEventArgs e)
        {
            Windows.Authorization_win authorization = new Authorization_win();
            authorization.Show();
            this.Close();
        }
    }
}
