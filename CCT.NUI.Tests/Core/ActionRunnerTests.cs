using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading;
using CCT.NUI.Core;

namespace CCT.NUI.Tests.Core
{
    [TestClass]
    public class ActionRunnerTests
    {
        private ActionRunner actionRunner;
        private bool actionHasRun = false;
        private bool stopActionHasRun = false;

        [TestInitialize]
        public void Setup()
        {
            this.actionRunner = new ActionRunner(Action, StopAction);
        }

        [TestMethod]
        public void Start_Runs_The_Action_And_StopAction()
        {
            this.actionRunner.Start();
            Thread.Sleep(100);
            this.actionRunner.Stop();
            AssertAsync.IsTrue(() => this.actionHasRun, 200);
            AssertAsync.IsTrue(() => this.stopActionHasRun, 200);
        }

        [TestMethod]
        public void IsRunning_Returns_True_If_Not_Running()
        {
            Assert.IsFalse(this.actionRunner.IsRunning);
        }

        [TestMethod]
        public void IsRunning_Returns_True_If_Running()
        {
            try
            {
                this.actionRunner.Start();
                Assert.IsTrue(this.actionRunner.IsRunning);
            }
            finally
            {
                this.actionRunner.Stop();
            }
        }

        private void Action()
        {
            this.actionHasRun = true;
        }

        private void StopAction()
        {
            this.stopActionHasRun = true;
        }
    }
}
