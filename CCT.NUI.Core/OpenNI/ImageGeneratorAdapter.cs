using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenNI;

namespace CCT.NUI.Core.OpenNI
{
    public class ImageGeneratorAdapter : GeneratorAdapterBase<ImageGenerator>, IImageGenerator
    {
        public ImageGeneratorAdapter(ImageGenerator generator)
            : base(generator)
        { }

        public IntPtr ImagePointer
        {
            get { return this.Generator.ImageMapPtr; }
        }

        public int Width
        {
            get { return this.Generator.GetMetaData().XRes; }
        }

        public int Height
        {
            get { return this.Generator.GetMetaData().YRes; }
        }
    }
}
