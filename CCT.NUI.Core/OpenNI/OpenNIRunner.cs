using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Collections.Concurrent;
using System.Runtime.ExceptionServices;
using System.Runtime.InteropServices;
using OpenNI;

namespace CCT.NUI.Core.OpenNI
{
    public class OpenNIRunner 
    {
        private bool existing = false;
        private ConcurrentBag<IGenerator> generators;

        private Context context;
        private Thread thread;
        private bool run;

        public OpenNIRunner(Context context)
        {
            lock (typeof(OpenNIRunner))
            {
                if (existing)
                {
                    throw new NotSupportedException("Only one instance of OpenNIRunner must exist");
                }
                existing = true;
            }
            this.context = context;
            this.generators = new ConcurrentBag<IGenerator>();
        }

        public void Add(IGenerator generator)
        {
            this.generators.Add(generator);
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

        [HandleProcessCorruptedStateExceptions]
        private void Run()
        {
            while (this.run)
            {
                try
                {
                    this.context.WaitAnyUpdateAll();
                    foreach (var generator in this.generators)
                    {
                        generator.Update();
                    }
                }
                catch (AccessViolationException)
                { }
                catch (SEHException)
                { }
            }
        }
    }
}
