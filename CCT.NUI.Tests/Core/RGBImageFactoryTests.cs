using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Drawing;
using CCT.NUI.Core;
using CCT.NUI.Core.Video;

namespace CCT.NUI.Tests.Core
{
    [TestClass]
    public class RGBImageFactoryTests
    {
        [TestMethod]
        public void Can_Create_Color_Image()
        {
            var bitmap = new Bitmap(20, 10);
            var imageFactory = new RgbBitmapFactory();

            var pointerFactory = new ArrayToPointerFactory();
            var data = new ushort[20 * 10 * 2];
            for (int index = data.Length / 2; index < data.Length; index++)
            {
                data[index] = ushort.MaxValue;
            }
            var pointer = pointerFactory.CreatePointer(data);
            try
            {
                imageFactory.CreateImage(bitmap, pointer);
            }
            finally
            {
                pointerFactory.Destroy(pointer);
            }

            var color = bitmap.GetPixel(0, 0);
            var color2 = bitmap.GetPixel(10, 9);

            Assert.AreEqual(0, color.R);
            Assert.AreEqual(0, color.G);
            Assert.AreEqual(0, color.B);

            Assert.AreEqual(255, color2.R);
            Assert.AreEqual(255, color2.G);
            Assert.AreEqual(255, color2.B);
        }
    }
}
