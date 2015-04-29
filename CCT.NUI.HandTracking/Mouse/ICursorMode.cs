using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CCT.NUI.Core;

namespace CCT.NUI.HandTracking.Mouse
{
    public interface ICursorMode
    {
        bool HasPoint(HandCollection handData);

        Point GetPoint(HandCollection handData);

        CursorMode EnumValue { get; }
    }
}
