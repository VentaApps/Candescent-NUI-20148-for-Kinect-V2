Candescent Start Menu v1.0

Developed by Stefan Stegmueller
Website: http://www.candescent.ch
Blog:    http://blog.candescent.ch
Project: http://candescentnui.codeplex.com

Prerequisites:
- Windows XP SP2 or newer
- .NET Framework 4 (Download here http://msdn.microsoft.com/en-us/library/5a4x27ek.aspx)
- Avin2 SensorKinect driver (Download here https://github.com/avin2/SensorKinect)

Tutorial:
To show the menu, present your open hand to the kinect at a distance of around 0.5 - 1 meters.
To close the menu again, just close the hand. To start an application, stretch out one finger,
move it over the icon (which gets highlighted), then close the hand quickly, so no finger is
visible anymore.

There is currently only one item in the start menu by default (Windows Explorer). You can
configure programs or files to be shown in the Settings View. To open it, right click the
task bar icon and select "Settings..." or double click the icon.

The configuration is stored in the file "menu_config.csv", you can also edit this file manually.
Please don't change the first line. All other lines have to be in the format <Name>;<Path>.

To stop the program, select the exit menu item, press escape while the start menu is visible or
right click the task bar icon ans select "Close".

