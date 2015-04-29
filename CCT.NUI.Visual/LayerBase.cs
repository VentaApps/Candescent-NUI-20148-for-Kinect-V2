using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace CCT.NUI.Visual
{
    public abstract class LayerBase : ILayer
    {
        public abstract void Paint(Graphics g);

        public virtual void Dispose() { }

        protected void OnRequestRefresh()
        {
            if (this.RequestRefresh != null)
            {
                this.RequestRefresh(this, EventArgs.Empty);
            }
        }
        public event EventHandler RequestRefresh;
    }
}
