using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CCT.NUI.Core;
using Moq;

namespace CCT.NUI.Tests.Core
{
    [TestClass]
    public class DataSourceProcessorTests : TestBase
    {
        private Mock<IDataSource<int>> dataSourceMock;
        private TestableDataSourceProcessor<string, int> dataSource;

        [TestInitialize]
        public override void Setup()
        {
            base.Setup();
            this.dataSourceMock = CreateMock<IDataSource<int>>();
            this.dataSource = new TestableDataSourceProcessor<string, int>(dataSourceMock.Object, (i) => i.ToString());
        }

        [TestMethod]
        public void DataSourceProcessor_Reacts_To_Event()
        {
            this.dataSourceMock.Setup(m => m.Start());

            var aNumber = 1;
            this.dataSource.Start(); 
            this.dataSourceMock.Raise((s) => s.NewDataAvailable += null, aNumber);
            Assert.AreEqual("1", this.dataSource.CurrentValue);
        }

        [TestMethod]
        public void Start_Stop_Logic()
        {
            this.dataSourceMock.Setup(m => m.Start());
            this.dataSourceMock.Setup(m => m.Stop());

            this.dataSource.Start();
            this.dataSource.Stop();            
        }

        [TestMethod]
        public void Is_Running_Returns_IsRunning()
        {
            this.dataSourceMock.Setup(m => m.IsRunning).Returns(true);
            Assert.AreEqual(true, this.dataSource.IsRunning);
        }

        [TestMethod]
        public void Dispose_Disposes_DataSource()
        {
            this.dataSourceMock.Setup(m => m.Dispose());
            this.dataSource.Dispose();
        }

        [TestMethod]
        public void Dispose_Disposes_CurrentValue()
        {
            var sourceMock = CreateMock<IDataSource<IDisposable>>();
            var disposableMock = CreateMock<IDisposable>();
            var dataSource = new TestableDataSourceProcessor<IDisposable, IDisposable>(sourceMock.Object, (d) => d);
            sourceMock.Setup(m => m.Dispose());
            disposableMock.Setup(m => m.Dispose());

            dataSource.SetValue(disposableMock.Object);

            dataSource.Dispose();
        }
    }

    class TestableDataSourceProcessor<TValue, TSource> : DataSourceProcessor<TValue, TSource>
    {
        private Func<TSource, TValue> func;

        public TestableDataSourceProcessor(IDataSource<TSource> dataSource, Func<TSource, TValue> func)
            : base(dataSource)
        {
            this.func = func;
        }

        protected override TValue Process(TSource sourceData)
        {
            return func(sourceData);
        }

        public void SetValue(TValue value)
        {
            this.CurrentValue = value;
        }
    }
}
