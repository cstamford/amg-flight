/*
 * 		File: ForceCameraSwitch.cs
 * 
 * 		Version: 1.0
 * 
 * 		Author: Johnathon Forster
 * 
 * 		Description:
 * 				Script to be attached to a Camera object
 * 				When initialised, camera will be selected as active camera
 * 				All other cameras in the scene are disabled
 */
using UnityEngine;
using System.Collections;

public class ForceCameraSwitch : MonoBehaviour {

	// Use this for initialization
	void Start () {
		//	Disable all cameras in scene
		Camera[] cameras = Camera.allCameras;
		foreach(Camera camera in cameras)
		{
			camera.enabled = false;
		}

		//	Enable this camera
		Camera thisCamera = (Camera)GetComponent<Camera>();
		thisCamera.enabled = true;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
