using HrHelper.Classes;
using HrHelper.Windows;
using PdfSharp.Pdf.Content.Objects;
using System;
using System.Diagnostics;
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

        // Метод для изменения темы цветов приложения
        private void ChangeColorsTheme(string theme)
        {
            try
            {
                // Создаем объект Uri для файла ресурсов XAML с указанным именем темы
                Uri url = new Uri($@"Dictionary\{theme}", UriKind.Relative);

                // Получаем объект приложения
                var app = (App)Application.Current;

                // Вызываем метод ChangeTheme объекта приложения, чтобы изменить тему цветов на новую тему, определенную в файле ресурсов
                app.ChangeTheme(url);

                Classes.Settings.WriteToJson("AppTheme", $"{theme}");
            }
            catch(Exception ex)
            {
                MyMessageBox.Show("Ошибка", ex.Message, true);
            }
        }

        private void themeColors_cb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {         
            if (themeColors_cb.SelectedIndex  == 0)
            {
                ChangeColorsTheme("WhiteThemeColors.xaml");
            }
                
            else if (themeColors_cb.SelectedIndex == 1)
            {
                ChangeColorsTheme("BlackThemeColors.xaml");
            }
                
        }



        private void userGuide_tblock_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            // Указываем путь к файлу руководства пользователя
            string userManualPath = Path.Combine(Directory.GetCurrentDirectory(), "Руководство пользователя.chm");

            // Проверяем, существует ли файл по указанному пути
            if (File.Exists(userManualPath))
            {
                // Открываем файл руководства пользователя во внешней программе
                ProcessStartInfo processStartInfo = new ProcessStartInfo(userManualPath);
                processStartInfo.UseShellExecute = true;
                Process.Start(processStartInfo);
            }
            else
            {
                // Если файл не найден, выводим сообщение об ошибке
                MyMessageBox.Show("Ошибка", "Файл руководства пользователя не найден по указанному пути.", true);
            }
        }
    }
}
