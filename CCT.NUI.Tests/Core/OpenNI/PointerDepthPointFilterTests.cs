using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CCT.NUI.Core.OpenNI;
using CCT.NUI.Core;

namespace CCT.NUI.Tests.Core.OpenNI
{
    [TestClass]
    public class PointerDepthPointFilterTests
    {
        private PointerDepthPointFilter filter;
        private IntSize size;

        [TestInitialize]
        public void Setup()
        {
            this.size = new IntSize(20, 10);
            this.filter = new PointerDepthPointFilter(size, 500, 800, 0);
        }

        [TestMethod]
        public void Filters_Depth_Data_Correctly()
        {
            var data = PrepareDepthData();
            var pointerFactory = new ArrayToPointerFactory();
            var pointer = pointerFactory.CreatePointer(data.ToArray());

            var result = this.filter.Filter(pointer);
            pointerFactory.Destroy(pointer);

            Assert.AreEqual(2, result.Count);
            Assert.IsTrue(result.Contains(new Point(2, 2, 700)));
            Assert.IsTrue(result.Contains(new Point(15, 8, 700)));
        }

        private List<ushort> PrepareDepthData()
        {
            var data = new List<ushort>();
            for (int y = 0; y < this.size.Height; y++)
            {
                for (int x = 0; x < this.size.Width; x++)
                {
                    ushort depthValue = 0;
                    if ((x == 2 && y == 2) || (x == 15 && y == 8))
                    {
                        depthValue = 700;
                    }
                    data.Add(depthValue);
                }
            }
            return data;
        }
    }
}
