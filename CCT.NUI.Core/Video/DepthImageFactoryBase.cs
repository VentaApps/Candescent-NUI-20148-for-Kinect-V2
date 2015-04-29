using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.ExceptionServices;
using System.Runtime.InteropServices;

namespace CCT.NUI.Core.Video
{
    public class DepthImageFactoryBase
    {
        protected Histogram histogram;

        public DepthImageFactoryBase(int maxDepth)
        {
            this.histogram = new Histogram(maxDepth);
        }

        [HandleProcessCorruptedStateExceptions]
        protected unsafe void CreateHistogram(IntPtr sourceData, int width, int height)
        {
            try
            {
                this.histogram.Reset();

                int points = 0;
                var pDepth = (ushort*)sourceData.ToPointer();
                for (int y = 0; y < height; ++y)
                {
                    for (int x = 0; x < width; ++x, ++pDepth)
                    {
                        ushort depthVal = *pDepth;
                        if (depthVal > 0)
                        {
                            this.histogram.Increase(depthVal);
                            points++;
                        }
                    }
                }
                this.histogram.PostProcess(points);
            }
            catch (AccessViolationException)
            { }
            catch (SEHException)
            { }
        }
    }
}
