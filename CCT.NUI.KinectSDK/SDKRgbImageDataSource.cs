using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Kinect;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows;
using System.IO;

namespace CCT.NUI.KinectSDK
{
    public class SDKRgbImageDataSource : SDKImageDataSource
    {
        private byte[] data;

        public SDKRgbImageDataSource(IKinectSensor nuiRuntime)
            : base(nuiRuntime)
        { }

        public override int Width
        {
            get { return this.Sensor.ColorStreamWidth; }
        }

        public override int Height
        {
            get { return this.Sensor.ColorStreamHeight; }
        }

        protected override void InnerStart()
        {
            //this.Sensor.ColorFrameReady += new EventHandler<ColorImageFrameReadyEventArgs>(Sensor_ColorFrameReady);       // older version
            this.Sensor.ColorReader.FrameArrived += new EventHandler<ColorFrameArrivedEventArgs>(Sensor_ColorFrameReady);       // update: adding Color frame arrival event handler
        }

        protected override void InnerStop()
        {
            //this.Sensor.ColorFrameReady -= new EventHandler<ColorImageFrameReadyEventArgs>(Sensor_ColorFrameReady);       // older version
            this.Sensor.ColorReader.FrameArrived -= new EventHandler<ColorFrameArrivedEventArgs>(Sensor_ColorFrameReady);       // update: removing Color frame arrival event handler
        }

        //void Sensor_ColorFrameReady(object sender, ColorImageFrameReadyEventArgs e)       // older version
        //{
        //    using (var image = e.OpenColorImageFrame())
        //    {
        //        if (image != null)
        //        {
        //            if (this.data == null)
        //            {
        //                this.data = new byte[image.PixelDataLength];
        //            }
        //            image.CopyPixelDataTo(this.data);

        //            this.writeableBitmap.WritePixels(new Int32Rect(0, 0, image.Width, image.Height), data, image.Width * image.BytesPerPixel, 0);
        //            this.OnNewDataAvailable();
        //        }
        //    }
        //}

        void Sensor_ColorFrameReady(object sender, ColorFrameArrivedEventArgs e)       // update: creating color frame arrival event handler
        {
            //using (var image = e.OpenColorImageFrame())       // older version
            using (var image = e.FrameReference.AcquireFrame())       // update: fetching new color frame form the sensor
            {
                if (image != null)
                {
                    if (this.data == null)
                    {
                        //this.data = new byte[image.PixelDataLength];       // older version
                        int BGRA_BITS_PER_PIXEL = (PixelFormats.Bgr32.BitsPerPixel + 7) / 8;       // added: creating the suitable number of bytes per pixel to convert the image from Yuy2(kinect v2 default) to RGBA
                        this.data = new byte[BGRA_BITS_PER_PIXEL * image.FrameDescription.Width * image.FrameDescription.Height];       // update: creating byte array with the suitable size after converting the image from Yuy2 (Kinect v2 default) to RGBA
                    }
                    //image.CopyPixelDataTo(this.data);       // older version
                    if (image.RawColorImageFormat == ColorImageFormat.Bgra)       // added: checking the current format of the fetched frame
                    {
                        image.CopyRawFrameDataToArray(data);       // update: copy if the current fetched frame format is RGBA
                    }
                    else
                    {
                        image.CopyConvertedFrameDataToArray(data, ColorImageFormat.Bgra);       // update: Convert the current fetched frame to RGBA and copping it's data
                    }

                    //this.writeableBitmap.WritePixels(new Int32Rect(0, 0, image.Width, image.Height), data, image.Width * image.BytesPerPixel, 0);       // older version

                    int stride = image.FrameDescription.Width * PixelFormats.Bgr32.BitsPerPixel / 8;       // added

                    //BitmapSource bitmap = BitmapSource.Create(image.FrameDescription.Width, image.FrameDescription.Height, 96, 96,                 // added: trial to resize the image but it doesn't make any difference in the performance, it just makes it a little bit slower.....
                    //    PixelFormats.Bgr32, null, data, stride);
                    //ScaleTransform scale = new ScaleTransform((480 / bitmap.PixelWidth), (640 / bitmap.PixelHeight));
                    //TransformedBitmap tbBitmap = new TransformedBitmap(bitmap, scale);
                    
                    //// Calculate stride of source
                    //int scalledStride = tbBitmap.PixelWidth * (tbBitmap.Format.BitsPerPixel / 8);

                    //// Create data array to hold source pixel data
                    //Byte[] scalledData = new byte[stride * tbBitmap.PixelHeight];

                    //// Copy source image pixels to the data array
                    //tbBitmap.CopyPixels(scalledData, stride, 0);

                    //this.writeableBitmap.WritePixels(new Int32Rect(0, 0, tbBitmap.PixelWidth, tbBitmap.PixelHeight), scalledData,       // older version
                    //    scalledStride, 0);

                    this.writeableBitmap.WritePixels(new Int32Rect(0, 0, image.FrameDescription.Width, image.FrameDescription.Height), data,       // update
                        stride , 0);
                    
                    this.OnNewDataAvailable();
                }
            }
        }
    }
}
