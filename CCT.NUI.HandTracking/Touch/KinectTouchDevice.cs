using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using System.Windows;
using System.Windows.Media;
using System.Windows.Controls;
using CCT.NUI.HandTracking;
using System.Windows.Interop;

namespace CCT.NUI.Touch
{
    internal enum TouchState { Up, Down }

    internal class KinectTouchDevice : TouchDevice, IDisposable
    {
        private DateTime? firstTouch;

        public KinectTouchDevice(int id, PresentationSource source)
            : base(id)
        {
            this.Position = new Point();
            this.TouchState = TouchState.Up;
            this.SetActiveSource(source);
        }

        public Point Position { get; private set; }

        internal TouchState TouchState { get; private set; }

        public void Touch(Point position)
        {
            if (!this.firstTouch.HasValue)
            {
                this.firstTouch = DateTime.Now;
                return;
            }
            else if (DateTime.Now.Subtract(this.firstTouch.Value).TotalMilliseconds < 100)
            {
                return;
            }
            this.Position = position;
            if (!this.IsActive)
            {
                this.Activate();
            }
            if (this.TouchState != TouchState.Down)
            {
                this.Dispatcher.Invoke(new Func<bool>(this.ReportDown));
                this.TouchState = TouchState.Down;
            }
            else
            {
                this.Dispatcher.Invoke(new Func<bool>(this.ReportMove));
            }
        }

        public void NoTouch()
        {
            this.firstTouch = null;
            if (TouchState == TouchState.Down)
            {
                this.Dispatcher.Invoke(new Func<bool>(this.ReportUp));
            }
            this.TouchState = TouchState.Up;
        }

        public override TouchPointCollection GetIntermediateTouchPoints(IInputElement relativeTo)
        {
            return new TouchPointCollection();
        }

        public override TouchPoint GetTouchPoint(IInputElement relativeTo)
        {
            var point = this.Position;
            if (relativeTo != null)
            {
                point = this.ActiveSource.RootVisual.TransformToDescendant((Visual)relativeTo).Transform(point);
            }
            return new TouchPoint(this, point, new Rect(point, new Size(1, 1)), TouchAction.Move);
        }

        public void Dispose()
        {
            if (this.IsActive)
            {
                this.Deactivate();
            }
        }
    }
}
