using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using CCT.NUI.Core.Video;
using CCT.NUI.Core;

namespace CCT.NUI.Tests.Core
{
    [TestClass]
    public class RGBImageSourceFactoryTests
    {
        [TestMethod]
        public void Can_Create_Color_Image()
        {
            var imageFactory = new RgbImageSourceFactory();

            var pointerFactory = new ArrayToPointerFactory();
            var data = new ushort[20 * 10 * 2];
            for (int index = data.Length / 2; index < data.Length; index++)
            {
                data[index] = ushort.MaxValue;
            }

            var bitmap = new WriteableBitmap(20, 10, 96, 96, PixelFormats.Bgr24, null);
            var pointer = pointerFactory.CreatePointer(data);
            try
            {
                 imageFactory.CreateImage(bitmap, pointer);
            }
            finally
            {
                pointerFactory.Destroy(pointer);
            }

            int height = bitmap.PixelHeight;
            int width = bitmap.PixelWidth;
            int stride = bitmap.PixelWidth * 3;
            byte[] pixelByteArray = new byte[bitmap.PixelHeight * stride];
            bitmap.CopyPixels(pixelByteArray, stride, 0);

            Assert.AreEqual(0, pixelByteArray[0]);
            Assert.AreEqual(0, pixelByteArray[1]);
            Assert.AreEqual(0, pixelByteArray[2]);

            Assert.AreEqual(255, pixelByteArray[10 * 3 + 9 * stride]);
            Assert.AreEqual(255, pixelByteArray[10 * 3 + 9 * stride + 1]);
            Assert.AreEqual(255, pixelByteArray[10 * 3 + 9 * stride + 2]);
        }
    }
}
