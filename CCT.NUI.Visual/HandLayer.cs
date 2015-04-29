using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using CCT.NUI.Core;
using Size = CCT.NUI.Core.Size;
using CCT.NUI.HandTracking;

namespace CCT.NUI.Visual
{
    public class HandLayer : LayerBase
    {
        private Pen redPen = new Pen(Brushes.Red, 3);
        private Pen yellowPen = new Pen(Brushes.Yellow, 3);
        private Font font = new Font("Arial", 10, FontStyle.Bold);
        private IHandDataSource dataSource;
        private float zoomFactor;
        private float zoomHandFactor = 1;

        public HandLayer(IHandDataSource dataSource)
            : this(dataSource, 1)
        { }

        public HandLayer(IHandDataSource dataSource, float zoomFactor)
        {
            this.dataSource = dataSource;
            this.dataSource.NewDataAvailable += dataSource_NewDataAvailable;
            this.SetZoomFactor(zoomFactor);

            this.ShowConvexHull = true;
            this.ShowContour = true;
            this.ShowFingerDepth = true;
        }

        public bool ShowContour { get; set; }

        public bool ShowConvexHull { get; set; }

        public bool ShowFingerDepth { get; set; }

        public void SetZoomFactor(float zoomFactor)
        {
            this.zoomFactor = zoomFactor;
        }

        public void SetZoomHandFactor(float zoomFactor)
        {
            this.zoomHandFactor = zoomFactor;
        }

        public float ZoomFactor
        {
            get { return this.zoomFactor; }
        }

        public void SetTargetSize(System.Drawing.Size size)
        {
            float xRatio = (float)size.Width / this.dataSource.Width;
            float yRatio = (float)size.Height / this.dataSource.Height;
            this.SetZoomFactor(Math.Min(xRatio, yRatio));
        }

        public override void Dispose()
        {
            base.Dispose();
            this.dataSource.NewDataAvailable -= dataSource_NewDataAvailable;
            this.redPen.Dispose();
            this.yellowPen.Dispose();
            this.font.Dispose();
        }

        public override void Paint(Graphics g)
        {
            var brushSwitcher = new BrushSwitcher();
            foreach (var hand in this.dataSource.CurrentValue.Hands)
            {
                PaintHand(g, hand);
            }
        }

        private void PaintHand(Graphics g, HandData hand)
        {
            g.TranslateTransform(hand.Location.X * this.zoomFactor, hand.Location.Y * this.zoomFactor);
            g.ScaleTransform(this.zoomHandFactor, this.zoomHandFactor);
            g.TranslateTransform(-hand.Location.X * this.zoomFactor, -hand.Location.Y * this.zoomFactor);
            g.ScaleTransform(this.zoomFactor, this.zoomFactor);
            if (this.ShowConvexHull)
            {
                this.PaintCovexHull(hand, g);
            }
            if (this.ShowContour && hand.Contour != null)
            {
                this.PaintContour(hand, g);
            }
            DrawFingerPoints(hand, g);
            this.DrawCenter(hand, g);
            g.ResetTransform();
        }

        void dataSource_NewDataAvailable(HandCollection handData)
        {
            this.OnRequestRefresh();
        }

        protected virtual void DrawCenter(HandData hand, Graphics g)  
        {
            g.FillEllipse(Brushes.Blue, hand.Location.X - 5, hand.Location.Y - 5, 10, 10);

            if (hand.HasPalmPoint)
            {
                g.FillEllipse(Brushes.SpringGreen, hand.PalmPoint.Value.X - 5, hand.PalmPoint.Value.Y - 5, 10, 10);
                var palmSize = hand.PalmDistance;
                g.DrawEllipse(Pens.SpringGreen, (int)(hand.PalmPoint.Value.X - palmSize), (int)(hand.PalmPoint.Value.Y - palmSize), (int)(palmSize * 2), (int)(palmSize * 2));
            }
        } 

        protected virtual void PaintContour(HandData hand, Graphics g)
        {
            if (hand.Contour.Points.Count > 1)
            {
                var points = hand.Contour.Points.Select(p => new System.Drawing.Point((int)p.X, (int)p.Y)).ToArray();
                g.DrawLines(yellowPen, points);
            }
        }

        protected virtual void PaintCovexHull(HandData hand, Graphics g)
        {
            if (hand.ConvexHull.Count > 3)
            {
                g.DrawLines(Pens.White, hand.ConvexHull.Points.Select(p => new System.Drawing.Point((int)p.X, (int)p.Y)).ToArray());
            }
        }

        protected virtual void DrawFingerPoints(HandData hand, Graphics g)
        {
            foreach (var point in hand.FingerPoints)
            {
                PaintFingerPoint(g, point);
            }
        }

        private void PaintFingerPoint(Graphics g, FingerPoint point)
        {
            g.FillEllipse(Brushes.Red, point.X - 5, point.Y - 5, 11, 11);
            g.DrawEllipse(Pens.Red, point.X - 30, point.Y - 30, 60, 60);

            g.DrawString(point.Z.ToString(), this.font, Brushes.White, point.X + 3, point.Y + 3);

            g.FillEllipse(Brushes.Orange, point.BaseLeft.X - 5, point.BaseLeft.Y - 5, 11, 11);
            g.FillEllipse(Brushes.Orange, point.BaseRight.X - 5, point.BaseRight.Y - 5, 11, 11);

            if (!CCT.NUI.Core.Point.IsZero(point.BaseLeft) && !CCT.NUI.Core.Point.IsZero(point.BaseRight))
            {
                var baseCenter = CCT.NUI.Core.Point.Center(point.BaseLeft, point.BaseRight);
                g.DrawLine(Pens.Orange, baseCenter.X, baseCenter.Y, point.X + point.DirectionVector.X * 60, point.Y + point.DirectionVector.Y * 60);
            }
        }
    }
}
