//
// Filename : Quit.cs
// Author : Louis Dimmock
// Date : 8th April 2014
//
// Version : 1.0
// Version Info : 
// 		Simple script to quit the application when the EXIT button has been pressed 

using cst.Common;
using UnityEngine;
using System.Collections;
using Action = cst.Common.Action;

namespace Louis
{
	public class Quit : MonoBehaviour
	{
		// Define our input manager
		private InputManager m_inputManager;

		void Start ()
		{
			// Find the input manager
			m_inputManager = gameObject.GetComponent<InputManager>();
			
			// Check if the input manager was found
			if( m_inputManager == null)
			{
				Debug.Log("InputManager could not been found");
			}
		}

		void Update ()
		{
			// Check if the escape button has been pressed
			if( m_inputManager.actionFired(Action.EXIT) )
			{
				// Quit the application
				Application.Quit();
				Debug.Log("EXIT button pressed.");
			}
		}
	}
}
