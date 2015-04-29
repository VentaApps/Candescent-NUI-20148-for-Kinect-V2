using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CCT.NUI.Core.Clustering
{
    public class ClusterPrototype
    {
        protected Point center;
        protected volatile IList<Point> points;
        private IList<Point> temporaryPoints = new List<Point>();

        public ClusterPrototype(Point center) 
        {
            this.center = center;
            this.points = new List<Point>();
        }

        public ClusterPrototype(int x, int y, int z)
            : this(new Point(x, y, z))
        { }

        public ClusterPrototype(Point center, IList<Point> points)
        {
            this.center = center;
            this.points = points;
            this.CalculateCenter();
        }

        public Point Center
        {
            get { return this.center; }
        }

        //TODO: Use Convex Hull?
        public double DistanceMetric(ClusterPrototype otherCluster) 
        {
            var p1 = Point.FindNearestPoint(this.center, otherCluster.Points);
            var p2 = Point.FindNearestPoint(otherCluster.Center, this.Points);
            return Point.Distance(p1, p2);
        }

        public double Calc2DDistance(Point point)
        {
            return Point.Distance(point, this.center);
        }

        public void AddPoint(Point point)
        {
            this.temporaryPoints.Add(point);
        }

        public void FinishAddingPoints()
        {
            this.points = this.temporaryPoints;
            this.temporaryPoints = new List<Point>();
            this.CalculateCenter();
        }
        
        private void CalculateCenter()
        {
            if (this.points.Count > 0)
            {
                this.center = this.points[0];
                for (int index = 1; index < this.points.Count; index++)
                {
                    var p = this.points[index];
                    center.X += p.X;
                    center.Y += p.Y;
                    center.Z += p.Z;
                }

                center.X /= this.points.Count;
                center.Y /= this.points.Count;
                center.Z /= this.points.Count;
            }
        }

        public void ClearPoints()
        {  
            this.points = new List<Point>();
        }

        public IEnumerable<Point> Points
        {
            get { return this.points; }
        }

        public void SetCenter(int x, int y, int z)
        {
            this.center = new Point(x, y, z);
        }

        public void Flatten(int maxDepth)
        {
            var rangeZ = new Range(this.points.Select(p => p.Z));
            float maxZValue = rangeZ.Min + maxDepth;
            if (rangeZ.Interval > maxDepth)
            {
                this.points = this.points.Where(p => p.Z < maxZValue).ToList();
                this.CalculateCenter();
            }
        }

        public int PointCount
        {
            get { return this.points.Count; }
        }

        public static ClusterPrototype Merge(ClusterPrototype cluster1, ClusterPrototype cluster2)
        {
            return new ClusterPrototype(Point.Center(cluster1.Center, cluster2.Center), cluster1.points.ToList().Union(cluster2.points).ToList());
        }

        public Cluster ToCluster()
        {
            return new Cluster(this.center, this.points);
        }
    }
}
