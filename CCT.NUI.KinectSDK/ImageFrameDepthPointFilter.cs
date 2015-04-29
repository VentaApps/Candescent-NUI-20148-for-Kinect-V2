using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Kinect;
using CCT.NUI.Core;
using CCT.NUI.Core.Clustering;

namespace CCT.NUI.KinectSDK
{
    //public class ImageFrameDepthPointFilter : IDepthPointFilter<DepthImageFrame>       // older version
    public class ImageFrameDepthPointFilter : IDepthPointFilter<DepthFrame>          // update: using DepthFrame instead of DepthImageFrame
    {
        private IKinectSensor sensor;
        private IntSize size;
        private int minimumDepthThreshold;
        private int maximumDepthThreshold;
        private int lowerBorder;

        //private short[] data;       // older version
        private ushort[] data;        // update: Depth Frame data in kinect v2 is unsgned 16bits so we used ushort instead of short

        public ImageFrameDepthPointFilter(IKinectSensor sensor, IntSize size, int minimumDepthThreshold, int maximumDepthThreshold, int lowerBorder)
        {
            this.sensor = sensor;
            this.size = size;
            this.minimumDepthThreshold = minimumDepthThreshold;
            this.maximumDepthThreshold = maximumDepthThreshold;
            this.lowerBorder = lowerBorder;
        }

        //public IList<Point> Filter(DepthImageFrame source)       // older version
        public IList<Point> Filter(DepthFrame source)          // update: using DepthFrame instead of DepthImageFrame
        {
            var result = new List<Point>();

            var localHeight = this.size.Height; //5ms faster when it's a local variable
            var localWidth = this.size.Width;
            var maxY = localHeight - this.lowerBorder;
            var minDepth = this.minimumDepthThreshold;
            var maxDepth = this.maximumDepthThreshold;
            if (this.data == null)
            {
                //this.data = new short[source.PixelDataLength];       // older version
                this.data = new ushort[source.FrameDescription.Width * source.FrameDescription.Height];          // update: using FrameDescription data
            }
            //source.CopyPixelDataTo(this.data);       // older version
            source.CopyFrameDataToArray(this.data);       // update: copping frame data to ushort array
            var pointer = 0;

            for (int y = 0; y < localHeight; y++)
            {
                for (int x = 0; x < localWidth; x++)
                {
                    //int realDepth = data[pointer] >> DepthImageFrame.PlayerIndexBitmaskWidth;       // older version
                    int realDepth = data[pointer];             // update: real depth frame data arre the ones in the array and doesn't hold any data for the players

                    if (realDepth <= maxDepth && realDepth >= minDepth && y < maxY) //Should not be put in a seperate method for performance reasons
                    {
                        result.Add(new Point(x, y, realDepth));
                    }
                    pointer++;
                }
            }
            return result;
        }
    }
}
