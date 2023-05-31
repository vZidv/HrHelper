using HrHelper.Classes;
using HrHelper.Windows;
using PdfSharp.Pdf.Content.Objects;
using System;
using System.IO;
using System.Security.Policy;
using System.Windows;
using System.Windows.Controls;
using Wpf.Ui.Appearance;

namespace HrHelper.Pages
{
    public partial class Settings_page : Page
    {
        public Settings_page() => InitializeComponent();

        private void deleteDb_but_Click(object sender, RoutedEventArgs e)
        {
            if (!Classes.MyMessageBox.Show("Внимание!", "Вы собираетесь удалить базу данных! " +
                "После удаления вся информация из базы данных будет удалена без возможности восстановления, программа автоматически закроется. Вы уверены, что хотите это сделать?", Classes.MyMessageBoxOptions.YesNo))
                return;
            using (HrHelperDatabaseContext db = new HrHelperDatabaseContext())
                db.Database.EnsureDeleted();
            Classes.MyMessageBox.Show("Внимание!", "База данных удалина!");

            Application.Current.Shutdown();
        }

        private void ChangeColorsTheme(string theme)
        {
            Uri url = new Uri($@"Dictionary\{theme}", UriKind.Relative);
            var app = (App)Application.Current;
            app.ChangeTheme(url);
            Application.Current.MainWindow.InvalidateVisual();
            Application.Current.MainWindow.UpdateLayout();
            this.UpdateLayout();
            this.InvalidateVisual();


        }

        private void themeColors_cb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {         
            if (themeColors_cb.SelectedIndex  == 0)
            {
                ChangeColorsTheme("WhiteThemeColors.xaml");
                Wpf.Ui.Appearance.Theme.Apply(
                  Wpf.Ui.Appearance.ThemeType.Light,     // Theme type
                  Wpf.Ui.Appearance.BackgroundType.Mica, // Background type
                  true                                   // Whether to change accents automatically
                );
            }
                
            else if (themeColors_cb.SelectedIndex == 1)
            {
                ChangeColorsTheme("BlackThemeColors.xaml");
                Wpf.Ui.Appearance.Theme.Apply(
                  Wpf.Ui.Appearance.ThemeType.Dark,     // Theme type
                  Wpf.Ui.Appearance.BackgroundType.Mica, // Background type
                  true                                   // Whether to change accents automatically
                );
            }
                
        }
    }
}
