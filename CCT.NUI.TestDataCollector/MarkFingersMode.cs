using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CCT.NUI.Core;

namespace CCT.NUI.TestDataCollector
{
    public class MarkFingersMode : ISelectMode
    {
        private HandDataViewModel handData;

        public MarkFingersMode(HandDataViewModel handData)
        {
            this.handData = handData;
        }

        public void SelectPoint(Core.Point point)
        {
            if (point.Z > 0)
            {
                var existingPoint = this.handData.FingerPoints.Where(p => Point.Distance(p.Point, point) < 15).FirstOrDefault();

                if (existingPoint == null)
                {
                    this.handData.MarkFinger(new FingerPointViewModel(point));
                }
                else
                {
                    existingPoint.Point = point;
                }
            }
        }
    }
}
