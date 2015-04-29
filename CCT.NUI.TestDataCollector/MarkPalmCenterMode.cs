using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CCT.NUI.TestDataCollector
{
    public class MarkPalmCenterMode : ISelectMode
    {
        private HandDataViewModel handData;

        public MarkPalmCenterMode(HandDataViewModel handData)
        {
            this.handData = handData;
        }

        public void SelectPoint(Core.Point point)
        {
            if (point.Z > 0)
            {
                this.handData.MarkCenterOfPalm(point);
            }
        }
    }
}
