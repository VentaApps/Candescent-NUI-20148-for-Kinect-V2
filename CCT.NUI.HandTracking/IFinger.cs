using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CCT.NUI.Core;

namespace CCT.NUI.HandTracking
{
    public interface IFinger : ILocatable
    {
        Point Fingertip { get; }
    }
}
