// Worked on by Chris and Louis

using UnityEngine;
using System.Collections;

public class FlightMovement : MonoBehaviour {

    public float m_forwardSpeed = 2.5f;

    private Vector3 m_rotation = new Vector3(0.0f, 0.0f, 0.0f);
    private Vector3 m_position = new Vector3(0.0f, 0.0f, 0.0f);

    private float m_leftTurnSpeed = 0.0f;
    private float m_rightTurnSpeed = 0.0f;

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

        Debug.Log(Time.deltaTime + " " + delta);
        MoveForward(delta, true);

        if (Input.GetKey(KeyCode.A))
            TurnLeft(delta, true);
        else
            TurnLeft();

        if (Input.GetKey(KeyCode.D))
            TurnRight(delta, true);
        else
            TurnRight();

        transform.position = m_position;
        transform.eulerAngles = m_rotation;
	}

    void MoveForward(float delta = 1.0f, bool keydown = false)
    {
	    float radians;

	    // Update the forward speed movement based on the frame time and whether the user is holding the key down or not.
	    if(keydown)
	    {
		    m_forwardSpeed += delta * 1.0f;

            if (m_forwardSpeed > delta * 5.0f)
		    {
                m_forwardSpeed = delta * 5.0f;
		    }
	    }
	    else
	    {
            m_forwardSpeed -= delta * 5.0f;

		    if(m_forwardSpeed < 0.0f)
		    {
                m_forwardSpeed = 0.0f;
		    }
	    }

	    // Convert degrees to radians.
	    radians = m_rotation.y * 0.0174532925f;

	    // Update the position.
	    m_position.x += Mathf.Sin(radians) * m_forwardSpeed;
	    m_position.z += Mathf.Cos(radians) * m_forwardSpeed;
	    m_position.y -= 1;

        //transform.position += transform.forward * m_forwardSpeed;

	    return;
    }

    void TurnLeft(float delta = 1.0f, bool keydown = false)
    {
	    float speed = 0.01f;
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
        if (m_rotation.z > 30.0f)
	    {
            m_rotation.z = 30.0f;
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
	    float speed = 0.01f;
	    float maxSpeed = speed * 5;
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
        if (m_rotation.z < -30.0f)
	    {
            m_rotation.z = -30.0f;
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
}
