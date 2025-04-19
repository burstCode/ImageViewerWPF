using System.Windows;

namespace ImageViewerWPF
{
    public static class DragAndDropHelper
    {
        public static readonly DependencyProperty IsDragOverProperty =
            DependencyProperty.RegisterAttached("IsDragOver", typeof(bool), typeof(DragAndDropHelper),
                new PropertyMetadata(false));

        public static bool GetIsDragOver(DependencyObject obj) => (bool)obj.GetValue(IsDragOverProperty);
        public static void SetIsDragOver(DependencyObject obj, bool value) => obj.SetValue(IsDragOverProperty, value);

        public static readonly DependencyProperty IsEnabledProperty =
            DependencyProperty.RegisterAttached("IsEnabled", typeof(bool), typeof(DragAndDropHelper),
                new PropertyMetadata(false, OnIsEnabledChanged));

        public static void OnIsEnabledChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is UIElement element)
            {
                if ((bool)e.NewValue)
                {
                    element.DragOver += OnDragOver;
                    element.DragLeave += OnDragLeave;
                    element.Drop += OnDrop;
                }
                else
                {
                    element.DragOver -= OnDragOver;
                    element.DragLeave -= OnDragLeave;
                    element.Drop -= OnDrop;
                }
            }
        }

        public static void OnDragOver(object sender, DragEventArgs e)
        {
            if (sender is DependencyObject d)
            {
                SetIsDragOver(d, true);
            }

            if (IsValidDragData(e.Data))
            {
                e.Effects = DragDropEffects.Copy;
                e.Handled = true;
            }
        }

        public static void OnDragLeave(object sender, DragEventArgs e)
        {
            if (sender is DependencyObject d)
            {
                SetIsDragOver(d, false);
            }
        }

        public static void OnDrop(object sender, DragEventArgs e)
        {
            if (sender is DependencyObject d)
            {
                SetIsDragOver(d, false);
            }

            if (IsValidDragData(e.Data))
            {
                HandleDrop(e.Data);
                e.Handled = true;
            }
        }

        public static bool IsValidDragData(IDataObject data)
        {
            return data.GetDataPresent(DataFormats.FileDrop) ||
                   data.GetDataPresent(DataFormats.Bitmap);
        }

        public static void HandleDrop(IDataObject data)
        {
            // Ваша логика обработки перетаскивания
            if (data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])data.GetData(DataFormats.FileDrop);
                // Обработка файлов и папок
            }
        }

        public static bool GetIsEnabled(DependencyObject obj) => (bool)obj.GetValue(IsEnabledProperty);
        public static void SetIsEnabled(DependencyObject obj, bool value) => obj.SetValue(IsEnabledProperty, value);
    }
}
