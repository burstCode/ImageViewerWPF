using System.ComponentModel;
using System.Windows;
using System.Windows.Media.Imaging;
using System.IO;
using System.Windows.Controls;

namespace ImageViewerWPF
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private readonly List<string> _imageFiles;
        private int _imageIndex;
        private string _currentFileName;
        private string _currentFileCounter;

        public event PropertyChangedEventHandler PropertyChanged;

        public string CurrentFileName
        {
            get => _currentFileName;
            set
            {
                _currentFileName = value;
                OnPropertyChanged(nameof(CurrentFileName));
            }
        }

        public string CurrentFileCounter
        {
            get => _currentFileCounter;
            set
            {
                _currentFileCounter = value;
                OnPropertyChanged(nameof(CurrentFileCounter));
            }
        }

        public BitmapImage CurrentImage { get; private set; }

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            _imageFiles = new List<string>();
        }

        public MainWindow(List<string> imageFiles) : this()
        {
            _imageFiles = imageFiles;
            _imageIndex = 0;
            LoadImage();
            UpdateNavigationButtons();
        }

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void LoadImage()
        {
            if (_imageFiles.Count == 0)
            {
                CurrentImage = null;
                CurrentFileName = "Нет изображений";
                CurrentFileCounter = "0/0";
                return;
            }

            try
            {
                var bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri(_imageFiles[_imageIndex]);
                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                bitmap.EndInit();
                CurrentImage = bitmap;

                CurrentFileName = Path.GetFileName(_imageFiles[_imageIndex]);
                CurrentFileCounter = $"{_imageIndex + 1}/{_imageFiles.Count}";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки изображения: {ex.Message}");
            }

            OnPropertyChanged(nameof(CurrentImage));
        }

        private void UpdateNavigationButtons()
        {
            PrevButton.IsEnabled = _imageIndex > 0;
            NextButton.IsEnabled = _imageIndex < _imageFiles.Count - 1;
        }

        private void PrevButton_Click(object sender, RoutedEventArgs e)
        {
            if (_imageIndex > 0)
            {
                _imageIndex--;
                LoadImage();
                UpdateNavigationButtons();
            }
        }

        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            if (_imageIndex < _imageFiles.Count - 1)
            {
                _imageIndex++;
                LoadImage();
                UpdateNavigationButtons();
            }
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            var pressedMenuItem = sender as MenuItem;

            if (pressedMenuItem == null) { MessageBox.Show("Произошла ошибка при обработке нажатия меню"); return; }

            switch (pressedMenuItem.Header)
            {
                case "Открыть файл":
                    {
                        var folders = FileHelper.OpenFolder();
                        if (folders == null) { break; }

                        var imageFiles = FileHelper.ProceedFiles(folders);
                        if (imageFiles == null) { break; }

                        FileHelper.OpenViewer(imageFiles);
                        Close();

                        break;
                    }
                case "Открыть папку":
                    {
                        var files = FileHelper.OpenFile();
                        if (files == null) { break; }

                        var imageFiles = FileHelper.ProceedFiles(files);
                        if (imageFiles == null) { break; }

                        FileHelper.OpenViewer(imageFiles);
                        Close();

                        break;
                    }
                case "Закрыть":
                    {
                        StartWindow startWindow = new StartWindow();
                        startWindow.Show();
                        Close();

                        break;
                    }
                case "Поворот на 90 градусов по часовой стрелке":
                    {
                        break;
                    }
                case "Поворот на 90 градусов против часовой стрелке":
                    {
                        break;
                    }
                case "Перевернуть по горизонтали":
                    {
                        break;
                    }
                case "Перевернуть по вертикали":
                    {
                        break;
                    }
                case "Масштаб 1:1 (пиксель-в-пиксель)":
                    {
                        break;
                    }
                case "Вписать в окно с сохранением пропорций":
                    {
                        break;
                    }
                case "25%":
                    {
                        break;
                    }
                case "50%":
                    {
                        break;
                    }
                case "75%":
                    {
                        break;
                    }
                case "100%":
                    {
                        break;
                    }
                case "200%":
                    {
                        break;
                    }
                case "300%":
                    {
                        break;
                    }
                case "400%":
                    {
                        break;
                    }
                case "500%":
                    {
                        break;
                    }
                case "О текущем файле":
                    {
                        break;
                    }
                case "О программе":
                    {
                        break;
                    }
            }
        }
    }
}
