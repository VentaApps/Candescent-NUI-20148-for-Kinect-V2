using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CCT.NUI.Core;

namespace CCT.NUI.Tests.Core
{
    [TestClass]
    public class ArrayToPointerFactoryTests
    {
        [TestMethod]
        public unsafe void Does_Create_Correct_Pointer()
        {
            var factory = new ArrayToPointerFactory();

            var data = new ushort[] { 0, 0, 700, 700, 0, 0 };
            var pointer = factory.CreatePointer(data);

            ushort* pDepth = (ushort*)pointer;
            for (int index = 0; index < data.Length; index++)
            {
                Assert.AreEqual(data[index], *pDepth);
                pDepth++;
            }
            factory.Destroy(pointer);
        }
    }
}
