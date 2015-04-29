using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenNI;

namespace CCT.NUI.Core.OpenNI
{
    public class GeneratorAdapterBase<TGenerator> : IGenerator
        where TGenerator : Generator
    {
        public GeneratorAdapterBase(TGenerator generator)
        {
            this.Generator = generator;
        }

        public void Update()
        {
            if (NewData != null)
            {
                this.NewData(this, EventArgs.Empty);
            }
        }

        protected TGenerator Generator { get; private set; }

        public event EventHandler NewData;
    }
}
