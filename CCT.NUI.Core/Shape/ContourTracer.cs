using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CCT.NUI.Core.Shape
{
    public class ContourTracer
    {
        private DepthMap contourMap;
        private IList<Point> contourPoints;

        public ContourTracer()
        { }

        public IList<Point> GetContourPoints(DepthMap contourMap)
        {
            this.contourMap = contourMap;
            this.contourPoints = new List<Point>();
            this.Process();
            return this.contourPoints;
        }

        private int Width { get { return this.contourMap.Width; } }

        private int Height { get { return this.contourMap.Height; } }

        private int ResultCount { get { return this.contourPoints.Count; } }

        private void Process() 
        {
            var firstPoint = this.FindFirstPoint();
            var directionPoint = new Point(firstPoint.X, firstPoint.Y - 1, firstPoint.Z);
            var currentPoint = firstPoint;
            this.contourPoints.Add(currentPoint);

            Point? nextPoint;
            do
            {
                nextPoint = this.GetNextPoint(currentPoint, directionPoint);
                if (nextPoint.HasValue)
                {
                    directionPoint = currentPoint;
                    this.contourPoints.Add(nextPoint.Value);
                    currentPoint = nextPoint.Value;
                }
            } while (nextPoint.HasValue && !firstPoint.Equals(nextPoint));

            this.contourPoints.Add(firstPoint);
        }

        private Point FindFirstPoint()
        {
            int width = this.Width;
            int height = this.Width;
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    if (this.contourMap.IsSet(x, y))
                    {
                        return new Point(x, y, this.contourMap[x, y]);
                    }
                }
            }
            throw new ArgumentException("Contour has no points");
        }

        private Point? GetNextPoint(Point currentPoint, Point directionPoint)
        {
            var diffX = (int) (directionPoint.X - currentPoint.X);
            var diffY = (int) (directionPoint.Y - currentPoint.Y);
            var startIndex = GetStartIndex(diffX, diffY);

            for (int index = startIndex; index < startIndex + PointRotation.Count; index++)
            {
                var rotationPoint = PointRotation.GetPoint(index);
                var x = (int)(currentPoint.X + rotationPoint.X);
                var y = (int)(currentPoint.Y + rotationPoint.Y);
                if (this.contourMap.IsSet(x, y))
                {
                    return new Point(x, y, this.contourMap[x, y]);
                }
            }
            return null;
        }

        private int GetStartIndex(int diffX, int diffY)
        {
            if (diffY == -1)
            {
                if (diffX == -1)
                {
                    return 6;
                }
                if (diffX == 0)
                {
                    return 7;
                }
                if (diffX == 1)
                {
                    return 0;
                }
            }
            if (diffY == 0)
            {
                if (diffX == -1)
                {
                    return 5;
                }
                if (diffX == 1)
                {
                    return 1;
                }
            }
            if (diffY == 1)
            {
                if (diffX == -1)
                {
                    return 4;
                }
                if (diffX == 0)
                {
                    return 3;
                }
                if (diffX == 1)
                {
                    return 2;
                }
            }
            return 0;
        }
    }

    class PointRotation
    {
        private static IList<Point> points;

        static PointRotation()
        {
            points = new List<Point>();
            points.Add(new Point(1, 0, 0));
            points.Add(new Point(1, 1, 0));
            points.Add(new Point(0, 1, 0));
            points.Add(new Point(-1, 1, 0));
            points.Add(new Point(-1, 0, 0));
            points.Add(new Point(-1, -1, 0));
            points.Add(new Point(0, -1, 0));
            points.Add(new Point(1, -1, 0));
        }

        public static Point GetPoint(int index)
        {
            return points[index % Count];
        }

        public static int Count
        {
            get { return points.Count; }
        }

        public static IList<Point> Points
        {
            get { return points; }
        }
    }
}
