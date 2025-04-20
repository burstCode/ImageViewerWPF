using System.IO;
using System.Windows;
using System.Windows.Controls;

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
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                
                var imageFiles = FileHelper.ProceedFiles(files);
                if (imageFiles == null) { return; }

                FileHelper.OpenViewer(imageFiles);

                Close();
            }
            e.Handled = true;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var pressedButton = sender as Button;

            if (pressedButton == null) { MessageBox.Show("Произошла ошибка при обработке нажатия кнопки"); return; }

            switch (pressedButton.Content)
            {
                case "Открыть папку":
                    {
                        var folders = FileHelper.OpenFolder();
                        if (folders == null) { break; }

                        var imageFiles = FileHelper.ProceedFiles(folders);
                        if (imageFiles == null) { break; }

                        FileHelper.OpenViewer(imageFiles);
                        Close();

                        break;
                    }
                case "Открыть файл":
                    {
                        var files = FileHelper.OpenFile();
                        if (files == null) { break; }

                        var imageFiles = FileHelper.ProceedFiles(files);
                        if (imageFiles == null) { break; }

                        FileHelper.OpenViewer(imageFiles);
                        Close();

                        break;
                    }
                default:
                    {
                        MessageBox.Show("Не обнаружено изображений формата \".jpg\", \".jpeg\", \".png\", \".bmp\", \".gif\"");
                        return;
                    }
            }

            return;
        }
    }
}
