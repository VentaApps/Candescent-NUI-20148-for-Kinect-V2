using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CCT.NUI.Core.Clustering
{
    public class KMeansClusterFactory : IClusterFactory
    {
        private KMeans algorithm;
        private IClusterMergeStrategy mergeStrategy;

        private ClusterDataSourceSettings settings;
        private ClusterCollection value;

        public KMeansClusterFactory(ClusterDataSourceSettings settings, IntSize size)
            : this(settings, new DefaultMergeStrategy(settings), size)
        { }

        public KMeansClusterFactory(ClusterDataSourceSettings settings, IClusterMergeStrategy mergeStrategy, IntSize size)
        {
            this.settings = settings;
            this.mergeStrategy = mergeStrategy;
            this.algorithm = new KMeans(this.settings.ClusterCount, settings.DepthRange, size);
            this.value = new ClusterCollection();
        }

        public ClusterCollection Create(IList<Point> points)
        {
            var reducedPoints = this.ReducePoints(points);

            if (this.AreEnoughPointsForClustering(reducedPoints.Count))
            {
                this.FindClusters(reducedPoints);
                this.AssignAllPoints(points);
            }
            else
            {
                this.value = new ClusterCollection();
            }
            return this.value;
        }

        private bool AreEnoughPointsForClustering(int count)
        {
            return count >= settings.MinimalPointsForClustering;
        }

        private IList<Point> ReducePoints(IList<Point> points)
        {
            return points.Where(p => p.X % this.settings.PointModulo == 0 && p.Y % this.settings.PointModulo == 0).ToList();
        }

        private void FindClusters(IList<Point> pointList)
        {
            this.InitializeAlgorithm(pointList);
            this.algorithm.IterateUntilStable();

            if (this.algorithm.ClusterCount > 0)
            {
                var prototypes = this.FlattenIfRequired(this.MergeClustersIfRequired(this.algorithm.Clusters));
                this.value = new ClusterCollection(prototypes.Select(p => p.ToCluster()).ToList());
            }
        }

        private IList<ClusterPrototype> FlattenIfRequired(IList<ClusterPrototype> clusters)
        {
            if (this.settings.MaximumClusterDepth.HasValue)
            {
                foreach (var cluster in clusters)
                {
                    cluster.Flatten(this.settings.MaximumClusterDepth.Value);
                }
            }
            return clusters;
        }

        private void InitializeAlgorithm(IList<Point> pointList)
        {
            this.algorithm.Initialize(pointList);
        }

        private void AssignAllPoints(IList<Point> fullList) 
        {
            foreach (var cluster in this.value.Clusters)
            {
                var allPoints = new List<Point>();
                var area = cluster.Area;
                foreach (var point in fullList)
                {
                    if (area.Contains(point))
                    {
                        allPoints.Add(point);
                    }
                }
                cluster.AllPoints = allPoints;
            }
        }

        private IList<ClusterPrototype> MergeClustersIfRequired(IEnumerable<ClusterPrototype> clusters)
        {
            IList<ClusterPrototype> localClusters = clusters.Where(c => c.PointCount >= settings.MinimalPointsForValidCluster).ToList();
            if (localClusters.Count > 1)
            {
                int clusterCount;
                do
                {
                    clusterCount = localClusters.Count;
                    localClusters = this.mergeStrategy.MergeClustersIfRequired(localClusters);
                }             
                while (localClusters.Count != clusterCount);
            }
            return localClusters.Where(c => c.PointCount <= settings.MaximalPointsForValidCluster).ToList();
        }
    }
}
