using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CCT.NUI.Core
{
    public interface IImagePointerDataSource : IDataSource<IntPtr>
    {
        IntSize Size { get; }

        int Width { get; }

        int Height { get; }
    }
}
