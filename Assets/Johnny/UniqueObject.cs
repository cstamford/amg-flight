/*
 * 		File: UniqueObject.cs
 * 
 * 		Version: 1.0
 * 
 * 		Author: Johnathon Forster
 * 
 * 		Description:
 * 				Allows only one instance of an object with a specified script attached
 * 				On instantiation, if another object is detected, that object is copied and destroyed
 */

using UnityEngine;
using System.Collections;

public class UniqueObject : MonoBehaviour {

	//public Component uniqueComponent = cst.Flight.SeraphController;

	/*
	void Awake()
	{
		//	Check all objects in scene for UniqueComponent Component
		GameObject[] gameObjects = (GameObject[])FindObjectsOfType(typeof(GameObject));
		foreach(GameObject gameObject in gameObjects)
		{
			Component[] components = gameObject.GetComponents();
			foreach(Component component in components)
			{
				if(component == uniqueComponent)
				{
					//	Copy duplicate component
					this.GetComponent<uniqueComponent>() = gameObject.GetComponent<component>();
					DestroyObject(gameObject);
				}
			}
		}
	}
	*/
}
