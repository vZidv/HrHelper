
using Newtonsoft.Json;
using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace HrHelper.Classes
{
    static public class Settings
    {
        // Основной кадр приложения
        static public Frame mainFrame { get; set; }

        // Главное окно приложения
        static public Window mainWindow { get; set; }

        // Последнее активное окно приложения
        static public Window? lastWindow { get; set; }

        // Текущий авторизованный пользователь
        static public AuthorizationUser currentUser { get; set; }

        /// <summary>
        /// Получает значение свойства из файла appSettings.json
        /// </summary>
        /// <param name="propertyName">Имя свойства</param>
        /// <returns>Значение свойства в виде строки</returns>
        static public string GetFrommJson(String propertyName)
        {
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "appSettings.json");
            string json = File.ReadAllText(filePath);

            dynamic jsonObj = JsonConvert.DeserializeObject(json);

            string value = jsonObj[$"{propertyName}"].ToString();

            return value;
        }

        /// <summary>
        /// Записывает значение свойства в файл appSettings.json
        /// </summary>
        /// <param name="propertyName">Имя свойства</param>
        /// <param name="propertyValue">Значение свойства</param>
        static public void WriteToJson(string propertyName, string propertyValue)
        {
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "appSettings.json");
            string json = File.ReadAllText(filePath);

            dynamic jsonObj = JsonConvert.DeserializeObject(json);

            jsonObj[propertyName] = propertyValue;

            // Сериализация объекта JSON с отступами для лучшей читаемости
            string output = JsonConvert.SerializeObject(jsonObj, Formatting.Indented);
            File.WriteAllText(filePath, output);
        }
    }
}
