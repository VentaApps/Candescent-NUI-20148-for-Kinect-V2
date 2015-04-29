using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Shapes;
using System.Windows.Media;
using System.Windows;

namespace CCT.NUI.WPFSamples.PinCode
{
    public class PathFactory
    {
        public const double FULL_ARC = 360;

        public Path CreatePath(Point location, double angle, double radius, double innerRadius, Brush brush)
        {
            var isLargeArc = angle > FULL_ARC / 2;

            var path = new Path();
            var segments = new PathSegmentCollection();
            var arcPoint = ConvertRadianToCartesian(angle, radius);
            var innerArcPoint = ConvertRadianToCartesian(angle, innerRadius);

            segments.Add(new LineSegment(new Point(location.X, location.Y - radius), false));
            segments.Add(new ArcSegment(new Point(location.X + arcPoint.X, location.Y + arcPoint.Y), new Size(radius, radius), 0, isLargeArc, SweepDirection.Clockwise, false));
            segments.Add(new LineSegment(new Point(location.X + innerArcPoint.X, location.Y + innerArcPoint.Y), false));
            segments.Add(new ArcSegment(new Point(location.X, location.Y - innerRadius), new Size(innerRadius, innerRadius), 0, isLargeArc, SweepDirection.Counterclockwise, false));

            var figure = new PathFigure(location, segments, true);
            path.Data = new PathGeometry { Figures = new PathFigureCollection { figure } };

            path.Fill = brush;
            return path;
        }

        private Point ConvertRadianToCartesian(double angle, double radius)
        {
            var angleRadius = (Math.PI / (FULL_ARC / 2)) * (angle - FULL_ARC / 4);
            var x = radius * Math.Cos(angleRadius);
            var y = radius * Math.Sin(angleRadius);
            return new Point(x, y);
        }
    }
}
