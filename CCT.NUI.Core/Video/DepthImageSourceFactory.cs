using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Windows;
using System.Runtime.InteropServices;
using System.Runtime.ExceptionServices;

namespace CCT.NUI.Core.Video
{
    public class DepthImageSourceFactory : DepthImageFactoryBase, IImageFactory
    {        
         public DepthImageSourceFactory(int maxDepth)
             : base(maxDepth)
        { }

        [HandleProcessCorruptedStateExceptions]
        public unsafe void CreateImage(WriteableBitmap target, IntPtr pointer)
        {
            Int32Rect rectangle = default(Int32Rect);
            target.Dispatcher.Invoke(new Action(() =>
            {
                rectangle = new Int32Rect(0, 0, target.PixelWidth, target.PixelHeight);
            }));

            this.CreateHistogram(pointer, rectangle.Width, rectangle.Height);
            var pixelcount = rectangle.Width * rectangle.Height;
            var buffer = new byte[pixelcount * 3];
            try
            {
                ushort* pDepth = (ushort*)pointer;
                for (int index = 0; index < pixelcount; index++)
                {
                    byte pixel = (byte)histogram.GetValue(*pDepth);
                    buffer[index * 3] = pixel;
                    buffer[index * 3 + 1] = pixel;
                    buffer[index * 3 + 2] = pixel;
                    pDepth++;
                }
            }
            catch (AccessViolationException)
            { }
            catch (SEHException)
            { }

            target.Dispatcher.Invoke(new Action(() =>
                {
                    target.Lock();
                    target.WritePixels(rectangle, buffer, rectangle.Width * 3, 0);
                    target.Unlock();
                }));
        }
    }
}
