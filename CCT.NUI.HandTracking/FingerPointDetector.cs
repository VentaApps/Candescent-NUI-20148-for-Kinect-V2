using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CCT.NUI.Core;
using CCT.NUI.Core.Shape;

namespace CCT.NUI.HandTracking
{
    internal class FingerPointDetector
    {
        private HandDataSourceSettings settings;
        private IList<Point> contourPoints;
        private LineThinner lineThinner;

        public FingerPointDetector(HandDataSourceSettings settings)
        {
            this.settings = settings;
            this.lineThinner = new LineThinner(this.settings.MinimumDistanceBetweenFingerPoints, true);
        }

        public IList<FingerPoint> FindFingerPoints(Contour contour, ConvexHull convexHull)
        {
            this.contourPoints = contour.Points;
            var thinnedHullPoints = this.lineThinner.Filter(convexHull.Points);
            var verifiedHullPoints = this.VerifyPointsByContour(thinnedHullPoints);
            return verifiedHullPoints.Select(r => new FingerPoint(r)).ToList();
        }

        private IList<Point> VerifyPointsByContour(IEnumerable<Point> candidatePoints)
        {
            return candidatePoints.Where(p => VerifyIsFingerPointByContour(p)).ToList();
        }

        private bool VerifyIsFingerPointByContour(Point candidatePoint)
        {
            var index = this.FindIndexOfClosestPointOnContour(candidatePoint);

            var pointOnContour = contourPoints[index];
            var point1 = FindPointInDistanceForward(pointOnContour, index);
            var point2 = FindPointInDistanceBackward(pointOnContour, index);

            var distance = Point.Distance(point1, point2);
            if (distance > this.settings.MaximumDistanceBetweenIntersectionPoints) 
            {
                return false;
            }

            var center = Point.Center(point1, point2);
            return Point.Distance(center, pointOnContour) >= this.settings.MinimumDistanceFingerPointToIntersectionLine;
        }

        private int FindIndexOfClosestPointOnContour(Point fingerPoint)
        {
            int index = 0;
            int resultIndex = -1;
            double minDist = double.MaxValue;
            foreach (Point p in this.contourPoints) 
            {
                var distance = Point.Distance(p.X, p.Y, fingerPoint.X, fingerPoint.Y);
                if (distance < minDist)
                {
                    resultIndex = index;
                    minDist = distance;
                }
                index++;
            }
            return resultIndex;
        }

        private Point FindPointInDistanceForward(Point candidatePoint, int startIndex)
        {
            return this.FindPointInDistance(candidatePoint, startIndex, (i) => i + 1);
        }

        private Point FindPointInDistanceBackward(Point candidatePoint, int startIndex)
        {
            return this.FindPointInDistance(candidatePoint, startIndex, (i) => i - 1);
        }

        private Point FindPointInDistance(Point candidatePoint, int startIndex, Func<int, int> directionFunc)
        {
            int resultIndex = startIndex;
            do
            {
                resultIndex = directionFunc(resultIndex);
                if (resultIndex < 0)
                {
                    resultIndex = contourPoints.Count - 1;
                }
                if (resultIndex >= contourPoints.Count)
                {
                    resultIndex = 0;
                }
            }
            while (Point.Distance(candidatePoint, this.contourPoints[resultIndex]) < this.settings.MinimumDistanceIntersectionPoints && resultIndex != startIndex);

            return this.contourPoints[resultIndex];
        }
    }
}