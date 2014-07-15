//
// Filename : QuickRestart.cs
// Author : Louis Dimmock
// Date : 8th April 2014
// 
// Version : 1.0
// Version Info :
//		Rewritten script based on DoorTrigger.cs
//		Quickly restarts the game by reloading the scene.
//

using cst.Common;
using Action = cst.Common.Action;
using UnityEngine;
using System.Collections;

namespace Louis
{
	public class QuickRestart : MonoBehaviour
	{
		public Color m_fadeColor = new Color(1, 1, 1, 1);
		public float m_fadeTime = 2.0f;

		private bool m_isWarping = false;
		private float m_elapsedTime = 0.0f;
		private float m_prevTime = 0.0f;

		private InputManager m_inputManager;
		
		void Start()
		{
			// Retrieve how long its been since the level load
			m_prevTime = Time.realtimeSinceStartup;

			// Find the input manager
			m_inputManager = this.gameObject.GetComponent<InputManager>();

			// Check if the input manager was found
			if( m_inputManager == null)
			{
				Debug.Log("InputManager could not been found");
			}
		}
		
		void Update ()
		{
			if( m_inputManager.actionFired(Action.EXIT) )
			{
				m_isWarping = true;
			}
			CheckWarping ();
		}

		private void CheckWarping()
		{
			// Calculate how long its been since load up
			float TimeDifference = Time.realtimeSinceStartup - m_prevTime;
			m_prevTime = Time.realtimeSinceStartup;

			// Check if we are wanting to warp or not
			if( m_isWarping )
			{
				// Increase the time elapsed since warping began
				m_elapsedTime += TimeDifference;

				// Check if we should warp yet
				if( m_elapsedTime >= m_fadeTime)
				{
					// Reload the scene
					Application.LoadLevel( Application.loadedLevelName );
				}
				
				Time.timeScale = 1.0f - ( m_elapsedTime / m_fadeTime );
				
				m_fadeColor.a = m_elapsedTime / m_fadeTime;
			}
		}

		void OnGUI()
		{
			if ( m_isWarping )
			{
				GUI.color = m_fadeColor;
				Texture2D texture = new Texture2D (1, 1);
				texture.SetPixel (0, 0, m_fadeColor);
				texture.Apply ();
				GUI.skin.box.normal.background = texture;
				GUI.Box (new Rect (0, 0, Screen.width, Screen.height), GUIContent.none);
			}
		}
	}
}