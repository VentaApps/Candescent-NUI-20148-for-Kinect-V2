using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CCT.NUI.Core.Clustering;
using CCT.NUI.Core.OpenNI;
using CCT.NUI.Tests.Core.OpenNI;
using CCT.NUI.Core;

namespace CCT.NUI.Tests.Core.OpenNI
{
    [TestClass]
    public class ClusterDataSourceTests
    {
        private OpenNIClusterDataSource clusterDataSource;
        private DepthPointerDataSourceStub depthPointerSourceStub;
        private ClusterDataSourceSettings settings;

        [TestInitialize]
        public void Setup()
        {
            this.settings = new ClusterDataSourceSettings();
            this.depthPointerSourceStub = new DepthPointerDataSourceStub { Width = 20, Height = 20 };
            this.clusterDataSource = new OpenNIClusterDataSource(this.depthPointerSourceStub, this.settings);
        }

        [TestMethod]
        public void Test_Size_Properties()
        {
            Assert.AreEqual(this.depthPointerSourceStub.Width, this.clusterDataSource.Width);
            Assert.AreEqual(this.depthPointerSourceStub.Height, this.clusterDataSource.Height);

            Assert.AreEqual(this.depthPointerSourceStub.Width, this.clusterDataSource.Size.Width);
            Assert.AreEqual(this.depthPointerSourceStub.Height, this.clusterDataSource.Size.Height);
        }
    }
}
