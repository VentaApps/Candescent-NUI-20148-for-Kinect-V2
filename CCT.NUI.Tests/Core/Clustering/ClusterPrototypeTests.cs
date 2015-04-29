using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CCT.NUI.Core.Clustering;
using CCT.NUI.Core;

namespace CCT.NUI.Tests.Core.Clustering
{
    [TestClass]
    public class ClusterPrototypeTests
    {       
        [TestMethod]
        public void Can_Set_Center()
        {
            var cluster = new ClusterPrototype(Point.Zero);
            cluster.SetCenter(1, 2, 3);
            AssertClusterCenterIsAt(cluster, 1, 2, 3);
        }

        private void AssertClusterCenterIsAt(ClusterPrototype cluster, int x, int y, int z)
        {
            Assert.AreEqual(new Point(x, y, z), cluster.Center);
        }
    }
}
