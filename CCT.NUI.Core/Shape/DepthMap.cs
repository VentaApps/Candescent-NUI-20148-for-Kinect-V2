using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CCT.NUI.Core.Shape
{
    public class DepthMap
    {
        private int[,] map;

        public DepthMap(int[,] map)
        {
            this.map = map;
        }

        public DepthMap(int width, int height)
        {
            this.map = new int[width, height];
        }

        public int this[int x, int y]
        {
            get 
            {
                if (this.IsOutOfBounds(x, y))
                {
                    return 0;
                }
                return this.map[x, y];
            }
            set
            {
                this.map[x, y] = value;
            }
        }

        public bool IsSet(int x, int y)
        {
            return this[x, y] > 0;
        }

        public int Width
        {
            get { return this.map.GetLength(0); }
        }

        public int Height
        {
            get { return this.map.GetLength(1); }
        }

        public int[,] Map
        {
            get { return this.map; }
        }

        public float FillRate
        {
            get 
            {
                int setCount = 0;
                int width = this.Width; //making these local increases speed
                int height = this.Height;
                for (int x = 0; x < width; x++)
                {
                    for (int y = 0; y < height; y++)
                    {
                        if (IsSet(y, y))
                        {
                            setCount++;
                        }
                    }
                }
                return (float)setCount / (width * height);
            }
        }

        private bool IsOutOfBounds(int x, int y)
        {
            return x < 0 || y < 0 || x >= this.Width || y >= this.Height;
        }
    }
}
