using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CCT.NUI.Core;

namespace CCT.NUI.Tests.Core
{
    [TestClass]
    public class RectangleTests
    {
        [TestMethod]
        public void Test_ToString()
        {
            var rectangle = new Rectangle(1, 2, 3, 4);
            var rectangleText = rectangle.ToString();

            Assert.IsTrue(rectangleText.Contains("1"));
            Assert.IsTrue(rectangleText.Contains("2"));
            Assert.IsTrue(rectangleText.Contains("3"));
            Assert.IsTrue(rectangleText.Contains("4"));
        }

        [TestMethod]
        public void Test_Equals()
        {
            var rectangle = new Rectangle(1, 2, 3, 4);
            var equalRectange = new Rectangle(1, 2, 3, 4);
            var unequalRectangle = new Rectangle(2, 3, 4, 5);

            Assert.AreEqual(rectangle, equalRectange);
            Assert.AreNotEqual(rectangle, unequalRectangle);

            Assert.AreNotEqual(rectangle, null);
            Assert.AreNotEqual(rectangle, new object());
        }

        [TestMethod]
        public void Test_GetHashcode()
        {
            Assert.IsTrue(new Rectangle(1, 2, 3, 4).GetHashCode() != 0);
        }
    }
}
