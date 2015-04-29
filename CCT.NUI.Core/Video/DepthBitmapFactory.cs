using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.ExceptionServices;
using System.Runtime.InteropServices;

namespace CCT.NUI.Core.Video
{
    public class DepthBitmapFactory : DepthImageFactoryBase, IBitmapFactory
    {
        public DepthBitmapFactory(int maxDepth)
            : base(maxDepth)
        { }

        [HandleProcessCorruptedStateExceptions]
        public unsafe void CreateImage(Bitmap targetImage, IntPtr pointer)
        {
            var area = new System.Drawing.Rectangle(0, 0, targetImage.Width, targetImage.Height); 
            this.CreateHistogram(pointer, area.Width, area.Height);

            var data = targetImage.LockBits(area, ImageLockMode.WriteOnly, System.Drawing.Imaging.PixelFormat.Format24bppRgb);

            try
            {
                ushort* pDepth = (ushort*)pointer;
                for (int y = 0; y < area.Height; y++)
                {
                    byte* pDest = (byte*)data.Scan0.ToPointer() + y * data.Stride;
                    for (int x = 0; x < area.Width; x++, ++pDepth, pDest += 3)
                    {
                        byte pixel = (byte)histogram.GetValue(*pDepth);
                        pDest[0] = pixel;
                        pDest[1] = pixel;
                        pDest[2] = pixel;
                    }
                }
            }
            catch (AccessViolationException)
            { }
            catch (SEHException)
            { }

            targetImage.UnlockBits(data);
        }
    }
}
