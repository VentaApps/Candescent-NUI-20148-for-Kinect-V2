using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CCT.NUI.HandTracking.Mouse
{
    public interface IClickMode
    {
        void Process(HandCollection handData);

        ClickMode EnumValue { get; }
    }
}
