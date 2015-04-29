using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Windows;

namespace CCT.NUI.Core.Video
{
    public class RgbImageSourceFactory : IImageFactory
    {
        public RgbImageSourceFactory()
        { }

        public unsafe void CreateImage(WriteableBitmap target, IntPtr pointer)
        {
            Int32Rect rectangle = default(Int32Rect);
            target.Dispatcher.Invoke(new Action(() => 
                {
                    rectangle = new Int32Rect(0, 0, target.PixelWidth, target.PixelHeight);
                }));

            byte* pImage = (byte*)pointer.ToPointer();

            var pixelCount = rectangle.Width * rectangle.Height;
            var buffer = new byte[pixelCount * 3];

            for (int index = 0; index < pixelCount; index++)
            {
                buffer[index * 3] = pImage[2];
                buffer[index * 3 + 1] = pImage[1];
                buffer[index * 3 + 2] = pImage[0];
                pImage += 3;
            }

            target.Dispatcher.Invoke(new Action(() =>
            {
                target.Lock();
                target.WritePixels(rectangle, buffer, rectangle.Width * 3, 0);
                target.Unlock();
            }));
        }
    }
}
