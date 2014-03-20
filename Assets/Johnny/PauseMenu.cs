/*
 * File: PauseMenu.cs
 * 
 * Version: 1.0
 * 
 * Author: Johnathon Forster
 * 
 * Description: 
 * 		When the user presses the 'p' key the game will slow down over the course of several seconds
 * 		When the game has slowed to nothing the scene "Menu Scene" will be loaded.
 */

using UnityEngine;
using System.Collections;

public class PauseMenu : MonoBehaviour
{

	enum PauseState { playing, pausing, paused };
	PauseState pauseState;

	//	Time taken for game to pause
	float pauseTime;
	//	Time passed since game was paused
	float elapsedTime;
	float prevTime;

	public string pauseKey;

	// Use this for initialization
	void Start ()
	{
		pauseState = PauseState.playing;
		pauseTime = 2.0f;
		elapsedTime = 0.0f;
		prevTime = Time.realtimeSinceStartup;
		//	TimeScale persists despite level being unloaded
		//	Reset timeScale here
		Time.timeScale = 1.0f;

		loadData ();
	}

	void Awake()
	{
		//	Need to make sure this object persists to remove data
		DontDestroyOnLoad(transform);
	}
	
	// Update is called once per frame
	void Update ()
	{
		float time = Time.realtimeSinceStartup - prevTime;
		prevTime = Time.realtimeSinceStartup;
		//	Reducing timescale based on pause
		if(pauseState == PauseState.pausing){
			elapsedTime += time;
			if(elapsedTime >= pauseTime)
			{
				saveData();
				pauseState = PauseState.paused;

				//pause ();
				Application.LoadLevel("Menu Scene");
				//Application.LoadLevelAdditive("Menu Scene");

                Time.timeScale = 1.0f;
				return;
			}
			Time.timeScale = 1.0f - (elapsedTime/pauseTime);
		}
		//	Detecting key presses
		if(Input.GetKeyDown(KeyCode.P))
		{
			if(pauseState == PauseState.playing){
				pauseState = PauseState.pausing;
			}
		}
	}

	void pause()
	{
		//	Disable all of the audio listener and seraph controller scripts on the player
		Camera camera = (Camera)FindObjectOfType(typeof(Camera));
		if(camera != null)
		{
			((AudioListener)camera.GetComponent<AudioListener>()).enabled = false;
			((cst.Flight.SeraphController)camera.GetComponent<cst.Flight.SeraphController>()).enabled = false;
		}
	}

	void saveData()
	{
		//	Search for an object with the "SeraphController" script attached
		//	Will need to add appropriate getters to SeraphController script
		Camera camera = (Camera)FindObjectOfType(typeof(Camera));
		if(camera != null)
		{
			//	If found, copy the SeraphController properties to userprefs
			cst.Flight.SeraphController seraphController = camera.GetComponent<cst.Flight.SeraphController>();
			if(seraphController != null)
			{
				//	Setting a flag to signify that there's data saved
				PlayerPrefs.SetInt("DATA_SAVED", 1);

				//	Getting player position from controller
				Vector3 positon = seraphController.transform.position;
				print ("position x: " + positon.x);
				PlayerPrefs.SetFloat("PLAYER_POSITION_X", positon.x);
				print ("position y: " + positon.y);
				PlayerPrefs.SetFloat("PLAYER_POSITION_Y", positon.y);
				print ("position z: " + positon.z);
				PlayerPrefs.SetFloat("PLAYER_POSITION_Z", positon.z);

				//	Getting player rotation from controller
				Quaternion rotation = seraphController.transform.rotation;
				print ("rotation w: " + rotation.w);
				PlayerPrefs.SetFloat("PLAYER_ROTATION_W", rotation.w);
				print ("rotation x: " + rotation.z);
				PlayerPrefs.SetFloat("PLAYER_ROTATION_X", rotation.x);
				print ("rotation y: " + rotation.y);
				PlayerPrefs.SetFloat("PLAYER_ROTATION_Y", rotation.y);
				print ("rotation z: " + rotation.z);
				PlayerPrefs.SetFloat("PLAYER_ROTATION_Z", rotation.z);
				
				//	Getting state from controller
				string playerCapability = seraphController.capability.ToString();
				print ("Capability: " + playerCapability);
				PlayerPrefs.SetString("PLAYER_CAPABILITY", playerCapability);

				string playerState = seraphController.state.ToString();
				print ("State: " + playerState);
				PlayerPrefs.SetString("PLAYER_STATE", playerState);

				//	Not sure if I need this right now
				//	Depending on the state, get various properties from player
				switch(seraphController.state)
				{
				case cst.Flight.SeraphState.FLYING:
					
					break;
				case cst.Flight.SeraphState.GLIDING:
					
					break;
				case cst.Flight.SeraphState.GROUNDED:
					
					break;
				case cst.Flight.SeraphState.LANDING:
					
					break;
				}

				//	Need to commit changes
				PlayerPrefs.Save();
			}
		}
	}

	void loadData()
	{
		//	Check if data has been saved
		if(PlayerPrefs.GetInt("DATA_SAVED") == 1)
		{
            //	If data has been saved, load data and reset flag
			Camera camera = (Camera)FindObjectOfType(typeof(Camera));
			if(camera != null)
			{
				cst.Flight.SeraphController seraphController = camera.GetComponent<cst.Flight.SeraphController>();
				if(seraphController != null)
				{
					//	Loading player position
					Vector3 position = new Vector3(PlayerPrefs.GetFloat("PLAYER_POSITION_X"),
					                               PlayerPrefs.GetFloat("PLAYER_POSITION_Y"),
					                               PlayerPrefs.GetFloat("PLAYER_POSITION_Z"));
					seraphController.transform.position = position;

					//	Loading player rotation
					Quaternion rotation = new Quaternion(PlayerPrefs.GetFloat("PLAYER_ROTATION_X"),
					                                     PlayerPrefs.GetFloat("PLAYER_ROTATION_Y"),
					                                     PlayerPrefs.GetFloat("PLAYER_ROTATION_Z"),
					                                     PlayerPrefs.GetFloat("PLAYER_ROTATION_W"));
					seraphController.transform.rotation = rotation;

					//	Loading player prefs
					seraphController.state = (cst.Flight.SeraphState)System.Enum.Parse(typeof(cst.Flight.SeraphState), PlayerPrefs.GetString("PLAYER_STATE"));
					seraphController.capability = (cst.Flight.SeraphCapability)System.Enum.Parse(typeof(cst.Flight.SeraphCapability), PlayerPrefs.GetString("PLAYER_CAPABILITY"));
				}
			}

			PlayerPrefs.SetInt("DATA_SAVED", 0);
		}
	}

	void OnApplicationQuit()
	{
		//	Clear position based UserPrefs  
	}
}
