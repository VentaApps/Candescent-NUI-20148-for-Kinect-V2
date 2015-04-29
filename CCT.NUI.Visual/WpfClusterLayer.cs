using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Shapes;
using System.Windows.Media;
using System.Windows;
using CCT.NUI.Core;
using Size = CCT.NUI.Core.Size;
using CCT.NUI.Core.Clustering;

namespace CCT.NUI.Visual
{
    public class WpfClusterLayer : UIElement, IWpfLayer
    {
        private IClusterDataSource dataSource;
        private int centerSize = 5;
        private int clusterPointSize = 3;

        private Canvas canvas;

        public WpfClusterLayer(IClusterDataSource dataSource)
        {
            this.dataSource = dataSource;
            this.dataSource.NewDataAvailable += dataSource_NewDataAvailable;
        }

        public void Activate(Canvas canvas)
        {
            this.canvas = canvas;
            this.canvas.Children.Add(this);
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);
            var brushSwitcher = new WpfBrushSwitcher();

            foreach (var cluster in this.dataSource.CurrentValue.Clusters)
            {
                this.DrawClusterPoints(cluster, brushSwitcher.GetNext(), drawingContext);
                this.DrawCenter(cluster, drawingContext);
            }
        }

        private void DrawCenter(Cluster cluster, DrawingContext drawingContext)
        {
            drawingContext.DrawEllipse(Brushes.Blue, null, new System.Windows.Point(cluster.Center.X, cluster.Center.Y), centerSize, centerSize);
        }

        private void DrawClusterPoints(Cluster cluster, Brush brush, DrawingContext drawingContext)
        {
            foreach (var point in cluster.Points)
            {
                drawingContext.DrawRectangle(brush, null, new Rect(point.X, point.Y, clusterPointSize, clusterPointSize));
            }
        }

        public void Dispose()
        {
            this.dataSource.NewDataAvailable -= new NewDataHandler<ClusterCollection>(dataSource_NewDataAvailable);
            if (this.canvas != null)
            {
                this.canvas.Children.Remove(this);
            }
        }

        private void dataSource_NewDataAvailable(ClusterCollection clusters)
        {
            this.Dispatcher.Invoke(new Action(() => InvalidateVisual()));
        }
    }
}
