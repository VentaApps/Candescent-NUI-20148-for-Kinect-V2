using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CCT.NUI.Core
{
    [Serializable]
    public struct Point
    {
        public float X;
        public float Y;
        public float Z;

        public Point(float x, float y, float z)
            : this()
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
        }

        public Point(Point p)
            : this()
        {
            this.X = p.X;
            this.Y = p.Y;
            this.Z = p.Z;
        }

        public void Adapt(Point point)
        {
            this.X = point.X;
            this.Y = point.Y;
            this.Z = point.Z;
        }

        public override string ToString()
        {
            return string.Format("x:{0} y:{1} z:{2}", this.X, this.Y, this.Z);
        }

        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is Point))
            {
                return false;
            }
            var point = ((Point)obj);
            return point.X == this.X && point.Y == this.Y && point.Z == this.Z;            
        }

        public override int GetHashCode()
        {
            return this.X.GetHashCode() ^ this.Y.GetHashCode() ^ this.Y.GetHashCode();
        }

        public static Point Zero
        {
            get { return zero; }
        }

        public static bool IsZero(Point point)
        {
            return Zero.Equals(point);
        }

        public static double Distance(Point p1, Point p2)
        {
            return Math.Sqrt(Math.Pow(p1.X - p2.X, 2) + Math.Pow(p1.Y - p2.Y, 2) + Math.Pow(p1.Z - p2.Z, 2));
        }

        public static double Distance(double x1, double y1, double x2, double y2)
        {
            return Math.Sqrt(Math.Pow(x1 - x2, 2) + Math.Pow(y1 - y2, 2));
        }

        public static double Distance2D(Point p1, Point p2)
        {
            return Math.Sqrt(Math.Pow(p1.X - p2.X, 2) + Math.Pow(p1.Y - p2.Y, 2));
        }

        public static Point Center(Point p1, Point p2)
        {
            return new Point((p1.X + p2.X) / 2, (p1.Y + p2.Y) / 2, (p1.Z + p2.Z) / 2);
        }

        public static Point Center(IList<Point> points)
        {
            var center = Point.Zero;
            if (points.Count > 0)
            {
                for (int index = 0; index < points.Count; index++)
                {
                    var p = points[index];
                    center.X += p.X;
                    center.Y += p.Y;
                    center.Z += p.Z;
                }

                center.X /= points.Count;
                center.Y /= points.Count;
                center.Z /= points.Count;
            }
            return center;
        }

        public static Point FindNearestPoint(Point target, IEnumerable<Point> points)
        {
            var pointList = points.ToList();
            return pointList[FindIndexOfNearestPoint(target, pointList)];
        }

        public static int FindIndexOfNearestPoint(Point target, IList<Point> points)
        {
            int index = 0;
            int resultIndex = -1;
            double minDist = double.MaxValue;
            foreach (Point p in points)
            {
                var distance = Distance(p.X, p.Y, target.X, target.Y);
                if (distance < minDist)
                {
                    resultIndex = index;
                    minDist = distance;
                }
                index++;
            }
            return resultIndex;
        }

        public static Vector Subtract(Point point1, Point point2)
        {
            return new Vector(point1.X - point2.X, point1.Y - point2.Y, point1.Z - point2.Z);
        }

        private static Point zero = new Point(0, 0, 0);
    }
}
