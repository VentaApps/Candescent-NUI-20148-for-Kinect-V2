using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CCT.NUI.Core.Shape
{
    public class LineThinner
    {
        private bool checkBoundary;
        private float mindDistBetweenPoints;

        public LineThinner(float mindDistBetweenPoints, bool checkBoundary) 
        {
            this.checkBoundary = checkBoundary;
            this.mindDistBetweenPoints = mindDistBetweenPoints;
        }

        public IList<Point> Filter(IList<Point> points)
        {
            IList<Point> result = new List<Point>();
            if (points.Count == 0)
            {
                return result;
            }

            var point = new Point(points.First());
            result.Add(point);

            foreach (var currentSourcePoint in points.Skip(1))
            {
                if (!this.DistanceIsTooSmall(currentSourcePoint, point))
                {
                    point = new Point(currentSourcePoint);
                    result.Add(point);
                }
            }

            if (this.checkBoundary && result.Count > 1)
            {
                CheckFirstAndLastPoint(result);
            }

            return result;
        }

        private void CheckFirstAndLastPoint(IList<Point> points)
        {
            if (this.DistanceIsTooSmall(points.Last(), points.First()))
            {
                points.RemoveAt(points.Count - 1);
            }
        }

        private bool DistanceIsTooSmall(Point sourcePoint, Point destPoint)
        {
            return Point.Distance(sourcePoint, destPoint) < this.mindDistBetweenPoints;
        }
    }  
}
