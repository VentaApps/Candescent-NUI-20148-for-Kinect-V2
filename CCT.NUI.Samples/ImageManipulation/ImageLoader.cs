using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace CCT.NUI.Samples.ImageManipulation
{
    internal class ImageLoader
    {
        private int startPositionX;
        private int startPositionY;
        private int maxWidth;

        public ImageLoader(int startPositionX, int startPositionY, int maxWidth) 
        {
            this.startPositionX = startPositionX;
            this.startPositionY = startPositionY;
            this.maxWidth = maxWidth;
        }

        public IList<InteractiveImage> LoadImages() 
        {
            var result = new List<InteractiveImage>();

            int x = this.startPositionX, y = this.startPositionY;
            foreach (var imagePath in Directory.GetFiles(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Images"), "*.jpg"))
            {
                var newImage = new InteractiveImage((System.Drawing.Bitmap)System.Drawing.Bitmap.FromFile(imagePath), x, y);
                result.Add(newImage);
                x += (int)newImage.Area.Size.Width;
                if (x + newImage.Area.Size.Width > this.maxWidth)
                {
                    y += (int)newImage.Area.Size.Height;
                    x = startPositionX;
                }
            }

            return result;
        }
    }
}
