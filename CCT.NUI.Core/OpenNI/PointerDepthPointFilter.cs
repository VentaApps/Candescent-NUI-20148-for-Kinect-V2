using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CCT.NUI.Core.Clustering;
using System.Runtime.ExceptionServices;
using System.Runtime.InteropServices;

namespace CCT.NUI.Core.OpenNI
{
    public class PointerDepthPointFilter : IDepthPointFilter<IntPtr>
    {
        private int minimumDepthThreshold;
        private int maximumDepthThreshold;
        private int lowerBorder;
        private IntSize size;

        public PointerDepthPointFilter(IntSize size, int minimumDepthThreshold, int maximumDepthThreshold, int lowerBorder)
        {
            this.size = size;
            this.minimumDepthThreshold = minimumDepthThreshold;
            this.maximumDepthThreshold = maximumDepthThreshold;
            this.lowerBorder = lowerBorder;
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
                int maxY = localHeight - lowerBorder;
                int minDepth = minimumDepthThreshold;
                int maxDepth = maximumDepthThreshold;

                for (int y = 0; y < localHeight; y++)
                {
                    for (int x = 0; x < localWidth; x++)
                    {
                        ushort depthValue = *pDepth;
                        if (depthValue >= minDepth && depthValue <= maxDepth && y < maxY) //Should not be put in a seperate method for performance reasons
                        {
                            result.Add(new Point(x, y, depthValue));
                        }
                        pDepth++;
                    }
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
