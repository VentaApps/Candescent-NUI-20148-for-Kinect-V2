using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CCT.NUI.Core.Shape;
using CCT.NUI.Core;

namespace CCT.NUI.Tests.Core
{
    [TestClass]
    public class ContourTracerTests
    {
        [TestMethod]
        public void Test()
        {
            var tracer = new ContourTracer();

            var map = new DepthMap(10, 10);
            map[6, 6] = 100;
            map[5, 5] = 100;
            map[6, 5] = 100;
            map[4, 5] = 100;
            map[3, 5] = 100;
            map[4, 4] = 100;
            map[5, 4] = 100;
            map[6, 4] = 100;
            map[4, 3] = 100;
            map[5, 3] = 100;
            map[6, 3] = 100;
            map[5, 2] = 100;

            var result = tracer.GetContourPoints(map);

            Assert.IsTrue(!result.Contains(new Point(5, 4, 100)));
        }
    }
}
