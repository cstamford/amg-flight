/*
 * 		File: PersistThroughScenes.cs
 * 
 * 		Version: 1.0
 * 
 * 		Author: Johnathon Forster
 * 
 * 		Description:
 * 				Objects with this script attached will not be destroyed when a new scene is loaded
 * 				This allows the state of objects to persist through scene transitions
 */

using UnityEngine;
using System.Collections;

public class PersistThroughScenes : MonoBehaviour {

	void Awake () {
		DontDestroyOnLoad(transform.gameObject);
	}
}
