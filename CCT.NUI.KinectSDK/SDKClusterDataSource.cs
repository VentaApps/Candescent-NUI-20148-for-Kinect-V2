using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Kinect;
using CCT.NUI.Core;
using CCT.NUI.Core.Clustering;
using System.Collections.Concurrent;

namespace CCT.NUI.KinectSDK
{
    public class SDKClusterDataSource : SensorDataSource<ClusterCollection>, IClusterDataSource
    {
        private IClusterFactory clusterFactory;
        //private IDepthPointFilter<DepthImageFrame> filter;       // older version
        private IDepthPointFilter<DepthFrame> filter;           // update: using DepthFrame instead of DepthImageFrame
        //private ConcurrentQueue<DepthImageFrame> queue;       // older version
        private ConcurrentQueue<DepthFrame> queue;             // update: using DepthFrame instead of DepthImageFrame
        private ActionRunner runner;

        //public SDKClusterDataSource(IKinectSensor nuiRuntime, IClusterFactory clusterFactory, IDepthPointFilter<DepthImageFrame> filter)
        //    : base(nuiRuntime)       // older version
        public SDKClusterDataSource(IKinectSensor nuiRuntime, IClusterFactory clusterFactory, IDepthPointFilter<DepthFrame> filter)
            : base(nuiRuntime)             // update: using DepthFrame instead of DeothImageFrame
        {
            this.CurrentValue = new ClusterCollection();
            this.clusterFactory = clusterFactory;
            this.filter = filter;
            //this.queue = new ConcurrentQueue<DepthImageFrame>();       // older version
            this.queue = new ConcurrentQueue<DepthFrame>();              // update: using DepthFrame instead of DepthImageFrame
            this.runner = new ActionRunner(() => Process());
        }

        public override int Width
        {
            get { return this.Sensor.DepthStreamWidth; }
        }

        public override int Height
        {
            get { return this.Sensor.DepthStreamHeight; }
        }

        //protected ClusterCollection Process(DepthImageFrame image)       // older version
        protected ClusterCollection Process(DepthFrame image)          // update: using DepthFrame instead of DepthImageFrame
        {
            return this.clusterFactory.Create(this.FindPointsWithinDepthRange(image));
        }

        //protected virtual IList<Point> FindPointsWithinDepthRange(DepthImageFrame image)       // older version
        protected virtual IList<Point> FindPointsWithinDepthRange(DepthFrame image)          // update: using DepthFrame instead of DepthImageFrame
        {
            return this.filter.Filter(image);
        }

        protected override void InnerStart()
        {
            //this.Sensor.DepthFrameReady += new EventHandler<DepthImageFrameReadyEventArgs>(nuiRuntime_DepthFrameReady);       // older version
            this.Sensor.DepthReader.FrameArrived += new EventHandler<DepthFrameArrivedEventArgs>(nuiRuntime_DepthFrameReady);          // update: adding depth frame arrival event handler
            this.runner.Start();
        }

        protected override void InnerStop()
        {
            //this.Sensor.DepthFrameReady -= new EventHandler<DepthImageFrameReadyEventArgs>(nuiRuntime_DepthFrameReady);       // older version
            this.Sensor.DepthReader.FrameArrived -= new EventHandler<DepthFrameArrivedEventArgs>(nuiRuntime_DepthFrameReady);          // update: removing depth frame arrival event handler
            this.runner.Stop();
        }

        private void Process()
        {
            //DepthImageFrame frame;       // older version
            DepthFrame frame;           // update: using DepthFrame instead of DepthImageFrame
            if (this.queue.TryDequeue(out frame))
            {
                this.CurrentValue = this.Process(frame);
                frame.Dispose();
                this.OnNewDataAvailable();
            }
        }

        //private void nuiRuntime_DepthFrameReady(object sender, DepthImageFrameReadyEventArgs e)       // older version
        //{
        //    var frame = e.OpenDepthImageFrame();
        //    if (frame != null)
        //    {
        //        this.queue.Enqueue(frame);
        //        System.Diagnostics.Debug.WriteLine(this.queue.Count);
        //    }
        //}

        private void nuiRuntime_DepthFrameReady(object sender, DepthFrameArrivedEventArgs e)       // update: creating depth frame event handler
        {
            //var frame = e.OpenDepthImageFrame();       // older version
            var frame = e.FrameReference.AcquireFrame();       // update: fetching frame from the kinect
            if (frame != null)
            {
                this.queue.Enqueue(frame);
                System.Diagnostics.Debug.WriteLine(this.queue.Count);
            }
        }
    }
}
