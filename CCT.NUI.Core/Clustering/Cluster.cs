using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CCT.NUI.Core.Clustering
{
    public class Cluster : ILocatable
    {
        protected Point center;
        protected volatile IList<Point> points;
        private Volume volume;

        public Cluster(IList<Point> points)
            : this(Point.Center(points), points)
        { }

        public Cluster(Point center, IList<Point> points)
        {
            this.center = center;
            this.points = points;
            this.volume = new Volume(points);
        }

        public Cluster(Point center, IList<Point> points, Volume volume)
        {
            this.center = center;
            this.points = points;
            this.volume = volume;
        }

        public Point Center
        {
            get { return this.center; }
        }

        public Point Location
        {
            get { return this.center; }
        }

        public Volume Volume
        {
            get { return this.volume; }
        }

        public IList<Point> AllPoints { get; set; }

        public IList<Point> Points
        {
            get { return this.points; }
        }

        public int PointCount
        {
            get { return this.points.Count; }
        }

        public float X
        {
            get { return this.volume.X; }
        }

        public float Y
        {
            get { return this.volume.Y; }
        }

        public float Width
        {
            get { return this.volume.Width; }
        }

        public float Height
        {
            get { return this.volume.Height; }
        }

        public Rectangle Area
        {
            get { return new Rectangle(this.X, this.Y, this.Width, this.Height); }
        }
    }
}
