using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CCT.NUI.Core.Clustering
{
    public class ClusterCollection
    {
        public ClusterCollection()
        {
            this.Clusters = new List<Cluster>();
        }

        public ClusterCollection(IList<Cluster> clusters)
        {
            this.Clusters = clusters;
        }

        public IList<Cluster> Clusters { get; private set; }

        public int Count
        {
            get { return this.Clusters.Count; }
        }
    }
}
