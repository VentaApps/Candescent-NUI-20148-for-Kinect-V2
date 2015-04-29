using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CCT.NUI.Core;

namespace CCT.NUI.KinectSDK
{
    public abstract class SensorDataSource<TValue>
    {
        private IKinectSensor sensor;
        private bool isRunning;

        public SensorDataSource(IKinectSensor sensor)
        {
            this.sensor = sensor;
        }

        public TValue CurrentValue { get; protected set; }

        protected IKinectSensor Sensor
        {
            get { return this.sensor; }
        }

        public IntSize Size
        {
            get { return new IntSize(this.Width, this.Height); }
        }

        public abstract int Width
        {
            get;
        }

        public abstract int Height
        {
            get;
        }

        public bool IsRunning
        {
            get { return this.isRunning; }
        }

        public void Start()
        {
            if (!isRunning)
            {
                this.InnerStart();
                this.isRunning = true;
            }
        }

        protected abstract void InnerStart();

        public void Stop()
        {
            if (isRunning)
            {
                this.InnerStop();
                this.isRunning = false;
            }
        }

        protected abstract void InnerStop();

        public virtual void Dispose()
        { }

        protected void OnNewDataAvailable()
        {
            if (this.NewDataAvailable != null)
            {
                this.NewDataAvailable(this.CurrentValue);
            }
        }
        public event NewDataHandler<TValue> NewDataAvailable;
    }
}
