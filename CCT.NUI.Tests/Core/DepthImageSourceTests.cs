using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CCT.NUI.Core;
using CCT.NUI.Core.Video;

namespace CCT.NUI.Tests.Core
{
    [TestClass]
    public class DepthImageSourceTests : TestBase
    {
        [TestMethod]
        public void Can_Create()
        {
            var pointerSourceMock = this.CreateMock<IDepthPointerDataSource>();
            pointerSourceMock.Setup(m => m.MaxDepth).Returns(500);
            pointerSourceMock.Setup(m => m.Width).Returns(20);
            pointerSourceMock.Setup(m => m.Height).Returns(10);

            var source = new DepthImageDataSource(pointerSourceMock.Object);          
        }
    }
}
