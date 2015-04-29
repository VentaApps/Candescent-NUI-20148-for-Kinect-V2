using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CCT.NUI.Core.Clustering;
using CCT.NUI.Core;

namespace CCT.NUI.Tests.Core.Clustering
{
    public class ClusterBuilder
    {
        public Cluster Create2DClusterAtZero()
        {
            return Create2DClusterAt(Point.Zero);
        }

        public Cluster Create2DClusterAt(Point center)
        {
            var points = new[] { new Point(center.X - 1, center.Y, center.Z), new Point(center.X + 1, center.Y, center.Z), new Point(center.X, center.Y - 1, center.Z), new Point(center.X, center.Y + 1, center.Z) };
            return new Cluster(points);
        }
    }
}
