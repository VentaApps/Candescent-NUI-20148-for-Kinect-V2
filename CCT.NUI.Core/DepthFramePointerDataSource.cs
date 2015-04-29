using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Runtime.InteropServices;
using CCT.NUI.Core;

namespace CCT.NUI.Core
{
    public class DepthFramePointerDataSource : IDepthPointerDataSource
    {
        private DepthDataFrame frame;
        private ArrayToPointerFactory pointerFactory;

        public DepthFramePointerDataSource(DepthDataFrame frame)
        {
            this.frame = frame;
            this.pointerFactory = new ArrayToPointerFactory();
            this.CurrentValue = this.GetPointer(frame);
        }

        private unsafe IntPtr GetPointer(DepthDataFrame frame)
        {
            return this.pointerFactory.CreatePointer(frame.Data);
        }

        public IntSize Size { get { return this.frame.Size; } }

        public int Width { get { return this.frame.Width; } }

        public int Height { get { return this.frame.Height; } }

        public int MaxDepth
        {
            get { return this.frame.MaxDepth; }
        }

        public void Start()
        { }

        public void Stop()
        { }

        public bool IsRunning
        {
            get { return true; }
        }

        public IntPtr CurrentValue
        {
            get;
            protected set;
        }

        public void Push()
        {
            if (this.NewDataAvailable != null)
            {
                this.NewDataAvailable(this.CurrentValue);
            }
        }

        public event NewDataHandler<IntPtr> NewDataAvailable;

        public void Dispose()
        {
            this.pointerFactory.Destroy(this.CurrentValue);
        }
    }
}
