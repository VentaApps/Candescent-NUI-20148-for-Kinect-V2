using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;

namespace CCT.NUI.Core.Clustering
{
    internal class ClusterPrototypeFactory
    {
        private Random random = new Random();

        public IList<ClusterPrototype> CreateClusters(int numberOfClusters, IntSize areaSize)
        {
            Contract.Requires(numberOfClusters >= 0);
            Contract.Requires(areaSize.Width > 0 && areaSize.Height > 0);

            var result = new List<ClusterPrototype>();
            float sliceWidth = areaSize.Width / numberOfClusters;
            float sliceHeight = areaSize.Height / numberOfClusters;

            for (int index = 0; index < numberOfClusters; index++)
            {
                int minX = (int)((index) * sliceWidth);
                int maxX = (int)((index + 1) * sliceWidth);
                int minY = (int)((index) * sliceHeight);
                int maxY = (int)((index + 1) * sliceHeight);
                result.Add(this.CreateClusterWithin(minX, maxX, minY, maxY));
            }

            return result;
        }

        private ClusterPrototype CreateClusterWithin(int minX, int maxX, int minY, int maxY)
        {
            return new ClusterPrototype(random.Next(minX, maxX - 1), random.Next(minY, maxY - 1), 0);
        }
    }
}
