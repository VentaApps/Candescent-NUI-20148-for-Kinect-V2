using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CCT.NUI.Core
{
    public class Range
    {
        public Range(float min, float max)
        {
            this.Min = min;
            this.Max = max;
        }

        public Range(IEnumerable<float> values)
        {
            this.Min = values.First();
            this.Max = this.Min;
            foreach (var value in values.Skip(1))
            {
                this.Min = Math.Min(value, this.Min);
                this.Max = Math.Max(value, this.Max);
            }
        }

        public float Min { get; set; }

        public float Max { get; set; }

        public float Interval { get { return this.Max - this.Min; } }

        public override string ToString()
        {
            return "{" + Min.ToString() + " - " + Max.ToString() + "} Range: " + Interval.ToString();
        }
    }
}
