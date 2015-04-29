using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CCT.NUI.Core.Clustering
{
    public interface IClusterFactory
    {
        ClusterCollection Create(IList<Point> points);
    }
}
