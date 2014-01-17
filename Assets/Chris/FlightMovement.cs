// Worked on by Chris and Louis

using UnityEngine;
using System.Collections;

public class FlightMovement : MonoBehaviour {

    public float maxSpeed = 7.5f;

    private Vector3 m_rotation = new Vector3(0.0f, 0.0f, 0.0f);
    private Vector3 m_position = new Vector3(0.0f, 0.0f, 0.0f);

    private float m_forwardSpeed = 10.0f;
    private float m_leftTurnSpeed = 0.0f;
    private float m_rightTurnSpeed = 0.0f;

    private float m_pitchSpeed = 0.0f;

	// Use this for initialization
	void Start () 
    {
        m_position = transform.position;
        m_rotation = transform.eulerAngles;
	}
	
	// Update is called once per frame
	void Update () 
    {
        float delta = Time.deltaTime * 100.0f;

        handlePitch(delta, Input.GetKey(KeyCode.S), Input.GetKey(KeyCode.W));

        MoveForward(delta);

        if (Input.GetKey(KeyCode.A))
            TurnLeft(delta, true);
        else
            TurnLeft(delta, false);

        if (Input.GetKey(KeyCode.D))
            TurnRight(delta, true);
        else
            TurnRight(delta, false);

        transform.eulerAngles = m_rotation;
        transform.Rotate(new Vector3(1.0f, 0.0f, 0.0f), m_pitchSpeed);
        transform.position = m_position;
	}

    void MoveForward(float delta = 1.0f)
    {
	    // Update the forward speed movement based on the frame time
	    m_forwardSpeed += delta * 1.0f;

        if (m_forwardSpeed > maxSpeed)
            m_forwardSpeed = maxSpeed;

        if (m_forwardSpeed < 0.0f)
            m_forwardSpeed = 0.0f;

        m_position += transform.forward * m_forwardSpeed;
    }

    void TurnLeft(float delta = 1.0f, bool keydown = false)
    {
        float speed = 0.05f;
	    float maxSpeed = speed * 10;

	    // Update the left turn speed movement based on the frame time and whether the user is holding the key down or not.
	    if(keydown)
	    {
            m_leftTurnSpeed += delta * speed;

            if (m_leftTurnSpeed > (delta * maxSpeed))
		    {
                m_leftTurnSpeed = delta * maxSpeed;
		    }
	    }
	    else
	    {
            m_leftTurnSpeed -= delta * 0.005f;

		    if(m_leftTurnSpeed < 0.0f)
		    {
			    m_leftTurnSpeed = 0.0f;
		    }
	    }

	    // Update the rotation.
        m_rotation.z += m_leftTurnSpeed;

	    // Keep the rotation in the 0 to 360 range.
        if (m_rotation.z > 45.0f)
	    {
            m_rotation.z = 45.0f;
	    }
        m_rotation.y -= m_leftTurnSpeed / 2;

	    // Keep the rotation in the 0 to 360 range.
        if (m_rotation.y < 0.0f)
	    {
            m_rotation.y += 360.0f;
	    }

        //transform.position += transform.right * -m_leftTurnSpeed;

	    return;
    }


    void TurnRight(float delta = 1.0f, bool keydown = false)
    {
        float speed = 0.05f;
	    float maxSpeed = speed * 10;

	    // Update the right turn speed movement based on the frame time and whether the user is holding the key down or not.
	    if(keydown)
	    {
            m_rightTurnSpeed += delta * speed;

            if (m_rightTurnSpeed > delta * maxSpeed)
		    {
                m_rightTurnSpeed = delta * maxSpeed;
		    }
	    }
	    else
	    {
            m_rightTurnSpeed -= delta * 0.005f;

		    if(m_rightTurnSpeed < 0.0f)
		    {
			    m_rightTurnSpeed = 0.0f;
		    }
	    }

	    // Update the rotation.
        m_rotation.z -= m_rightTurnSpeed;

	    // Keep the rotation in the 0 to 360 range.
        if (m_rotation.z < -45.0f)
	    {
            m_rotation.z = -45.0f;
	    }

        m_rotation.y += m_rightTurnSpeed / 2;

	    // Keep the rotation in the 0 to 360 range.
        if (m_rotation.y > 360.0f)
	    {
            m_rotation.y -= 360.0f;
	    }

        //transform.position += transform.right * m_rightTurnSpeed;

	    return;
    }

    void handlePitch(float delta = 1.0f, bool sDown = false, bool wDown = false)
    {
        float speed = delta * 0.25f;

        if (wDown)
        {
            m_pitchSpeed += speed;
        }
        else
        {
            if (!sDown)
            {
            }
            else
            {
            }
        }

        if (sDown)
        {
            m_pitchSpeed -= speed;
        }
        else
        {
            if (!wDown)
            {
            }
            else
            {
            }
        }

        if (transform.forward.y < -0.1f)
            m_forwardSpeed += delta * (m_pitchSpeed - (transform.forward.y / 1000.0f));

        else if (transform.forward.y > 0.1f)
            m_forwardSpeed -= delta * (m_pitchSpeed + (transform.forward.y / 1000.0f));
    }
}
