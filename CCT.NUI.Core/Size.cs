using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CCT.NUI.Core
{
    public struct Size
    {
        public Size(float width, float height)
            : this()
        {
            this.Width = width;
            this.Height = height;
        }

        public float Width { get; set; }

        public float Height { get; set; }

        public override string ToString()
        {
            return this.Width + " / " + this.Height;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is Size))
            {
                return false;
            }
            var size = ((Size)obj);
            return size.Width == Width && size.Height == Height;
        }

        public override int GetHashCode()
        {
            return this.Width.GetHashCode() ^ this.Height.GetHashCode();
        }
    }
}
