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
    public class KMeansClusterFactoryTests
    {
        private KMeansClusterFactory factory;
        private IntSize size;

        [TestInitialize]
        public void Setup()
        {
            var settings = new ClusterDataSourceSettings();
            settings.LowerBorder = 0;
            settings.MinimalPointsForClustering = 1;
            settings.MinimalPointsForValidCluster = 1;
            settings.PointModulo = 1;
            settings.MergeMaximumClusterCenterDistances = 1;
            settings.MergeMaximumClusterCenterDistances2D = 1;
            settings.MergeMinimumDistanceToCluster = 1;

            this.size = new IntSize(20, 10);

            this.factory = new KMeansClusterFactory(settings, this.size);
        }

        [TestMethod]
        public void Creates_Two_Clusters()
        {
            var data = PrepareTwoClusterData();

            var result = this.factory.Create(data);

            Assert.AreEqual(2, result.Count);
        }

        private IList<Point> PrepareTwoClusterData()
        {
            var data = new List<Point>();
            for (int x = 0; x < this.size.Width; x++)
            {
                for (int y = 0; y < this.size.Height; y++)
                {
                    if ((x == 2 && y == 2) || (x == 15 && y == 8))
                    {
                        data.Add(new Point(x, y, 700));
                    }
                }
            }
            return data;
        }
    }
}
