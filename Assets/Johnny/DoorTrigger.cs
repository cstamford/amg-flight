/*
 * 		File: DoorTrigger.cs
 * 
 * 		Version: 1.2
 * 
 * 		Author: Johnathon Forster
 * 
 * 		Description:
 * 			Loads a specified scene after a set amount of time
 * 			Slowing timescale and fading in a colour is an option
 * 			Saving player's orientation is also an option
 */

using UnityEngine;
using System.Collections;

public class DoorTrigger : MonoBehaviour {

	public string warp;

	public bool fadeIn = true;
	
	public Color fadeColour = new Color(1, 1, 1, 1);
	public float fadeTime = 2.0f;
	
	public bool useColourFade = true;
	public bool useTimeFade = true;

	public bool savePlayerOrientation;
	public GameObject player;
	
	private bool finished;
	
	private float elapsedTime;
	private float prevTime;

	void Start()
	{
		finished = true;
		
		elapsedTime = 0.0f;
		prevTime = Time.realtimeSinceStartup;
	}

	void OnTriggerEnter(Collider col)
	{

		if(col.gameObject.tag == "Player")
		{
			print ("DoorTrigger enter");

			if (fadeIn == false)
			{
				warpToLocation();
			} else {
				finished = false;
			}
		}
	}

	private void warpToLocation()
	{
		print ("Warp to " + warp);
		Application.LoadLevel(warp);
	}

	void Update ()
	{
		float time = Time.realtimeSinceStartup - prevTime;
		prevTime = Time.realtimeSinceStartup;
		
		if(finished == false)
		{
			elapsedTime += time;
			//print (elapsedTime);
			if(elapsedTime >= fadeTime)
			{
				if (savePlayerOrientation == true)
				{
					saveOrientation();
				}
				warpToLocation();
				return;
			}
			if(useTimeFade == true)
			{
				Time.timeScale = 1.0f - (elapsedTime/fadeTime);
			}
			if (useColourFade == true)
			{
				fadeColour.a = elapsedTime/fadeTime;
			}
		}
	}

	private void saveOrientation()
	{
		if (savePlayerOrientation == true) {
			if (player != null)
			{
				//	Getting player rotation from controller
				Quaternion rotation = player.transform.rotation;
				print ("rotation w: " + rotation.w);
				PlayerPrefs.SetFloat ("PLAYER_ROTATION_W", rotation.w);
				print ("rotation x: " + rotation.z);
				PlayerPrefs.SetFloat ("PLAYER_ROTATION_X", rotation.x);
				print ("rotation y: " + rotation.y);
				PlayerPrefs.SetFloat ("PLAYER_ROTATION_Y", rotation.y);
				print ("rotation z: " + rotation.z);
				PlayerPrefs.SetFloat ("PLAYER_ROTATION_Z", rotation.z);
			}
		}
	}
	
	void OnGUI()
	{
		if (useColourFade == true) {
			if (finished == false) {
				GUI.color = fadeColour;
				Texture2D texture = new Texture2D (1, 1);
				texture.SetPixel (0, 0, fadeColour);
				texture.Apply ();
				GUI.skin.box.normal.background = texture;
				GUI.Box (new Rect (0, 0, Screen.width, Screen.height), GUIContent.none);
			}
		}
	}
}

/*
 *		Version History:
 *			1.0: File was created
 *			1.1: Added colour fading and timescale slowing
 *			1.2: Added saving player orientation
 */