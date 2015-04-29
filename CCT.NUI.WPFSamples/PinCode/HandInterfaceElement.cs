using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Shapes;
using System.Windows.Media;
using System.Windows;
using System.Globalization;
using System.Windows.Media.Animation;
using System.Timers;
using CCT.NUI.HandTracking;

namespace CCT.NUI.WPFSamples.PinCode
{
    public class HandInterfaceElement : UIElement
    {
        private HandCollection currentData = new HandCollection();
        private CountHistory countHistory;
        private static bool stopped = false;
        private int fingerPointCount = 0;
        private InterfacePainter painter;
        private Timer timer;

        public HandInterfaceElement()
        {
            this.Value = 0;
            this.InterfaceOpacity = 0;
            this.painter = new InterfacePainter();
            this.countHistory = new CountHistory { Length = 3 };

            this.timer = new Timer();
            this.timer.Interval = 1000;
            this.timer.Elapsed += new ElapsedEventHandler(timer_Elapsed);
        }

        public byte InterfaceOpacity
        {
            get { return (byte)GetValue(InterfaceOpacityProperty); }
            set { SetValue(InterfaceOpacityProperty, value); }
        }
        public static readonly DependencyProperty InterfaceOpacityProperty = DependencyProperty.Register("InterfaceOpacity", typeof(byte), typeof(HandInterfaceElement), new PropertyMetadata(OnDependencyPropertyChanged));

        public double Value
        {
            get { return (double)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }
        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register("Value", typeof(double), typeof(HandInterfaceElement), new PropertyMetadata(OnDependencyPropertyChanged));

        public int Number
        {
            get { return this.fingerPointCount; }
        }

        public event ElapsedEventHandler CharEnter;

        public event EventHandler Reset;

        public void FadeOut()
        {
            this.Dispatcher.Invoke(new Action(() =>
            {
                this.FadeOut(500);
            }));
        }

        public void Stop()
        {
            stopped = true;
            FadeOut();
        }

        private void FadeIn(int durationInMilliseconds)
        {
            var animation = new ByteAnimation(100, new Duration(TimeSpan.FromMilliseconds(durationInMilliseconds)));
            this.BeginAnimation(HandInterfaceElement.InterfaceOpacityProperty, animation);
        }

        private void FadeOut(int durationInMilliseconds)
        {
            this.timer.Enabled = false;
            var animation = new ByteAnimation(0, new Duration(TimeSpan.FromMilliseconds(durationInMilliseconds)));
            this.BeginAnimation(HandInterfaceElement.InterfaceOpacityProperty, animation);
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);

            if (this.currentData.Count > 0)
            {
                this.painter.Opacity = this.InterfaceOpacity;
                this.painter.Value = this.Value;
                this.painter.DrawHand(this.currentData.Hands.First(), drawingContext);
            }
        }

        private static void OnDependencyPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (!stopped)
            {
                var brush = new SolidColorBrush(Color.FromArgb((d as HandInterfaceElement).InterfaceOpacity, 255, 255, 255));
                (d as HandInterfaceElement).painter.UpdateProgress(brush);
            }
            (d as HandInterfaceElement).InvalidateVisual();
        }

        private void StartAnimation(int durationInMilliseconds)
        {
            if (this.Reset != null)
            {
                this.Reset(this, EventArgs.Empty);
            }
            this.painter.AnimationInProgress = true;

            this.FadeIn(500);
            this.timer.Enabled = true;

            var animation = new DoubleAnimation(0, PathFactory.FULL_ARC - 0.1, new Duration(TimeSpan.FromMilliseconds(durationInMilliseconds)));
            animation.RepeatBehavior = RepeatBehavior.Forever;
            this.BeginAnimation(HandInterfaceElement.ValueProperty, animation);
        }

        void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (this.CharEnter != null)
            {
                this.CharEnter(this, e);
            }
        }

        private void UpdateFingerCount(HandData hand)
        {
            this.countHistory.Add(hand.FingerCount);
            if (this.countHistory.AllEqual())
            {
                this.fingerPointCount = hand.FingerCount;
            }
        }

        internal void Update(HandCollection handCollection)
        {
            if (stopped)
            {
                return;
            }
            this.Dispatcher.Invoke(new Action(() =>
                {
                    this.currentData = handCollection;
                    if (!handCollection.IsEmpty)
                    {
                        var hand = handCollection.Hands.First();
                        if (hand.HasPalmPoint)
                        {
                            this.UpdateFingerCount(hand);
                            if (this.fingerPointCount >= 1)
                            {
                                UpdateHandValues(hand);
                            }
                        }
                    }
                    if ((handCollection.IsEmpty || this.fingerPointCount == 0) && this.InterfaceOpacity == 100)
                    {
                        this.FadeOut(500);
                    }
                    InvalidateVisual();
                }));
        }

        private void UpdateHandValues(HandData hand)
        {
            var distance = CalculateFingerDistance(hand);
            this.painter.UpdateCenter(hand, distance);
            this.painter.UpdateRadius(distance);

            if (this.InterfaceOpacity <= 1 && fingerPointCount == 5)
            {
                this.StartAnimation(1000);
            }
        }

        private static double CalculateFingerDistance(HandData hand)
        {
            double distance = 0;
            foreach (var finger in hand.FingerPoints)
            {
                distance = Math.Max(distance, CCT.NUI.Core.Point.Distance2D(finger.Location, hand.PalmPoint.Value));
            }
            return distance;
        }
    }
}
