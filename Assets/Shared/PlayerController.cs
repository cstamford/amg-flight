//===========================================================================\\
// Author : Louis Dimmock													 \\
// Created : January 31st 2014												 \\
// Version : 1.0															 \\
//																			 \\
// Description:	This file controls how the player moves when not flying. 	 \\
//===========================================================================\\

using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    public bool m_isFlying = false;

	// Vectors to store player information
	private Vector3 m_position = new Vector3(0.0f, 0.0f, 0.0f);
	private Vector3 m_rotation = new Vector3(0.0f, 0.0f, 0.0f);
	private Vector3 m_velocity = new Vector3(0.0f, 0.0f, 0.0f);
	private Vector3 m_turnSpeed = new Vector3(0.0f, 0.0f, 0.0f);
	
	// Define how much to increase speeds by
    [SerializeField] private float m_forwardWalkSpeed = 50.0f;
	[SerializeField] private float m_backwardWalkSpeed = 50.0f;
	[SerializeField] private float m_frictionAmount = 50.0f;
	[SerializeField] private float m_strafeWalkSpeed = 50.0f;
	[SerializeField] private float m_incrementWalkTurnSpeed = 3.0f;
	[SerializeField] private float m_fallSpeed = 20.0f;
	
	// Define the maximum values
    [SerializeField] private float m_maxForwardWalkSpeed = 150.0f;
	[SerializeField] private float m_maxBackwardWalkSpeed = -150.0f;
	[SerializeField] private float m_maxWalkStrafeSpeed = 150.0f;
	[SerializeField] private float m_maxTurnSpeed = 65.0f;
	[SerializeField] private float m_maxFallSpeed = -100.0f;
	[SerializeField] private float m_maxPitch = 45.0f;
	
	// Collision variables
    [SerializeField] public float m_playerHeight = 20.0f;
	[SerializeField] private float m_colliderHeight = 0.0f;
    [SerializeField] private float m_respawnHeight = -6000.0f;

    // Delta time
    private float m_deltaTime = 0.0f;

	private bool m_isGrounded = false;
	private bool m_hasWings = false;
	
	// Key checks (Makes the code easier to read later on)
	private bool m_wKey = false;
	private bool m_sKey = false;
	private bool m_aKey = false;
	private bool m_dKey = false;
	private bool m_upKey = false;
	private bool m_downKey = false;
	private bool m_leftKey = false;
	private bool m_rightKey = false;

    private FlightMovement m_flight;

	public void Start ()
	{
		// Take a copy of the cameras position
		m_position = transform.position;
		m_rotation = transform.eulerAngles;
		
		// Grab the size of the player collider box
		m_colliderHeight = collider.bounds.extents.y;

        m_flight = GetComponent("FlightMovement") as FlightMovement;
	}

    public void Update()
	{
        if (m_isFlying)
            return;

        m_rotation = transform.eulerAngles;
        m_position = transform.position;

		// Get the delta time
		m_deltaTime = Time.deltaTime;

		// Get the status of each of the keys used by movement
		HandleKeys();

		// Handle the arrow keys
		HandleTurning();

		// Handle the movement keys
		HandleMovement();

        if (m_flight != null)
        {
            m_flight.SetRotation(m_rotation);
            m_flight.SetPosition(m_position);
        }
	}

	// Handle collisions
	public void OnTriggerEnter(Collider otherObj)
	{
		if (otherObj.gameObject.tag == "Wings")
		{
			Destroy (otherObj.gameObject);   
			m_hasWings = true;
			Debug.Log ("Flight Mode Activated");
		}
	}

	// Main function to handle rotation in the axes
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
			m_turnSpeed.y -= m_incrementWalkTurnSpeed * m_deltaTime;
			
			// Cap the maximum turn speed
			LimitValue(ref m_turnSpeed.y, -m_maxTurnSpeed * m_deltaTime, 0);
		}
		else if( m_rightKey ) // Turn right
		{
			// Increase turn speed
			m_turnSpeed.y += m_incrementWalkTurnSpeed * m_deltaTime;
			
			// Cap the maximum turn speed
			LimitValue(ref m_turnSpeed.y, 0, m_maxTurnSpeed * m_deltaTime);
		}
		else
		{
			m_turnSpeed.y = 0.0f;
		}
		
		// Rotate the camera 
		m_rotation.y += m_turnSpeed.y;
		
		// Make sure the rotation stays within the boundaries of 0 and 360
		if(m_rotation.y < 0.0f)
			m_rotation.y += 360.0f;
		else if(m_rotation.y > 360.0f)
			m_rotation.y -= 360.0f;
	}

	// Handle X-axis rotation
	private void HandlePitch()
	{
		if( m_upKey ) // Look up
		{
			// Increase turn speed
			m_turnSpeed.x -= m_incrementWalkTurnSpeed * m_deltaTime;
		}
		else if( m_downKey ) // Look down
		{
			// Increase turn speed
			m_turnSpeed.x += m_incrementWalkTurnSpeed * m_deltaTime;
		}
		else
		{
			// Stop turning
			m_turnSpeed.x = 0.0f;
		}
		// Rotate the camera 
		m_rotation.x += m_turnSpeed.x;
	}

	// Main function for handling movement in the axes
	private void HandleMovement()
	{
		// Handle movement in the z-axis
		HandleWalking(); 
		
		// Handle movement in the x-axis
		HandleStrafing();
		
		// Handle falling in the y-axis
		HandleFalling();

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
			if(m_velocity.z < 0)
			{
				m_velocity.z += m_forwardWalkSpeed * 5 * m_deltaTime;
			}
			else
			{
				m_velocity.z += m_forwardWalkSpeed * m_deltaTime;
			}
			// Keep the velocity with the maximum speed boundaries so we dont travel too fast
			LimitValue(ref m_velocity.z, -m_maxForwardWalkSpeed * m_deltaTime, m_maxForwardWalkSpeed * m_deltaTime);
		}
		else if( m_sKey ) // Move backward
		{
			if(m_velocity.z > 0)
			{
				m_velocity.z -= m_backwardWalkSpeed * 5 * m_deltaTime;
			}
			else
			{
				m_velocity.z -= m_backwardWalkSpeed * m_deltaTime;
			}
			
			// Keep the velocity with the maximum speed boundaries so we dont travel too fast
			LimitValue(ref m_velocity.z, m_maxBackwardWalkSpeed * m_deltaTime, -m_maxBackwardWalkSpeed * m_deltaTime);
		}
		else // Reduce speed 
		{
			if( m_velocity.z < 0.0f )
			{
				// Moving backwards so increase velocity to slow down
				m_velocity.z += m_frictionAmount * m_deltaTime;
				// Stop the velocity from reversing
				if(m_velocity.z > 0.0f)
					m_velocity.z = 0.0f;
			}
			else if( m_velocity.z > 0.0f )
			{
				// Moving forwards so decrease velocity to slow down
				m_velocity.z -= m_frictionAmount * m_deltaTime;
				// Stop the velocity from reversing
				if(m_velocity.z < 0.0f)
					m_velocity.z = 0.0f;
			}
		}
	}

	// Handle movement in the X-axis
	private void HandleStrafing()
	{
		if(m_aKey) // Strafe left
		{
			// Increase movement speed
			if(m_velocity.x > 0)
			{
				// Reduce speed quicker so we can start moving in other direction
				// We use this so we dont just suddenly stop
				// Needs testing with the oculus rift
				m_velocity.x = 0;
			}
			else
			{
				m_velocity.x -= m_strafeWalkSpeed * m_deltaTime;
			}
		}
		else if(m_dKey) // Strafe right
		{
			// Increase movement speed
			if(m_velocity.x < 0)
			{
				// Reduce speed quicker so we can start moving in other direction
				// We use this so we dont just suddenly stop
				// Needs testing with the oculus rift
				m_velocity.x += m_strafeWalkSpeed * 5 * m_deltaTime;
			}
			else
			{
				m_velocity.x += m_strafeWalkSpeed * m_deltaTime;
			}
		}
		else
		{
			// If we are moving backwards
			if( m_velocity.x < 0.0f )
			{
				// Bring the velocity back towards zero
				m_velocity.x += m_frictionAmount * m_deltaTime;
				
				// Stop the velocity from reversing
				if(m_velocity.x > 0.0f)
					m_velocity.x = 0.0f;
			}
			else if( m_velocity.x > 0.0f ) // If we are moving forwards
			{
				// Bring the velocity back towards zero
				m_velocity.x -= m_frictionAmount * m_deltaTime;
				
				// Stop the velocity from reversing
				if(m_velocity.x < 0.0f)
					m_velocity.x = 0.0f;
			}
		}
		
		// Keep the velocity within two boundaries so we dont travel too fast
		LimitValue(ref m_velocity.x, -m_maxWalkStrafeSpeed * m_deltaTime, m_maxWalkStrafeSpeed * m_deltaTime);
	}

	// Handle movement in the Y-axis
	private void HandleFalling()
	{
		// Check if we are colliding with the ground
		m_isGrounded = Physics.Raycast (transform.position, -Vector3.up, m_colliderHeight + m_playerHeight + 0.1f);

        Debug.Log(m_isGrounded);
		
		// If we are on the ground
		if(m_isGrounded)
		{
			if(m_isFlying)
			{
				m_isFlying = false;
			}
			else
			{
				// Disable falling
				m_velocity.y = 0.0f;
                m_hasWings = true;
			}
		}
		else // We are falling
		{
			if(m_hasWings)
			{
				// Enable flying
				m_isFlying = true;
				Debug.Log ("I can flyyyyyyyyyyyyyyy");
			}
			else
			{
				// Apply gravity
				HandleGravity();
			}
		}
	}

	// Function to apply gravity if we dont have wings
	private void HandleGravity()
	{
		// Apply gravity
		m_velocity.y -= m_fallSpeed * m_deltaTime;
		
		// Make sure we dont fall too fast
		if(m_velocity.y < m_maxFallSpeed * m_deltaTime)
			m_velocity.y = m_maxFallSpeed * m_deltaTime;		
	}

	// Apply the new translations our position and update the camera
	private void HandlePosition()
	{
		// Move forward in the direction we are facing
		// We dont want Y to change otherwise we have problems!
		m_position.x += transform.forward.x * m_velocity.z;
		m_position.z += transform.forward.z * m_velocity.z;
		
		// Move right in the direction to our right
		m_position += transform.right * m_velocity.x;
		
		// Move up and down 
		m_position += transform.up * m_velocity.y;
		
		// Check if we have fallen to our death
		if (m_position.y < m_respawnHeight)
		{
			m_position = new Vector3(0.0f, 15.0f, 0.0f);
			m_rotation = new Vector3(0.0f, 0.0f, 0.0f);
			Debug.Log("You fell to your death.");
		}
	}

	//
	// General use functions
	// 

	// Function to keep a value between a minimum and a maximum
	private void LimitValue(ref float value, float min, float max)
	{
		// Value is less than the minimum value it can go
		if (value < min)
		{
			value = min;
		}
		// Value is greater than the minimum value it can go
		else if (value > max)
		{
			value = max;
		}
		else
			return;
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