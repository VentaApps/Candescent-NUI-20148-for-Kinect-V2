using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace CCT.NUI.StartMenu.Model
{
    public interface IMenuItem
    {
        string Label { get; }

        bool Exists { get; }

        Icon LoadIcon();

        void Start();

        IMenuItem Clone();
    }
}
