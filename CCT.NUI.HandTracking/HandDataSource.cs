using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CCT.NUI.Core;
using CCT.NUI.Core.Shape;

namespace CCT.NUI.HandTracking
{
    public class HandDataSource : DataSourceProcessor<HandCollection, ShapeCollection>, IHandDataSource
    {
        private IntSize size;
        private ShapeHandDataFactory factory;

        public HandDataSource(IShapeDataSource shapeDataSource)
            : this(shapeDataSource, new HandDataSourceSettings())
        { }

        public HandDataSource(IShapeDataSource shapeDataSource, HandDataSourceSettings settings)
            : base(shapeDataSource)
        {
            this.factory = new ShapeHandDataFactory(settings);
            this.size = shapeDataSource.Size;
            this.CurrentValue = new HandCollection();
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

        protected override unsafe HandCollection Process(ShapeCollection shapeData)
        {
            return this.factory.Create(shapeData);
        }
    }
}
