using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace CCT.NUI.Visual
{
    public interface ILayer : IDisposable
    {
        void Paint(Graphics g);

        event EventHandler RequestRefresh;
    }
}
