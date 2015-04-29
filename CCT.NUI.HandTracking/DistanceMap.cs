using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CCT.NUI.Core;

namespace CCT.NUI.HandTracking
{
    internal class DistanceMap<TLeft, TRight>
        where TLeft : ILocatable
        where TRight : ILocatable
    {
        private IList<TLeft> originalItems;
        private double maxMoveDistance = 100; //TODO: Configure

        internal DistanceMap(IEnumerable<TLeft> originalItems)
        {
            this.originalItems = originalItems.ToList();
        }

        internal DistanceMap(IEnumerable<TLeft> originalItems, double maxMoveDistance)
        {
            this.originalItems = originalItems.ToList();
            this.maxMoveDistance = maxMoveDistance;
        }

        internal void Map(IEnumerable<TRight> newItems)
        {
            this.MappedItems = new List<Tuple<TLeft, TRight>>();
            this.UnmappedItems = new List<TRight>();
            foreach (var newItem in newItems)
            {
                var minItem = default(TLeft);
                var minDistance = double.MaxValue;
                foreach (var oldItem in this.originalItems)
                {
                    var distance = Point.Distance(oldItem.Location, newItem.Location);
                    if (distance < minDistance)
                    {
                        minItem = oldItem;
                        minDistance = distance;
                    }
                }
                if (minDistance <= maxMoveDistance)
                {
                    this.originalItems.Remove(minItem);
                    this.MappedItems.Add(new Tuple<TLeft, TRight>(minItem, newItem));
                }
                else
                {
                    this.UnmappedItems.Add(newItem);
                }
            }
        }

        public IList<Tuple<TLeft, TRight>> MappedItems
        {
            get;
            protected set;
        }

        public IList<TRight> UnmappedItems
        {
            get;
            protected set;
        }

        public IList<TLeft> DiscontinuedItems
        {
            get { return this.originalItems; }
        }
    }
}
