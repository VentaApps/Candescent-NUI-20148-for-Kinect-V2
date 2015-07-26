![Logo](https://raw.githubusercontent.com/VentaApps/Candescent-NUI-20148-for-Kinect-V2/master/ProjectLogo.png)

By [Venta Apps](http://ventaapps.com/)

Candescent NUI is a great library for detection and tracking hands and fingers using OPENNI and Kinect for Windows V1 SDK. Now Kinect V2 SDK has been released and it’s time to use this great library with the better sensors of Kinect V2.

Currently it works on Kinect V2, all you need is:
```
•	Kinect V2 Sensor
•	Kinect for Windows V2 SDK
•	USB 3.0
```

Using Candescent NUI with OPENNI is out of the scope of this documentation but I think there wouldn’t be any problems as the changed code was only in the CCT.NUI.KinectSDK project and no naming conventions were changed.

Changes:
---------
	Mainly the changes weren’t in the logic level, I changed the way Candescent NUI
	connects to and reads frames from the Kinect sensor. In Kinect V1 there were
	multiple Streams that provide Frames taken by the sensor, but in Kinect V2
	instead of the Stream Layer there is a Source layer that provides multiple
	Readers that provide frames taken by the sensor. Therefore reading data and
	setting event handlers were the major changes done in the porting.


Dependency and Installation Issues
-------------------------------------
In order to run the project successfully the following dependencies are needed
```
OpenNI.net.dll
OpenNI64.dll
XnVNITE.net.dll
Microsoft.Kinect.dll (Kinect SDK V2 dll)
```

About
-----

Candescent NUI [20148] port for Kinect V2 is maintained by Venta Apps, LLC.

![Venta Apps](https://raw.githubusercontent.com/VentaApps/Candescent-NUI-20148-for-Kinect-V2/master/VentaappsLogo.png)

The names and logos for Venta Apps are trademarks of Venta Apps, LLc.

We love open source software!
[hire us][hire] to help build your product.

[hire]: http://ventaapps.com/quote.html
