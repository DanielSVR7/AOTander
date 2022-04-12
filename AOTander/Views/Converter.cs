using System;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace AOTander.Views
{
    class BoolToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            Visibility ReturnValue = Visibility.Collapsed;

            switch ((bool)value)
            {
                case true: ReturnValue = Visibility.Visible; break;
                case false: ReturnValue = Visibility.Collapsed; break;
            }

            return ReturnValue;
        }

        public object ConvertBack(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            bool ReturnValue = false;

            switch ((Visibility)value)
            {
                case Visibility.Visible: ReturnValue = true; break;
                case Visibility.Collapsed: ReturnValue = false; break;
                case Visibility.Hidden: ReturnValue = false; break;
            }

            return ReturnValue;
        }
    }
    class BitmapToImageSourceConverter : IValueConverter
    {
        public object Convert(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            using (MemoryStream memory = new MemoryStream())
            {
                Bitmap bitmap = (Bitmap)value;
                bitmap.Save(memory, System.Drawing.Imaging.ImageFormat.Bmp);
                memory.Position = 0;
                BitmapImage bitmapimage = new BitmapImage();
                bitmapimage.BeginInit();
                bitmapimage.StreamSource = memory;
                bitmapimage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapimage.EndInit();

                return bitmapimage;
            }
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

