using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Drawing;
using System.Drawing.Imaging;

namespace CCT.NUI.Core.Video
{
    public class RGBBitmapDataSource : BitmapDataSource
    {
        public RGBBitmapDataSource(IRgbPointerDataSource dataSource)
            : base(dataSource, new RgbBitmapFactory())
        { }
    }
}
