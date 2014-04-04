/*
		File: SceneFadeIn.cs

		Version: 1.1

		Author: Johnathon Forster

		Description:
			Allows a scene to fade in from a certain colour over a specified time period
			Allows for a time scale manipulation to also have the game slowly fade in
			Ignores first frame which can take a long time to compute when loading
			-Long first frames can result in transitions not appearing.
 */

using UnityEngine;
using System.Collections;

public class SceneFadeIn : MonoBehaviour {

	public Color fadeColour = new Color(1, 1, 1, 1);
	public float fadeTime = 2.0f;

	public bool useColourFade = true;
	public bool useTimeFade = false;

	private bool finished;

	private float elapsedTime;
	private float prevTime;

	private bool firstFrame;

	// Use this for initialization
	void Start ()
	{
		finished = false;

		elapsedTime = 0.0f;
		prevTime = Time.realtimeSinceStartup;

		firstFrame = true;
	}
	
	// Update is called once per frame
	void Update()
	{
		float time = Time.realtimeSinceStartup - prevTime;
		prevTime = Time.realtimeSinceStartup;

		if (firstFrame == true)
		{
			firstFrame = false;
			return;
		}
		
		if(finished == false)
		{
			elapsedTime += time;
			//print (elapsedTime);
			if(elapsedTime >= fadeTime)
			{
				Time.timeScale = 1.0f;
				finished = true;
				return;
			}
			if(useTimeFade == true)
			{
				Time.timeScale = elapsedTime/fadeTime;
			}
			if(useColourFade == true)
			{
				fadeColour.a = 1.0f - elapsedTime/fadeTime;
			}
		}
	}
	
	void OnGUI()
	{
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

/*
 *		Version History:
 *			1.0: File was created
 *			1.1: Added ignoring of first frame for smoother transitions.
 */