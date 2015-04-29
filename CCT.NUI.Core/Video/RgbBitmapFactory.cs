using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;

namespace CCT.NUI.Core.Video
{
    public class RgbBitmapFactory : IBitmapFactory
    {
        public RgbBitmapFactory()
        { }

        public unsafe void CreateImage(Bitmap targetImage, IntPtr pointer)
        {
            var area = new System.Drawing.Rectangle(0, 0, targetImage.Width, targetImage.Height);
            BitmapData bitmapData = targetImage.LockBits(area, ImageLockMode.WriteOnly, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            byte* pImage = (byte*)pointer.ToPointer();
            byte* pDest = (byte*)bitmapData.Scan0.ToPointer();

            int maxIndex = area.Width * area.Height;
            for (int index = 0; index < maxIndex; index++)
            {
                pDest[0] = pImage[2];
                pDest[1] = pImage[1];
                pDest[2] = pImage[0];
                pDest += 3;
                pImage += 3;
            }
            targetImage.UnlockBits(bitmapData);
        }
    }
}
