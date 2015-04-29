using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CCT.NUI.HandTracking
{
    internal class IdGenerator
    {
        private IList<int> usedIds = new List<int>();

        public int GetNextId()
        {
            var nextId = 1;
            while (this.usedIds.Contains(nextId))
            {
                nextId++;
            }
            this.usedIds.Add(nextId);
            return nextId;
        }

        public void Return(int id)
        {
            this.usedIds.Remove(id);
        }

        public void Clear()
        {
            this.usedIds.Clear();
        }

        public void SetUsed(int id)
        {
            this.usedIds.Add(id);
        }
    }
}
