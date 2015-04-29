using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CCT.NUI.Core;

namespace CCT.NUI.Tests.Core
{
    [TestClass]
    public class HistogramTests
    {
        private int maxDepth;
        private Histogram histogram;

        [TestInitialize]
        public void Setup()
        {
            this.maxDepth = 100;
            this.histogram = new Histogram(this.maxDepth);
        }

        [TestMethod]
        public void Length_Equals_MaxDepth()
        {
            Assert.AreEqual(this.maxDepth, this.histogram.Length);
        }

        [TestMethod]
        public void Reset_Sets_All_Values_To_0()
        {
            for (int index = 0; index < this.maxDepth; index++)
            {
                this.histogram.Increase(1);
            }
            this.histogram.Reset();
            for (int index = 0; index < this.maxDepth; index++)
            {
                Assert.AreEqual(0, this.histogram.GetValue((ushort)index));
            }
        }

        [TestMethod]
        public void Test_Increase()
        {
            ushort value = 55;
            this.histogram.Increase(value);
            Assert.AreEqual(1, this.histogram.GetValue(value));
            this.histogram.Increase(value);
            Assert.AreEqual(2, this.histogram.GetValue(value));
        }

        [TestMethod]
        public void Increase_Beyond_Boundary_Does_Nothing()
        {
            this.histogram.Increase(200);
            Assert.AreEqual(0, this.histogram.GetValue(200));
        }

        [TestMethod]
        public void Value_Beyond_Boundary_Returns_Zero()
        {
            Assert.AreEqual(0, this.histogram.GetValue(101));
        }
    }
}
