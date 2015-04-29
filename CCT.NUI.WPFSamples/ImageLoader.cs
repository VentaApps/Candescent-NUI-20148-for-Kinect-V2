using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Media;

namespace CCT.NUI.WPFSamples
{
    internal class ImageLoader
    {
        public ImageLoader()
        { }

        public IList<Image> LoadImages() 
        {
            var result = new List<Image>();
            foreach (var imagePath in Directory.GetFiles(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Images"), "*.jpg"))
            {
                var source = new BitmapImage();
                source.BeginInit();
                source.UriSource = new Uri(imagePath);
                source.CacheOption = BitmapCacheOption.OnLoad;
                source.EndInit();
                var image = new Image { Source = source };
                image.Width = 600;
                image.Height = source.Height * image.Width / source.Width;
                result.Add(image);
            }
            return result;
        }
    }
}
