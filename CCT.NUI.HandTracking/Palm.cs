using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CCT.NUI.Core;

namespace CCT.NUI.HandTracking
{
    public class Palm
    {
        private Point location;
        private double distanceToContour;

        public Palm(Point location, double distanceToContour)
        {
            this.location = location;
            this.distanceToContour = distanceToContour;
        }

        public Point Location
        {
            get { return this.location; }
        }

        public double DistanceToContour
        {
            get { return this.distanceToContour; }
        }
    }
}
