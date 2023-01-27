using HrHelper.Classes;
using System.Windows;
using System.Windows.Input;

namespace HrHelper.Windows
{
    public partial class Messagebox_win : Window
    {
        public string title
        {
            get
            {
                return title_tb.Text;
            }
            set
            {
                title_tb.Text = value;
            }
        }
        public string message
        {
            get
            {
                return message_tb.Text;
            }
            set
            {
                message_tb.Text = value;
            }
        }
        public bool error
        {
            get
            {
                return error;
            }
            set
            {
                if (value == true)
                    errorIcon_path.Visibility = Visibility.Visible;
                else errorIcon_path.Visibility = Visibility.Hidden;
            }
        }
        public bool result = true;
        public Messagebox_win(MyMessageBoxOptions messageBoxoptions)
        {
            InitializeComponent();

            ChooseTypeButton(messageBoxoptions);
        }
        private void ChooseTypeButton(MyMessageBoxOptions messageBoxoptions)
        {
            if (messageBoxoptions == MyMessageBoxOptions.Ok)
                ok_sp.Visibility = Visibility.Visible;
            if (messageBoxoptions == MyMessageBoxOptions.YesNo)
                yesNo_sp.Visibility = Visibility.Visible;
        }

        //Toolbar buttons
        #region Toolbar buttons
        private void toolBar_grid_MouseDown(object sender, MouseButtonEventArgs e) => DragMove();

        private void close_but_Click(object sender, RoutedEventArgs e) => this.Close();

        private void minWind_but_Click(object sender, RoutedEventArgs e) => this.WindowState = WindowState.Minimized;
        #endregion

        //Types buttons
        #region Types buttons

        #region OK button
        private void ok_but_Click(object sender, RoutedEventArgs e) => this.Close();
        #endregion

        #region Yes/No button
        private void yes_but_Click(object sender, RoutedEventArgs e)
        {
            result = true;
            this.Close();
        }

        private void no_but_Click(object sender, RoutedEventArgs e)
        {
            result = false;
            this.Close();
        }
        #endregion

        #endregion
    }
}
