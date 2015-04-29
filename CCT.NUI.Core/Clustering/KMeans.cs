using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace CCT.NUI.Core.Clustering
{
    internal class KMeans
    {
        private static ClusterPrototypeFactory clusterFactory = new ClusterPrototypeFactory();

        private Random random = new Random();
        private IList<ClusterPrototype> clusters;
        private IList<Point> points;
        private IntSize size;
        private Range zRange;

        public KMeans(int numberOfClusters, Range zRange, IntSize size)
        {
            this.size = size;
            this.zRange = zRange;
            this.clusters = clusterFactory.CreateClusters(numberOfClusters, size);
        }

        public void Initialize(IList<Point> points)
        {
            this.points = points;
        }

        public IEnumerable<ClusterPrototype> Clusters
        {
            get { return this.clusters.OrderByDescending(c => c.Center.X).ThenBy(c => c.Center.Y); }
        }

        public int ClusterCount
        {
            get { return this.clusters.Count; }
        }

        public void IterateUntilStable()
        {
            int[] counts;
            do
            {
                counts = this.clusters.Select(c => c.PointCount).ToArray();
                this.IterateOnce();
            } while (DetectCountChange(counts));
        }

        public void IterateOnce()
        {
            this.ClearPoints();
            this.DistributePointsToClusters();
            this.FinishDistribution();
        }

        private void FinishDistribution()
        {
            foreach (var cluster in this.clusters)
            {
                cluster.FinishAddingPoints();
                SetCenterRandomlyForEmptyCluster(cluster);
            }
        }

        private void SetCenterRandomlyForEmptyCluster(ClusterPrototype cluster)
        {
            if (cluster.PointCount == 0)
            {
                cluster.SetCenter(this.random.Next(0, this.size.Width - 1), this.random.Next(0, this.size.Height - 1), this.random.Next((int)this.zRange.Min, (int)this.zRange.Max));
            }
        }

        private void ClearPoints()
        {
            foreach (var cluster in this.clusters)
            {
                cluster.ClearPoints();
            }
        }

        private void DistributePointsToClusters()
        {
            foreach (var point in this.points)
            {
                this.AddToMinimalDistanceCluster(point);
            }
        }

        private bool DetectCountChange(int[] counts)
        {
            for (int index = 0; index < counts.Length; index++)
            {
                var newCount = this.clusters[index].PointCount;
                if (newCount == 0 || newCount != counts[index])
                {
                    return true;
                }
            }
            return false;
        }

        private double CalcDistance(int clusterIndex, Point point)
        {
            return this.clusters[clusterIndex].Calc2DDistance(point);
        }

        private void AddToMinimalDistanceCluster(Point point)
        {
            int clusterIndex = 0;
            var minDist = CalcDistance(0, point);
            for (int index = 1; index < this.clusters.Count; index++)
            {
                var dist = this.CalcDistance(index, point);
                if (dist < minDist)
                {
                    clusterIndex = index;
                    minDist = dist;
                }
            }
            this.clusters[clusterIndex].AddPoint(point);
        }
    }
}
