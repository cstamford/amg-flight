// Worked on by Chris, Sean, and Louis

using UnityEngine;

public class FlightMovement : MonoBehaviour 
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
    [SerializeField] private float m_playerMass = 3.5f;
    [SerializeField] private float m_playerDrag = 1.0f;
    [SerializeField] private float m_playerLift = 1.0f;
    [SerializeField] private float m_restingSpeed = 400.0f;
    [SerializeField] private float m_maxSpeed = 750.0f;
    [SerializeField] private float m_maxRollAngle = 37.5f;
    [SerializeField] private float m_maxPitchAngle = 65.0f;
    [SerializeField] private float m_turnTightness = 2.0f;
    [SerializeField] private float m_incrementTurnSpeed = 2.0f;
    [SerializeField] private float m_returnTurnSpeed = 1.5f;
    [SerializeField] private float m_incrementPitchSpeed = 2.0f;
    [SerializeField] private float m_returnPitchSpeed = 1.5f;

    // Private variables
    private Vector3 m_position = new Vector3(0.0f, 0.0f, 0.0f);
    private Vector3 m_rotation = new Vector3(0.0f, 0.0f, 0.0f);
    private Vector3 m_velocity = new Vector3(0.0f, 0.0f, 0.0f);
    private float m_airSpeed;
    private float m_sinkSpeed;
    private float m_weight;

    // Controller
    private PlayerController m_playerController;

	// Initialise the flight variables
	public void Start() 
    {
        m_playerController = GetComponent("PlayerController") as PlayerController;
        m_position = transform.position;
        m_rotation = transform.eulerAngles;

        m_airSpeed = m_restingSpeed;
        m_weight = m_playerMass * Physics.gravity.y;

        m_playerDrag = low(m_playerDrag, 0.01f);
	}

    // Update the position
    public void Update()
    {
        if (m_playerController != null && !m_playerController.m_isFlying)
            return;

        Debug.Log(m_restingSpeed + "" + m_airSpeed);

        float delta = Time.deltaTime;

        handleOrientationChange(delta);
        handlePositionChange(delta);
        handleCollisions();
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

    private void handleCollisions()
    {
        if (m_playerController != null && Physics.Raycast(m_position, new Vector3(0.0f, -1.0f, 0.0f), collider.bounds.extents.y + 20.0f))
        {
            m_playerController.m_isFlying = false;
        }
    }

    // Handles rotation
    private void handleOrientationChange(float delta)
    {
        handlePitchChange(delta);
        handleYawChange(delta);
        handleRollChange(delta);

        // Update the game object
        transform.eulerAngles = m_rotation;
    }
    
    // Handles movement
    private void handlePositionChange(float delta)
    {
        handleForwardSpeedChange(delta);
        handleSinkSpeedChange(delta);

        // Update the game object
        m_position += m_velocity * delta;
        transform.position = m_position;
    }

    // Handles the chagne in pitch based on the user input
    private void handlePitchChange(float delta)
    {
        bool wDown = Input.GetKey(KeyCode.W);
        bool sDown = Input.GetKey(KeyCode.S);

        if (wDown || sDown)
        {
            if (wDown)
            {
                turnVertical(PitchDirection.UP, delta);
            }

            if (sDown)
            {
                turnVertical(PitchDirection.DOWN, delta);
            }
        }
        else
        {
            turnVertical(PitchDirection.NONE, delta);
        }
    }

    // Handles the change in yaw based on the current roll
    private void handleYawChange(float delta)
    {
        m_rotation.y = wrapAngle(m_rotation.y - (delta * m_rotation.z * m_turnTightness));
    }

    // Handles the change in roll based on the user input
    private void handleRollChange(float delta)
    {
        bool aDown = Input.GetKey(KeyCode.A);
        bool dDown = Input.GetKey(KeyCode.D);

        if (aDown || dDown)
        {
            if (aDown)
            {
                turnHorizontal(YawDirection.LEFT, delta);
            }

            if (dDown)
            {
                turnHorizontal(YawDirection.RIGHT, delta);
            }
        }
        else
        {
            turnHorizontal(YawDirection.NONE, delta);
        }
    }

    // Handles updating the rotation based on vertical input (up/down)
    private void turnVertical(PitchDirection direction, float delta)
    {
        switch (direction)
        {
            case PitchDirection.UP:     turnVerticalUp(delta); break;
            case PitchDirection.DOWN:   turnVerticalDown(delta); break;
            case PitchDirection.NONE:   turnVerticalNone(delta); break;
        }
    }

    // Handles updating the rotation based on horizontal input (left/right)
    private void turnHorizontal(YawDirection direction, float delta)
    {
        switch (direction)
        {
            case YawDirection.LEFT:     turnHorizontalLeft(delta); break;
            case YawDirection.RIGHT:    turnHorizontalRight(delta); break;
            case YawDirection.NONE:     turnHorizontalNone(delta); break;
        }
    }

    // Changes pitch based on up user input
    private void turnVerticalUp(float delta)
    {
        m_rotation.x = high(m_rotation.x + (delta * m_incrementPitchSpeed * 25.0f), m_maxPitchAngle);
    }
   
    // Changes pitch based on down user input
    private void turnVerticalDown(float delta)
    {
        m_rotation.x = low(m_rotation.x - (delta * m_incrementPitchSpeed * 25.0f), -m_maxPitchAngle);
    }

    // Defaults the pitch on no user input
    private void turnVerticalNone(float delta)
    {
        
        
        //  Do we want our pitch to default to zero, or do we just 
        //  want it to remain stationary when a button is not pressed?

        /*
        if (m_rotation.x > 0.0f)
        {
            m_rotation.x -= delta * m_returnPitchSpeed * 25.0f;

            if (m_rotation.x < 0.0f)
                m_rotation.x = 0.0f;
        }
        else
        {
            m_rotation.x += delta * m_returnPitchSpeed * 25.0f;

            if (m_rotation.x > 0.0f)
                m_rotation.x = 0.0f;
        }
         */
    }

    // Changes roll based on left user input
    private void turnHorizontalLeft(float delta)
    {
        m_rotation.z = high(m_rotation.z + (delta * m_incrementTurnSpeed * 25.0f), m_maxRollAngle);
    }

    // Changes roll based on right user input
    private void turnHorizontalRight(float delta)
    {
        m_rotation.z = low(m_rotation.z - (delta * m_incrementTurnSpeed * 25.0f), -m_maxRollAngle);
    }

    // Returns roll to neutral on no user input
    private void turnHorizontalNone(float delta)
    {
        if (m_rotation.z > 0.0f)
        {
            m_rotation.z -= delta * m_returnTurnSpeed * 25.0f;

            if (m_rotation.z < 0.0f)
                m_rotation.z = 0.0f;
        }
        else
        {
            m_rotation.z += delta * m_returnTurnSpeed * 25.0f;

            if (m_rotation.z > 0.0f)
                m_rotation.z = 0.0f;
        }

    }

    // Handles the effects of the forward speed
    private void handleForwardSpeedChange(float delta)
    {        
        
        // Changes velocity based on air speed
        // TODO:    Change velocity based on "angle of attack"
        //          ie. based on the pitch of the camera
        
        
        m_airSpeed += (m_playerLift / m_playerDrag);

        if (m_airSpeed > m_maxSpeed)
            m_airSpeed = m_maxSpeed;

        m_velocity = transform.forward * m_airSpeed;
    }

    // Handles the effects of the sink speed
    private void handleSinkSpeedChange(float delta)
    {
        m_velocity.y += (delta * m_weight);
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
