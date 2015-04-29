using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using System.Windows;

namespace CCT.NUI.StartMenu
{
    internal class CoordinateMap
    {
        private double xMultiplicator;
        private double yMultiplicator;

        public CoordinateMap(double xMultiplicator, double yMultiplicator)
        {
            this.xMultiplicator = xMultiplicator;
            this.yMultiplicator = yMultiplicator;
        }

        public PointCollection ConvertPoints(IList<Core.Point> points)
        {
            return new PointCollection(points.Select(p => new System.Windows.Point(p.X * this.xMultiplicator, p.Y * this.yMultiplicator)));
        }

        public Point ConvertPoint(Core.Point point)
        {
            return new Point(point.X * this.xMultiplicator + 5, point.Y * this.yMultiplicator + 10);
        }
    }
}
