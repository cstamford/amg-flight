using Louis.Controllers;
using UnityEngine;
using System.Collections;

namespace Louis.Controller
{
	public class SeraphController : MonoBehaviour
	{
		enum SeraphState
		{
			WALK,
			FLYING
		};

		// Define our controllers for the player to use
		private WalkController m_walkController;
		private FlightController m_flightController;

		// State that our seraph is currently in
		private SeraphState m_seraphState;

		// Seraph transforms
		private Vector3 m_position;
		private Vector3 m_rotation;

		// Collision variables
		private float m_playerHeight = 3.0f;
		private float m_colliderHeight = 0.0f;
		private float m_respawnHeight = -50.0f;
		private bool m_isGrounded = false;
		private bool m_hasWings = false;
        
        // Use this for initialization
		void Start ()
		{
			// Create controllers for walking and flying
			m_walkController = new WalkController();
			m_flightController = new FlightController();

			// Load the seraphs starting position
			m_position = transform.position;
			m_rotation = transform.eulerAngles;

			// Retrieve the size of the player collider box
			m_colliderHeight = collider.bounds.extents.y;
        }
		
		// Update is called once per frame
		void Update ()
		{
			switch( m_seraphState )
			{
				case SeraphState.WALK:
					m_walkController.Update();
					break;

				case SeraphState.FLYING:
					m_flightController.Update ();
					break;
			}

			CheckGrounded();
		}

		private void CheckGrounded()
		{
			// Check if we are colliding with the ground beneath us
			m_isGrounded = Physics.Raycast (transform.position, -Vector3.up, m_colliderHeight + 0.1f);

			// If we are
			if( m_isGrounded )
			{
				// Set our state to walking
				m_seraphState = SeraphState.WALK;
			}
			else
			{
				// Set our state to flying
				m_seraphState = SeraphState.FLYING;
			}
		}

		// Handle collisions
		void OnTriggerEnter(Collider otherObj)
		{
			// If we collide with the wings
			if (otherObj.gameObject.tag == "Wings")
			{
				// Destroy them in the scene
				Destroy (otherObj.gameObject);   
				
				// Flag that we have the wings
				m_hasWings = true;
	            
	            Debug.Log ("Flight Mode Activated");
	        }
	    }
	}
}
