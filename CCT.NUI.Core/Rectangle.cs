using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CCT.NUI.Core
{
    public struct Rectangle
    {
        public Rectangle(Point location, Size size)
            : this()
        {
            this.Location = location;
            this.Size = size;
        }

        public Rectangle(float x, float y, float width, float height)
            : this(new Point(x, y, 0), new Size(width, height))
        { }

        public Point Location;

        public Size Size;

        public bool Contains(Point p)
        {
            return p.X >= this.Location.X && p.Y >= this.Location.Y && p.X <= this.Location.X + this.Size.Width && p.Y <= this.Location.Y + this.Size.Height;
        }

        public override string ToString()
        {
            return this.Location.ToString() + " / " + this.Size.ToString();
        }

        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is Rectangle))
            {
                return false;
            }
            var rectangle = ((Rectangle)obj);
            return rectangle.Location.Equals(this.Location) && rectangle.Size.Equals(this.Size);
        }

        public override int GetHashCode()
        {
            return this.Location.GetHashCode() ^ this.Size.GetHashCode();
        }
    }
}
