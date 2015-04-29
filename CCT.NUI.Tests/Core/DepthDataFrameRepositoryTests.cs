using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CCT.NUI.Core;
using System.IO;

namespace CCT.NUI.Tests.Core
{
    [TestClass]
    public class DepthDataFrameRepositoryTests
    {
        [TestMethod]
        public void Saved_Frame_Can_Be_Restored()
        {
            var repository = new DepthDataFrameRepository(new IntSize(20, 10));
            var frame = new DepthDataFrame(20, 10);
            int anIndex = 1;
            ushort aValue = 2;

            frame.Data[anIndex] = aValue;
            var path = Path.GetTempFileName();
            repository.Save(frame, path);
            
            var loadedFrame = repository.Load(path);
            File.Delete(path);

            Assert.AreEqual(aValue, loadedFrame.Data[anIndex]);
        }
    }
}
