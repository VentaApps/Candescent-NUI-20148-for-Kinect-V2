using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Kinect;
using CCT.NUI.Core;
using CCT.NUI.Core.Clustering;
using CCT.NUI.Core.Shape;
using CCT.NUI.Core.OpenNI;
using CCT.NUI.KinectSDK;

namespace CCT.NUI.HandTracking
{
    public class HandDataFactory
    {
        private ClusterDataSourceSettings clusteringSettings;
        private ShapeDataSourceSettings shapeSettings;
        private HandDataSourceSettings handSettings;

        private IDepthPointFilter<IntPtr> filter;
        private ImageFrameDepthPointFilter sdkFilter;

        private IClusterFactory clusterFactory;
        private IClusterShapeFactory shapeFactory;
        private IHandDataFactory handFactory;       

        public HandDataFactory(IntSize size)
            : this(size, new ClusterDataSourceSettings(), new ShapeDataSourceSettings(), new HandDataSourceSettings())
        { }

        public HandDataFactory(IntSize size, ClusterDataSourceSettings clusteringSettings, ShapeDataSourceSettings shapeSettings, HandDataSourceSettings handSettings)
        {
            this.clusteringSettings = clusteringSettings;
            this.shapeSettings = shapeSettings;
            this.handSettings = handSettings;

            this.clusterFactory = new KMeansClusterFactory(this.clusteringSettings, size);
            this.filter = new PointerDepthPointFilter(size, this.clusteringSettings.MinimumDepthThreshold, this.clusteringSettings.MaximumDepthThreshold, this.clusteringSettings.LowerBorder);

            this.shapeFactory = new ClusterShapeFactory(this.shapeSettings);
            this.handFactory = new ShapeHandDataFactory(this.handSettings);
        }

        public HandDataFactory(IKinectSensor sensor, IntSize size, ClusterDataSourceSettings clusteringSettings, ShapeDataSourceSettings shapeSettings, HandDataSourceSettings handSettings)
            : this(size, clusteringSettings, shapeSettings, handSettings)
        {
            this.sdkFilter = new ImageFrameDepthPointFilter(sensor, size, this.clusteringSettings.MinimumDepthThreshold, this.clusteringSettings.MaximumDepthThreshold, this.clusteringSettings.LowerBorder);
        }

        public HandCollection Create(IntPtr depthData)
        {
            return this.Create(this.filter.Filter(depthData));
        }

        //public HandCollection Create(DepthImageFrame imageFrage)       // older version
        public HandCollection Create(DepthFrame imageFrage)       // update: using DepthFrame instead of DepthImageframe
        {
            return this.Create(this.sdkFilter.Filter(imageFrage));
        }

        private HandCollection Create(IList<Point> allPointsInDepthRange)
        {
            return this.handFactory.Create(this.shapeFactory.Create(this.clusterFactory.Create(allPointsInDepthRange)));
        }
    }
}
