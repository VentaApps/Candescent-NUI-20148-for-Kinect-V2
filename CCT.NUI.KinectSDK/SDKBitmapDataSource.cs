using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Kinect;
using System.Drawing;
using CCT.NUI.Core;
using CCT.NUI.Core.Video;

namespace CCT.NUI.KinectSDK
{
    public abstract class SDKBitmapDataSource : SensorDataSource<Bitmap>, IBitmapDataSource
    {
        public SDKBitmapDataSource(IKinectSensor nuiRuntime)
            : base(nuiRuntime)
        {
            this.CurrentValue = new Bitmap(this.Width, this.Height, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
        }

        public override void Dispose()
        {
            this.CurrentValue.Dispose();
        }
    }
}
