using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CCT.NUI.Core;
using CCT.NUI.HandTracking;
using CCT.NUI.Core.Clustering;
using CCT.NUI.Core.OpenNI;
using CCT.NUI.Core.Shape;

namespace CCT.NUI.Tests
{
    [TestClass]
    public class HandDetectionTests
    {
        [TestMethod]
        public void A_Valid_Hand_Is_Recognized()
        {
             RunTest(@"handtracking\valid_hand.frm", (handData) =>
            {
                Assert.AreEqual(1, handData.Count);
                Assert.AreEqual(5, handData.Hands[0].FingerCount);
            });
        }

        [TestMethod]
        public void When_No_Hand_Is_Present_No_Hand_Is_Recognized()
        {
            RunTest(@"handtracking\no_hand.frm", (handData) =>
            {
                Assert.AreEqual(0, handData.Count);
            });
        }

        private void RunTest(string framePath, Action<HandCollection> assertions)
        {
            var frameSize = new IntSize(640, 480);
            var frame = new DepthDataFrameRepository(frameSize).Load(framePath);
            using (var frameDataSource = new DepthFramePointerDataSource(frame))
            {
                var src = new HandDataSource(new ClusterShapeDataSource(new OpenNIClusterDataSource(frameDataSource, new ClusterDataSourceSettings())), new HandDataSourceSettings { FramesForNewFingerPoint = 0 });
                src.Start();
                frameDataSource.Push();
                src.Stop();
                assertions(src.CurrentValue);
            }
        }
    }
}
