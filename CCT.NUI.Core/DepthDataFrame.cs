using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.IO;

namespace CCT.NUI.Core
{
    public class DepthDataFrame
    {
        private ushort[] data;
        private IntSize size;

        public DepthDataFrame(int width, int height)
            : this(width, height, new ushort[width * height])
        { }

        public DepthDataFrame(int width, int height, ushort[] data)
            : this(new IntSize(width, height), data)
        { }

        public DepthDataFrame(IntSize size, ushort[] data)
        {
            this.data = data;
            this.size = size;
        }

        public ushort[] Data
        {
            get { return this.data; }
        }

        public int MaxDepth
        {
            get { return this.data.Max(); }
        }

        public ushort this[int x, int y] 
        {
            get { return this.data[x + y * this.size.Width]; }
        }

        public int Width 
        {
            get { return this.size.Width; }
        }

        public int Height
        {
            get { return this.size.Height; }
        }

        public IntSize Size
        {
            get { return this.size; }
        }
    }
}
