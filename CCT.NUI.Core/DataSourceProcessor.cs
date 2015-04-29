using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CCT.NUI.Core
{
    public abstract class DataSourceProcessor<TValue, TSourceData> : IDataSource<TValue>
    {
        private IDataSource<TSourceData> dataSource;
        private TValue data;

        public DataSourceProcessor(IDataSource<TSourceData> dataSource)
        {
            this.dataSource = dataSource;
        }

        public void Start()
        {
            this.dataSource.NewDataAvailable += new NewDataHandler<TSourceData>(dataSource_NewDataAvailable);
            this.dataSource.Start();
        }

        public void Stop()
        {
            this.dataSource.NewDataAvailable -= new NewDataHandler<TSourceData>(dataSource_NewDataAvailable);
            this.dataSource.Stop();
        }

        public virtual void Dispose()
        {
            this.dataSource.Dispose();
            if (this.CurrentValue is IDisposable)
            {
                (this.CurrentValue as IDisposable).Dispose();
            }
        }

        public bool IsRunning
        {
            get { return this.dataSource.IsRunning; }
        }

        public TValue CurrentValue
        {
            get { return this.data; }
            protected set { this.data = value; }
        }

        void dataSource_NewDataAvailable(TSourceData sourceData)
        {
            this.CurrentValue = Process(sourceData);
            if (this.CurrentValue != null && this.NewDataAvailable != null)
            {
                this.NewDataAvailable(this.data);
            }
        }

        protected abstract TValue Process(TSourceData sourceData);

        public event NewDataHandler<TValue> NewDataAvailable;
    }
}
