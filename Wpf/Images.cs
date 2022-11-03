using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using Color = System.Drawing.Color;

namespace Wpf
{
    public static class Images
    {
        private static Dictionary<string, Bitmap> _cache = new();

        public static Bitmap Load(string imageUri)
        {
            if (_cache.ContainsKey(imageUri))
            {
                return _cache[imageUri];
            }
            else
            {
                _cache[imageUri] = new Bitmap(imageUri);
                return _cache[imageUri];
            }
        }

        public static void Clear()
        {
            _cache.Clear();
        }

        public static Bitmap EmptyBitmap(int width, int height)
        {
            string key = "empty";
            if (!_cache.ContainsKey(key))
            {
                _cache.Add(key, new Bitmap(width, height));
                Graphics graphics = Graphics.FromImage(_cache[key]);
                graphics.FillRectangle(new SolidBrush(Color.Gray), 0, 0, width, height);
            }

            return (Bitmap)_cache[key].Clone();
        }

        public static BitmapSource CreateBitmapSourceFromGdiBitmap(Bitmap bitmap)
        {
            if (bitmap == null)
                throw new ArgumentNullException("bitmap");

            var rect = new Rectangle(0, 0, bitmap.Width, bitmap.Height);

            var bitmapData = bitmap.LockBits(
                rect,
                ImageLockMode.ReadWrite,
                System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            try
            {
                var size = (rect.Width * rect.Height) * 4;

                return BitmapSource.Create(
                    bitmap.Width,
                    bitmap.Height,
                    bitmap.HorizontalResolution,
                    bitmap.VerticalResolution,
                    PixelFormats.Bgra32,
                    null,
                    bitmapData.Scan0,
                    size,
                    bitmapData.Stride);
            }
            finally
            {
                bitmap.UnlockBits(bitmapData);
            }
        }
    }
}
