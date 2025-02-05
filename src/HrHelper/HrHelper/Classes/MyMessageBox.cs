using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace HrHelper.Classes
{
    /// <summary>
    /// Перечисление, определяющее типы кнопок в окне сообщения
    /// </summary>
    public enum MyMessageBoxOptions
    {
        YesNo, // Две кнопки: Да и Нет
        Ok // Одна кнопка: ОК
    }

    /// <summary>
    /// Класс, представляющий пользовательский MessageBox
    /// </summary>
    public static class MyMessageBox
    {
        /// <summary>
        /// Создание экземпляра MessageBox
        /// </summary>
        /// <param name="title">Заголовок MessageBox</param>
        /// <param name="message">Сообщение MessageBox</param>
        /// <param name="messageBoxButtons">Тип кнопок в MessageBox</param>
        /// <returns>Экземпляр MessageBox</returns>
        static Windows.Messagebox_win CreateMessageBox(string title, string message, MyMessageBoxOptions messageBoxButtons)
        {
            Windows.Messagebox_win messagebox = new Windows.Messagebox_win(messageBoxButtons);
            messagebox.title = title;
            messagebox.message = message;
            return messagebox;
        }

        /// <summary>
        /// Отображение MessageBox с одной кнопкой ОК
        /// </summary>
        /// <param name="title">Заголовок MessageBox</param>
        /// <param name="message">Сообщение MessageBox</param>
        public static void Show(string title, string message)
        {
            Windows.Messagebox_win messagebox = CreateMessageBox(title, message, MyMessageBoxOptions.Ok);
            SystemSounds.Exclamation.Play();
            messagebox.ShowDialog();
        }

        /// <summary>
        /// Отображение MessageBox с двумя кнопками Да и Нет
        /// </summary>
        /// <param name="title">Заголовок MessageBox</param>
        /// <param name="message">Сообщение MessageBox</param>
        /// <param name="messageBoxButtons">Тип кнопок в MessageBox</param>
        /// <returns>True, если была нажата кнопка Да, иначе - false</returns>
        public static bool Show(string title, string message, MyMessageBoxOptions messageBoxButtons)
        {
            Windows.Messagebox_win messagebox = CreateMessageBox(title, message, messageBoxButtons);
            SystemSounds.Exclamation.Play();
            messagebox.ShowDialog();
            return messagebox.result;
        }

        /// <summary>
        /// Отображение MessageBox с одной кнопкой ОК и возможностью указать, что это сообщение об ошибке
        /// </summary>
        /// <param name="title">Заголовок MessageBox</param>
        /// <param name="message">Сообщение MessageBox</param>
        /// <param name="IsError">True, если это сообщение об ошибке, иначе - false</param>
        /// <returns>True, если была нажата кнопка ОК, иначе - false</returns>
        public static bool Show(string title, string message, bool IsError)
        {
            Windows.Messagebox_win messagebox = CreateMessageBox(title, message, MyMessageBoxOptions.Ok);
            SystemSounds.Exclamation.Play();
            messagebox.error = IsError;
            messagebox.ShowDialog();
            return messagebox.result;
        }
    }
}
