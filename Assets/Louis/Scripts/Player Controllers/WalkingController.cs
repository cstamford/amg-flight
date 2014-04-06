//
// Filename: PlayerController.cs
// Author: Louis Dimmock
// Date : 31st January 2014
//
// Current Version: 1.0
// Version information : 
//		Provides simple movement functionality.
//		Allows the character to move around using the WASD keys
//		Allows the character to look around using the arrow keys
//		Allows the character to transition between walking and flying
//

using Louis.Common;
using UnityEngine;
using System.Collections;

namespace Louis.Controllers
{
	public class WalkController : MonoBehaviour
	{
		// Vectors to store player information
		private Vector3 m_position = new Vector3(0.0f, 0.0f, 0.0f);
		private Vector3 m_rotation = new Vector3(0.0f, 0.0f, 0.0f);
		private Vector3 m_velocity = new Vector3(0.0f, 0.0f, 0.0f);
		private Vector3 m_turnSpeed = new Vector3(0.0f, 0.0f, 0.0f);
		
		// Define how much to increase speeds by
		private float m_forwardWalkSpeed = 0.2f;
		private float m_backwardWalkSpeed = 0.075f;
		private float m_frictionAmount = 0.5f;
		private float m_strafeWalkSpeed = 0.15f;
		private float m_incrementWalkTurnSpeed = 3.0f;
		private float m_fallSpeed = 0.5f;
		
		// Define the maximum values
		private float m_maxForwardWalkSpeed = 10.0f;
		private float m_maxBackwardWalkSpeed = -5.0f;
		private float m_maxWalkStrafeSpeed = 7.0f;
		private float m_maxTurnSpeed = 65.0f;
		private float m_maxFallSpeed = -20.0f;
		private float m_maxPitch = 45.0f;
		
		// Key checks (Makes the code easier to read later on)
		private bool m_wKey = false;
		private bool m_sKey = false;
		private bool m_aKey = false;
		private bool m_dKey = false;
		private bool m_upKey = false;
		private bool m_downKey = false;
		private bool m_leftKey = false;
		private bool m_rightKey = false;

		public void Update()
		{
			// Get the status of each of the keys used by movement
			HandleKeys();

			// Handle looking
			HandleTurning();

			// Handle walking
			HandleMovement();
		}

		private void HandleTurning()
		{
			// Handle rotations 
			HandleYaw(); // Y-axis
			HandlePitch(); // X-axis
			
			// Update the camera with the new rotation
			transform.eulerAngles = m_rotation;
		}

		// Handle Y-axis rotation
		private void HandleYaw()
		{
			if( m_leftKey ) // Turn left
			{
				// Increase turn speed
				m_turnSpeed.y -= m_incrementWalkTurnSpeed * Time.deltaTime;
				
				// Cap the maximum turn speed
				Helper.Limit(ref m_turnSpeed.y, -m_maxTurnSpeed * Time.deltaTime, 0);
			}
			else if( m_rightKey ) // Turn right
			{
				// Increase turn speed
				m_turnSpeed.y += m_incrementWalkTurnSpeed * Time.deltaTime;
				
				// Cap the maximum turn speed
				Helper.Limit(ref m_turnSpeed.y, 0, m_maxTurnSpeed * Time.deltaTime);
			}
			else
			{
				// Reset turn speed
				m_turnSpeed.y = 0.0f;
			}
			
			// Rotate the camera 
			m_rotation.y += m_turnSpeed.y;
			
			// Make sure the rotation stays within the boundaries of 0 and 360
			Helper.Wrap( ref m_rotation.y, 0.0f, 360.0f );
		}

		// Handle X-axis rotation
		private void HandlePitch()
		{
			if( m_upKey ) // Look up
			{
				// Increase turn speed
				m_turnSpeed.x -= m_incrementWalkTurnSpeed * Time.deltaTime;
				
				// Cap the turn speed
				Helper.Limit(ref m_turnSpeed.x, -m_maxTurnSpeed * Time.deltaTime, 0);
			}
			else if( m_downKey ) // Look down
			{
				// Increase turn speed
				m_turnSpeed.x += m_incrementWalkTurnSpeed * Time.deltaTime;
				
				// Cap the turn speed
				Helper.Limit(ref m_turnSpeed.x, 0, m_maxTurnSpeed * Time.deltaTime);
			}
			else
			{
				// Stop turning
				m_turnSpeed.x = 0.0f;
			}
			
			// Rotate the camera 
			m_rotation.x += m_turnSpeed.x;
			
			// Stop the camera from tilting up/down too much
			// When we add support for OR, we shouldnt need this?
			// Value should be restricted based on users head angle
			Helper.Limit(ref m_rotation.x, -m_maxPitch, m_maxPitch);
		}

		// Main function for handling movement in the axes
		private void HandleMovement()
		{
			// Handle movement in the z-axis
			HandleWalking(); 
			
			// Handle movement in the x-axis
			HandleStrafing();

			// Apply transforms to the position
			HandlePosition();
			
			// Update the camera with new position
			transform.position = m_position;
		}

		// Handle movement in the Z-axis
		private void HandleWalking()
		{
			// Move forward
			if( m_wKey )
			{
				// If we are currently moving backwards
				if(m_velocity.z < 0)
				{
					// Reduce velocity faster
					m_velocity.z += m_forwardWalkSpeed * 5 * Time.deltaTime;
				}
				else
				{
					// Increase velocity
					m_velocity.z += m_forwardWalkSpeed * Time.deltaTime;
				}
				
				// Cap the walking velocity
				Helper.Limit(ref m_velocity.z, -m_maxForwardWalkSpeed * Time.deltaTime, m_maxForwardWalkSpeed * Time.deltaTime);
			}
			else if( m_sKey ) // Move backward
			{
				// If we are currently moving forward
				if(m_velocity.z > 0)
				{
					// Reduce speed quicker
					m_velocity.z -= m_backwardWalkSpeed * 5 * Time.deltaTime;
				}
				else
				{
					// Walk backwards
					m_velocity.z -= m_backwardWalkSpeed * Time.deltaTime;
				}
				
				// Cap the walking velocity
				Helper.Limit(ref m_velocity.z, m_maxBackwardWalkSpeed * Time.deltaTime, -m_maxBackwardWalkSpeed * Time.deltaTime);
			}
			else // Reduce speed 
			{
				// Currently walking backwards
				if( m_velocity.z < 0.0f )
				{
					// Moving backwards so increase velocity to slow down
					m_velocity.z += m_frictionAmount * Time.deltaTime;
					
					// Stop the velocity from reversing
					Helper.Limit(ref m_velocity.z, m_velocity.z, 0.0f);
				}
				else if( m_velocity.z > 0.0f )
				{
					// Moving forwards so decrease velocity to slow down
					m_velocity.z -= m_frictionAmount * Time.deltaTime;
					
					// Stop the velocity from reversing
					Helper.Limit(ref m_velocity.z, 0.0f, m_velocity.z);
				}
			}
		}

		// Handle movement in the X-axis
		private void HandleStrafing()
		{
			if(m_aKey) // Strafe left
			{
				// If we are currently strafing right
				if(m_velocity.x > 0)
				{
					// Reduce speed quicker so we can start moving in other direction
					// We use this so we dont just suddenly stop
					// Needs testing with the oculus rift
					m_velocity.x = 0;
				}
				else
				{
					// Strafe left
					m_velocity.x -= m_strafeWalkSpeed * Time.deltaTime;
				}
			}
			else if(m_dKey) // Strafe right
			{
				// If we are currently strafing left
				if(m_velocity.x < 0)
				{
					// Reduce speed quicker so we can start moving in other direction
					// We use this so we dont just suddenly stop
					// Needs testing with the oculus rift
					m_velocity.x += m_strafeWalkSpeed * 5 * Time.deltaTime;
				}
				else
				{
					// Strafe right
					m_velocity.x += m_strafeWalkSpeed * Time.deltaTime;
				}
			}
			else
			{
				// If we are currently strafing left
				if( m_velocity.x < 0.0f )
				{
					// Bring the velocity back towards zero
					m_velocity.x += m_frictionAmount * Time.deltaTime;
					
					// Stop the velocity from reversing
					Helper.Limit( ref m_velocity.x, m_velocity.x, 0.0f );
				}
				else if( m_velocity.x > 0.0f ) // If we are strafing right
				{
					// Bring the velocity back towards zero
					m_velocity.x -= m_frictionAmount * Time.deltaTime;
					
					// Stop the velocity from reversing
					Helper.Limit( ref m_velocity.x, 0.0f, m_velocity.x );
				}
			}
			
			// Keep the velocity within two boundaries so we dont travel too fast
			Helper.Limit(ref m_velocity.x, -m_maxWalkStrafeSpeed * Time.deltaTime, m_maxWalkStrafeSpeed * Time.deltaTime);
		}

		private void HandleGravity()
		{
			// Apply gravity
			m_velocity.y -= m_fallSpeed * Time.deltaTime;
			
			// Make sure we dont fall too fast
			Helper.Limit( ref m_velocity.y, m_maxFallSpeed * Time.deltaTime, m_velocity.y );
		}

		// Apply the new translations our position and update the camera
		private void HandlePosition()
		{
			// Move forward
			m_position.x += transform.forward.x * m_velocity.z;
			m_position.z += transform.forward.z * m_velocity.z;
			
			// Move left/right
			m_position += transform.right * m_velocity.x;
			
			// Fall
			m_position += transform.up * m_velocity.y;
		}

		// Function to grab the status of keys used by the file
		private void HandleKeys()
		{
			// Get the status of the movement keys
			m_wKey = Input.GetKey(KeyCode.W);
			m_sKey = Input.GetKey(KeyCode.S);
			m_aKey = Input.GetKey(KeyCode.A);
			m_dKey = Input.GetKey(KeyCode.D);
			
			// Get the status of the look keys
			m_upKey = Input.GetKey(KeyCode.UpArrow);
			m_downKey = Input.GetKey(KeyCode.DownArrow);
			m_leftKey = Input.GetKey(KeyCode.LeftArrow);
			m_rightKey = Input.GetKey(KeyCode.RightArrow);
		}
	}
}