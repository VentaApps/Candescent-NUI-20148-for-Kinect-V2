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
using System.Reflection;
using CCT.NUI.HandTracking;
using CCT.NUI.Core;
using CCT.NUI.Touch;

namespace CCT.NUI.WPFSamples
{
    /// <summary>
    /// Interaction logic for TouchWindow.xaml
    /// </summary>
    public partial class TouchWindow : Window
    {        private System.Windows.Point lastPoint = new System.Windows.Point();

        private KinectMultiTouchDevice device;
        private IHandDataSource handDataSource;

        public TouchWindow()
        {
            InitializeComponent();            
        }

        public TouchWindow(IHandDataSource handDataSource)
            : this()
        {
            this.handDataSource = handDataSource;
        }

        protected override void OnTouchDown(TouchEventArgs e)
        {
            base.OnTouchDown(e);
        }

        protected override void OnTouchMove(TouchEventArgs e)
        {
            base.OnTouchMove(e);
            TouchPoint tp = e.GetTouchPoint(this.mainCanvas);
            if (lastPoint == tp.Position)
            {
                return;
            }
            if (lastPoint == new System.Windows.Point())
            {
                lastPoint = tp.Position;
                return;
            }
            Line _line = new Line();
            _line.Stroke = new RadialGradientBrush(Colors.White, Colors.Black);
            _line.X1 = lastPoint.X;
            _line.X2 = tp.Position.X;
            _line.Y1 = lastPoint.Y;
            _line.Y2 = tp.Position.Y;

            _line.StrokeThickness = 2;
            mainCanvas.Children.Add(_line);
            lastPoint = tp.Position;
        }

        protected override void OnTouchUp(TouchEventArgs e)
        {
            base.OnTouchUp(e);
            this.lastPoint = new System.Windows.Point();
        }

        private void handDataSource_NewDataAvailable(HandCollection data)
        {
            if (data.IsEmpty || !data.Hands.First().HasFingers)
            {
                this.Dispatcher.Invoke(new Action(() =>
                {
                    foreach (var line in mainCanvas.Children.OfType<Line>().ToList())
                    {
                        mainCanvas.Children.Remove(line);
                    }
                }));
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            this.device.Dispose();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.device = new KinectMultiTouchDevice(handDataSource, this);
            handDataSource.NewDataAvailable += new Core.NewDataHandler<HandCollection>(handDataSource_NewDataAvailable);
        }
    }
}
