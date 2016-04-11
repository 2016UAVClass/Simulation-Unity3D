using UnityEngine;
using System.Collections;

public class UAVMagnet : MonoBehaviour 
{

	GameObject ConnectedTrap;

	public void Release()
	{
		if(ConnectedTrap != null)
		{
			Destroy(ConnectedTrap.GetComponent<FixedJoint>());
			ConnectedTrap = null;
		}
	}

	void OnTriggerEnter(Collider col)
	{
		if(col.GetComponent<TrapMagnet>() != null && ConnectedTrap == null)
		{
			GameObject Trap = col.GetComponentInParent<Rigidbody>().gameObject;
			Trap.AddComponent<FixedJoint>();
			Trap.GetComponent<FixedJoint>().connectedBody = GetComponent<Rigidbody>();
			ConnectedTrap = Trap;
		}
	}
}
