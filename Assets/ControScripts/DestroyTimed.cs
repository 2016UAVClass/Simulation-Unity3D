using UnityEngine;
using System.Collections;

public class DestroyTimed : MonoBehaviour {

	public float Seconds;
	
	void Start () 
	{
		Destroy(gameObject, Seconds);
	}

}
