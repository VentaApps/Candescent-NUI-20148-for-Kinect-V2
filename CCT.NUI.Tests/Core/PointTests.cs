using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CCT.NUI.Core;

namespace CCT.NUI.Tests.Core
{
    [TestClass]
    public class PointTests
    {
        [TestMethod]
        public void Test_Equals()
        {
            var point = new Point(2, 5, 9);
            var equalPoint = new Point(2, 5, 9);
            var unequalPoint = new Point(1, 2, 3);

            Assert.AreEqual(point, equalPoint);
            Assert.AreNotEqual(point, unequalPoint);

            Assert.AreNotEqual(point, null);
            Assert.AreNotEqual(point, new object());
        }

        [TestMethod]
        public void Test_ToString()
        {
            var pointString = new Point(1, 2, 3).ToString();

            Assert.IsTrue(pointString.Contains("1"));
            Assert.IsTrue(pointString.Contains("2"));
            Assert.IsTrue(pointString.Contains("3"));
        }

        [TestMethod]
        public void Test_Zero()
        {
            Assert.AreEqual(Point.Zero, new Point(0, 0, 0));
        }
    }
}
