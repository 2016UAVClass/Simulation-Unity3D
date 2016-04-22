using ROSBridgeLib;
using ROSBridgeLib.std_msgs;
using ROSBridgeLib.turtlesim;
using System.Collections;
using SimpleJSON;
using UnityEngine;

/**
 * This is a toy example of the Unity-ROS interface talking to the TurtleSim 
 * tutorial (circa Groovy). Note that due to some changes since then this will have
 * to be slightly re-written, but as its a test ....
 * 
 * This defines the callback that links the pose message. It moves the Dalek with
 * the turtlesim
 * 
 * @author Michael Jenkin, Robert Codd-Downey and Andrew Speers
 * @version 3.0
 **/

public class UAV_Path : ROSBridgeSubscriber {
	
	public new static string GetMessageTopic() {
		return "/uav_path_topic";
	}  
	
	public new static string GetMessageType() {
		return "turtlesim/Pose"; //??? 
	}
	
	public new static ROSBridgeMsg ParseMessage(JSONNode msg) {
		return new PathMsg(msg);
	}
	
	public new static void CallBack(ROSBridgeMsg msg) {
		GameObject uav = GameObject.Find ("UAV");
		if (uav == null)
			Debug.Log ("Can't find the uav???");
		else {
			PathMsg path = (PathMsg) msg;
			for (int i = 0; i < path.poses.length(); i = i+1){
			// Just need to add a point type for each pose message	
			}
			
			robot.transform.position = new Vector3(pose.GetX (), 0.2f, pose.GetY());
			robot.transform.rotation = Quaternion.AngleAxis (-pose.GetTheta() * 180.0f / 3.1415f, Vector3.up);
		}
	}
}
