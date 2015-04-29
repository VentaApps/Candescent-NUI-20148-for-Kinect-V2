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
    public class RGBPointerDataSourceTests
    {
        private RgbPointerDataSource datasource;
        private ImageGeneratorStub generatorStub;
        private bool newDataCalled = false;

        [TestInitialize]
        public void Setup()
        {
            this.generatorStub = new ImageGeneratorStub();
            this.datasource = new RgbPointerDataSource(this.generatorStub);
        }

        [TestCleanup]
        public void Teardown()
        {
            this.datasource.Dispose();
        }

        [TestMethod]
        public void Test_Size_Properties()
        {
            Assert.AreEqual(this.generatorStub.Width, this.datasource.Width);
            Assert.AreEqual(this.generatorStub.Height, this.datasource.Height);

            Assert.AreEqual(this.generatorStub.Width, this.datasource.Size.Width);
            Assert.AreEqual(this.generatorStub.Height, this.datasource.Size.Height);
        }

        [TestMethod]
        public void Test_Running_State()
        {
            var datasource = new RgbPointerDataSource(new ImageGeneratorStub());
            Assert.IsFalse(datasource.IsRunning);
            datasource.Start();
            Assert.IsTrue(datasource.IsRunning);
            datasource.Stop();
            Assert.IsFalse(datasource.IsRunning);
        }

        [TestMethod]
        public void Current_Value_Is_Set()
        {
            this.datasource.Start();
            this.generatorStub.Invoke();
            Assert.IsTrue(generatorStub.ImagePointer == datasource.CurrentValue);
            this.datasource.Stop();
        }

        [TestMethod]
        public void Invokes_NewDataAvailable_Event_When_Running()
        {
            this.datasource.NewDataAvailable += new NUI.Core.NewDataHandler<IntPtr>(datasource_NewDataAvailable);
            this.datasource.Start();
            this.generatorStub.Invoke();
            Assert.IsTrue(this.newDataCalled);
            this.datasource.Stop();
        }

        void datasource_NewDataAvailable(IntPtr data)
        {
            newDataCalled = true;
        }
    }

    class ImageGeneratorStub : IImageGenerator
    {
        public ImageGeneratorStub()
        {
            this.ImagePointer = IntPtr.Add(IntPtr.Zero, 5);
        }

        public IntPtr ImagePointer { get; set; }

        public int Width
        {
            get { return 20; }
        }

        public int Height
        {
            get { return 10; }
        }

        public void Invoke()
        {
            this.NewData(this, EventArgs.Empty);
        }

        public event EventHandler NewData;
    }
}
