using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CCT.NUI.Core;

namespace CCT.NUI.Tests.Core
{
    [TestClass]
    public class VolumeTests
    {
        [TestMethod]
        public void Test_Volume_Contains()
        {
            var volume = new Volume { X = 0, Y = 0, Z = 0, Width = 10, Height = 10, Depth = 10 };
            Assert.IsTrue(volume.Contains(5, 5, 5));
            Assert.IsFalse(volume.Contains(11, 11, 11));
        }

        [TestMethod]
        public void Test_Construction()
        {
            var volume = new Volume { X = 1, Y = 2, Z = 3, Width = 10, Height = 11, Depth = 12 };
            Assert.AreEqual(1, volume.X);
            Assert.AreEqual(2, volume.Y);
            Assert.AreEqual(3, volume.Z);

            Assert.AreEqual(10, volume.Width);
            Assert.AreEqual(11, volume.Height);
            Assert.AreEqual(12, volume.Depth);
        }
    }
}
