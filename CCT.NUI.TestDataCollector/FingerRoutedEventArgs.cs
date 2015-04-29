using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace CCT.NUI.TestDataCollector
{
    public class FingerRoutedEventArgs : RoutedEventArgs
    {
        public FingerRoutedEventArgs(RoutedEvent RemoveFingerEvent, HandSettingsControl handControl, HandDataViewModel handData, FingerPointViewModel point)
            : base(RemoveFingerEvent, handControl)
        {
            this.HandData = handData;
            this.Point = point;
        }

        public HandDataViewModel HandData { get; private set; }
        public FingerPointViewModel Point { get; private set; }
    }
}
