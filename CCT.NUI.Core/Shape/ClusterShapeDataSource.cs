using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CCT.NUI.Core.Clustering;

namespace CCT.NUI.Core.Shape
{
    public class ClusterShapeDataSource : DataSourceProcessor<ShapeCollection, ClusterCollection>, IShapeDataSource
    {
        private IntSize size;
        private IClusterShapeFactory factory;

        public ClusterShapeDataSource(IClusterDataSource clusterDataSource)
            : this(clusterDataSource, new ShapeDataSourceSettings())
        { }

        public ClusterShapeDataSource(IClusterDataSource clusterDataSource, ShapeDataSourceSettings settings)
            : base(clusterDataSource)
        {
            this.factory = new ClusterShapeFactory(settings);
            this.size = clusterDataSource.Size;
            this.CurrentValue = new ShapeCollection();
        }

        public int Width
        {
            get { return this.size.Width; }
        }

        public int Height
        {
            get { return this.size.Height; }
        }

        public IntSize Size
        {
            get { return this.size; }
        }

        protected override unsafe ShapeCollection Process(ClusterCollection clusterData)
        {
            return this.factory.Create(clusterData);
        }
    }
}
