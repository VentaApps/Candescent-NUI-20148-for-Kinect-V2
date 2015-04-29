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
    public class ClusterTests
    {
        private ClusterBuilder builder = new ClusterBuilder();

        [TestMethod]
        public void Can_Create_Cluster()
        {
            var cluster = builder.Create2DClusterAtZero();
            AssertClusterCenterIsAt(cluster, 0, 0, 0);
        }

        private void AssertClusterCenterIsAt(Cluster cluster, int x, int y, int z)
        {
            Assert.AreEqual(new Point(x, y, z), cluster.Center);
            Assert.AreEqual(cluster.Location, cluster.Center);
        }
    }
}
