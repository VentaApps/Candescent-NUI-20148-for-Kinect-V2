using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using CCT.NUI.Core;
using Size = CCT.NUI.Core.Size;
using CCT.NUI.Core.Clustering;

namespace CCT.NUI.Visual
{
    public class ClusterLayer : LayerBase
    {
        private IClusterDataSource dataSource;
        private float zoomFactor;
        private int centerSize = 10;
        private int clusterPointSize = 3;

        public ClusterLayer(IClusterDataSource dataSource)
            : this(dataSource, 1)
        { }

        public ClusterLayer(IClusterDataSource dataSource, float zoomFactor)
        {
            this.dataSource = dataSource;
            this.dataSource.NewDataAvailable += dataSource_NewDataAvailable;
            this.zoomFactor = zoomFactor;
        }

        public void SetZoomFactor(float zoomFactor)
        {
            this.zoomFactor = zoomFactor;
        }

        public override void Paint(Graphics g)
        {
            g.ScaleTransform(this.zoomFactor, this.zoomFactor);
            var brushSwitcher = new BrushSwitcher();
            foreach (var cluster in this.dataSource.CurrentValue.Clusters)
            {
                this.DrawClusterPoints(cluster, g, brushSwitcher.GetNext());
                this.DrawCenter(cluster, g);
            }
        }

        private void DrawCenter(Cluster cluster, Graphics g)  
        {
            var halfSize = centerSize / 2;
            g.FillEllipse(Brushes.Blue, cluster.Center.X - halfSize, cluster.Center.Y - halfSize, centerSize, centerSize);
        }

        private void DrawClusterPoints(Cluster cluster, Graphics g, Brush brush)
        {
            foreach (var point in cluster.Points)
            {
                var halfSize = clusterPointSize / 2;
                g.FillRectangle(brush, point.X - halfSize, point.Y - halfSize, clusterPointSize, clusterPointSize);
            }
        }

        private void dataSource_NewDataAvailable(ClusterCollection clusters)
        {
            this.OnRequestRefresh();
        }

        public override void Dispose()
        {
            base.Dispose();
            this.dataSource.NewDataAvailable -= dataSource_NewDataAvailable;
        }
    }
}
