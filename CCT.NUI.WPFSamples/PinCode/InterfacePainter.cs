using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using System.Windows.Shapes;
using CCT.NUI.HandTracking;
using System.Windows;
using System.Globalization;

namespace CCT.NUI.WPFSamples.PinCode
{
    public class InterfacePainter
    {
        private Path path = null;
        private FormattedText text;
        private FormattedText number;
        private Typeface typeFace = new Typeface(new FontFamily("Arial"), FontStyles.Normal, FontWeights.Bold, FontStretches.Normal);

        private PathFactory pathFactory;

        public InterfacePainter ()
	    {
            this.pathFactory = new PathFactory();
	    }

        public bool AnimationInProgress { get; set; }

        public double Radius { get; set; }

        public Point Center { get; set; }

        public byte Opacity { get; set; }

        public double Value { get; set; }

        internal void DrawHand(HandData handData, DrawingContext drawingContext)
        {
            if (this.AnimationInProgress)
            {
                var brush = new SolidColorBrush(Color.FromArgb(this.Opacity, 255, 255, 255));
                this.CreateText((int)this.Center.X + " | " + (int)this.Center.Y + "\n" + new Random().NextDouble(), brush);
                this.CreateNumberText(handData.FingerCount.ToString(), brush);

                drawingContext.DrawText(this.text, this.Center);
                drawingContext.DrawText(this.number, new Point(this.Center.X, this.Center.Y - this.Radius - 40));
                drawingContext.DrawEllipse(null, new Pen(brush, 4), this.Center, this.Radius - 20, this.Radius - 20);
                drawingContext.DrawEllipse(null, new Pen(brush, 4), this.Center, this.Radius + 10, this.Radius + 10);
                this.DrawFingerPoints(handData, drawingContext);
                this.UpdateProgress(brush);
                drawingContext.DrawGeometry(brush, null, path.Data);
            }
        }

        internal void UpdateRadius(double distance)
        {
            if (Math.Abs(distance - this.Radius) > 25)
            {
                this.Radius = distance;
            }
            else
            {
                this.Radius += (distance - this.Radius) / 5;
            }
            this.Radius = Math.Max(80, this.Radius);
        }

        internal void UpdateProgress(Brush brush)
        {
            this.path = this.pathFactory.CreatePath(this.Center, Value, this.Radius, Math.Max(1, this.Radius - 10), brush);
        }

        private void CreateText(string text, Brush brush)
        {
            this.text = new FormattedText(text, CultureInfo.CurrentCulture, FlowDirection.LeftToRight, this.typeFace, 8.25, brush);
        }

        private void CreateNumberText(string text, Brush brush)
        {
            this.number = new FormattedText(text, CultureInfo.CurrentCulture, FlowDirection.LeftToRight, this.typeFace, 22, brush);
        }

        protected virtual void DrawFingerPoints(HandData cluster, DrawingContext drawingContext)
        {
            foreach (var point in cluster.FingerPoints)
            {
                PaintFingerPoint(point, drawingContext);
            }
        }

        private void PaintFingerPoint(FingerPoint point, DrawingContext drawingContext)
        {
            drawingContext.DrawEllipse(new SolidColorBrush(Color.FromArgb(Math.Min((byte)100, this.Opacity), 255, 255, 255)), null, new Point(point.X, point.Y), 10, 10);
            drawingContext.DrawEllipse(new SolidColorBrush(Color.FromArgb(this.Opacity, 255, 255, 255)), null, new Point(point.X, point.Y), 5.5, 5.5);
        }

        private void DrawLines(DrawingContext drawingContext, Pen pen, Point[] points)
        {
            var pathGeometry = new PathGeometry();
            var figure = new PathFigure(points.First(), points.Skip(1).Select(p => new LineSegment(p, true)), true);
            pathGeometry.Figures = new PathFigureCollection { figure };
            drawingContext.DrawGeometry(null, pen, pathGeometry);
        }


        internal void UpdateCenter(HandData hand, double distance)
        {
            var newCenter = new Point(hand.PalmX, hand.PalmY - distance / 3);
            if (CCT.NUI.Core.Point.Distance(newCenter.X, newCenter.Y, this.Center.X, this.Center.Y) > 75)
            {
                this.Center = newCenter;
            }
            else
            {
                this.Center = new Point(this.Center.X + (newCenter.X - this.Center.X) / 5, this.Center.Y + (newCenter.Y - this.Center.Y) / 5);
            }
        }
    }
}
