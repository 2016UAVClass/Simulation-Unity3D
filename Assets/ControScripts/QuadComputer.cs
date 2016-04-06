using UnityEngine;
using System.Collections;

public class QuadComputer : MonoBehaviour {


	public float P = 0.5f;
	public float D = 0.5f;
	public float I = 0.5f;


	public Transform Base;

	public GameObject MoveTarget;

	public SpinProppeller Prop1;
	public SpinProppeller Prop2;
	public SpinProppeller Prop3;
	public SpinProppeller Prop4;

	Vector3 WantedPosition;

	public bool isOn;
	bool GoTo;
	private Rigidbody Br;
	

	float WantY = 0;
	float LastY = 0;
	
	void Start()
	{
		MoveTarget.transform.position = new Vector3(0,-100,0);
		Br = Base.GetComponent<Rigidbody>();
		ToggleRun();
		LastY = transform.eulerAngles.y;
		WantY = LastY;
	}

	// Use this for initialization
	public void ToggleRun() 
	{
		isOn = !isOn;
		if(!isOn)
		{
			GoTo = false;
			MoveTarget.transform.position = new Vector3(0,-100,0);
		}
	}


	public void SetTarget(Vector3 pos)
	{
		if(isOn)
		{
			GoTo = true;
			WantedPosition = new Vector3(pos.x, 0, pos.z);
			if(MoveTarget != null)
			{
				MoveTarget.transform.position = pos;
			}
		}
	}

	public float GetHeight()
	{
		float extra = 0;
		Ray r = new Ray(Base.transform.position, Vector3.down);
		RaycastHit hit;
		if(Physics.Raycast(r, out hit, 50))
		{
			extra = hit.distance;
		}
		return extra;
	}

	int i = 1;

	void FixedUpdate () 
	{
		Vector3 Brot = Base.eulerAngles;
		float x = Brot.x;
		float y = Brot.y;
		float z = Brot.z;
		if(x > 180)
		{
			x = -360.0f + x;
		}
		if(z > 180)
		{
			z = -360.0f + z;
		}
		if(y > 180)
		{
			y = -360.0f + y;
		}
		Brot = new Vector3(x, y, z);

		Vector3 Bpos = Base.position;

		Vector3 OldRot = Brot;

		//float UpForce =  (-1.7f*(Input.GetAxis ("Power"))); //Mathf.Clamp(((5 - GetHeight())/5)-Br.velocity.y/2, 0 , 2 ) + (1*(1+Input.GetAxis ("Power")));


		if(isOn)
		{
			Vector3 av = Base.transform.TransformDirection(Br.angularVelocity);
			Vector2 VC = RotationCancel(av);
			
			if(GoTo)
			{
				Debug.Log("Moving to position: " + WantedPosition);
				SetForce(WantedPosition, Brot);
				float dHeight = (4-Br.transform.position.y)/4.0f;
				Prop4.SpinProp((dHeight - 0 + GetNorm(-Brot.x, 15) * (1.5f-VC.x)));
				Prop2.SpinProp((dHeight - 0 + GetNorm(Brot.x, 15) * (1.5f-VC.x) ));
				Prop3.SpinProp((dHeight + 0 + GetNorm(-Brot.z, 15) *  (1.5f-VC.y)));
				Prop1.SpinProp((dHeight + 0 + GetNorm(Brot.z, 15) * (1.5f-VC.y)));
			}

			/*

			Brot = SetDir(-.8f*Input.GetAxis("Vertical"), Input.GetAxis("Strafe")/1.5f, Input.GetAxis("Horizontal"), Brot);

			Vector3 WantRot = SetDir(-.8f*Input.GetAxis("Vertical"), Input.GetAxis("Strafe")/1.5f, Input.GetAxis("Horizontal"), Vector3.zero);

			Vector3 av = Base.transform.TransformDirection(Br.angularVelocity);
			Vector2 VC = RotationCancel(av);
			float Const = 1f;



			float spin = 0;

			float dist = Brot.y - WantY;
			if(Mathf.Abs(dist) > 180)
			{
				dist = WantY - Brot.y;
			}

			spin = Mathf.Clamp((dist + (av.y*10))/180, -1.0f, 1);

			DebugGraph.MultiLog("Y Rotation", Color.blue, Brot.y);
			DebugGraph.MultiLog("Y Rotation", Color.red, WantY);

			Prop4.SpinProp(Const*(UpForce - spin + GetNorm(-Brot.x, 15) * (1.5f-VC.x)));
			Prop2.SpinProp(Const*(UpForce - spin + GetNorm(Brot.x, 15) * (1.5f-VC.x) ));
			Prop3.SpinProp(Const*(UpForce + spin + GetNorm(-Brot.z, 15) *  (1.5f-VC.y)));
			Prop1.SpinProp(Const*(UpForce + spin + GetNorm(Brot.z, 15) * (1.5f-VC.y)));

			/*
			DebugGraph.MultiLog("Prop 1", Color.blue, Const*( + GetNorm(Brot.z, 15) * VC.y));
			DebugGraph.MultiLog("Prop 1", Color.red, GetNorm(Brot.z, 15) - Br.angularVelocity.z/3);

			DebugGraph.MultiLog("Prop 2", Color.blue, Const*( + GetNorm(Brot.x, 15) * VC.x ));
			DebugGraph.MultiLog("Prop 2", Color.red, GetNorm(Brot.x, 15) - Br.angularVelocity.x/3);

			DebugGraph.MultiLog("Prop 3", Color.blue, Const*( + GetNorm(-Brot.z, 15)  * (1.5f-VC.y)));
			DebugGraph.MultiLog("Prop 3", Color.red, GetNorm(-Brot.z, 15) + Br.angularVelocity.z/3);

			DebugGraph.MultiLog("Prop 4", Color.blue, Const*( + GetNorm(-Brot.x, 15) * (1.5f-VC.x)));
			DebugGraph.MultiLog("Prop 4", Color.red, GetNorm(-Brot.x, 15) + Br.angularVelocity.x/3);
			*/


			/*
			DebugGraph.MultiLog("Z Values", Color.red, av.z/4);
			DebugGraph.MultiLog("Z Values", Color.blue, VC.y);
			*/

		}
		/*
		else
		{
			float power = (-1.7f*Input.GetAxis ("Power"));
			float FB = Input.GetAxis("Strafe")/2;
			float LR = -1*Input.GetAxis("Vertical")/2;
			float SR = Input.GetAxis("Horizontal")/2;

			Prop4.SpinProp(power+(FB - LR - SR));
			Prop2.SpinProp(power+(FB + LR + SR));
			Prop3.SpinProp(power+(-FB - LR + SR));
			Prop1.SpinProp(power+(-FB + LR - SR));

		}
		*/
	}

	/* Quad Prop Setup
	 * 	 
	 * 	 Forward					Z+
	 * 	  	
	 * 	3		4					3
	 * 	 \	   /					|
	 * 	  \	  /						|
	 * 		X				 2______X______4   X+
	 * 		X						X
	 * 	  /	  \						|
	 * 	 /	   \					|
	 *  2		1					1
	 *
	 *		Back
	 */

	public Vector2 RotationCancel(Vector3 v)
	{
		// Input v is the Quad's rigidbody.angularVelocity


		float x = 0;
		float y = 0;

		float vx = 1/(1+v.x/8);
		float vy = 1/(1+v.y/8);

		x = Mathf.Clamp(vx , -1.5f, 1.5f);
		y = Mathf.Clamp(vy , -1.5f, 1.5f);


		return Vector2.zero;//new Vector4(x,y);
	}

	public Vector3 SetDir(float fb, float lr, float sr, Vector3 Rot)
	{
		/*
		Prop1.SpinProp( Mathf.Clamp(((1.3f*fb*0)+(-3*lr))/3f, 0, .9f));
		Prop2.SpinProp( Mathf.Clamp(((1.3f*fb*0)+(3*lr))/3f, 0, .9f));
		Prop3.SpinProp( Mathf.Clamp(((-1.3f*fb*0)+(-3*lr))/3f, 0, .9f));
		Prop4.SpinProp( Mathf.Clamp(((-1.3f*fb*0)+(3*lr))/3f, 0, .9f));
		*/
		WantY += lr*2f;
		if(WantY < -180)
			WantY = 180;
		else if(WantY > 180)
			WantY = -180;

		float AddAmt = 45;
		Vector3 NewRot = new Vector3(Rot.x + AddAmt*fb + (AddAmt)*sr, Rot.y, Rot.z + AddAmt*fb - (AddAmt)*sr);
		return NewRot;
	}



	public void SetForce(Vector3 WantPos, Vector3 Cur)
	{
		Vector3 NoHB = new Vector3(Base.transform.position.z, 0, Base.transform.position.z);
		if(Vector3.Distance(WantPos, NoHB) < 2 && Br.velocity.magnitude < 0.7f)
		{
			return;
		}
		float MaxForce = .1f * Mathf.Clamp(Vector3.Distance(WantPos, NoHB)/3, 0, 1);
		Vector3 Diff = new Vector3(Base.transform.position.x - WantPos.x , 0, Base.transform.position.z-WantPos.z);
		Vector3 RelWant = Base.transform.InverseTransformDirection(Diff);
		Vector3 RelVel = Base.transform.InverseTransformDirection(Br.velocity);
		float P3Force = Mathf.Clamp(RelWant.x/2 + RelVel.x/2, 0, MaxForce);
		float P1Force = Mathf.Clamp(-1 * RelWant.x - RelVel.x/2, 0, MaxForce);
		float P2Force = Mathf.Clamp(RelWant.z/2 + RelVel.z/2, 0, MaxForce);
		float P4Force = Mathf.Clamp(-1 * RelWant.z/2 - RelVel.z/2, 0, MaxForce);
		if(Cur.z > -8)
			Prop1.SpinProp(P1Force*GetNorm(Cur.z, 8));
		if(Cur.x > -8)
			Prop2.SpinProp(P2Force*GetNorm(Cur.x, 8));
		if(Cur.z < 8)
			Prop3.SpinProp(P3Force*GetNorm(Cur.z, 8));
		if(Cur.x < 8)
			Prop4.SpinProp(P4Force*GetNorm(Cur.x, 8));

	}


	float GetNorm(float inpt, float div)
	{
		return Mathf.Clamp(((inpt)/(div)) , 0, 1);
	}
}
