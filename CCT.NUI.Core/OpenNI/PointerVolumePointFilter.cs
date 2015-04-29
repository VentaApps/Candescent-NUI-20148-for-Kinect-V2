using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CCT.NUI.Core.Clustering;
using System.Runtime.ExceptionServices;
using System.Runtime.InteropServices;

namespace CCT.NUI.Core.OpenNI
{
    public class PointerVolumePointFilter : IDepthPointFilter<IntPtr>
    {
        private int leftBorder;
        private int rightBorder;
        private int topBorder;
        private int lowerBorder;
        private int minDepth;
        private int maxDepth;
        private IntSize size;

        public PointerVolumePointFilter(IntSize size, int leftBorder, int rightBorder, int topBorder, int lowerBorder, int minDepth, int maxDepth)
        {
            this.size = size;
            this.leftBorder = leftBorder;
            this.rightBorder = rightBorder;
            this.topBorder = topBorder;
            this.lowerBorder = lowerBorder;
            this.minDepth = minDepth;
            this.maxDepth = maxDepth;
        }

        [HandleProcessCorruptedStateExceptions]
        public unsafe IList<Point> Filter(IntPtr source)
        {
            var result = new List<Point>();
            try
            {
                ushort* pDepth = (ushort*)source.ToPointer();

                int localHeight = this.size.Height; //5ms faster when it's a local variable
                int localWidth = this.size.Width;
                int minY = this.topBorder;
                int maxY = localHeight - this.lowerBorder;
                int minX = this.leftBorder;
                int maxX = localWidth - this.rightBorder;
                int minZ = this.minDepth;
                int maxZ = this.maxDepth;
                int nextLine = this.rightBorder + minX;

                pDepth += minY * localWidth;
                pDepth += minX;
                for (int y = minY; y < maxY; y++)
                {
                    for (int x = minX; x < maxX; x++)
                    {
                        ushort depthValue = *pDepth;
                        if (depthValue >= minZ && depthValue <= maxZ) //Should not be put in a seperate method for performance reasons
                        {
                            result.Add(new Point(x, y, depthValue));
                        }
                        pDepth++;
                    }
                    pDepth += nextLine; 
                }
            }
            catch (AccessViolationException)
            { }
            catch (SEHException)
            { }
            return result;
        }
    }
}
