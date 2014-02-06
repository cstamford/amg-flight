// Authors : Chris, Louis and Sean

using UnityEngine;

public class FlightControllerLouis : MonoBehaviour 
{
    private enum YawDirection
    {
        NONE= 0,
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
	private float m_deltaTime = 0.0f;

	// Initialise the flight variables
	public void Start() 
    {
        m_position = transform.position;
        m_rotation = transform.eulerAngles;
        m_forwardSpeed = m_restingSpeed;
	}

	public void Frame()
	{
		m_deltaTime = Time.deltaTime;
		
		handleOrientationChange();
		handleMoveForward();
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

    private void handleOrientationChange()
    {
        handlePitchChange();
        handleYawChange();
        handleRollChange();
    }

    // Handles the chagne in pitch based on the user input
    private void handlePitchChange()
    {
        bool wDown = Input.GetKey(KeyCode.W);
        bool sDown = Input.GetKey(KeyCode.S);

        if (wDown || sDown)
        {
            if (wDown)
            {
                turnVertical(PitchDirection.DOWN);
            }

            if (sDown)
            {
                turnVertical(PitchDirection.UP);
            }
        }
        else
        {
            turnVertical(PitchDirection.NONE);
        }
    }

    // Handles the change in yaw based on the current roll
    private void handleYawChange()
    {
        m_rotation.y = wrapAngle(m_rotation.y - (m_deltaTime * m_rotation.z * m_turnTightness));
    }

    // Handles the change in roll based on the user input
    private void handleRollChange()
    {
        bool aDown = Input.GetKey(KeyCode.A);
        bool dDown = Input.GetKey(KeyCode.D);

        if (aDown || dDown)
        {
            if (aDown)
            {
                turnHorizontal(YawDirection.LEFT);
            }

            if (dDown)
            {
                turnHorizontal(YawDirection.RIGHT);
            }
        }
        else
        {
            turnHorizontal(YawDirection.NONE);
        }
    }

    // Handles updating the rotation based on vertical input (up/down)
    private void turnVertical(PitchDirection direction)
    {
        switch (direction)
        {
            case PitchDirection.UP:     turnVerticalUp(); break;
            case PitchDirection.DOWN:   turnVerticalDown(); break;
            case PitchDirection.NONE:   turnVerticalNone(); break;
        }
    }

    // Handles updating the rotation based on horizontal input (left/right)
    private void turnHorizontal(YawDirection direction)
    {
        switch (direction)
        {
            case YawDirection.LEFT:     turnHorizontalLeft(); break;
            case YawDirection.RIGHT:    turnHorizontalRight(); break;
            case YawDirection.NONE:     turnHorizontalNone(); break;
        }
    }

    private void turnVerticalUp()
    {
    }

    private void turnVerticalDown()
    {
    }

    private void turnVerticalNone()
    {
        
    }

    // Changes roll based on left user input
    private void turnHorizontalLeft()
    {
        m_rotation.z = high(m_rotation.z + (m_deltaTime * m_incrementTurnSpeed * 25.0f), m_maxRollAngle);
    }

    // Changes roll based on right user input
    private void turnHorizontalRight()
    {
        m_rotation.z = low(m_rotation.z - (m_deltaTime * m_incrementTurnSpeed * 25.0f), -m_maxRollAngle);
    }

    // Returns roll to neutral on no user input
    private void turnHorizontalNone()
    {
        if (m_rotation.z > 0.0f)
        {
            m_rotation.z -= m_deltaTime * m_returnTurnSpeed * 25.0f;

            if (m_rotation.z < 0.0f)
                m_rotation.z = 0.0f;
        }
        else
        {
            m_rotation.z += m_deltaTime * m_returnTurnSpeed * 25.0f;

            if (m_rotation.z > 0.0f)
                m_rotation.z = 0.0f;
        }

    }

    // Handles moving forward
    private void handleMoveForward()
    {
        // Update the forward speed movement based on the frame time
        // TODO: change based on vector
        m_forwardSpeed += m_deltaTime / 2.0f;

        // TODO: change based on vector
        if (m_forwardSpeed > m_maxSpeed)
            m_forwardSpeed = m_maxSpeed;

        if (m_forwardSpeed < 0.0f)
            m_forwardSpeed = 0.0f;

        m_position += transform.forward * m_forwardSpeed;
    }

    // Wraps a value between positive numbers lowBound and highBound
    private float wrapValue(float value, float lowBound, float highBound)
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
    private float wrapAngle(float value)
    {
        return wrapValue(value, 0.0f, 360.0f);
    }

    // Clamp a value between lowBound and highBound;
    private float clamp(float value, float lowBound, float highBound)
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
