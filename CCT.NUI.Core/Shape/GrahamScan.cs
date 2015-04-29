using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CCT.NUI.Core.Shape
{
    internal class GrahamScan
    {
        private IList<Point> points;

        public GrahamScan(IList<Point> points)
        {
            this.points = points;
        }

        public ConvexHull FindHull()
        {
            if (this.points.Count <= 3)
            {
                return new ConvexHull(this.points);
            }

            var pointsSortedByAngle = this.SortPointsByAngle();
            int index = 1;

            while (index + 1 < pointsSortedByAngle.Count)
            {
                var value = PointAngleComparer2D.Compare(pointsSortedByAngle[index - 1], pointsSortedByAngle[index + 1], pointsSortedByAngle[index]);
                if (value < 0)
                {
                    index++;
                }
                else //Also removes points that are on a line when value == 0
                {
                    pointsSortedByAngle.RemoveAt(index);
                    if (index > 1)
                    {
                        index--;
                    }
                }
            }

            pointsSortedByAngle.Add(pointsSortedByAngle.First());
            return new ConvexHull(pointsSortedByAngle);
        }

        private Point FindMinimalOrdinatePoint()
        {
            var minPoint = this.points[0];
            for (int index = 1; index < this.points.Count; index++)
            {
                minPoint = ReturnMinPoint(minPoint, this.points[index]);
            }
            return minPoint;
        }

        private Point ReturnMinPoint(Point p1, Point p2)
        {
            if (p1.Y < p2.Y)
            {
                return p1;
            }
            else if (p1.Y == p2.Y)
            {
                if (p1.X < p2.X)
                {
                    return p1;
                }
            }
            return p2;
        }

        private IList<Point> SortPointsByAngle()
        {
            var p0 = this.FindMinimalOrdinatePoint();
            var comparer = new PointAngleComparer2D(p0);
            var sortedPoints = new List<Point>(this.points);
            sortedPoints.Remove(p0);
            sortedPoints.Insert(0, p0);
            sortedPoints.Sort(1, sortedPoints.Count - 1, comparer);
            return sortedPoints;
        }
    } 
}
