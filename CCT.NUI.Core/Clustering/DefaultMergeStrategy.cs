using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CCT.NUI.Core.Clustering
{
    public class DefaultMergeStrategy : IClusterMergeStrategy
    {
        private ClusterDataSourceSettings settings;

        public DefaultMergeStrategy(ClusterDataSourceSettings settings)
        {
            this.settings = settings;
        }

        public IList<ClusterPrototype> MergeClustersIfRequired(IList<ClusterPrototype> clusters)
        {
            var clustersToIterateOver = new List<ClusterPrototype>(clusters);
            foreach (var cluster in clustersToIterateOver)
            {
                foreach (var otherCluster in new List<ClusterPrototype>(clusters))
                {
                    if (cluster != otherCluster && this.IsMergeRequired(cluster, otherCluster))
                    {
                        clusters.Remove(cluster);
                        clusters.Remove(otherCluster);
                        clusters.Add(ClusterPrototype.Merge(cluster, otherCluster));
                    }
                }
            }
            return clusters;
        }

        private bool IsMergeRequired(ClusterPrototype cluster1, ClusterPrototype cluster2)
        {
            return  cluster1.DistanceMetric(cluster2) < this.settings.MergeMinimumDistanceToCluster ||
                    Point.Distance(cluster1.Center, cluster2.Center) < this.settings.MergeMaximumClusterCenterDistances ||
                    Point.Distance(cluster1.Center.X, cluster1.Center.Y, cluster2.Center.X, cluster2.Center.Y) < this.settings.MergeMaximumClusterCenterDistances2D;
        }
    }
}
