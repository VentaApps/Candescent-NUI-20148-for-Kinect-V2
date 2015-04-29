using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace CCT.NUI.Core.OpenNI
{
    public class DepthDataFrameSource : OpenNIDataSourceBase<DepthDataFrame, IDepthGenerator>
    {
        private DepthDataFrameFactory factory;

        public DepthDataFrameSource(IDepthGenerator generator)
            : base(generator)
        {
            this.factory = new DepthDataFrameFactory(this.Size);
            this.CurrentValue = new DepthDataFrame(this.Width, this.Height);
        }

        protected override unsafe void Run()
        {
            this.factory.Create(this.CurrentValue, this.Generator.ImagePointer);
        }

        public void ForceRun()
        {
            this.Run();
        }
    }
}
