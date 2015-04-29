using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CCT.NUI.Core
{
    public interface IDepthPointerDataSource : IImagePointerDataSource
    {
        int MaxDepth { get; }
    }
}
