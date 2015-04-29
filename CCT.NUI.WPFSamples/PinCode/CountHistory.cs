using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CCT.NUI.WPFSamples.PinCode
{
    public class CountHistory
    {
        private Queue<int> queue;

        public CountHistory()
        {
            this.queue = new Queue<int>();
        }

        public void Add(int value)
        {
            this.queue.Enqueue(value);
            if (this.queue.Count > this.Length)
            {
                this.queue.Dequeue();
            }
        }

        public int Length { get; set; }

        public bool AllEqual()
        {
            if (this.queue.Count <= 1)
            {
                return true;
            }
            var value = this.queue.First();
            return this.queue.All(v => v == value);            
        }        
    }
}
