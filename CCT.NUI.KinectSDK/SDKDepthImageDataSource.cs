using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Microsoft.Kinect;
using System.Windows;

namespace CCT.NUI.KinectSDK
{
    public class SDKDepthImageDataSource : SDKImageDataSource
    {
        private byte[] depthFrame32;
        //private short[] data;       // older version
        private ushort[] data;       // update: using unsigned 16 bits array for depth frames of kinect v2

        public SDKDepthImageDataSource(IKinectSensor nuiRuntime)
            : base(nuiRuntime)
        {
            this.depthFrame32 = new byte[Width * Height * 4];
        }

        public override int Width
        {
            get { return this.Sensor.DepthStreamWidth; }
        }

        public override int Height
        {
            get { return this.Sensor.DepthStreamHeight; }
        }

        protected override void InnerStart()
        {
            //this.Sensor.DepthFrameReady += new EventHandler<DepthImageFrameReadyEventArgs>(Sensor_DepthFrameReady);       // older version
            this.Sensor.DepthReader.FrameArrived += new EventHandler<DepthFrameArrivedEventArgs>(Sensor_DepthFrameReady);       // update: adding Depth Frame Arrival event handler
        }

        protected override void InnerStop()
        {
            //this.Sensor.DepthFrameReady -= new EventHandler<DepthImageFrameReadyEventArgs>(Sensor_DepthFrameReady);       // older version
            this.Sensor.DepthReader.FrameArrived -= new EventHandler<DepthFrameArrivedEventArgs>(Sensor_DepthFrameReady);       // update: removing Depth Frame Arrival event handler
        }

        //void Sensor_DepthFrameReady(object sender, DepthImageFrameReadyEventArgs e)       // older version
        //{
        //    using (var image = e.OpenDepthImageFrame())
        //    {
        //        if (image != null)
        //        {
        //            if (this.data == null)
        //            {
        //                this.data = new short[image.PixelDataLength];
        //            }
        //            image.CopyPixelDataTo(this.data);
        //            this.ConvertData(this.data);
        //            this.writeableBitmap.WritePixels(new Int32Rect(0, 0, image.Width, image.Height), this.depthFrame32, image.Width * 4, 0);
        //            this.OnNewDataAvailable();
        //        }
        //    }
        //}

        void Sensor_DepthFrameReady(object sender, DepthFrameArrivedEventArgs e)       // update: Creating depth frame arrival event handler
        {
            //using (var image = e.OpenDepthImageFrame())       // older version
            using (var image = e.FrameReference.AcquireFrame())       // update: fetching new depth frame from the sensor
            {
                if (image != null)
                {
                    if (this.data == null)
                    {
                        //this.data = new short[image.PixelDataLength];       // older version
                        this.data = new ushort[image.FrameDescription.Width * image.FrameDescription.Height];       // update: creating unsignd 16 bits array for the depth frame data
                    }
                    //image.CopyPixelDataTo(this.data);       // older version
                    image.CopyFrameDataToArray(this.data);       // update: copping depth frame data
                    this.ConvertData(this.data);

                    //this.writeableBitmap.WritePixels(new Int32Rect(0, 0, image.Width, image.Height), this.depthFrame32, image.Width * 4, 0);       // older version
                    this.writeableBitmap.WritePixels(new Int32Rect(0, 0, image.FrameDescription.Width,
                        image.FrameDescription.Height), this.depthFrame32, image.FrameDescription.Width * 4, 0);       // update
                    this.OnNewDataAvailable();
                }
            }
        }

        //private void ConvertData(short[] data)
        private void ConvertData(ushort[] data)
        {
            for (int i16 = 0, i32 = 0; i16 < data.Length && i32 < this.depthFrame32.Length; i16++, i32 += 4)
            {
                //int realDepth = data[i16] >> DepthImageFrame.PlayerIndexBitmaskWidth;
                int realDepth = data[i16];
                byte intensity = (byte)(~(realDepth >> 4));

                this.depthFrame32[i32 + 2] = intensity;
                this.depthFrame32[i32 + 1] = intensity;
                this.depthFrame32[i32] = intensity;
            }
        }
    }
}
