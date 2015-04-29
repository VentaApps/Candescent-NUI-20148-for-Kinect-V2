using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Controls;

namespace CCT.NUI.Visual
{
    public interface IWpfLayer : IDisposable
    {
        void Activate(Canvas canvas);
    }
}
