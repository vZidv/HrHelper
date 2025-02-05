using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HrHelper.Classes
{
    public static class PhotoFolder
    {
        /// <summary>
        /// Возвращает текущий путь к проекту
        /// </summary>
        /// <returns>Строка, содержащая путь к проекту</returns>
        static public string ProjectPath()
        {
            return Directory.GetCurrentDirectory();
        }

        /// <summary>
        /// Проверяет наличие папки Photo в текущем каталоге проекта
        /// </summary>
        /// <returns>True, если папка существует, иначе - false</returns>
        static public bool CheckPhotoFolder()
        {
            try
            {
                string path = ProjectPath();
                path += "\\Photo";
                return Directory.Exists(path);
            }
            catch (Exception ex)
            {
                return MyMessageBox.Show("Ошибка", ex.Message, true);
            }
        }

        /// <summary>
        /// Создает папку Photo в текущем каталоге проекта
        /// </summary>
        static public void CreatePhotoFolder()
        {
            string path = ProjectPath();
            Directory.CreateDirectory(path + "\\Photo");
        }

        /// <summary>
        /// Добавляет фотографию в папку Photo и возвращает относительный путь к файлу
        /// </summary>
        /// <param name="sourcePath">Путь к исходному файлу</param>
        /// <param name="fileName">Имя файла</param>
        /// <param name="format">Формат файла (например, ".jpg")</param>
        /// <returns>Относительный путь к файлу в папке Photo</returns>
        static public string AddPhoto(string sourcePath, string fileName, string format)
        {
            string filePath = $"\\Photo\\{fileName}{format}";
            File.Copy(sourcePath, ProjectPath() + filePath, true);
            return filePath;
        }

    }
}
