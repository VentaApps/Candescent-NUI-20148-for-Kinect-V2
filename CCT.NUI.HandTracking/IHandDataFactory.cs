using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CCT.NUI.Core.Shape;

namespace CCT.NUI.HandTracking
{
    public interface IHandDataFactory
    {
        HandCollection Create(ShapeCollection shapes);
    }
}
