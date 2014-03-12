using UnityEngine;
using System.Collections;

public class DoorTrigger : MonoBehaviour {

	public string warp;

	void OnTriggerEnter(Collider col)
	{
		print ("DoorTrigger enter");
		if(col.gameObject.tag == "MainCamera")
		{
			print ("Warp to " + warp);
			Application.LoadLevel(warp);
		}
	}
}
