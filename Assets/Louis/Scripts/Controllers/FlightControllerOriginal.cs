//
// Filename: FlightController.shader
// Author: Louis Dimmock
// Date : 16th January 2014
//
// Current Version: 1.0
// Version information : 
//		Provides simple flying functionality.
// 		Allows banking left and right.
//

using UnityEngine;

public class FlightControllerOriginal : MonoBehaviour 
{
	// Define our states for flying directions
    private enum YawDirection
    {
        NONE = 0,
        LEFT,
        RIGHT
    }

    private enum PitchDirection
    {
        NONE = 0,
        UP,
        DOWN
    }

    // Inspector fields
    [SerializeField] private float m_restingSpeed = 7.5f;
    [SerializeField] private float m_maxSpeed = 12.5f;
    [SerializeField] private float m_maxRollAngle = 37.5f;
    [SerializeField] private float m_maxPitchAngle = 65.0f;
    [SerializeField] private float m_turnTightness = 2.0f;
    [SerializeField] private float m_incrementTurnSpeed = 1.0f;
    [SerializeField] private float m_returnTurnSpeed = 0.75f;
    [SerializeField] private float m_incrementPitchSpeed = 1.0f;
    [SerializeField] private float m_returnPitchSpeed = 0.75f;

    // Private variables
    private Vector3 m_position = new Vector3(0.0f, 0.0f, 0.0f);
    private Vector3 m_rotation = new Vector3(0.0f, 0.0f, 0.0f);
    private float m_forwardSpeed;

	public void Start() 
    {
		// Retrieve our current transforms
        m_position = transform.position;
        m_rotation = transform.eulerAngles;
		
		// Set our forward momentum
        m_forwardSpeed = m_restingSpeed;
	}

	public void Update()
	{		
		HandleOrientation();
		HandleMoveForward();
	}
	
	public void SetPosition(Vector3 position)
	{ 
		m_position = position;
	}
	
	public void SetRotation(Vector3 rotation)
	{ 
		m_rotation = rotation;
	}

	public Vector3 getPosition()
	{
		return m_position;
	}

	public Vector3 getRotation()
	{
		return m_rotation;
	}

    private void HandleOrientation()
    {
		// Handle X-axis rotation
        HandlePitch();
		
		// Handle Y-axis rotation
        HandleYaw();
		
		// Handle Z-axis rotation
        HandleRoll();
    }

    private void HandlePitch()
    {
		// Check which keys have been pressed
        bool wDown = Input.GetKey(KeyCode.W);
        bool sDown = Input.GetKey(KeyCode.S);

		// If either pitch key has been pressed
        if (wDown || sDown)
        {
            if (wDown)
            {
				// Fly downwards
                TurnVertical(PitchDirection.DOWN);
            }
            else if (sDown)
            {
				// Fly upwards
                TurnVertical(PitchDirection.UP);
            }
        }
        else
        {
			// Return to normal
            TurnVertical(PitchDirection.NONE);
        }
    }

    // Handles the change in yaw based on the current roll
    private void HandleYaw()
    {
        m_rotation.y = WrapAngle(m_rotation.y - (Time.deltaTime * m_rotation.z * m_turnTightness));
    }

    // Handles the change in roll based on the user input
    private void HandleRoll()
    {
		// Check for key presses
        bool aDown = Input.GetKey(KeyCode.A);
        bool dDown = Input.GetKey(KeyCode.D);

		// If the turn left or turn right key is pressed
        if (aDown || dDown)
        {
            if (aDown)
            {
				// Bank left
                TurnHorizontal(YawDirection.LEFT);
            }
            else if (dDown)
            {
				// Bank right
                TurnHorizontal(YawDirection.RIGHT);
            }
        }
        else
        {
			// Return to normal
            TurnHorizontal(YawDirection.NONE);
        }
    }

    // Handles updating the rotation based on vertical input (up/down)
    private void TurnVertical(PitchDirection direction)
    {
        switch (direction)
        {
            case PitchDirection.UP:
				TurnVerticalUp();
				break;
            case PitchDirection.DOWN:
				TurnVerticalDown();
				break;
            case PitchDirection.NONE:
				TurnVerticalNone();
				break;
        }
    }

    // Handles updating the rotation based on horizontal input (left/right)
    private void TurnHorizontal(YawDirection direction)
    {
        switch (direction)
        {
            case YawDirection.LEFT:
				TurnHorizontalLeft();
				break;
            case YawDirection.RIGHT:
				TurnHorizontalRight();
				break;
            case YawDirection.NONE:
				TurnHorizontalNone();
				break;
        }
    }

    private void TurnVerticalUp()
    {
		// TODO
    }

    private void TurnVerticalDown()
    {
		// TODO
    }

    private void TurnVerticalNone()
    {
        // TODO
    }

    private void TurnHorizontalLeft()
    {
		// Rotate left but make sure we dont go over the limit
        m_rotation.z = high(m_rotation.z + (Time.deltaTime * m_incrementTurnSpeed * 25.0f), m_maxRollAngle);
    }

    private void TurnHorizontalRight()
    {
		// Rotate right but make sure we dont go over the limit
        m_rotation.z = low(m_rotation.z - (Time.deltaTime * m_incrementTurnSpeed * 25.0f), -m_maxRollAngle);
    }

    private void TurnHorizontalNone()
    {
		// If we are currently banking right
        if (m_rotation.z > 0.0f)
        {
			// Bank left back to normal
            m_rotation.z -= Time.deltaTime * m_returnTurnSpeed * 25.0f;

			// Stop rotating once we are back at resting
            if (m_rotation.z < 0.0f)
                m_rotation.z = 0.0f;
        }
        else
        {
			// Bank left back to normal
            m_rotation.z += Time.deltaTime * m_returnTurnSpeed * 25.0f;

			// Stop rotating once we are back at resting
            if (m_rotation.z > 0.0f)
                m_rotation.z = 0.0f;
        }
    }

    private void HandleMoveForward()
    {
        // Update the forward speed movement based on the frame time
        m_forwardSpeed += Time.deltaTime / 2.0f;

        // Cap the forward speed so we cant go too fast
        if (m_forwardSpeed > m_maxSpeed)
            m_forwardSpeed = m_maxSpeed;

		// Limit the forward speed so we cant go backwards
        if (m_forwardSpeed < 0.0f)
            m_forwardSpeed = 0.0f;

		// Translate us forward in the direction we are facing
        m_position += transform.forward * m_forwardSpeed;
    }

    // Wraps a value between positive numbers lowBound and highBound
    private float WrapValue(float value, float lowBound, float highBound)
    {
        if (highBound < lowBound)
            return -1.0f;

        while (value < lowBound)
        {
            value += highBound;
        }

        while (value > highBound)
        {
            value -= highBound;
        }

        return value;
    }

    // Wraps between 0 ... 360
    private float WrapAngle(float value)
    {
        return WrapValue(value, 0.0f, 360.0f);
    }

    // Clamp a value between lowBound and highBound;
    private float ClampValue(float value, float lowBound, float highBound)
    {
        value = high(value, highBound);
        value = low(value, lowBound);

        return value;
    }

    // Returns value, or bound if value is greater than bound
    private float high(float value, float bound)
    {
        return value > bound ? bound : value;
    }

    // Returns value, or bound if value is less than bound
    private float low(float value, float bound)
    {
        return value < bound ? bound : value;
    }
}
