using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using CCT.NUI.Core;
using CCT.NUI.Core.Clustering;
using CCT.NUI.Core.Shape;

namespace CCT.NUI.HandTracking
{
    public class HandData : IHand
    {
        private int id;
        private IList<FingerPoint> fingerPoints;

        private Shape shape;
        private Palm palm;

        public HandData(int id, Shape shape, Palm palm, IList<FingerPoint> fingerPoints)
        {
            this.id = id;
            this.shape = shape;
            this.palm = palm;
            this.fingerPoints = fingerPoints;
        }

        public int Id
        {
            get { return this.id; }
        }

        public Point Location
        {
            get { return this.shape.Location; }
        }

        public Volume Volume
        {
            get { return this.shape.Volume; }
        }

        public ConvexHull ConvexHull
        {
            get { return this.shape.ConvexHull; }
        }

        public Contour Contour
        {
            get { return this.shape.Contour; }
        }

        public bool HasPalmPoint
        {
            get { return this.palm != null; }
        }

        public Point? PalmPoint
        {
            get 
            {
                if (this.palm == null)
                {
                    return null;
                }
                return this.palm.Location; 
            }
        }

        public float PalmX
        {
            get { return this.PalmPoint.GetValueOrDefault().X; }
        }

        public float PalmY
        {
            get { return this.PalmPoint.GetValueOrDefault().Y; }
        }

        public double PalmDistance
        {
            get { return this.palm.DistanceToContour; }
        }

        public IList<FingerPoint> FingerPoints
        {
            get { return this.fingerPoints; }
        }

        public IList<FingerPoint> NewlyDetectedFingerPoints { get; set; }

        public IEnumerable<IFinger> Fingers
        {
            get { return this.fingerPoints.OfType<IFinger>(); }
        }

        public int FingerCount
        {
            get { return this.fingerPoints.Count; }
        }

        public bool HasContour
        {
            get { return this.Contour != null; }
        }

        public bool HasFingers
        {
            get { return this.FingerCount > 0; }
        }
    }
}
