using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using OpenNI;
using CCT.NUI.Core.Clustering;
using CCT.NUI.Core.OpenNI;
using CCT.NUI.Core.Shape;
using CCT.NUI.Core.Video;

namespace CCT.NUI.Core.OpenNI
{
    public class OpenNIDataSourceFactory : IDataSourceFactory
    {
        private Context context;
        private IDepthPointerDataSource depthPointerDataSource = null;
        private IRgbPointerDataSource rgbPointerDataSource = null;

        private OpenNIRunner runner;

        public OpenNIDataSourceFactory(string configFile)
        {
            if (!File.Exists(configFile))
            {
                throw new FileNotFoundException("Config file is missing: " + configFile);
            }
            ScriptNode node = null;
            this.context = Context.CreateFromXmlFile(configFile, out node);

            this.runner = new OpenNIRunner(this.context);
            this.runner.Start();
        }

        public OpenNIRunner Runner
        {
            get { return this.runner; }
        }

        public Context Context
        {
            get { return this.context; }
        }

        public DepthGenerator GetDepthGenerator()
        {
            return this.context.FindExistingNode(NodeType.Depth) as DepthGenerator;
        }

        public ImageGenerator GetImageGenerator()
        {
            return this.context.FindExistingNode(NodeType.Image) as ImageGenerator;
        }

        public IImageDataSource CreateRGBImageDataSource()
        {
            return new RgbImageDataSource(this.GetRGBPointerDataSource());
        }

        public IImageDataSource CreateDepthImageDataSource()
        {
            return new DepthImageDataSource(this.GetDepthPointerDataSource());
        }

        public IBitmapDataSource CreateRGBBitmapDataSource()
        {
            return new RGBBitmapDataSource(this.GetRGBPointerDataSource());
        }

        public IBitmapDataSource CreateDepthBitmapDataSource()
        {
            return new DepthBitmapDataSource(this.GetDepthPointerDataSource());
        }

        public IClusterDataSource CreateClusterDataSource(ClusterDataSourceSettings clusterDataSourceSettings)
        {
            return new OpenNIClusterDataSource(this.GetDepthPointerDataSource(), clusterDataSourceSettings);
        }

        public IClusterDataSource CreateClusterDataSource()
        {
            return this.CreateClusterDataSource(new ClusterDataSourceSettings());
        }

        public IShapeDataSource CreateShapeDataSource()
        {
            return new ClusterShapeDataSource(this.CreateClusterDataSource());
        }

        public IShapeDataSource CreateShapeDataSource(IClusterDataSource clusterdataSource)
        {
            return new ClusterShapeDataSource(clusterdataSource, new ShapeDataSourceSettings());
        }

        public IShapeDataSource CreateShapeDataSource(IClusterDataSource clusterdataSource, ShapeDataSourceSettings shapeDataSourceSettings)
        {
            return new ClusterShapeDataSource(clusterdataSource, shapeDataSourceSettings);
        }

        public IShapeDataSource CreateShapeDataSource(ClusterDataSourceSettings clusterDataSourceSettings, ShapeDataSourceSettings shapeDataSourceSettings)
        {
            return new ClusterShapeDataSource(this.CreateClusterDataSource(clusterDataSourceSettings), shapeDataSourceSettings);
        }

        private IDepthPointerDataSource GetDepthPointerDataSource()
        {
            lock (this)
            {
                if (this.depthPointerDataSource == null)
                {
                    var adapter = new DepthGeneratorAdapter(this.GetDepthGenerator());
                    this.runner.Add(adapter);
                    this.depthPointerDataSource = new DepthPointerDataSource(adapter);
                }
            }
            return this.depthPointerDataSource;
        }

        private IRgbPointerDataSource GetRGBPointerDataSource()
        {
            lock (this)
            {
                if (this.rgbPointerDataSource == null)
                {
                    var adapter = new ImageGeneratorAdapter(this.GetImageGenerator());
                    this.runner.Add(adapter);
                    this.rgbPointerDataSource = new RgbPointerDataSource(adapter);
                }
            }
            return this.rgbPointerDataSource;
        }

        public void Dispose()
        {
            this.runner.Stop();
            this.context.Dispose();
        }

        public void SetAlternativeViewpointCapability()
        {
            this.GetDepthGenerator().AlternativeViewpointCapability.SetViewpoint(this.GetImageGenerator());
        }

        public TrackingClusterDataSource CreateTrackingClusterDataSource()
        {
            return new TrackingClusterDataSource(this.Context, this.GetDepthGenerator(), this.GetDepthPointerDataSource());
        }
    }
}
