using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class QCVTwo : MonoBehaviour {

	public Mode CurrentMode;


	public float ComputerRigidity = 45f;
	public float ControlPower = 45f;

	public PointType ttype;

	public bool HQuad;

	[Range(0f, 20f)]
	public float DesiredHieght = 3;

	[Range(0.5f, 5.5f)]
	public float ThrottlePower;
	[Range(0.5f, 5.5f)]
	public float YawPower;
	[Range(0.5f, 5.5f)]
	public float RollPower;
	[Range(0.5f, 5.5f)]
	public float PitchPower;

	[Range(1.0f, 3.0f)]
	public float ThrottleExponent;
	[Range(1.0f, 3.0f)]
	public float YawExponent;
	[Range(1.0f, 3.0f)]
	public float RollExponent;
	[Range(1.0f, 3.0f)]
	public float PitchExponet;

	public float MaxHeight;
	
	public float MotorPower;

	public float MaxSpeed;

	[Range(-1.0f, 1.0f)]
	public float TrimPitch;
	[Range(-1.0f, 1.0f)]
	public float TrimRoll;
	[Range(-1.0f, 1.0f)]
	public float TrimYaw;
	[Range(-1.0f, 1.0f)]
	public float TrimThrottle;


	public bool ReachedTarget;


	public SpinProppeller P1;
	public SpinProppeller P2;
	public SpinProppeller P3;
	public SpinProppeller P4;

	public GameObject QuadCopter;

	Rigidbody Qrb;
	Transform Qtr;

	[System.Serializable]
	public enum Mode
	{
		Acrobatic,
		Stabalized
	}

	public Vector3 WantedPosition;
	public GameObject MoveTarget;

	public void SetTarget(Vector3 pos)
	{
		ReachedTarget = false;
		WantedPosition = new Vector3(pos.x, 0, pos.z);
		if(MoveTarget != null)
		{
			MoveTarget.transform.position = pos;
		}
	}

	public void ToggleStabalize()
	{
		if(CurrentMode == Mode.Stabalized)
		{
			CurrentMode = Mode.Acrobatic;
		}
		else
		{
			CurrentMode = Mode.Stabalized;
		}
	}
	
	void Start () 
	{
		CurrentMode = Mode.Stabalized;
		Qrb = QuadCopter.GetComponent<Rigidbody>();
		Qtr = QuadCopter.transform;
		WantedPosition = Qtr.position;
	}

	void FixedUpdate () 
	{
		Qrb.velocity *= .96f;
		Vector3 CurRot = Qtr.localEulerAngles;
		float x = CurRot.x;
		float z = CurRot.z;
		if(x > 180)
			x = -360.0f + x;
		if(z > 180)
			z = -360.0f + z;

		float FBAngle = (x + z)/2.0f;
		float LRAngle = (z-x)/2.0f;

		Vector4 vals = QuadMoveTo(WantedPosition, CurRot);

		//Throttle
		float power = vals.x*2.5f;//(1.7f*Mathf.Pow(ThrottlePower*Mathf.Abs(cInput.GetAxis("Power")), ThrottleExponent)*-Mathf.Sign(cInput.GetAxis("Power"))) + TrimThrottle;

		//Roll
		float FB = vals.y; //(Mathf.Pow(RollPower*Mathf.Abs(cInput.GetAxis("Strafe")), RollExponent) * Mathf.Sign(cInput.GetAxis("Strafe"))) + TrimRoll - (LRAngle/800f);

		//Pitch
		float LR = vals.z; //(Mathf.Pow(PitchPower*Mathf.Abs(cInput.GetAxis("Vertical")), PitchExponet)  * -Mathf.Sign(cInput.GetAxis("Vertical"))) + TrimPitch + (FBAngle/800f);

		//Yaw
		float SR = vals.w; //( Mathf.Pow(YawPower*Mathf.Abs(cInput.GetAxis("Horizontal")), YawExponent)  * Mathf.Sign(cInput.GetAxis("Horizontal")) )+ TrimYaw;

		Vector3 WantRotation = Vector3.zero;
		
		
		Vector3 Qav = Qtr.TransformVector(Qrb.angularVelocity/Mathf.Rad2Deg);
		CurRot = new Vector3(x,CurRot.y, z);
		float SpinLeft = 0;
		float SpinRight = 0;
		Vector3 AngDif = Vector3.zero;


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
		
		if(CurrentMode == Mode.Acrobatic)
		{


			P1.SpinProp((power+(-SR + LR - FB))*MotorPower);
			P2.SpinProp((power+(SR + LR + FB))*MotorPower);
			P3.SpinProp((power+(-SR - LR + FB))*MotorPower);
			P4.SpinProp((power+(SR - LR - FB))*MotorPower);
		}
		else if(CurrentMode == Mode.Stabalized)
		{
			//Max height power
			//power = Mathf.Clamp(((MaxHeight - GetHeight())/MaxHeight)-Qrb.velocity.y/6, 0 , 10) * power;
			if(HQuad)
			{
				WantRotation = new Vector3(-ControlPower*FB, CurRot.y - SR, -(ControlPower)*LR);
			}
			else
			{
				WantRotation = new Vector3(ControlPower*LR - (ControlPower)*FB, CurRot.y - SR, ControlPower*LR + (ControlPower)*FB);
			}
			AngDif = new Vector3(CurRot.x - WantRotation.x, Mathf.Clamp(CurRot.y - WantRotation.y, -1.5f, 1.5f), CurRot.z - WantRotation.z);
			if(AngDif.y > 0)
				SpinRight = AngDif.y;
			else
				SpinLeft = -1* AngDif.y;
			if(HQuad)
			{
				P1.SpinProp((power + GetNorm(AngDif.z, ComputerRigidity) + GetNorm(-AngDif.x, ComputerRigidity) + SpinLeft -  Qav.z*20)*MotorPower);
				P2.SpinProp((power + GetNorm(AngDif.z, ComputerRigidity) + GetNorm(AngDif.x, ComputerRigidity) + SpinRight -  Qav.x*20)*MotorPower);
				P3.SpinProp((power + GetNorm(-AngDif.z, ComputerRigidity) + GetNorm(AngDif.x, ComputerRigidity) + SpinLeft + Qav.z*20)*MotorPower);
				P4.SpinProp((power + GetNorm(-AngDif.z, ComputerRigidity) + GetNorm(-AngDif.x, ComputerRigidity) + SpinRight + Qav.x*20)*MotorPower);
				
			}
			else
			{
				P1.SpinProp((power + GetNorm(AngDif.z, ComputerRigidity) - AngDif.y -  Qav.z*20)*MotorPower);
				P2.SpinProp((power + GetNorm(AngDif.x, ComputerRigidity) + AngDif.y -  Qav.x*20)*MotorPower);
				P3.SpinProp((power + GetNorm(-AngDif.z, ComputerRigidity) - AngDif.y + Qav.z*20)*MotorPower);
				P4.SpinProp((power + GetNorm(-AngDif.x, ComputerRigidity) + AngDif.y + Qav.x*20)*MotorPower);
			}

		}

	}

	Vector4 QuadMoveTo(Vector3 WantPos, Vector3 Cur)
	{
		Vector3 NoHB = new Vector3(Qtr.position.x, WantPos.y, Qtr.position.z);
		float DH = DesiredHieght;
		if(GetComponentInChildren<UAVMagnet>().hasTrap && ttype != PointType.Dropoff)
			DH += 1.33f;

		float throttle = (DH-Qtr.position.y)/Mathf.Max(0.1f, DesiredHieght);
		if(Vector3.Distance(WantPos,NoHB) < 0.2f && Qrb.velocity.magnitude < 0.5f && ttype == PointType.Waypoint)
		{
			ReachedTarget = true;
			return new Vector4(throttle,0,0,0);
		}
		if(Vector3.Distance(WantPos, NoHB) < .15f && Qrb.velocity.magnitude < 0.5f && !ReachedTarget)
		{
			if(ttype == PointType.Dropoff)
			{
				if(DesiredHieght > 1 && Qrb.velocity.y > -0.1f)
				{
					DesiredHieght -= Time.fixedDeltaTime;
					return new Vector4(throttle,0,0,0);
				}
				else if(DesiredHieght <= 1 && Qrb.velocity.y > -0.2f)
				{
					DesiredHieght = 5;
					GetComponentInChildren<UAVMagnet>().Release();
				}
				else
				{
					return new Vector4(throttle,0,0,0);
				}
			}
			else if(ttype == PointType.Takeoff)
			{
				DesiredHieght = 5;
			}
			else if(ttype == PointType.Land)
			{
				if(DesiredHieght > 0.1 && Qrb.velocity.y > -0.3f)
				{
					DesiredHieght -= Time.fixedDeltaTime;
					return new Vector4(throttle,0,0,0);
				}
				else if(DesiredHieght <= 0.1f)
					DesiredHieght = 0;
				else
				{
					return new Vector4(throttle,0,0,0);
				}
			}
			ReachedTarget = true;
			return new Vector4(throttle,0,0,0);
		}

		if(DesiredHieght == 0)
			return Vector4.zero;
		List<float> Dists = new List<float>();
		Dists.Add(Vector3.Distance(P1.transform.position, WantPos));
		Dists.Add(Vector3.Distance(P2.transform.position, WantPos));
		Dists.Add(Vector3.Distance(P3.transform.position, WantPos));
		Dists.Add(Vector3.Distance(P4.transform.position, WantPos));
		int low = 0;
		float lowDist = Dists[0];
		for(int i=0; i<Dists.Count; i++)
		{
			if(Dists[i] < lowDist)
			{
				low = i;
				lowDist = Dists[i];
			}
		}
		for(int i = 0; i< Dists.Count; i++)
		{
			Dists[i] -= lowDist;
		}
		float dist = Vector3.Distance(WantPos, NoHB);
		float mult = Mathf.Clamp(Mathf.Sqrt(dist/10.0f), 5.0f, 100.0f);
		mult /= 2;
		/*
		if(high == 0) P1.SpinProp(MotorPower/mult);
		if(high == 1) P2.SpinProp(MotorPower/mult);
		if(high == 2) P3.SpinProp(MotorPower/mult);
		if(high == 3) P4.SpinProp(MotorPower/mult);
		*/
		P1.SpinProp(MotorPower/(mult/Dists[0]));
		P2.SpinProp(MotorPower/(mult/Dists[1]));
		P3.SpinProp(MotorPower/(mult/Dists[2]));
		P4.SpinProp(MotorPower/(mult/Dists[3]));
		return new Vector4(throttle,0,0,0);
	}

	float GetNorm(float inpt, float div)
	{
		return Mathf.Clamp(((inpt)/(div)) , 0.0f, 1.0f);
	}

	public float GetHeight()
	{
		float distanceDown = 0;
		Ray r = new Ray(Qtr.position, Vector3.down);
		RaycastHit hit;
		if(Physics.Raycast(r, out hit, 50))
		{
			distanceDown = hit.distance;
		}
		return distanceDown;
	}
	



}
