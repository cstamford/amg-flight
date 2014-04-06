//
// Filename : LightbeamController.cs
// Author : Louis Dimmock
// Date : 4th April 2014
//
// Version : 1.1
// Version Info : 
//		Added more functionality
//		Opacity reduces as the player gets closer
//
// Previous Versions:
// 		1.0 : Simple script that controls the relic lights and applys a texture offset
//

using UnityEngine;
using System.Collections;

namespace Louis.Relics
{
	public class LightbeamController : MonoBehaviour
	{
		// Color of the lightbeam
		public Color m_colorTint = new Color(1.0f, 1.0f , 1.0f, 1);

		// Set up the speed that the texture will move at
		public float m_movementSpeed = 0.0f;

		// Total texture offset
		private Vector2 m_textureOffset = new Vector2(0.0f, 0.0f);
		
		// Variable for accessing the player
		private GameObject m_player;

		// Position of the relic
		private Vector3 m_relicPosition;

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
			m_relicPosition = transform.parent.transform.position;

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
			// Animate the texture
			ApplyOffset();

			// Calculate the distance between the player and the relic
			CalculateDistance( ref m_currentDistance );

			// Apply color changes based on distance
			ApplyDistanceChanges ();
		}

		private void ApplyOffset()
		{
			// Translate the texture by the specified amount
			m_textureOffset.x += m_movementSpeed;
			
			// Keep the texture offset between 0 and 1
			if (m_textureOffset.y > 1.0f)
				m_textureOffset.y -= 1.0f;
			
			renderer.material.SetTextureOffset ("_MainTex", m_textureOffset);
		}

		private void ApplyDistanceChanges()
		{
			// Calculate how much to decrease alpha by
			float initialAlphaIncrement = 0.5f / m_initialDistance;

			// Calculate the current alpha
			float alpha = (initialAlphaIncrement * m_currentDistance);

			// Cap the alpha so it remains between 0 and 1
			if (alpha > 1.0f)
				alpha = 1.0f;

			// Set the color tints alpha
			m_colorTint.a = alpha;

			// Apply it to the alpha
			renderer.material.SetColor ("_Color", m_colorTint);
		}

		private void CalculateDistance(ref float distance)
		{
			// Retrieve the latest player position
			m_playerPosition = m_player.transform.position;
			
			// Calculate the differences between the relic position and the player position
			float xDistance = m_playerPosition.x - m_relicPosition.x; 
			float yDistance = m_playerPosition.y - m_relicPosition.y; 
			float zDistance = m_playerPosition.z - m_relicPosition.z; 

			// Square those differences
			float xSquared = xDistance * xDistance;
			float ySquared = yDistance * yDistance;
			float zSquared = zDistance * zDistance;

			// Use pythagoras to calculate the distance
			distance = Mathf.Sqrt( xSquared + ySquared + zSquared );
		}
	}
}

