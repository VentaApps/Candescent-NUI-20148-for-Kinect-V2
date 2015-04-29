using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CCT.NUI.Core;

namespace CCT.NUI.Tests.Core
{
    [TestClass]
    public class RangeTests
    {
        [TestMethod]
        public void ToString_Contains_Relevant_Values()
        {
            var range = new Range(10, 25);
            var rangeString = range.ToString();

            Assert.IsTrue(rangeString.Contains("10"));
            Assert.IsTrue(rangeString.Contains("25"));
            Assert.IsTrue(rangeString.Contains("15"));
        }

      [TestMethod]
      public void Initialize_Range()
      {
          var range = new Range(new float[] { 12, 10, 5, 15, 25, 20});
          Assert.AreEqual(20, range.Interval);
          Assert.AreEqual(5, range.Min);
          Assert.AreEqual(25, range.Max);
      }
    }
}
