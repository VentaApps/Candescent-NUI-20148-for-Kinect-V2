using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Reflection;

namespace CCT.NUI.StartMenu.Model
{
    public class ExitMenuItem : IMenuItem
    {
        public string Label
        {
            get { return "Exit"; }
        }

        public Icon LoadIcon()
        {
            return Icon.FromHandle(Properties.Resources.endturn_32.GetHicon());
        }

        public void Start()
        {
            this.RequestExit(this, EventArgs.Empty);
        }

        public IMenuItem Clone()
        {
            return new ExitMenuItem();
        }

        public bool Exists
        {
            get { return true; }
        }

        public event EventHandler RequestExit;
    }
}
