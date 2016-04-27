using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GroundControl : MonoBehaviour {

	public List<QuadCommand> TargetList;
	public QCVTwo UAV;
	int i = 0;

	public GameObject WaypointRender;

	List<GameObject> points;

	// Use this for initialization
	void Start () 
	{
		points = new List<GameObject>();
		if(UAV == null)
			UAV = FindObjectOfType<QCVTwo>();

		for(int i=0; i<TargetList.Count; i++)
		{
			Vector3 p1 = TargetList[i].Pos;
			Vector3 p2 = TargetList[i].Pos;
			if(i+1 < TargetList.Count)
				p2 = TargetList[i+1].Pos;
			GameObject nW = (GameObject)Instantiate(WaypointRender, p1, Quaternion.identity);
			points.Add (nW);
			nW.transform.SetParent(transform);
			nW.GetComponent<LineRenderer>().SetPosition(0, p1);
			nW.GetComponent<LineRenderer>().SetPosition(1, p2);
		}

	}

	bool reset;
	
	// Update is called once per frame
	void Update () 
	{
		if(Input.GetKeyDown(KeyCode.R))
		{
			reload();
		}
		if(UAV.ReachedTarget == true && i < TargetList.Count)
		{
			if(i > 0)
				points[i-1].GetComponent<MeshRenderer>().material.color = new Color(0, 1, 0, 0.5f);
			Debug.Log("Starting Waypoint action " + (i+1));
			UAV.SetTarget(TargetList[i].Pos);
			UAV.ttype = TargetList[i].type;
			i++;
		}
		else if(i >= TargetList.Count && UAV.ReachedTarget == true && !reset)
		{
			reset = true;
			Invoke("reload", 4f);
		}
	}

	void reload()
	{
		Application.LoadLevel(0);
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