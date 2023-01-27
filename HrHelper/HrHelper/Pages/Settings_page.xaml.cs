using System;
using System.Windows;
using System.Windows.Controls;

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

        private void ChangeColorsTheme(string newtheme, string nowtheme)
        {
            Uri newThemeuri = new Uri($@"Dictionary\{newtheme}",UriKind.Relative);
            Uri nowThemeuri = new Uri($@"Dictionary\{nowtheme}", UriKind.Relative);
            ResourceDictionary newThemeDictionary = Application.LoadComponent(newThemeuri) as ResourceDictionary;
            ResourceDictionary nowThemeDictionary = Application.LoadComponent(nowThemeuri) as ResourceDictionary;

            Application.Current.Resources.Remove(nowThemeDictionary);
            Application.Current.Resources.MergedDictionaries.Add(newThemeDictionary);           
        }

        private void themeColors_cb_SelectionChanged(object sender, SelectionChangedEventArgs e) => ChangeColorsTheme("WhiteThemeColors.xaml", "BlackThemeColors.xaml");
    }
}
