using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace CCT.NUI.TestDataCollector
{
    public class HandRoutedEventArgs : RoutedEventArgs
    {
        public HandRoutedEventArgs(RoutedEvent routedEvent, object source, HandDataViewModel handData)
            : base(routedEvent, source)
        {
            this.HandData = handData;
        }

        public HandDataViewModel HandData { get; private set; }
    }
}
