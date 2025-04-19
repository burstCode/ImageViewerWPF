using System.IO;
using System.Windows;

namespace ImageViewerWPF
{
    /// <summary>
    /// Interaction logic for StartWindow.xaml
    /// </summary>
    public partial class StartWindow : Window
    {
        public StartWindow()
        {
            InitializeComponent();
        }

        protected override void OnDragEnter(DragEventArgs e)
        {
            base.OnDragEnter(e);
            if (DragAndDropHelper.IsValidDragData(e.Data))
            {
                e.Effects = DragDropEffects.Copy;
            }
            e.Handled = true;
        }

        protected override void OnDrop(DragEventArgs e)
        {
            base.OnDrop(e);
            if (DragAndDropHelper.IsValidDragData(e.Data))
            {
                // Ваша логика обработки сброса файлов
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                ProcessDroppedFiles(files);
            }
            e.Handled = true;
        }

        private void ProcessDroppedFiles(string[] paths)
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
                // Открываем просмотрщик с найденными изображениями
                //OpenViewer(imageFiles);
            }
        }

        private bool IsImageFile(string filePath)
        {
            string[] imageExtensions = { ".jpg", ".jpeg", ".png", ".bmp", ".gif" };
            string extension = Path.GetExtension(filePath).ToLower();
            return imageExtensions.Contains(extension);
        }

        private IEnumerable<string> GetImageFilesFromDirectory(string directoryPath)
        {
            string[] imageExtensions = { ".jpg", ".jpeg", ".png", ".bmp", ".gif" };
            return Directory.EnumerateFiles(directoryPath, "*.*", SearchOption.AllDirectories)
                           .Where(f => imageExtensions.Contains(Path.GetExtension(f).ToLower()));
        }
    }
}
