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
    /// Interaction logic for MinWin_win.xaml
    /// </summary>
    public partial class MinWin_win : Window
    {
        Page page;
        public MinWin_win(Page startPage)
        {
            InitializeComponent();

            page = startPage;

            Classes.Settings.mainFrame = mainFrame;
            Classes.Settings.mainWindow = this;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            mainFrame.Navigate(page);
        }
    }
}
