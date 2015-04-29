using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CCT.NUI.HandTracking.Persistence;
using CCT.NUI.Core;

namespace CCT.NUI.Tests.HandTracking.Persistence
{
    [TestClass]
    public class TestFrameRepositoryTests
    {
        private TestFrameRepository repository;

        [TestInitialize]
        public void Setup()
        {
            this.repository = new TestFrameRepository();
        }

        [TestMethod]
        public void Can_Load_Saved_Frame()
        {
            this.repository.Load(@"HandTracking\Persistence\778600c3-2c4e-44dc-99ff-824b757a5879.xfrm");
        }

        [TestMethod]
        public void Can_Save_Frame()
        {
            var testFrame = new TestFrameEntity(new DepthFrameEntity(new IntSize(640, 480), new ushort[] { 1, 2, 3 }));
            this.repository.Save(testFrame, @"HandTracking\Persistence\test.xfrm");
        }
    }
}
