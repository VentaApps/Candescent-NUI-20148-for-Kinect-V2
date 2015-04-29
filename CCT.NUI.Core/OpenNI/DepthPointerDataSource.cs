using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
namespace CCT.NUI.Core.OpenNI
{
    public class DepthPointerDataSource : OpenNIDataSourceBase<IntPtr, IDepthGenerator>, IDepthPointerDataSource
    {
        public DepthPointerDataSource(IDepthGenerator generator)
            : base(generator)
        { }

        protected override unsafe void Run()
        {
            this.CurrentValue = this.Generator.ImagePointer;
            this.OnNewDataAvailable(this.CurrentValue);
        }

        public int MaxDepth
        {
            get { return this.Generator.DeviceMaxDepth; }
        }
    }
}
