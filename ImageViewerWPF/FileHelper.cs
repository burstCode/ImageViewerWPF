using Microsoft.Win32;
using System.IO;
using static System.Environment;

namespace ImageViewerWPF
{
    public static class FileHelper
    {
        public static string[]? OpenFolder()
        {
            OpenFolderDialog ofd = new OpenFolderDialog();

            ofd.Multiselect = true;
            ofd.InitialDirectory = Environment.SpecialFolder.CommonPictures.ToString();

            if (ofd.ShowDialog() == true)
            {
                return ofd.FolderNames;
            }

            return null;
        }

        public static string[]? OpenFile()
        {
            OpenFileDialog ofd = new OpenFileDialog();
            
            ofd.Filter = "";
            ofd.Multiselect = true;
            ofd.InitialDirectory = Environment.SpecialFolder.CommonPictures.ToString();
            
            if (ofd.ShowDialog() == true)
            {
                return ofd.FileNames;
            }

            return null;
        }
    }
}
