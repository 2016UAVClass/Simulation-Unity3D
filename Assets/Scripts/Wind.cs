using UnityEngine;
using System.Collections;

public class Wind : MonoBehaviour {

	private WindAffector[] UAVs;

	[Range (0, 25)]
	public float WindForce;
	public Vector3 WindDirection;

	private float WindForceConst = 4.5f;

	void Start()
	{
		//Find all objects with WindAffector component at start of simulation
		UAVs = FindObjectsOfType<WindAffector>();
	}

	void FixedUpdate ()
	{
		for(int i=0; i < UAVs.Length; i++)
		{
			Rigidbody UAVBody = UAVs[i].GetComponent<Rigidbody>();
			if(UAVBody)
			{
				Vector3 wind = WindForce*WindForceConst*(WindDirection.normalized)*Random.Range(0.9f, 1.1f);
				Debug.DrawRay(UAVBody.transform.position, wind/(WindForceConst*2), Color.blue);
				UAVBody.AddForce(wind*Time.fixedDeltaTime);
			}
		}
	}
}
