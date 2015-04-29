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
    public class DepthImageFactoryTests
    {
        [TestMethod]
        public void DepthImage_Colors_Fade_With_Distance()
        {
            var bitmap = new Bitmap(20, 10);
            var factory = new DepthBitmapFactory(200);
            var frame = new DepthDataFrame(20, 10);
            for (int index = 0; index < frame.Data.Length; index++)
            {
                frame.Data[index] = (ushort) index;
            }

            var pointerFactory = new ArrayToPointerFactory();
            var pointer = pointerFactory.CreatePointer(frame.Data);
            try
            {
                factory.CreateImage(bitmap, pointer);
                AssertColorsFade(bitmap);
            }
            finally
            {
                pointerFactory.Destroy(pointer);
            }
        }

        private void AssertColorsFade(Bitmap bitmap)
        {
            var color = bitmap.GetPixel(1, 0);
            var color2 = bitmap.GetPixel(10, 8);
            var color3 = bitmap.GetPixel(15, 9);

            Assert.IsTrue(color.R > color2.R);
            Assert.IsTrue(color.G > color2.B);
            Assert.IsTrue(color.B > color2.B);

            Assert.IsTrue(color2.R > color3.R);
            Assert.IsTrue(color2.G > color3.B);
            Assert.IsTrue(color2.B > color3.B);
        }
    }
}
