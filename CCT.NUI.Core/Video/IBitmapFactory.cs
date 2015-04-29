using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace CCT.NUI.Core.Video
{
    public interface IBitmapFactory
    {
        unsafe void CreateImage(Bitmap targetImage, IntPtr pointer);
    }
}
