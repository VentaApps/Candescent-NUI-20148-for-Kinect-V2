using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media.Imaging;

namespace CCT.NUI.Core.Video
{
    public interface IImageFactory
    {
        unsafe void CreateImage(WriteableBitmap target, IntPtr pointer);
    }
}
