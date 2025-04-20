using Microsoft.Win32;
using System.IO;
using System.Windows;
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

        public static void OpenViewer(List<string> imageFiles)
        {
            MainWindow mainWindow = new(imageFiles);
            mainWindow.Show();
        }

        public static List<string>? ProceedFiles(string[] paths)
        {
            List<string> imageFiles = new List<string>();

            foreach (string path in paths)
            {
                if (File.Exists(path))
                {
                    if (IsImageFile(path))
                        imageFiles.Add(path);
                }
                else if (Directory.Exists(path))
                {
                    imageFiles.AddRange(GetImageFilesFromDirectory(path));
                }
            }

            if (imageFiles.Count > 0)
            {
                return imageFiles;
            }
            else
            {
                return null;
            }
        }

        public static bool IsImageFile(string filePath)
        {
            string[] imageExtensions = { ".jpg", ".jpeg", ".png", ".bmp", ".gif" };
            string extension = Path.GetExtension(filePath).ToLower();
            return imageExtensions.Contains(extension);
        }

        public static IEnumerable<string> GetImageFilesFromDirectory(string directoryPath)
        {
            string[] imageExtensions = { ".jpg", ".jpeg", ".png", ".bmp", ".gif" };
            return Directory.EnumerateFiles(directoryPath, "*.*", SearchOption.AllDirectories)
                           .Where(f => imageExtensions.Contains(Path.GetExtension(f).ToLower()));
        }
    }
}
