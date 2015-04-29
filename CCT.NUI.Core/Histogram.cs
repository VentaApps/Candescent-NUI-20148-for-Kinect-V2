using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CCT.NUI.Core
{
    public class Histogram
    {
        private int[] data = null;

        public Histogram(int maxValue)
        {
            this.data = new int[maxValue];
        }

        public void Reset()
        {
            for (int i = 0; i < this.data.Length; i++)
            {
                this.data[i] = 0;
            }
        }

        public void Increase(int depthValue)
        {
            if (depthValue < this.data.Length)
            {
                this.data[depthValue]++;
            }
        }

        public void PostProcess(int points)
        {
            for (int i = 1; i < this.data.Length; i++)
            {
                this.data[i] += this.data[i - 1];
            }

            if (points > 0)
            {
                for (int i = 1; i < this.data.Length; i++)
                {
                    this.data[i] = (int)(256 * (1.0f - (this.data[i] / (float)points)));
                }
            }
        }

        public int GetValue(ushort depthValue)
        {
            if (depthValue >= this.data.Length)
            {
                return 0;
            }
            return this.data[depthValue];
        }

        public int Length
        {
            get { return this.data.Length; }
        }
    }
}
