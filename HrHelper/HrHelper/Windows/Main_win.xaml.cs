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
using System.Reflection;
using System.IO;
using HrHelper.Classes;

namespace HrHelper.Windows
{
    /// <summary>
    /// Interaction logic for Main_win.xaml
    /// </summary>
    public partial class Main_win : Window
    {
        public bool someBool = false;
        public Main_win()
        {
            InitializeComponent();
            try
            {
                if (!PhotoFolder.CheckPhotoFolder())
                    PhotoFolder.CreatePhotoFolder();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            frameMain.Navigate(new Pages.Main_page());

            Settings.mainFrame = frameMain;
        }

        private void exit_but_Click(object sender, RoutedEventArgs e)
        {
            Authorization_win win = new Authorization_win();
            win.Show();
            this.Close();
        }


        private void person_but_Click(object sender, RoutedEventArgs e)
        {
            frameMain.Navigate(new Pages.Main_page());
            ChangeColorBut(sender as Button);
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

        private void settings_but_Click(object sender, RoutedEventArgs e)
        { 
            frameMain.Navigate(new Pages.Settings_page());
            ChangeColorBut(sender as Button);
        }

        private void vacancy_but_Click(object sender, RoutedEventArgs e)
        {
            frameMain.Navigate(new Pages.Vacancy_page());
            ChangeColorBut(sender as Button);
        }


        Button butLast;
        private void ChangeColorBut(Button button)
        {
            //someBool = true;
            //button.Background = (SolidColorBrush)Application.Current.Resources["Grey"];
            //if (butLast != null)
            //{
            //    butLast.Background = (SolidColorBrush)Application.Current.Resources["Chanel"];
            //}
            //butLast = button;
        }
    }
}
