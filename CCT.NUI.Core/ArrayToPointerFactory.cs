using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace CCT.NUI.Core
{
    public class ArrayToPointerFactory
    {
        public unsafe IntPtr CreatePointer(ushort[] data)
        {
            var pointerToMemory = Marshal.AllocHGlobal(data.Length * 2);
            var pointerToValue = (ushort*)pointerToMemory;
            for (int index = 0; index < data.Length; index++)
            {
                *pointerToValue = data[index];
                pointerToValue++;
            }
            return pointerToMemory;
        }

        public void Destroy(IntPtr pointer)
        {
            Marshal.FreeHGlobal(pointer);
        }
    }
}
