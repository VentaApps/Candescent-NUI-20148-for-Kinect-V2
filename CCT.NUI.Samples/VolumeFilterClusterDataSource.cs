using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CCT.NUI.Core.Clustering;
using CCT.NUI.Core;
using CCT.NUI.Core.OpenNI;

namespace CCT.NUI.Samples
{
    public class VolumeFilterClusterDataSource : OpenNIClusterDataSource
    {
        private IList<Volume> filterVolumes;
        private ClusterDataSourceSettings settings;

        public VolumeFilterClusterDataSource(IDepthPointerDataSource dataSource)
            : this(dataSource, new ClusterDataSourceSettings())
        { }

        public VolumeFilterClusterDataSource(IDepthPointerDataSource dataSource, ClusterDataSourceSettings settings)
            : base(dataSource, settings)
        {
            this.filterVolumes = new List<Volume>();
            this.settings = settings;
        }

        public void AddFilterVolume(Volume volume)
        {
            this.filterVolumes.Add(volume);
        }

        public void ClearFilterVolumes()
        {
            this.filterVolumes.Clear();
        }

        protected override unsafe IList<Point> FindPointsWithinDepthRange(IntPtr dataPointer)
        {
            var result = new List<Point>();
            ushort* pDepth = (ushort*)dataPointer.ToPointer();

            int localHeight = this.Size.Height; //5ms faster when it's a local variable
            int localWidth = this.Size.Width;
            int maxY = localHeight - this.settings.LowerBorder;
            int minDepth = this.settings.MinimumDepthThreshold;
            int maxDepth = this.settings.MaximumDepthThreshold;

            for (int y = 0; y < localHeight; y++)
            {
                for (int x = 0; x < localWidth; x++)
                {
                    ushort depthValue = *pDepth;
                    if (depthValue > 0 && y < maxY && depthValue <= maxDepth && depthValue >= minDepth) //Should not be put in a seperate method for performance reasons
                    {
                        if (this.filterVolumes.Count == 0 || this.filterVolumes.Any(f => f.Contains(x, y, depthValue)))
                        {
                            result.Add(new Point(x, y, depthValue));
                        }
                    }
                    pDepth++;
                }
            }
            return result;
        }
    }
}
