using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace CCT.NUI.Core.OpenNI
{
    public abstract class OpenNIDataSourceBase<TValue, TGenerator> : IDataSource<TValue>
        where TGenerator : IImageGenerator
    {
        private TValue data;
        private TGenerator generator;
        private bool isRunning = false;
        private volatile bool isProcessing = false;

        public OpenNIDataSourceBase(TGenerator generator)
        {
            this.generator = generator;
        }

        protected TGenerator Generator
        {
            get { return this.generator; }
        }

        public IntSize Size
        {
            get { return new IntSize(this.Width, this.Height); }
        }

        public int Width
        {
            get { return this.generator.Width; }
        }

        public int Height
        {
            get { return this.generator.Height; }
        }

        public bool IsRunning
        {
            get { return this.isRunning; }
        }

        public virtual TValue CurrentValue
        {
            get { return this.data; }
            protected set { this.data = value; }
        }

        public void Start()
        {
            if (!this.IsRunning)
            {
                this.generator.NewData += new EventHandler(generator_NewData);
                this.isRunning = true;
            }
        }

        public void Stop()
        {
            if (this.IsRunning)
            {
                this.generator.NewData -= new EventHandler(generator_NewData);
                this.isRunning = false;
            }
        }

        public virtual void Dispose()
        {
            this.Stop();
            SpinWait.SpinUntil(() => !isProcessing);
        }

        protected abstract unsafe void Run();

        protected void OnNewDataAvailable(TValue newData)
        {
            if (this.NewDataAvailable != null)
            {
                this.NewDataAvailable(newData);
            }
        }
        public event NewDataHandler<TValue> NewDataAvailable;

        void generator_NewData(object sender, EventArgs e)
        {
            isProcessing = true;
            this.Run();
            isProcessing = false;
        }
    }
}
