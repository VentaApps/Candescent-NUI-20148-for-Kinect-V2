using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenNI;
using CCT.NUI.Core.OpenNI;

namespace CCT.NUI.Samples
{
    public class MultiGeneratorDataSource : NIContextDataSourceBase<ImageData>
    {
        private ImageGenerator imageGenerator;
        private DepthGenerator depthGenerator;

        public MultiGeneratorDataSource(Context context, ImageGenerator imageGenerator, DepthGenerator depthGenerator)
            : base(context)
        {
            this.imageGenerator = imageGenerator;
            this.depthGenerator = depthGenerator;
        }

        protected override unsafe void InternalRun()
        {
            this.CurrentValue = new ImageData(this.depthGenerator.DepthMapPtr, this.imageGenerator.ImageMapPtr);
            this.OnNewDataAvailable(this.CurrentValue);
        }
    }

    public class ImageData
    {
        public ImageData(IntPtr depthPointer, IntPtr rgbPointer)
        {
            this.DepthPointer = depthPointer;
            this.RgbPointer = rgbPointer;
        }

        public IntPtr DepthPointer
        {
            get;
            private set;
        }

        public IntPtr RgbPointer
        {
            get;
            private set;
        }
    }
}
