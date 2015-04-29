using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenNI;

namespace CCT.NUI.Core.OpenNI
{
    public class DepthGeneratorAdapter : GeneratorAdapterBase<DepthGenerator>, IDepthGenerator
    {
        public DepthGeneratorAdapter(DepthGenerator generator)
            : base(generator)
        { }

        public IntPtr ImagePointer
        {
            get { return this.Generator.DepthMapPtr; }
        }

        public int Width
        {
            get { return this.Generator.GetMetaData().XRes; }
        }

        public int Height
        {
            get { return this.Generator.GetMetaData().YRes; }
        }

        public int DeviceMaxDepth
        {
            get { return this.Generator.DeviceMaxDepth; }
        }
    }
}
