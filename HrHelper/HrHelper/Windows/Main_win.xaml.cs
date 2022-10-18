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
        public Main_win()
        {
            InitializeComponent();            
            if(!PhotoFolder.CheckPhotoFolder())
                PhotoFolder.CreatePhotoFolder();
            frameMain.Navigate(new Pages.Main_page());

            Settings.mainFrame = frameMain;
        }

        private void exit_but_Click(object sender, RoutedEventArgs e)
        {
           Application.Current.Shutdown();
        }

        private void person_but_Click(object sender, RoutedEventArgs e)
        {
            frameMain.Navigate(new Pages.Main_page());
        }
    }
}
