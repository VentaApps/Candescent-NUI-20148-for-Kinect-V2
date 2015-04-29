using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CCT.NUI.Core.Shape
{
    public class Shape : ILocatable
    {
        private Point center;
        private Volume volume;
        private Contour contour;
        private ConvexHull convexHull;

        private IList<Point> points;

        public Shape(Point center, Volume volume, Contour contour, ConvexHull convexHull, IList<Point> points)
        {
            this.center = center;
            this.volume = volume;
            this.contour = contour;
            this.convexHull = convexHull;
            this.points = points;
        }

        public Point Location
        {
            get { return this.center; }
        }

        public Volume Volume
        {
            get { return this.volume; }
        }

        public Contour Contour
        {
            get { return this.contour; }
        }

        public ConvexHull ConvexHull
        {
            get { return this.convexHull; }
        }

        public IList<Point> Points
        {
            get { return this.points; }
        }

        public int PointCount
        {
            get { return this.points.Count; }
        }
    }
}
