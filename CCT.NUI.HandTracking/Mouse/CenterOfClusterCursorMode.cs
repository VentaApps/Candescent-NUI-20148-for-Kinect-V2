using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CCT.NUI.Core;

namespace CCT.NUI.HandTracking.Mouse
{
    class CenterOfClusterCursorMode : ICursorMode
    {
        public bool HasPoint(HandCollection handData)
        {
            return handData.Count > 0;
        }

        public Point GetPoint(HandCollection handData)
        {
            return handData.Hands.First().Location;
        }

        public CursorMode EnumValue
        {
            get { return CursorMode.CenterOfCluster; }
        }
    }
}
