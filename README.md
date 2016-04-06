# Simulation-Unity3D
Unity3D UAV Simulation

## ROS Integration
Simple setup to get ROS on Linux VM connected to Unity3D on host computer
#### ROS Bridge
To connect ROS on VM to host computer, ROS-Bridge is required<br /><br />
Install: `sudo apt-get install ros-indigo-rosbridge-suite` <br />
Run: `roslaunch rosbridge_server rosbridge_websocket.launch` <br />
This creates a websocket on port 9090 by default
#### Connection in Unity3D
Edit line 29 in TurtlesimViewer.cs to use the VM IP address <br />
`ros = new ROSBridgeWebSocketConnection ("ws://VM.IP.ADDRESS", 9090);`<br />
The VM IP address can be found at the top right of VM (two arrows up/down) -> "Connection Information" -> IPv4 IP Address
#### Running System
To test connection, run the ROS turtle sim (http://wiki.ros.org/turtlesim) and run ROS-Bridge on the VM.<br />
In Unity3D open RosScene and click Play.<br />
Use arrow keys inside Unity to move turtle in ROS on VM
