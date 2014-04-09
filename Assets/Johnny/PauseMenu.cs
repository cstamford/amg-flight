/*
 * File: PauseMenu.cs
 * 
 * Version: 1.2
 * 
 * Author: Johnathon Forster
 * 
 * Description: 
 * 		When the user presses the 'p' key the game will slow down over the course of several seconds
 * 		Scene is loaded asynchronously while the game slows to nothing and fades to the specified colour;
 * 		When the game has slowed to nothing the scene "Menu Scene" will be loaded.
 * 		Seraph position, orientation and state will be stored using playerPrefs and restored when un-pausing.
 */

using UnityEngine;
using System.Collections;
using cst.Flight;

public class PauseMenu : MonoBehaviour
{
	public string menuSceneName = "Menu";
	public Color fadeColour = new Color(1, 1, 1, 0);

	public bool loadAsync = true;

	enum PauseState { playing, pausing, paused };
	private PauseState pauseState;

	//	Time taken for game to pause
	private float pauseTime;
	//	Time passed since game was paused
	private float elapsedTime;
	private float prevTime;

	private AsyncOperation async;

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
		if(pauseState == PauseState.pausing)
		{
			elapsedTime += time;
			if(elapsedTime >= pauseTime)
			{
				PlayerPrefs.SetInt("PAUSING", 1);
				saveData();
				//pauseState = PauseState.paused;

				//pause ();
				if(loadAsync == true)
				{
					activateScene();
				} else {
					Application.LoadLevel(menuSceneName);
				}
				//Application.LoadLevelAdditive("Menu Scene");

                //Time.timeScale = 1.0f;
				return;
			}
			Time.timeScale = 1.0f - (elapsedTime/pauseTime);
			fadeColour.a = 1.0f - Time.timeScale;
		}

		//	Detecting key presses
		if(Input.GetKeyDown(KeyCode.P))
		{
			if(pauseState == PauseState.playing){
				pauseState = PauseState.pausing;
				if(loadAsync == true) startLoading();
				print ("Pausing");
			}
		}
	}

	void OnGUI()
	{
		if (pauseState == PauseState.pausing) {
			GUI.color = fadeColour;
			Texture2D texture = new Texture2D (1, 1);
			texture.SetPixel (0, 0, fadeColour);
			texture.Apply ();
			GUI.skin.box.normal.background = texture;
			GUI.Box (new Rect (0, 0, Screen.width, Screen.height), GUIContent.none);
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
		//	Search for an object tagged "Player"
		SeraphController seraphController = (SeraphController)GameObject.FindWithTag("Player").GetComponent<SeraphController>();
		
		//	Setting a flag to signify that there's data saved
		PlayerPrefs.SetInt("DATA_SAVED", 1);
		print ("Data saved");

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

		//	Need to commit changes
		PlayerPrefs.Save();
	}

	void loadData()
	{
		//	Check if data has been saved
		SeraphController seraphController = (SeraphController)GameObject.FindWithTag("Player").GetComponent<SeraphController>();
		if (seraphController != null) {
			print ("Load called");
			if (PlayerPrefs.GetInt ("DATA_SAVED") == 1) {
				//	Loading player position
				Vector3 position = new Vector3 (PlayerPrefs.GetFloat ("PLAYER_POSITION_X"),
                       PlayerPrefs.GetFloat ("PLAYER_POSITION_Y"),
                       PlayerPrefs.GetFloat ("PLAYER_POSITION_Z"));
				seraphController.transform.position = position;
			}
			//	Loading player rotation
			Quaternion rotation = new Quaternion (PlayerPrefs.GetFloat ("PLAYER_ROTATION_X"),
                     PlayerPrefs.GetFloat ("PLAYER_ROTATION_Y"),
                     PlayerPrefs.GetFloat ("PLAYER_ROTATION_Z"),
                     PlayerPrefs.GetFloat ("PLAYER_ROTATION_W"));
			seraphController.transform.rotation = rotation;

			//	Loading player prefs
			seraphController.state = (cst.Flight.SeraphState)System.Enum.Parse(typeof(cst.Flight.SeraphState), PlayerPrefs.GetString("PLAYER_STATE"));
			seraphController.capability = (cst.Flight.SeraphCapability)System.Enum.Parse(typeof(cst.Flight.SeraphCapability), PlayerPrefs.GetString("PLAYER_CAPABILITY"));

			PlayerPrefs.SetInt ("DATA_SAVED", 0);
		}
	}

	//	This is required for ASyncLoading

	public void startLoading() {
		StartCoroutine("load");
	}
	
	IEnumerator load() {
		Debug.LogWarning("ASYNC LOAD STARTED - " +
		                 "DO NOT EXIT PLAY MODE UNTIL SCENE LOADS... UNITY WILL CRASH");
		async = Application.LoadLevelAsync(menuSceneName);
		async.allowSceneActivation = false;
		yield return async;
	}
	
	public void activateScene() {
		async.allowSceneActivation = true;
	}

	void OnApplicationQuit()
	{
		//	Clear position based UserPrefs
		PlayerPrefs.DeleteAll();
		PlayerPrefs.Save();
	}
}

/*
 *		Version History:
 *			1.0: File was created
 *			1.1: Added colour fading ad time fading options within inspector
 *			1.2: Added asynchronous loading
 */