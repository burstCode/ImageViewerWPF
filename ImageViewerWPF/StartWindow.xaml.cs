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
                OpenViewer(imageFiles);
            }
            else
            {
                MessageBox.Show("Пошел нахуй!", "Уебок", MessageBoxButton.OK, MessageBoxImage.Error);
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

        private void OpenViewer(List<string> imageFiles)
        {
            MainWindow mainWindow = new(imageFiles);
            mainWindow.Show();
            Close();
        }

        private void OpenFolderButton_Click(object sender, RoutedEventArgs e)
        {
            var folders = FileHelper.OpenFolder();

            if (folders == null) { MessageBox.Show("Не было выбрано никаких папок"); return; }

            ProcessDroppedFiles(folders);
        }

        private void OpenFileButton_Click(object sender, RoutedEventArgs e)
        {
            var files = FileHelper.OpenFile();

            if (files == null) { MessageBox.Show("Не было выбрано никаких файлов"); return; }

            ProcessDroppedFiles(files);
        }
    }
}
