using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CCT.NUI.Core;

namespace CCT.NUI.Samples.ImageManipulation
{
    public class Transformation
    {
        private float scale = 1;
        private float angle = 0;

        public Transformation()
        {
            this.Reset();
        }

        public void FindTransformation(Point p1, Point p2, Point t1, Point t2)
        {
            var factor = Point.Distance(t1, t2) / Point.Distance(p1, p2);
            this.scale = (float) factor;

            var delta = (float)((Math.Atan2(p2.Y - p1.Y, p2.X - p1.X) - Math.Atan2(t2.Y - t1.Y, t2.X - t1.X)) * 180 / Math.PI);
            if (Math.Abs(delta) < 30)
            {
                this.angle -= delta;
            }
        }

        public float Scale
        {
            get { return this.scale; }
        }

        public float Angle
        {
            get { return this.angle; }
        }

        internal void Reset()
        {
            this.angle = 0;
            this.scale = 1;
        }
    }
}
