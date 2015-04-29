using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CCT.NUI.TestDataCollector
{
    /// <summary>
    /// Interaction logic for HandSettingsControl.xaml
    /// </summary>
    public partial class HandSettingsControl : UserControl
    {
        public HandSettingsControl()
        {
            InitializeComponent();
        }

        public static readonly RoutedEvent MarkPalmCenterEvent = EventManager.RegisterRoutedEvent("MarkPalmCenter", RoutingStrategy.Bubble, typeof(EventHandler<HandRoutedEventArgs>), typeof(HandSettingsControl));

        public event EventHandler<HandRoutedEventArgs> MarkPalmCenter
        {
            add { AddHandler(MarkPalmCenterEvent, value); }
            remove { RemoveHandler(MarkPalmCenterEvent, value); }
        }

        public static readonly RoutedEvent CloseEvent = EventManager.RegisterRoutedEvent("Close", RoutingStrategy.Bubble, typeof(EventHandler<HandRoutedEventArgs>), typeof(HandSettingsControl));

        public event EventHandler<HandRoutedEventArgs> Close
        {
            add { AddHandler(CloseEvent, value); }
            remove { RemoveHandler(CloseEvent, value); }
        }

        public static readonly RoutedEvent MarkFingersEvent = EventManager.RegisterRoutedEvent("MarkFingers", RoutingStrategy.Bubble, typeof(EventHandler<HandRoutedEventArgs>), typeof(HandSettingsControl));

        public event EventHandler<HandRoutedEventArgs> MarkFingers
        {
            add { AddHandler(MarkFingersEvent, value); }
            remove { RemoveHandler(MarkFingersEvent, value); }
        }

        public static readonly RoutedEvent RemoveFingerEvent = EventManager.RegisterRoutedEvent("RemoveFinger", RoutingStrategy.Bubble, typeof(EventHandler<FingerRoutedEventArgs>), typeof(HandSettingsControl));

        public event EventHandler<FingerRoutedEventArgs> RemoveFinger
        {
            add { AddHandler(RemoveFingerEvent, value); }
            remove { RemoveHandler(RemoveFingerEvent, value); }
        }

        private void buttonMarkPalmCenter_Click(object sender, RoutedEventArgs e)
        {
            RaiseEvent(new HandRoutedEventArgs(MarkPalmCenterEvent, this, this.DataContext as HandDataViewModel));
        }

        private void buttonClose_Click(object sender, RoutedEventArgs e)
        {
            RaiseEvent(new HandRoutedEventArgs(CloseEvent, this, this.DataContext as HandDataViewModel));
        }

        private void buttonMarkFingers_Click(object sender, RoutedEventArgs e)
        {
            RaiseEvent(new HandRoutedEventArgs(MarkFingersEvent, this, this.DataContext as HandDataViewModel));
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            RaiseEvent(new FingerRoutedEventArgs(RemoveFingerEvent, this, this.DataContext as HandDataViewModel, (FingerPointViewModel)(e.Source as Button).DataContext));
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            foreach (var item in e.AddedItems)
            {
                (item as FingerPointViewModel).IsSelected = true;
            }
            foreach (var item in e.RemovedItems)
            {
                (item as FingerPointViewModel).IsSelected = false;
            }
        }

        private void Border_MouseEnter(object sender, MouseEventArgs e)
        {
            (this.DataContext as HandDataViewModel).IsSelected = true;
        }

        private void Border_MouseLeave(object sender, MouseEventArgs e)
        {
            if (this.DataContext is HandDataViewModel)
            {
                (this.DataContext as HandDataViewModel).IsSelected = false;
            }
        }
    }
}
