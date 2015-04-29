using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CCT.NUI.TestDataCollector
{
    public class SelectFrameEventArgs : EventArgs
    {
        public SelectFrameEventArgs(TestDepthFrame frame)
        {
            this.Frame = frame;
        }

        public TestDepthFrame Frame { get; private set; }
    }
}
