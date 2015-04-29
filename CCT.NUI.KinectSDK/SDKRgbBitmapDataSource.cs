using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Kinect;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Media;

namespace CCT.NUI.KinectSDK
{
    public class SDKRgbBitmapDataSource : SDKBitmapDataSource
    {
        public SDKRgbBitmapDataSource(IKinectSensor sensor)
            : base(sensor)
        { }

        public override int Width
        {
            get { return this.Sensor.ColorStreamWidth; }
        }

        public override int Height
        {
            get { return this.Sensor.ColorStreamHeight; }
        }

        protected override void  InnerStart()
        {
            this.Sensor.ColorReader.FrameArrived += new EventHandler<ColorFrameArrivedEventArgs>(sensor_ColorFrameReady);
        }

        protected override void InnerStop()
        {
            //this.Sensor.ColorFrameReady -= new EventHandler<ColorImageFrameReadyEventArgs>(sensor_ColorFrameReady);       // older version
            this.Sensor.ColorReader.FrameArrived -= new EventHandler<ColorFrameArrivedEventArgs>(sensor_ColorFrameReady);       // update; removing Colored frame arrival event handler
        }

        //protected void sensor_ColorFrameReady(object sender, ColorImageFrameReadyEventArgs e)       // older version
        //{
        //    using (var frame = e.OpenColorImageFrame())
        //    {
        //        if (frame != null)
        //        {
        //            this.ProcessFrame(frame);
        //        }
        //    }
        //}

        protected void sensor_ColorFrameReady(object sender, ColorFrameArrivedEventArgs e)       // update: creating colored frame arrival event handler
        {
            //using (var frame = e.OpenColorImageFrame())       // older version
            using (var frame = e.FrameReference.AcquireFrame())       // update: fetching new colored frame from the sensor
            {
                if (frame != null)
                {
                    this.ProcessFrame(frame);
                }
            }
        }

        //protected unsafe void ProcessFrame(ColorImageFrame frame)       // older version
        protected unsafe void ProcessFrame(ColorFrame frame)       // update: using ColorFrame instead of ColorImageFrame
        {
            //var bytes = new byte[frame.PixelDataLength];       // older version
            int BGRA_BITS_PER_PIXEL = (PixelFormats.Bgr32.BitsPerPixel + 7) / 8;       // added: creating the suitable number of bytes per pixel to convert the image from Yuy2(kinect v2 default) to RGBA
            var bytes = new byte[BGRA_BITS_PER_PIXEL * frame.FrameDescription.Width * frame.FrameDescription.Height];       // update: creating byte array with the suitable size after converting the image from Yuy2 (Kinect v2 default) to RGBA

            //frame.CopyPixelDataTo(bytes);       // older version
            if (frame.RawColorImageFormat == ColorImageFormat.Bgra)       // added: checking the current format of the fetched frame
            {
                frame.CopyRawFrameDataToArray(bytes);       // update: copy if the current fetched frame format is RGBA
            }
            else
            {
                frame.CopyConvertedFrameDataToArray(bytes, ColorImageFormat.Bgra);       // update: Convert the current fetched frame to RGBA and copping it's data
            }


            BitmapData bitmapData = this.CurrentValue.LockBits(new System.Drawing.Rectangle(0, 0, this.Width, this.Height), ImageLockMode.WriteOnly, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            byte* pDest = (byte*)bitmapData.Scan0.ToPointer();
            int pointer = 0;

            var maxIndex = this.Width * this.Height;
            for (int index = 0; index < maxIndex; index++)
            {
                pDest[0] = bytes[pointer];
                pDest[1] = bytes[pointer + 1];
                pDest[2] = bytes[pointer + 2];
                pDest += 3;
                pointer += 4;
            }
            this.CurrentValue.UnlockBits(bitmapData);
            this.OnNewDataAvailable();
        }
    }
}
