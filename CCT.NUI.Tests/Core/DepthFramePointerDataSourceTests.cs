using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CCT.NUI.Core;

namespace CCT.NUI.Tests.Core
{
    [TestClass]
    public class DepthFramePointerDataSourceTests
    {
        private DepthDataFrame depthDataFrame;
        private DepthFramePointerDataSource dataSource;

        [TestInitialize]
        public void Setup()
        {
            this.depthDataFrame = new DepthDataFrame(20, 10);
            this.depthDataFrame.Data[1] = 1;
            this.dataSource = new DepthFramePointerDataSource(this.depthDataFrame);
        }

        [TestMethod]
        public void Test_Size()
        {
            Assert.AreEqual(depthDataFrame.Width, dataSource.Width);
            Assert.AreEqual(depthDataFrame.Height, dataSource.Height);
            Assert.AreEqual(depthDataFrame.Width, dataSource.Size.Width);
            Assert.AreEqual(depthDataFrame.Height, dataSource.Size.Height);

            Assert.AreEqual(depthDataFrame.MaxDepth, dataSource.MaxDepth);
        }

        [TestMethod]
        public void Is_Always_Running()
        {
            this.dataSource.Start();
            this.dataSource.Stop();
            Assert.IsTrue(this.dataSource.IsRunning);
        }
    }
}
