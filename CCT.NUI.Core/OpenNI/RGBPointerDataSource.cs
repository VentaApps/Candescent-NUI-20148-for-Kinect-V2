using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace CCT.NUI.Core.OpenNI
{
    public class RgbPointerDataSource : OpenNIDataSourceBase<IntPtr, IImageGenerator>, IRgbPointerDataSource 
    {
        public RgbPointerDataSource(IImageGenerator generator)
            : base(generator)
        { }

        protected override unsafe void Run()
        {
            this.CurrentValue = this.Generator.ImagePointer;
            this.OnNewDataAvailable(this.CurrentValue);
        }
    }
}
