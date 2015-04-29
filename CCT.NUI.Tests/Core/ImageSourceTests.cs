using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using CCT.NUI.Core;
using CCT.NUI.Core.Video;

namespace CCT.NUI.Tests.Core
{
    [TestClass]
    public class ImageSourceTests : TestBase
    {
        private Mock<IBitmapFactory> imageFactoryMock;
        private Mock<IImagePointerDataSource> pointerDataSourceMock;

        private TestableImageSource imageSource;

        [TestInitialize]
        public override void Setup()
        {
            base.Setup();
            this.imageFactoryMock = CreateMock<IBitmapFactory>();
            this.pointerDataSourceMock = CreateMock<IImagePointerDataSource>();
            this.pointerDataSourceMock.Setup(m => m.Width).Returns(20);
            this.pointerDataSourceMock.Setup(m => m.Height).Returns(10);

            this.imageSource = new TestableImageSource(pointerDataSourceMock.Object, imageFactoryMock.Object);
        }

        [TestMethod]
        public void Listens_To_Event_And_Uses_Factory_To_Create_Image()
        {
            var pointer = IntPtr.Zero;

            imageFactoryMock.Setup(m => m.CreateImage(imageSource.CurrentValue, pointer));
            this.pointerDataSourceMock.Setup(m => m.Start());

            this.imageSource.Start();
            pointerDataSourceMock.Raise((s) => s.NewDataAvailable += null, pointer);
        }

        [TestMethod]
        public void Test_Size()
        {
            Assert.AreEqual(20, imageSource.Width);
            Assert.AreEqual(10, imageSource.Height);

            Assert.AreEqual(20, imageSource.Size.Width);
            Assert.AreEqual(10, imageSource.Size.Height);
        }
    }

    class TestableImageSource : BitmapDataSource
    {
        public TestableImageSource(IImagePointerDataSource dataSource, IBitmapFactory imageFactory)
            : base(dataSource, imageFactory)
        { }
    }
}
