using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CCT.NUI.Core.Shape
{
    internal class ContourFactory
    {
        private ContourTracer tracer;
        private LineThinner thinner;  

        public ContourFactory(float lineThinningDistance)
        {
            this.tracer = new ContourTracer();
            this.thinner = new LineThinner(lineThinningDistance, false);
        }

        public Contour CreateContour(DepthMap map, float left, float top)
        {
            if (map.FillRate < 0.01) 
            {
                return Contour.Empty;
            }
            var points = this.Translate(this.tracer.GetContourPoints(map), (int)left, (int)top);
            points = this.thinner.Filter(points);
            return new Contour(points);
        }

        private IList<Point> Translate(IEnumerable<Point> points, int deltaX, int deltaY)
        {
            var translatedPoints = new List<Point>();
            foreach (var p in points)
            {
                translatedPoints.Add(new Point(p.X + deltaX, p.Y + deltaY, p.Z));
            }
            return translatedPoints;
        }
    }
}
