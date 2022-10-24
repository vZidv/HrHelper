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
using System.IO;
namespace HrHelper.Windows
{
    /// <summary>
    /// Interaction logic for Authorization_win.xaml
    /// </summary>
    public partial class Authorization_win : Window
    {
        public Authorization_win()
        {
            InitializeComponent();

            mainFrame.Content = new Pages.Authorization_page() { authorization_win = this};
        }

        private void ToolBar_MouseDown(object sender, MouseButtonEventArgs e) => DragMove();

        private void close_but_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void minWind_but_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }
    }
}
