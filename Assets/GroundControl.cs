using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class GroundControl : MonoBehaviour {

	public List<QuadCommand> TargetList;
	public QCVTwo UAV;
	int i = 0;

	public GameObject WaypointRender;

	// Use this for initialization
	void Start () 
	{
		if(UAV == null)
			UAV = FindObjectOfType<QCVTwo>();

		for(int i=0; i<TargetList.Count; i++)
		{
			Vector3 p1 = TargetList[i].Pos;
			Vector3 p2 = TargetList[i].Pos;
			if(i+1 < TargetList.Count)
				p2 = TargetList[i+1].Pos;
			GameObject nW = (GameObject)Instantiate(WaypointRender, p1, Quaternion.identity);
			nW.transform.SetParent(transform);
			nW.GetComponent<LineRenderer>().SetPosition(0, p1);
			nW.GetComponent<LineRenderer>().SetPosition(1, p2);
		}

	}
	
	// Update is called once per frame
	void Update () 
	{
		if(UAV.ReachedTarget == true && i < TargetList.Count)
		{
			Debug.Log("Starting Waypoint action " + (i+1));
			UAV.SetTarget(TargetList[i].Pos);
			UAV.ttype = TargetList[i].type;
			i++;
		}
	}
}

[System.Serializable]
public class QuadCommand
{
	public Vector3 Pos;
	public PointType type;
}

public enum PointType
{
	Takeoff,
	Waypoint,
	Dropoff,
	Pickup,
	Land
}