using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ImageViewerWPF
{
    public static class ImageHelper
    {
        private static Dictionary<string, ImageState> _imageStates = new Dictionary<string, ImageState>();

        public static void InitializeImage(string imagePath, BitmapSource image)
        {
            if (!_imageStates.ContainsKey(imagePath))
            {
                _imageStates[imagePath] = new ImageState(image);
            }
        }

        public static Image ApplyTransforms(string imagePath, Image image)
        {
            if (_imageStates.TryGetValue(imagePath, out var state))
            {
                image.Source = state.GetTransformedImage();
            }
            return image;
        }

        public static Image Rotate90DegreesClockwise(string imagePath, Image image)
        {
            if (_imageStates.TryGetValue(imagePath, out var state))
            {
                state.Transforms.Children.Add(new RotateTransform(90));
                return ApplyTransforms(imagePath, image);
            }
            return image;
        }

        public static Image Rotate90DegreesCounterClockwise(string imagePath, Image image)
        {
            if (_imageStates.TryGetValue(imagePath, out var state))
            {
                state.Transforms.Children.Add(new RotateTransform(-90));
                return ApplyTransforms(imagePath, image);
            }
            return image;
        }

        public static Image FlipVertically(string imagePath, Image image)
        {
            if (_imageStates.TryGetValue(imagePath, out var state))
            {
                state.Transforms.Children.Add(new ScaleTransform(1, -1));
                return ApplyTransforms(imagePath, image);
            }
            return image;
        }

        public static Image FlipHorizontally(string imagePath, Image imageControl)
        {
            if (_imageStates.TryGetValue(imagePath, out var state))
            {
                state.Transforms.Children.Add(new ScaleTransform(-1, 1));
                return ApplyTransforms(imagePath, imageControl);
            }
            return imageControl;
        }
        
        public static void ResetTransforms(string imagePath, Image imageControl)
        {
            if (_imageStates.TryGetValue(imagePath, out var state))
            {
                state.Transforms = new TransformGroup();
                imageControl.Source = state.OriginalImage;
            }
        }
    }
}
