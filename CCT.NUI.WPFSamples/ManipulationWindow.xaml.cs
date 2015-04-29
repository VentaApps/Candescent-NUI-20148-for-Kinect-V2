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
    /// Interaction logic for ManipulationWindow.xaml
    /// </summary>
    public partial class ManipulationWindow : Window
    {
        private KinectMultiTouchDevice device;
        private IHandDataSource handDataSource;
        private IDictionary<int, TouchControl> touchPoints;

        public ManipulationWindow(IHandDataSource handDataSource)
        {
            InitializeComponent();
            this.handDataSource = handDataSource;
        }

        protected override void OnTouchMove(TouchEventArgs e)
        {
            base.OnTouchMove(e);
            HandleTouch(e);
        }

        protected override void OnTouchDown(TouchEventArgs e)
        {
            base.OnTouchDown(e);
            HandleTouch(e);
        }

        private void HandleTouch(TouchEventArgs e)
        {
            var visual = GetTouchVisual(e.TouchDevice.Id);
            var point = e.GetTouchPoint(this.fingerCanvas).Position;
            visual.SetValue(Canvas.LeftProperty, point.X);
            visual.SetValue(Canvas.TopProperty, point.Y);
        }

        protected override void OnTouchUp(TouchEventArgs e)
        {
            base.OnTouchUp(e);
            this.fingerCanvas.Children.Remove(this.touchPoints[e.TouchDevice.Id]);
            this.touchPoints.Remove(e.TouchDevice.Id);
        }

        private TouchControl GetTouchVisual(int deviceId)
        {
            if (this.touchPoints.ContainsKey(deviceId))
            {
                return this.touchPoints[deviceId];
            }

            var touchControl = new TouchControl(deviceId);
            this.touchPoints.Add(deviceId, touchControl);
            this.fingerCanvas.Children.Add(touchControl);
            return touchControl;
        }

        protected override void OnManipulationStarting(ManipulationStartingEventArgs e)
        {
            base.OnManipulationStarting(e);
            e.ManipulationContainer = this;
            e.Handled = true;
        }

        protected override void OnManipulationDelta(ManipulationDeltaEventArgs e)
        {
            base.OnManipulationDelta(e);
            var element = e.OriginalSource as UIElement;
            var transformation = this.mainCanvas.RenderTransform as MatrixTransform;
            var matrix = transformation == null ? Matrix.Identity : transformation.Matrix;
            matrix.ScaleAt(e.DeltaManipulation.Scale.X, e.DeltaManipulation.Scale.Y, e.ManipulationOrigin.X, e.ManipulationOrigin.Y);
            matrix.RotateAt(e.DeltaManipulation.Rotation, e.ManipulationOrigin.X, e.ManipulationOrigin.Y);
            matrix.Translate(e.DeltaManipulation.Translation.X, e.DeltaManipulation.Translation.Y);
            this.mainCanvas.RenderTransform = new MatrixTransform(matrix);
        }

        protected override void OnManipulationInertiaStarting(ManipulationInertiaStartingEventArgs e)
        {
            base.OnManipulationInertiaStarting(e);
            e.TranslationBehavior.DesiredDeceleration = 0.001;
            e.RotationBehavior.DesiredDeceleration = 0.01;
            e.ExpansionBehavior.DesiredDeceleration = 0.01;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.device = new KinectMultiTouchDevice(handDataSource, this);
            this.touchPoints = new Dictionary<int, TouchControl>();
            this.LoadImages();
        }

        private void LoadImages()
        {
            double x = 10;
            foreach (var image in new ImageLoader().LoadImages())
            {
                Canvas.SetLeft(image, x);
                Canvas.SetTop(image, 40);
                this.mainCanvas.Children.Add(image);
                x += image.Width;
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            this.device.Dispose();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.mainCanvas.RenderTransform = MatrixTransform.Identity;
        }
    }
}
