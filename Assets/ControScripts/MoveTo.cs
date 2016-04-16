using UnityEngine;
using System.Collections;

public class MoveTo : MonoBehaviour {
	
	public Vector3 WantPos;

	Vector3 wp;


	// Update is called once per frame
	void Update () 
	{
		if(Input.GetMouseButtonDown(0))
		{
			Ray ray = Camera.main.ScreenPointToRay( Input.mousePosition );
			RaycastHit hit;
			if( Physics.Raycast( ray, out hit ) )
			{

				if( hit.collider.tag == "Base" )
				{
					WantPos = hit.point;
				}
					 
			}
		}
		if(wp != WantPos)
		{
			wp = WantPos;
			QCVTwo[] t = GameObject.FindObjectsOfType<QCVTwo>();
			foreach(var v in t)
			{
				v.SetTarget(WantPos);
			}
		}
	}
}
