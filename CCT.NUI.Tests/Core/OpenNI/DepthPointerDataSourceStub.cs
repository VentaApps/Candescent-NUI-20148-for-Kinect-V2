using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CCT.NUI.Core;

namespace CCT.NUI.Tests.Core.OpenNI
{
    public class DepthPointerDataSourceStub : IDepthPointerDataSource
    {
        public int MaxDepth { get; set; }

        public IntSize Size
        {
            get { return new IntSize(this.Width, this.Height); }
        }

        public int Width { get; set; }

        public int Height { get; set; }

        public IntPtr CurrentValue { get; set; }

        public event NewDataHandler<IntPtr> NewDataAvailable;

        public void InvokeEvent()
        {
            this.NewDataAvailable(this.CurrentValue);
        }

        public void Start()
        { }

        public void Stop()
        { }

        public bool IsRunning
        {
            get; set;
        }

        public void Dispose()
        { }
    }
}
