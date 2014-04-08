//
// Filename : LightbeamController.cs
// Author : Louis Dimmock
// Date : 4th April 2014
//
// Version : 1.2
// Version Info : 
//		Further improved visuals based on feedback.
//
// Previous Versions:
//		1.1 : Added more functionality. Opacity reduces as the player gets closer.
// 		1.0 : Simple script that controls the relic lightbeam. 
//			  Applys a texture offset every frame to provide visuals.
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
		public float m_offsetSpeed = 0.0f;

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
			CalculateAlpha();
		}

		private void ApplyOffset()
		{
			// Translate the texture by the specified amount
			m_textureOffset.x += m_offsetSpeed;
			
			// Keep the texture offset between 0 and 1
			if (m_textureOffset.y > 1.0f)
				m_textureOffset.y -= 1.0f;
			
			renderer.material.SetTextureOffset ("_MainTex", m_textureOffset);
		}

		private void CalculateAlpha()
		{
			// Calculate how much to increase the alpha per unit
			float initialAlphaIncrement = 0.75f / m_initialDistance;

			// Calculate the alpha based on the current distance between player and relic
			float Alpha = (initialAlphaIncrement * m_currentDistance);

			// Cap the alpha so it remains between 0 and 1
			if (Alpha > 1.0f)
				Alpha = 1.0f;

			// Set the color tints alpha
			m_colorTint.a = Alpha;

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