using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace HrHelper.Windows
{
    /// <summary>
    /// Interaction logic for Admin_win.xaml
    /// </summary>
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

        private void user_but_Click(object sender, RoutedEventArgs e) => frameMain.Navigate(new Pages.Admin_page());


    }
}
