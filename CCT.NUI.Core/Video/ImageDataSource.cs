using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Media.Imaging;
using System.Windows.Media;

namespace CCT.NUI.Core.Video
{
    public class ImageDataSource : DataSourceProcessor<ImageSource, IntPtr>, IImageDataSource
    {
        private IntSize size;
        private IImageFactory imageFactory;
        protected WriteableBitmap writeableBitmap;

        public ImageDataSource(IImagePointerDataSource dataSource, IImageFactory imageFactory)
            : base(dataSource)
        {
            this.writeableBitmap = new WriteableBitmap(dataSource.Width, dataSource.Height, 96, 96, PixelFormats.Bgr24, null);
            this.CurrentValue = writeableBitmap;
            this.size = new IntSize(dataSource.Width, dataSource.Height);
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

        protected override ImageSource Process(IntPtr sourceData)
        {
            return this.ProcessUnsafe(sourceData);
        }

        private unsafe System.Windows.Media.ImageSource ProcessUnsafe(IntPtr sourceData)
        {
            lock (this.CurrentValue)
            {
                this.imageFactory.CreateImage(this.writeableBitmap, sourceData);
                return this.CurrentValue;
            }
        }
    }
}
