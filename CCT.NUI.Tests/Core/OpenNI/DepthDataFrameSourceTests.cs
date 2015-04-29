using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CCT.NUI.Core.OpenNI;
using CCT.NUI.Core;

namespace CCT.NUI.Tests.Core.OpenNI
{
    [TestClass]
    public class DepthDataFrameSourceTests
    {
        [TestMethod]
        public void Can_Create_DepthFrame_From_Pointer()
        {
            var generatorStub = new DepthGeneratorStub();
            var datasource = new DepthDataFrameSource(generatorStub);

            var pointerFactory = new ArrayToPointerFactory();
            var data = new List<ushort>();
            for (int index = 0; index < generatorStub.Width * generatorStub.Height; index++)
            {
                data.Add((ushort) index);
            }
                        
            var pointer = pointerFactory.CreatePointer(data.ToArray());
            generatorStub.ImagePointer = pointer;
            datasource.ForceRun();
            pointerFactory.Destroy(pointer);

            var frame = datasource.CurrentValue;
            Assert.AreEqual(20, frame.Width);
            Assert.AreEqual(10, frame.Height);
            Assert.AreEqual(frame.Data[1], 1);
            Assert.AreEqual(frame.Data[66], 66);
            Assert.AreEqual(frame.Data[199], 199);
        }
    }
}
