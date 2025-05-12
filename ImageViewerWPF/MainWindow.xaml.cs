using System.ComponentModel;
using System.Windows;
using System.Windows.Media.Imaging;
using System.IO;
using System.Windows.Controls;
using System.Windows.Media;

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
        private string _currentImagePath => _imageFiles.Count > 0 ? _imageFiles[_imageIndex] : null;

        private ImageDisplayMode _currentDisplayMode = ImageDisplayMode.FitToWindow;
        private double _currentScale = 1.0;

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

        public Image CurrentImage { get; private set; }

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            _imageFiles = new List<string>();

            SizeChanged += MainWindow_SizeChanged;
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

        private void MainWindow_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            ApplyDisplayMode();
        }

        private void ApplyDisplayMode()
        {
            if (CurrentImage == null || CurrentImage.Source == null) return;

            switch (_currentDisplayMode)
            {
                case ImageDisplayMode.Normal:
                    ImageScrollViewer.Visibility = Visibility.Visible;
                    FitImageView.Visibility = Visibility.Collapsed;
                    CurrentImageView.Stretch = Stretch.None;
                    CurrentImageView.StretchDirection = StretchDirection.Both;
                    ApplyScale(_currentScale);
                    break;

                case ImageDisplayMode.FitToWindow:
                    ImageScrollViewer.Visibility = Visibility.Collapsed;
                    FitImageView.Visibility = Visibility.Visible;
                    break;

                case ImageDisplayMode.PixelPerfect:
                    ImageScrollViewer.Visibility = Visibility.Visible;
                    FitImageView.Visibility = Visibility.Collapsed;
                    CurrentImageView.Stretch = Stretch.None;
                    CurrentImageView.StretchDirection = StretchDirection.Both;
                    ApplyScale(1.0);
                    break;
            }
        }

        private void ApplyScale(double scale)
        {
            _currentScale = scale;

            if (CurrentImage == null || CurrentImage.Source == null) return;

            var transform = new ScaleTransform(scale, scale);
            CurrentImageView.LayoutTransform = transform;

            // Обновляем ScrollViewer после изменения масштаба
            if (ImageScrollViewer.Visibility == Visibility.Visible)
            {
                ImageScrollViewer.ScrollToHorizontalOffset(0);
                ImageScrollViewer.ScrollToVerticalOffset(0);
            }
        }

        private void LoadImage()
        {
            if (_imageFiles.Count == 0)
            {
                CurrentImage = null;
                CurrentFileName = "Нет изображений";
                CurrentFileCounter = "0/0";
                ImageScrollViewer.Visibility = Visibility.Collapsed;
                FitImageView.Visibility = Visibility.Collapsed;
                return;
            }

            try
            {
                var bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri(_currentImagePath);
                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                bitmap.EndInit();

                ImageHelper.InitializeImage(_currentImagePath, bitmap);

                // Устанавливаем источник для обоих Image
                CurrentImage = ImageHelper.ApplyTransforms(_currentImagePath, CurrentImageView);
                FitImageView.Source = CurrentImage.Source;

                CurrentFileName = Path.GetFileName(_currentImagePath);
                CurrentFileCounter = $"{_imageIndex + 1}/{_imageFiles.Count}";

                // Применяем текущий режим отображения
                ApplyDisplayMode();
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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var pressedButton = sender as Button;
            if (pressedButton == null) { return; }

            switch (pressedButton.Content)
            {
                case "❮":
                    {
                        if (_imageIndex > 0)
                        {
                            _imageIndex--;
                            LoadImage();
                            UpdateNavigationButtons();
                        }

                        break;
                    }
                case "❯":
                    {
                        if (_imageIndex < _imageFiles.Count - 1)
                        {
                            _imageIndex++;
                            LoadImage();
                            UpdateNavigationButtons();
                        }

                        break;
                    }
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
                        ImageHelper.Rotate90DegreesClockwise(_currentImagePath, CurrentImageView);

                        break;
                    }
                case "Поворот на 90 градусов против часовой стрелке":
                    {
                        ImageHelper.Rotate90DegreesCounterClockwise(_currentImagePath, CurrentImageView);

                        break;
                    }
                case "Перевернуть по горизонтали":
                    {
                        ImageHelper.FlipHorizontally(_currentImagePath, CurrentImageView);

                        break;
                    }
                case "Перевернуть по вертикали":
                    {
                        ImageHelper.FlipVertically(_currentImagePath, CurrentImageView);

                        break;
                    }
                case "Сбросить преобразования":
                    {
                        ImageHelper.ResetTransforms(_currentImagePath, CurrentImageView);

                        break;
                    }
                case "Обычный":
                    _currentDisplayMode = ImageDisplayMode.Normal;
                    ApplyDisplayMode();
                    break;
                case "Масштаб 1:1 (пиксель-в-пиксель)":
                    _currentDisplayMode = ImageDisplayMode.PixelPerfect;
                    ApplyDisplayMode();
                    break;
                case "Вписать в окно с сохранением пропорций":
                    _currentDisplayMode = ImageDisplayMode.FitToWindow;
                    ApplyDisplayMode();
                    break;
                case "25%":
                    ApplyScale(0.25);
                    _currentDisplayMode = ImageDisplayMode.Normal;
                    break;
                case "50%":
                    ApplyScale(0.5);
                    _currentDisplayMode = ImageDisplayMode.Normal;
                    break;
                case "75%":
                    ApplyScale(0.75);
                    _currentDisplayMode = ImageDisplayMode.Normal;
                    break;
                case "100%":
                    ApplyScale(1.0);
                    _currentDisplayMode = ImageDisplayMode.Normal;
                    break;
                case "200%":
                    ApplyScale(2.0);
                    _currentDisplayMode = ImageDisplayMode.Normal;
                    break;
                case "300%":
                    ApplyScale(3.0);
                    _currentDisplayMode = ImageDisplayMode.Normal;
                    break;
                case "400%":
                    ApplyScale(4.0);
                    _currentDisplayMode = ImageDisplayMode.Normal;
                    break;
                case "500%":
                    ApplyScale(5.0);
                    _currentDisplayMode = ImageDisplayMode.Normal;
                    break;
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
