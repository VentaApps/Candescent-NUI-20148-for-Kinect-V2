using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CCT.NUI.Core;
using CCT.NUI.Core.OpenNI;

namespace CCT.NUI.HandTracking.Mouse
{
    public enum ClickMode { TwoFinger = 0, SecondHand = 1, Hand = 2 }
    public enum CursorMode { Finger = 0, CenterOfHand = 1, CenterOfCluster = 2, HandTracking = 3}

    public class MouseController : IDisposable
    {
        private IHandDataSource handSource;
        private Point? lastPointOnScreen;
        private IClickMode clickMode = new TwoFingerClickMode();
        private ICursorMode cursorMode = new FingerCursorMode();
        private TrackingClusterDataSource trackingClusterDataSource;

        public MouseController(IHandDataSource handSource)
        {
            this.handSource = handSource;
            this.handSource.NewDataAvailable += new NewDataHandler<HandCollection>(handSource_NewDataAvailable);
        }

        public MouseController(IHandDataSource handSource, bool enabled)
            : this(handSource)
        {
            this.Enabled = enabled;
        }

        public MouseController(IHandDataSource handSource, TrackingClusterDataSource trackingClusterDataSource)
            : this(handSource)
        {
            this.trackingClusterDataSource = trackingClusterDataSource;
        }

        public bool Enabled { get; set; }

        public void Dispose()
        {
            this.Enabled = false;
            this.handSource.NewDataAvailable -= new NewDataHandler<HandCollection>(handSource_NewDataAvailable);
        }

        public CursorMode CursorMode
        {
            get { return this.cursorMode.EnumValue; }
        }

        public ClickMode ClickMode
        {
            get { return this.clickMode.EnumValue; }
        }

        public void SetCursorMode(CursorMode mode)
        {
            switch (mode)
            {
                case CursorMode.Finger:
                    this.cursorMode = new FingerCursorMode();
                    break;
                case CursorMode.CenterOfHand:
                    this.cursorMode = new CenterOfHandCursorMode();
                    break;
                case CursorMode.CenterOfCluster:
                    this.cursorMode = new CenterOfClusterCursorMode();
                    break;
                case CursorMode.HandTracking:
                    this.cursorMode = new HandTrackingCursorMode(this.trackingClusterDataSource);
                    break;
            }
        }

        public void SetClickMode(ClickMode mode)
        {
            switch (mode)
            {
                case ClickMode.TwoFinger:
                    this.clickMode = new TwoFingerClickMode();
                    break;
                case ClickMode.SecondHand:
                    this.clickMode = new SecondHandClickMode();
                    break;
                case ClickMode.Hand:
                    this.clickMode = new HandClickMode();
                    break;
            }
        }

        void handSource_NewDataAvailable(HandCollection handData)
        {
            if (!this.Enabled || handData.Count == 0)
            {
                return;
            }

            if(this.cursorMode.HasPoint(handData))
            {
                var pointOnScreen = this.MapToScreen(this.cursorMode.GetPoint(handData));

                double newX = pointOnScreen.X;
                double newY = pointOnScreen.Y;

                if (lastPointOnScreen.HasValue)
                {
                    var distance = Point.Distance2D(pointOnScreen, lastPointOnScreen.Value);
                    if (distance < 100)
                    {
                        newX = lastPointOnScreen.Value.X + (newX - lastPointOnScreen.Value.X) * (distance / 100);
                        newY = lastPointOnScreen.Value.Y + (newY - lastPointOnScreen.Value.Y) * (distance / 100);
                    }
                    if (distance < 10)
                    {
                        newX = lastPointOnScreen.Value.X;
                        newY = lastPointOnScreen.Value.Y; 
                    }
                }

                UserInput.SetCursorPositionAbsolute((int)newX, (int)newY);
                lastPointOnScreen = new Point((float)newX, (float)newY, 0);

                this.clickMode.Process(handData);
            }
        }

        private Point MapToScreen(Point point)
        {
            var originalSize = new Size(this.handSource.Width, this.handSource.Height);
            return new Point(-50 + (float)(point.X / originalSize.Width * (System.Windows.SystemParameters.PrimaryScreenWidth + 100)), -50 + (float)(point.Y / originalSize.Height * (System.Windows.SystemParameters.PrimaryScreenHeight + 100)), point.Z);
        }
    }
}
