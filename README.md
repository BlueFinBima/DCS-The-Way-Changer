# DCS-The-Way-Changer
## About this Project
 This project provides a User interface to change the waypoint data created by the ["DCS The Way" project (BlueFinBima fork)](https://github.com/BlueFinBima/DCS-The-Way/).  "DCS The Way" allows the creation of waypoints on the DCS World F10 map which wen saved to an external file, can then be loaded into this program to be further tweaked.  Once all good, then the changed file can then be loaded back into "DCS The Way" in order to be loaded back into the aircraft.
 
 ## Downloading the Program
 [Only download this program from the Releases section of this project](https://github.com/BlueFinBima/DCS-The-Way-Changer/releases/latest)

The "DCS The Way Changer" is able to set more detailed information about a waypoint for certain aircraft (_currently only the AH-64D Apache_).  While adding and cloning waypoints is supported in the "DCS The Way Changer" it is anticipated that collection will take place in "DCS The Way".

![image](https://user-images.githubusercontent.com/18526232/183957505-771d2222-baee-4a72-ad7e-c9399c2a1174.png)

## Workflow
1. Ensure that the [installation instructions for "DCS The Way"](https://github.com/BlueFinBima/DCS-The-Way#how-to-install) are followed
2. Start "DCS The Way" with the following command from a windows command prompt `theway.exe -s -o c:\temp\waypoints.jspn`
3. Collect your waypoints from the F10 map
4. When all of the waypoints have been collected click the button `Send to DCS`
5. close the "DCS The Way" window.
6. The file `c:\temp\waypoints.jspn` should have been created.
7. Start `The-Way-Point-Editor.exe` and from the menu bar, select File -> Open to open the file  `c:\temp\waypoints.jspn`
8. Edit the waypoints as desired, and when complete, Use the save option from the menu bar to save `c:\temp\waypoints_new.jspn` **Note** it is vital that for the AH-64D apache, the waypoint type & waypoint identifier are a valid combination as per the DCS manual.
   1. For the AH-64D Apache, you should have drop downs at the top of the screen to help set valid waypoints.
   1. Check the `Use Waypoint Definition` checkbox
   2. select your desired waypoint information
   3. Select the waypoint you want to change, and the waypoint type should appear
   4. select another waypoint type
   5. select the waypoint
   7. When complete, uncheck the  `Use Waypoint Definition` checkbox 
10. Start "DCS The Way" with the following command from a windows command prompt `theway.exe -i c:\temp\waypoints_new.jspn`
11. Check that the "DCS The Way" window shows that there are waypoints available.
12. Make sure that the aircraft is in a state suitable to have the waypoints entered, and then click the button `Send to DCS`
13. Wait for all of the waypoints to be entered into the aircraft, then go have some fun. 
