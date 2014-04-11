//
// Filename : WaypointController.cs
// Author : Louis Dimmock
// Date : 9th April 2014
//
// Version : 1.0
// Version Info : 
//		An altered version LightbeamController.cs
//		Modified the alpha calculations so that the opacity increases as the player nears
//		Added an inspector field so a bounding area can be applied
//		Removed the ability to animate a texture
//

using Louis.Common;
using UnityEngine;
using System.Collections;

namespace Louis.Waypoint
{
	public class WaypointController : MonoBehaviour
	{
		// Color of the lightbeam
		public Color m_colorTint = new Color(1.0f, 1.0f , 1.0f, 0.0f);

		// The distance before the beam starts to become visible
		public float m_minDistance = 75.0f;

		// Variable for accessing the player
		private GameObject m_player;

		// Position of the relic
		private Vector3 m_waypointPosition;

		// Position of the player
		private Vector3 m_playerPosition;

		// How far the player is at the start of the level
		private float m_initialDistance = 0.0f;

		// How far the player is from the relic at the time of the update
		private float m_currentDistance = 0.0f;

		// Use this for initialization
		void Start ()
		{
			// Store the relics position
			m_waypointPosition = transform.position;

			// Find the player so we can access it
			m_player = GameObject.FindGameObjectWithTag ("Player");

			// Retrieve the latest player position
			m_playerPosition = m_player.transform.position;
		
			// Calculate the distance between player and relic starting positions
			CalculateDistance(ref m_initialDistance );
		}
		
		// Update is called once per frame
		void Update ()
		{	
			// Calculate the distance between the player and the relic
			CalculateDistance( ref m_currentDistance );

			// Apply color changes based on distance
			CalculateAlpha();
		}

		private void CalculateAlpha()
		{
			// Calculate how much to increase the alpha per unit
			float initialAlphaIncrement = 1.0f / m_minDistance;
	
			float Alpha = 0.0f;

			// Calculate the alpha based on the current distance between player and waypoint start
			if( m_currentDistance <= m_minDistance )
				Alpha = 0.5f - (initialAlphaIncrement * m_currentDistance);

			// Cap the alpha so it remains between 0 and 1
			Helper.Limit(ref Alpha, 0.0f, 1.0f);

			// Set the color tints alpha
			// We want it to be opaque when we are near it
			// We want to be transparent if we are no where near it
			m_colorTint.a = Alpha;

			// Apply it to the alpha
			renderer.material.SetColor ("_Color", m_colorTint);
		}

		private void CalculateDistance(ref float distance)
		{
			// Retrieve the latest player position
			m_playerPosition = m_player.transform.position;
			
			// Calculate the differences between the relic position and the player position
			float xDistance = m_playerPosition.x - m_waypointPosition.x; 
			float yDistance = m_playerPosition.y - m_waypointPosition.y; 
			float zDistance = m_playerPosition.z - m_waypointPosition.z; 

			// Square those differences
			float xSquared = xDistance * xDistance;
			float ySquared = yDistance * yDistance;
			float zSquared = zDistance * zDistance;

			// Use pythagoras to calculate the distance
			distance = Mathf.Sqrt( xSquared + ySquared + zSquared );
		}
	}
}