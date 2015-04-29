using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CCT.NUI.Core
{
    public struct IntSize
    {
        public IntSize(int width, int height)
            : this()
        {
            this.Width = width;
            this.Height = height;
        }

        public int Width { get; set; }

        public int Height { get; set; }

        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is IntSize))
            {
                return false;
            }
            var otherSize = (IntSize)obj;
            return otherSize.Width == this.Width && otherSize.Height == this.Height;
        }

        public override int GetHashCode()
        {
            return this.Width.GetHashCode() ^ this.Height.GetHashCode();
        }

        public override string ToString()
        {
            return this.Width + " / " + this.Height;
        }
    }
}
