using UnityEngine;
using System.Collections;

public class SpinProppeller : MonoBehaviour {

	public Rigidbody rb;
	public Rigidbody holder;
	public float force;
	public Vector3 TorqueForce;

	private bool broken;

	public GameObject Point;

	public GameObject Blur;

	void Start()
	{
		rb.maxAngularVelocity = 40;
	}

	void Update()
	{
		if(Mathf.Abs(rb.angularVelocity.y) > 10)
		{
			Blur.SetActive(true);
		}
		else
		{
			Blur.SetActive(false);
		}
	}

	public void SpinProp(float PortionForce)
	{
		if(broken)
			return;

		rb.AddRelativeTorque(TorqueForce*Time.deltaTime*PortionForce*Mathf.Clamp((0.5f/Mathf.Abs(rb.angularVelocity.y)), 0, 1));
		rb.AddForceAtPosition((transform.TransformDirection(Vector3.up)*force*Time.deltaTime*PortionForce) * GroundEffect(), transform.position);
		rb.AddForce((Point.transform.TransformDirection(Vector3.forward)*force*Time.deltaTime*PortionForce));
	}

	float GroundEffect()
	{
		float val = 1;
		float extra = 0;
		Ray r = new Ray(holder.transform.position, Vector3.down*0.25f);
		RaycastHit hit;
		if(Physics.Raycast(r, out hit))
		{
			extra = Random.Range(0.7f, Mathf.Clamp(1.0f/hit.distance, 0.0f, 1.8f)) * Mathf.Clamp(Mathf.Sqrt(1.0f/hit.distance), 0, 1.35f);
		}
		return val;//+extra;
	}

	void OnJointBreak(float breakForce) 
	{
		holder.angularDrag = .01f;
		broken = true;
	}
}
