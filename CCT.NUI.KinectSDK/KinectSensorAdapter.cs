using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Kinect;

namespace CCT.NUI.KinectSDK
{
    public class KinectSensorAdapter : IKinectSensor, IDisposable
    {
        private KinectSensor sensor;
        private ColorFrameReader _colorReader;
        private DepthFrameReader _depthReader;

        public KinectSensorAdapter(KinectSensor sensor, bool useNearMode)
        {
            this.sensor = sensor;
            //if (useNearMode)                // older version: near mode isn't applicable in kinect v2 as it already starts detecting depth from 500 millieters which is the near mode in kinect v1
            //{
            //    this.sensor.DepthStream.Range = DepthRange.Near;
            //}

            //this.sensor.ColorFrameReady += new EventHandler<ColorImageFrameReadyEventArgs>(sensor_ColorFrameReady);       // older version
            //this.sensor.DepthFrameReady += new EventHandler<DepthImageFrameReadyEventArgs>(sensor_DepthFrameReady);       // older version

            this._colorReader = this.sensor.ColorFrameSource.OpenReader();         // added: initializing the Colored images reader
            this._colorReader.FrameArrived += new EventHandler<ColorFrameArrivedEventArgs>(sensor_ColorFrameReady);           // update: intializing the event handler of colored frames arriving from kinect v2

            this._depthReader = this.sensor.DepthFrameSource.OpenReader();         // added: initializing the Depth images reader
            this._depthReader.FrameArrived += new EventHandler<DepthFrameArrivedEventArgs>(sensor_DepthFrameReady);           // update: intializing the event handler of depth frames arriving from kinect v2
        }

        public void Dispose()
        {
            //this.sensor.ColorFrameReady -= new EventHandler<ColorImageFrameReadyEventArgs>(sensor_ColorFrameReady);       // older version
            //this.sensor.DepthFrameReady -= new EventHandler<DepthImageFrameReadyEventArgs>(sensor_DepthFrameReady);       // older version

            this._colorReader.FrameArrived -= new EventHandler<ColorFrameArrivedEventArgs>(sensor_ColorFrameReady);       // update: deleting the Colored image event handler
            this._depthReader.FrameArrived -= new EventHandler<DepthFrameArrivedEventArgs>(sensor_DepthFrameReady);       // update: deleting the Depth image event handler
        }

        public int DepthStreamWidth
        {
            get { return this.sensor.DepthFrameSource.FrameDescription.Width; }
        }

        public int DepthStreamHeight
        {
            get { return this.sensor.DepthFrameSource.FrameDescription.Height; }
        }

        public int ColorStreamWidth
        {
            get { return this.sensor.ColorFrameSource.FrameDescription.Width; }
        }

        public int ColorStreamHeight
        {
            get { return this.sensor.ColorFrameSource.FrameDescription.Height; }
        }

        public ColorFrameReader ColorReader
        {
            get { return this._colorReader; }
        }

        public DepthFrameReader DepthReader
        {
            get { return this._depthReader; }
        }

        //public event EventHandler<DepthImageFrameReadyEventArgs> DepthFrameReady;       // older version

        //void sensor_DepthFrameReady(object sender, DepthImageFrameReadyEventArgs e)       // older version
        //{
        //    if (this.DepthFrameReady != null)
        //    {
        //        this.DepthFrameReady(this, e);
        //    }
        //}

        public event EventHandler<DepthFrameArrivedEventArgs> DepthFrameReady;            // update

        void sensor_DepthFrameReady(object sender, DepthFrameArrivedEventArgs e)          // update: Depth Frame arriving event handler
        {
            if (this.DepthFrameReady != null)
            {
                this.DepthFrameReady(this, e);
            }
        }

        //public event EventHandler<ColorImageFrameReadyEventArgs> ColorFrameReady;       // older version

        //void sensor_ColorFrameReady(object sender, ColorImageFrameReadyEventArgs e)       // older version
        //{
        //    if (this.ColorFrameReady != null)
        //    {
        //        this.ColorFrameReady(this, e);
        //    }
        //}
        public event EventHandler<ColorFrameArrivedEventArgs> ColorFrameReady;       // update

        void sensor_ColorFrameReady(object sender, ColorFrameArrivedEventArgs e)          // update: Color Frame arriving event handler
        {
            if (this.ColorFrameReady != null)
            {
                this.ColorFrameReady(this, e);
            }
        }
    }
}
