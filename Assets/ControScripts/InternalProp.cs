using UnityEngine;
using System.Collections;

public class InternalProp : MonoBehaviour {
	
	public Rigidbody Holder;
	
	void Update()
	{
		transform.localEulerAngles -= Vector3.up*Time.deltaTime*Mathf.Abs(Holder.angularVelocity.y)*50f;
	}

}
