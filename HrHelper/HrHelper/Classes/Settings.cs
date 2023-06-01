
using Newtonsoft.Json;
using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace HrHelper.Classes
{
    static public class Settings
    {
        static public Frame mainFrame { get; set; }
        static public Window mainWindow { get; set; }
        static public Window? lastWindow { get; set; }
        static public AuthorizationUser currentUser { get; set; }

        static public string GetFrommJson(String propertyName)
        {
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "appSettings.json");
            string json = File.ReadAllText(filePath);

            dynamic jsonObj = JsonConvert.DeserializeObject(json);

            string value = jsonObj[$"{propertyName}"].ToString();

            return value;
        }

        static public void WriteToJson(string propertyName, string propertyValue)
        {
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "appSettings.json");
            string json = File.ReadAllText(filePath);

            dynamic jsonObj = JsonConvert.DeserializeObject(json);

            jsonObj[propertyName] = propertyValue;

            string output = JsonConvert.SerializeObject(jsonObj, Formatting.Indented);
            File.WriteAllText(filePath, output);
        }
    }
}
