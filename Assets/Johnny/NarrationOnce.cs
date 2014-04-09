/*
 * 		File: NarrationOnce.cs
 * 
 * 		Version: 1.0
 * 
 * 		Author: Johnathon Forster
 * 
 * 		Description:
 * 			Saves data locally using PlayerPrefs to ensure that some objects are only created once
 */

using UnityEngine;
using System.Collections;

public class NarrationOnce : MonoBehaviour {

	// Use this for initialization
	void Start () {
		print (PlayerPrefs.GetInt ("PAUSING"));
		if (PlayerPrefs.GetInt("PAUSING") == 1)
		{
			//print("In pause menu");
			PlayerPrefs.SetInt("PAUSING", 0);
			//	Look for all objects with DestroyOnPause tag
			GameObject[] myObjects = GameObject.FindGameObjectsWithTag ("DestroyOnPause");
			foreach (GameObject obj in myObjects)
			{
				Destroy(obj);
			}
		} else {
			print("In main menu");
		}
	}

	void OnApplicationQuit()
	{
		PlayerPrefs.DeleteKey ("PAUSING");
	}
}
