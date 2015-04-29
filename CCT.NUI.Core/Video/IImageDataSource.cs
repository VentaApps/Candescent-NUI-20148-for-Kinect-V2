using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Media;

namespace CCT.NUI.Core.Video
{
    public interface IImageDataSource : IDataSource<ImageSource>
    {
        IntSize Size { get; }

        int Width { get; }

        int Height { get; }
    }
}
