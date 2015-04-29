using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CCT.NUI.Core.Clustering;

namespace CCT.NUI.Core.Shape
{
    public class ClusterShapeFactory : IClusterShapeFactory
    {
        private ShapeDataSourceSettings settings;
        private ContourFactory contourFactory;

        public ClusterShapeFactory(ShapeDataSourceSettings settings)
        {
            this.settings = settings;
            this.contourFactory = new ContourFactory(settings.ContourLineThinningDistance);
        }

        public ShapeCollection Create(ClusterCollection clusterData)
        {
            var result = new ShapeCollection();
            foreach (var cluster in clusterData.Clusters)
            {
                var convexHull = new GrahamScan(cluster.Points).FindHull();
                var map = CreateMap(cluster);
                var contour = CreateContour(map, cluster);

                if (contour.Count >= settings.MinimalPointsInContour)
                {
                    result.Shapes.Add(new Shape(cluster.Center, cluster.Volume, contour, convexHull, cluster.Points));
                }
            }
            return result;
        }

        private Contour CreateContour(DepthMap map, Cluster cluster)
        {
            return this.contourFactory.CreateContour(map, cluster.X, cluster.Y);
        }

        private DepthMap CreateMap(Cluster cluster)
        {
            var map = new int[(int)cluster.Width + 1, (int)cluster.Height + 1];
            foreach (var point in cluster.AllPoints)
            {
                map[(int)(point.X - cluster.X), (int)(point.Y - cluster.Y)] = (int)point.Z;
            }
            return new DepthMap(map);
        }
    }
}
