using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CCT.NUI.Core.Clustering;

namespace CCT.NUI.Core.OpenNI
{
    public class OpenNIClusterDataSource : DataSourceProcessor<ClusterCollection, IntPtr>, IClusterDataSource
    {
        private IntSize size;
        private IClusterFactory clusterFactory;
        private IDepthPointFilter<IntPtr> filter;

        public OpenNIClusterDataSource(IDepthPointerDataSource dataSource, ClusterDataSourceSettings settings)
            : this(dataSource, new KMeansClusterFactory(settings, dataSource.Size), new PointerDepthPointFilter(dataSource.Size, settings.MinimumDepthThreshold, settings.MaximumDepthThreshold, settings.LowerBorder))
        { }

        public OpenNIClusterDataSource(IDepthPointerDataSource dataSource, IClusterFactory clusterFactory, IDepthPointFilter<IntPtr> filter)
            : base(dataSource)
        {
            this.size = dataSource.Size;
            this.CurrentValue = new ClusterCollection();
            this.clusterFactory = clusterFactory;
            this.filter = filter;
        }

        public int Width
        {
            get { return this.size.Width; }
        }

        public int Height
        {
            get { return this.size.Height; }
        }

        public IntSize Size { get { return this.size; } }

        protected override ClusterCollection Process(IntPtr sourceData)
        {
            return this.clusterFactory.Create(this.FindPointsWithinDepthRange(sourceData));
        }

        protected virtual unsafe IList<Point> FindPointsWithinDepthRange(IntPtr dataPointer)
        {
            return this.filter.Filter(dataPointer);
        }
    }
}
