using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CCT.NUI.Core;

namespace CCT.NUI.Tests.Core
{
    [TestClass]
    public class VectorTests
    {
        [TestMethod]
        public void Test_Equals()
        {
            var vector = new Vector(2, 5, 9);
            var equalVector = new Vector(2, 5, 9);
            var unequalVector = new Vector(1, 2, 3);

            Assert.AreEqual(vector, equalVector);
            Assert.AreNotEqual(vector, unequalVector);

            Assert.AreNotEqual(vector, null);
            Assert.AreNotEqual(vector, new object());
        }

        [TestMethod]
        public void Test_ToString()
        {
            var vectorString = new Vector(1, 2, 3).ToString();

            Assert.IsTrue(vectorString.Contains("1"));
            Assert.IsTrue(vectorString.Contains("2"));
            Assert.IsTrue(vectorString.Contains("3"));
        }

        [TestMethod]
        public void Test_Zero()
        {
            Assert.AreEqual(Vector.Zero, new Vector(0, 0, 0));
        }

        [TestMethod]
        public void Test_GetHashcode()
        {
            var vector = new Vector(2, 5, 9);
            Assert.IsTrue(vector.GetHashCode() != 0);
        }
    }
}
