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
        static public string ProjectPath()
        {
            return Directory.GetCurrentDirectory();          
        }
        static public bool CheckPhotoFolder()
        {
            try
            {
                string path = ProjectPath();
                path += "\\Photo";
                return Directory.Exists(path);
            }
            catch(Exception ex)
            {
               return MyMessageBox.Show("Ошибка", ex.Message, true);
            }
        }
        static public void CreatePhotoFolder()
        {
            string path = ProjectPath();
            Directory.CreateDirectory(path + "\\Photo");
        }

        static public string AddPhoto(string sourcePath, string fileName, string format)
        {
            string filePath = $"\\Photo\\{fileName}{format}";
            File.Copy(sourcePath, ProjectPath() + filePath, true);
            return filePath;
        }
    }
}
