using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CCT.NUI.Core.Clustering;

namespace CCT.NUI.Core.Shape
{
    public interface IClusterShapeFactory
    {
        ShapeCollection Create(ClusterCollection clusterData);
    }
}
