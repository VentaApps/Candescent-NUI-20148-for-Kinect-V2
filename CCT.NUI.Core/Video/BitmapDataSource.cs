using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace CCT.NUI.Core.Video
{
    public class BitmapDataSource : DataSourceProcessor<Bitmap, IntPtr>, IBitmapDataSource
    {
        private IntSize size;
        private IBitmapFactory imageFactory;

        public BitmapDataSource(IImagePointerDataSource dataSource, IBitmapFactory imageFactory)
            : base(dataSource)
        {
            this.CurrentValue = new Bitmap(dataSource.Width, dataSource.Height, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            this.size = new IntSize(this.CurrentValue.Width, this.CurrentValue.Height);
            this.imageFactory = imageFactory;
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

        protected override Bitmap Process(IntPtr sourceData)
        {
            return this.ProcessUnsafe(sourceData);
        }

        private unsafe Bitmap ProcessUnsafe(IntPtr sourceData)
        {
            lock (this.CurrentValue)
            {
                this.imageFactory.CreateImage(this.CurrentValue, sourceData);
                return this.CurrentValue;
            }
        }
    }
}
