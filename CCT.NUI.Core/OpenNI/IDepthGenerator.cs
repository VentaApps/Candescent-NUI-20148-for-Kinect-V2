using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CCT.NUI.Core.OpenNI
{
    public interface IDepthGenerator : IImageGenerator
    {
        int DeviceMaxDepth { get; }
    }
}
