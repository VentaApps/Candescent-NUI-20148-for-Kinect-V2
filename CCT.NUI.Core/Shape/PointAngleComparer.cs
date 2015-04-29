using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CCT.NUI.Core.Shape
{
    /// <summary>
    /// http://de.wikipedia.org/wiki/Graham_Scan
    /// </summary>
    internal class PointAngleComparer2D : IComparer<Point>
    {
        private Point p0;

        public PointAngleComparer2D(Point zeroPoint)
        {
            this.p0 = zeroPoint;
        }

        public int Compare(Point p1, Point p2)
        {
            if (p1.Equals(p2))
            {
                return 0;
            }
            var value = Compare(this.p0, p1, p2);
            if (value == 0)
            {
                return 0;
            }
            if (value < 0)
            {
                return 1;
            }
            return -1;
        }

        public static float Compare(Point p0, Point p1, Point p2)
        {
            return (p1.X - p0.X) * (p2.Y - p0.Y) - (p2.X - p0.X) * (p1.Y - p0.Y);
        }
    }
}
