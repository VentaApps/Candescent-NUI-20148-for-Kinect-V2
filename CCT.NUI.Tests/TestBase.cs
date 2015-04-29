using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Moq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CCT.NUI.Tests
{
    public class TestBase
    {
        private MockRepository repository;

        [TestInitialize]
        public virtual void Setup()
        {
            this.repository = new MockRepository(MockBehavior.Strict);
        }

        [TestCleanup]
        public virtual void Teardown()
        {
            this.repository.VerifyAll();
        }

        protected Mock<T> CreateMock<T>()
            where T : class
        {
            return this.MockRepository.Create<T>();
        }

        protected MockRepository MockRepository
        {
            get { return this.repository; }
        }
    }
}
