using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace CCT.NUI.Core
{
    public class ActionRunner
    {
        private Action action;
        private Action afterStopAction;

        private Thread thread;
        private bool run;

        public ActionRunner(Action action)
        {
            this.action = action;
        }

        public ActionRunner(Action action, Action afterStopAction)
            : this(action)
        {
            this.afterStopAction = afterStopAction;
        }

        public bool IsRunning
        {
            get { return this.thread != null; }
        }

        public void Start()
        {
            this.thread = new Thread(new ThreadStart(Run));
            this.run = true;
            this.thread.Start();
        }

        public void Stop()
        {
            if (this.thread != null)
            {
                this.run = false;
                this.thread.Join();
                this.thread = null;
            }
        }

        private void Run()
        {
            while (this.run)
            {
                this.action();
            }
            if (this.afterStopAction != null)
            {
                this.afterStopAction();
            }
        }
    }
}
