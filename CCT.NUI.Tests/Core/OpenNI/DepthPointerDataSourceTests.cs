using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CCT.NUI.Core.OpenNI;
using OpenNI;

namespace CCT.NUI.Tests.Core.OpenNI
{
    [TestClass]
    public class DepthPointerDataSourceTests
    {
        private DepthPointerDataSource datasource;
        private DepthGeneratorStub generatorStub;

        [TestInitialize]
        public void Setup()
        {
            this.generatorStub = new DepthGeneratorStub();
            this.datasource = new DepthPointerDataSource(this.generatorStub);
        }

        [TestCleanup]
        public void Teardown()
        {
            this.datasource.Dispose();
        }

        [TestMethod]
        public void Test_Max_Depth_Property()
        {
            Assert.AreEqual(this.generatorStub.DeviceMaxDepth, this.datasource.MaxDepth);
        }

        [TestMethod]
        public void Current_Value_Is_Set()
        {
            this.datasource.Start();
            this.generatorStub.Invoke();
            Assert.IsTrue(generatorStub.ImagePointer == datasource.CurrentValue);
            this.datasource.Stop();
        }
    }

    class DepthGeneratorStub : ImageGeneratorStub, IDepthGenerator
    {
        public int DeviceMaxDepth
        {
            get { return 25; }
        }
    }
}
