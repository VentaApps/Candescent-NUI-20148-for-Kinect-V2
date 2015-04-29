using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using System.Windows;
using System.Windows.Media;
using CCT.NUI.HandTracking;

namespace CCT.NUI.Touch
{
    public class KinectMultiTouchDevice : IDisposable
    {
        private IHandDataSource handDataSource;
        private PresentationSource presentationSource;
        private IDictionary<int, KinectTouchDevice> touchDevices;

        private const int handMultiplier = 10;

        public KinectMultiTouchDevice(IHandDataSource handDataSource, FrameworkElement area)
        {
            this.touchDevices = new Dictionary<int, KinectTouchDevice>();
            this.TargetSize = new Size(area.ActualWidth, area.ActualHeight);
            this.presentationSource = PresentationSource.FromVisual(area);
            this.handDataSource = handDataSource;
            handDataSource.NewDataAvailable += new Core.NewDataHandler<HandCollection>(handDataSource_NewDataAvailable);
            area.SizeChanged += new SizeChangedEventHandler(area_SizeChanged);
        }

        public KinectMultiTouchDevice(IHandDataSource handDataSource, PresentationSource presentationSource, Size targetSize)
        {
            this.presentationSource = presentationSource;
            this.TargetSize = targetSize;
        }

        public Size TargetSize { get; set; }

        public void Dispose()
        {
            this.handDataSource.NewDataAvailable -= new Core.NewDataHandler<HandCollection>(handDataSource_NewDataAvailable);
            foreach (var device in this.touchDevices.Values)
            {
                device.Dispose();
            }
        }

        private void area_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            this.TargetSize = e.NewSize;
        }

        private void handDataSource_NewDataAvailable(HandCollection data)
        {
            if (data.IsEmpty) 
            {
                ReportNoTouch(this.touchDevices.Values);
                return;
            }

            var touchedDevices = this.ReportTouches(data);
            this.ReportNoTouch(this.touchDevices.Values.Except(touchedDevices));
        }

        private IList<KinectTouchDevice> ReportTouches(HandCollection data)
        {
            var touchedDevices = new List<KinectTouchDevice>();
            foreach (var hand in data.Hands)
            {
                foreach (var fingerPoint in hand.FingerPoints)
                {
                    var device = this.GetDevice(hand.Id * handMultiplier + fingerPoint.Id);
                    var pointOnPresentationArea = this.MapToPresentationArea(fingerPoint, new Size(this.handDataSource.Size.Width, this.handDataSource.Height));
                    device.Touch(pointOnPresentationArea);
                    touchedDevices.Add(device);
                }
            }
            return touchedDevices;
        }

        private void ReportNoTouch(IEnumerable<KinectTouchDevice> devices)
        {
            foreach (var device in devices)
            {
                device.NoTouch();
            }
        }

        private KinectTouchDevice GetDevice(int index)
        {
            if (!this.touchDevices.ContainsKey(index))
            {
                this.presentationSource.Dispatcher.Invoke(new Action(() =>
                {
                    this.touchDevices.Add(index, new KinectTouchDevice(index, this.presentationSource));
                }));
            }
            return this.touchDevices[index];
        }

        private Point MapToPresentationArea(FingerPoint fingerPoint, Size originalSize)
        {
            var point = new Point(fingerPoint.X, fingerPoint.Y);
            return new Point(point.X / originalSize.Width * this.TargetSize.Width, point.Y / originalSize.Height * this.TargetSize.Height);
        }
    }    
}
