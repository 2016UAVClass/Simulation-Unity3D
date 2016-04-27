using UnityEngine;
using System.Collections;

public class StartMap : MonoBehaviour {

	public GameObject[] Maplist;

	// Use this for initialization
	void Start () 
	{
		Maplist[Random.Range(0, Maplist.Length)].SetActive(true);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
