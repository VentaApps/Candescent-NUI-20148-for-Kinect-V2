using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CCT.NUI.Core.OpenNI
{
    public interface IImageGenerator
    {
        IntPtr ImagePointer { get; }

        int Width { get; }

        int Height { get; }

        event EventHandler NewData;
    }
}
