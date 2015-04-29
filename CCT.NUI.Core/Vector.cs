using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CCT.NUI.Core
{
    public struct Vector
    {
        public float X;
        public float Y;
        public float Z;

        public Vector(float x, float y, float z)
            : this()
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
        }

        public Vector GetNormalizedVector()
        {
            var length = this.Length;
            return new Vector(this.X /= length, this.Y /= length, this.Z /= length);
        }

        public float Length
        {
            get { return (float) Math.Sqrt(Math.Pow(this.X, 2) + Math.Pow(this.Y, 2) + Math.Pow(this.Z, 2)); }
        }

        public override string ToString()
        {
            return string.Format("x:{0} y:{1} z:{2}", this.X, this.Y, this.Z);
        }

        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is Vector))
            {
                return false;
            }
            var vector = ((Vector)obj);
            return vector.X == this.X && vector.Y == this.Y && vector.Z == this.Z;            
        }

        public override int GetHashCode()
        {
            return this.X.GetHashCode() ^ this.Y.GetHashCode() ^ this.Y.GetHashCode();
        }

        public static Vector Zero
        {
            get { return zero; }
        }

        private static Vector zero = new Vector(0, 0, 0);
    }
}
