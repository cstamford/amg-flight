/*
		File: SceneFadeIn.cs

		Version: 1.0

		Author: Johnathon Forster

		Description:
			Allows a scene to fade in from a certain colour over a specified time period
			Allows for a time scale manipulation to also have the game slowly fade in
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

	// Use this for initialization
	void Start ()
	{
		finished = false;

		elapsedTime = 0.0f;
		prevTime = Time.realtimeSinceStartup;
	}
	
	// Update is called once per frame
	void Update()
	{
		float time = Time.realtimeSinceStartup - prevTime;
		prevTime = Time.realtimeSinceStartup;
		
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
