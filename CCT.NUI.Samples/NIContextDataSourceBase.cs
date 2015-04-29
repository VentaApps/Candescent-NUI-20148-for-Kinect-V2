using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using OpenNI;
using CCT.NUI.Core;

namespace CCT.NUI.Samples
{
    public abstract class NIContextDataSourceBase<TValue> : IDataSource<TValue>
    {
        private ActionRunner actionRunner;
        private TValue data;
        private Context context;

        public NIContextDataSourceBase(Context context)
        {
            this.context = context;
            this.actionRunner = new ActionRunner(() => Run(), () => AfterRun());
        }

        public bool IsRunning
        {
            get { return this.actionRunner.IsRunning; }
        }

        public TValue CurrentValue
        {
            get { return this.data; }
            protected set { this.data = value; }
        }

        public void Start()
        {
            if (!this.IsRunning)
            {
                this.context.StartGeneratingAll();
                this.actionRunner.Start();
            }
        }

        public void Stop()
        {
            this.actionRunner.Stop();
        }

        public virtual void Dispose()
        { }

        protected Context Context
        {
            get { return this.context; }
        }

        protected void Run()
        {
            this.context.WaitAndUpdateAll();
            this.InternalRun();
        }

        private void AfterRun() 
        {
            this.context.StopGeneratingAll();
        }

        protected abstract unsafe void InternalRun();

        protected void OnNewDataAvailable(TValue newData)
        {
            if (this.NewDataAvailable != null)
            {
                this.NewDataAvailable(newData);
            }
        }
        public event NewDataHandler<TValue> NewDataAvailable;
    }
}
