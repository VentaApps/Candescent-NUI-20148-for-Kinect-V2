using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CCT.NUI.Core;

namespace CCT.NUI.Tests.Core
{
    [TestClass]
    public class DepthDataFrameTests
    {
        [TestMethod]
        public void Max_Depth_Returns_The_Maximum_Depth()
        {
            var frame = new DepthDataFrame(2, 2, new ushort[] { 1, 2, 3, 4 });
            Assert.AreEqual(4, frame.MaxDepth);
        }

        [TestMethod]
        public void Index_Access_Returns_The_Right_Value()
        {
            var frame = new DepthDataFrame(2, 2, new ushort[] { 1, 2, 3, 4 });
            Assert.AreEqual(1, frame[0, 0]);
            Assert.AreEqual(2, frame[1, 0]);
            Assert.AreEqual(3, frame[0, 1]);
            Assert.AreEqual(4, frame[1, 1]);
        }

        [TestMethod]
        public void Test_With_And_Height_Properties()
        {
            int width = 20;
            int height = 10;

            var frame = new DepthDataFrame(width, height);

            Assert.AreEqual(width, frame.Width);
            Assert.AreEqual(height, frame.Height);

            Assert.AreEqual(frame.Size.Width, frame.Width);
            Assert.AreEqual(frame.Size.Height, frame.Height);
        }

    }
}
