using System.Windows.Media.Imaging;
using System.Windows.Media;

namespace ImageViewerWPF
{
    public class ImageState
    {
        public BitmapSource OriginalImage { get; }
        public TransformGroup Transforms { get; set; } = new TransformGroup();

        public ImageState(BitmapSource originalImage)
        {
            OriginalImage = originalImage;
        }

        public BitmapSource GetTransformedImage()
        {
            if (OriginalImage == null) return null;

            var transformed = new TransformedBitmap();
            transformed.BeginInit();
            transformed.Source = OriginalImage;
            transformed.Transform = Transforms;
            transformed.EndInit();

            return transformed;
        }
    }
}
