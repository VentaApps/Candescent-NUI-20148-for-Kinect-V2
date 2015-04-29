using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CCT.NUI.Core;

namespace CCT.NUI.Tests.Core
{
    [TestClass]
    public class IntSizeTests
    {
        [TestMethod]
        public void Test_Equals()
        {
            var size = new IntSize(10, 10);
            var equalSize = new IntSize(10, 10);
            var unequalSize = new IntSize(11, 10);

            Assert.AreEqual(size, equalSize);
            Assert.AreNotEqual(size, unequalSize);

            Assert.AreNotEqual(size, null);
            Assert.AreNotEqual(size, new object());
        }

        [TestMethod]
        public void Test_ToString()
        {
            var size = new IntSize(10, 12);

            var sizeText = size.ToString();

            Assert.IsTrue(sizeText.Contains("10"));
            Assert.IsTrue(sizeText.Contains("12"));
        }
    }
}
