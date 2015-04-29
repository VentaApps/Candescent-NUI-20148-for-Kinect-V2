using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Kinect;

namespace CCT.NUI.KinectSDK
{
    public interface IKinectSensor
    {
        int DepthStreamWidth { get; }

        int DepthStreamHeight { get; }

        int ColorStreamWidth { get; }

        int ColorStreamHeight { get; }

        ColorFrameReader ColorReader { get; }             // added: Colored Frame Readder for Kinect v2

        DepthFrameReader DepthReader { get; }             // added: Depth Frame Readder for Kinect v2

        //event EventHandler<ColorImageFrameReadyEventArgs> ColorFrameReady;       // older version

        //event EventHandler<DepthImageFrameReadyEventArgs> DepthFrameReady;       // older version

        event EventHandler<ColorFrameArrivedEventArgs> ColorFrameReady;            // update: Event handler to be fired when a colored image frame arrives from kinect

        event EventHandler<DepthFrameArrivedEventArgs> DepthFrameReady;            // update: Event handler to be fired when a depth image frame arrives from kinect
    }
}
