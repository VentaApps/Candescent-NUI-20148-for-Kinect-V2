using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CCT.NUI.Core
{
    public struct Volume
    {
        private Point location;
        private float width;
        private float height;
        private float depth;

        public Volume(IList<Point> points)
        {
            var rangeX = new Range(points.Select(p => p.X));
            var rangeY = new Range(points.Select(p => p.Y));
            var rangeZ = new Range(points.Select(p => p.Z));

            this.location = new Point(rangeX.Min, rangeY.Min, rangeZ.Min);
            this.width = rangeX.Interval;
            this.height = rangeY.Interval;
            this.depth = rangeZ.Interval;
        }

        public float X
        {
            get { return this.location.X; }
            set { this.location.X = value; }
        }

        public float Y
        {
            get { return this.location.Y; }
            set { this.location.Y = value; }
        }

        public float Z
        {
            get { return this.location.Z; }
            set { this.location.Z = value; }
        }

        public float Width
        {
            get { return this.width; }
            set { this.width = value; }
        }

        public float Height
        {
            get { return this.height; }
            set { this.height = value; }
        }

        public float Depth
        {
            get { return this.depth; }
            set { this.depth = value; }
        }

        public void SetLocation(Point location)
        {
            this.location = location;
        }

        public bool Contains(int x, int y, int z)
        {
            return x >= this.location.X && x <= this.location.X + this.width && y >= this.location.Y && y <= this.location.Y + this.height && z >= this.location.Z && z <= this.location.Z + this.depth;
        }
    }
}
