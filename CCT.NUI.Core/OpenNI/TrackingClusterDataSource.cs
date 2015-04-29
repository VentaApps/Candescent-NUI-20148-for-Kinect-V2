using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NITE;
using OpenNI;
using CCT.NUI.Core.Clustering;

namespace CCT.NUI.Core.OpenNI
{
    public class TrackingClusterDataSource : DataSourceProcessor<ClusterCollection, IntPtr>, IClusterDataSource
    {
        private IntSize size;
        private Context context;
        private DepthGenerator depthGenerator;
        private SessionManager sessionManager;
        private PointControl pointControl;
        private Point3D? point;
        
        private const int areaSize = 100;
        private const int areaSizeZ = 50;
        private const int modulo = 5;
 
        public TrackingClusterDataSource(Context context, DepthGenerator depthGenerator, IDepthPointerDataSource dataSource)
            : base(dataSource)
        {
            this.context = context;
            this.depthGenerator = depthGenerator;
            this.size = dataSource.Size;
            this.CurrentValue = new ClusterCollection();
            
            this.sessionManager = new SessionManager(context, "Wave", "RaiseHand");
            this.pointControl = new PointControl();
            this.pointControl.PrimaryPointUpdate += new EventHandler<HandEventArgs>(pointControl_PrimaryPointUpdate);
            this.sessionManager.AddListener(this.pointControl);
        }

        public Point? TrackingPoint
        {
            get 
            {
                if (!this.point.HasValue)
                {
                    return null;
                }
                return new Point(this.point.Value.X, point.Value.Y, point.Value.Z);
            }
        }

        public int Width
        {
            get { return this.size.Width; }
        }

        public int Height
        {
            get { return this.size.Height; }
        }

        public IntSize Size { get { return this.size; } }

        public override void Dispose()
        {
            this.pointControl.PrimaryPointUpdate -= new EventHandler<HandEventArgs>(pointControl_PrimaryPointUpdate);
            this.sessionManager.RemoveListener(this.pointControl);
            base.Dispose();
        }

        protected override ClusterCollection Process(IntPtr sourceData)
        {
            this.sessionManager.Update(this.context);
            if (!this.point.HasValue)
            {
                return new ClusterCollection();
            }
            var cluster = CreateCluster(sourceData);
            return new ClusterCollection(new List<Cluster> { cluster });
        }

        private Cluster CreateCluster(IntPtr sourceData)
        {
            var points = this.FindPointsWithinDepthRange(sourceData);
            var filteredPoints = ReducePoints(points);
            var cluster = new Cluster(Point.Center(filteredPoints), filteredPoints, new Volume(points)) { AllPoints = points };
            return cluster;
        }

        protected virtual unsafe IList<Point> FindPointsWithinDepthRange(IntPtr dataPointer)
        {
            var location = this.point.Value;
            var filter = new PointerVolumePointFilter(this.size, (int)location.X - areaSize, this.size.Width - (int)location.X - areaSize, (int)location.Y - areaSize, this.Height - (int)location.Y - areaSize, (int)location.Z - areaSizeZ, (int)location.Z + areaSizeZ);
            return filter.Filter(dataPointer);
        }

        private static List<Point> ReducePoints(IList<Point> points)
        {
            var filteredPoints = new List<Point>();
            for (int index = 0; index < points.Count; index++)
            {
                if (points[index].X % modulo == 0 && points[index].Y % modulo == 0)
                {
                    filteredPoints.Add(points[index]);
                }
            }
            return filteredPoints;
        }

        private void pointControl_PrimaryPointUpdate(object sender, HandEventArgs e)
        {
            this.point = this.depthGenerator.ConvertRealWorldToProjective(e.Hand.Position);
        }
    }
}
