using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CCT.NUI.Core;

namespace CCT.NUI.HandTracking
{
    public class FingerPoint : IFinger, ILocatable
    {
        private Point location;

        public FingerPoint(Point location)
        {
            this.location = location;
            this.FrameCount = 1;
        }

        public int Id { get; set; }

        public int FrameCount { get; set; }

        public int NegativeFrameCount { get; set; }

        public float X
        {
            get { return this.location.X; }
        }

        public float Y
        {
            get { return this.location.Y; }
        }

        public float Z
        {
            get { return this.location.Z; }
        }

        public Point Location
        {
            get { return this.location; }
        }

        public Point Fingertip
        {
            get { return this.location; }
        }

        public Point BaseLeft { get; set; }

        public Point BaseRight { get; set; }

        public Vector DirectionVector { get; set; }
    }
}
