/*
 *		File: InitialLoad.cs
 *
 *		Version: 1.0
 *
 *		Author: Johnathon Forster
 *
 *		Description:
 *			Removes saved PlayerPrefs objects that may persist if the application closes improperly	
 */

using UnityEngine;
using System.Collections;

public class InitialLoad : MonoBehaviour {

	// Use this for initialization
	void Start()
	{
		/*	Need to remove position based preferences as soon as the application opens,
		 * 	otherwise there's a chance of data not being removed if the application
		 *	crashes or is killed by the user with Task Manager et al. */

		//	Removing position prefs
		PlayerPrefs.DeleteKey("PLAYER_POSITION_X");
		PlayerPrefs.DeleteKey("PLAYER_POSITION_Y");
		PlayerPrefs.DeleteKey("PLAYER_POSITION_Z");

		//	Removing player prefs
		PlayerPrefs.DeleteKey("PLAYER_CAPABILITY");
		PlayerPrefs.DeleteKey("PLAYER_STATE");
	}

	void Awake()
	{
		//	Need to make sure this persists across all scenes otherwise it might be
		//		instantiated multiple times and will clear preferences improperly
		DontDestroyOnLoad(transform.gameObject);
	}
}
