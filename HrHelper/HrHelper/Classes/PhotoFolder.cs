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
            string path = Directory.GetCurrentDirectory();
            int number = path.Length - 21;
            path = path.Remove(number, 21);
            return path;
        }
        static public bool CheckPhotoFolder()
        {
            string path = ProjectPath();
            path += "\\Photo";
            return Directory.Exists(path);
        }
        static public void CreatePhotoFolder()
        {
            string path = ProjectPath();
            Directory.CreateDirectory(path + "\\Photo");
        }
    }
}
