using UnityEngine;
using System.Collections;

public class MoveTo : MonoBehaviour {
	

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
					GameObject.FindObjectOfType<QCVTwo>().SetTarget(hit.point);
				}
					 
			}
		}
	}
}
