// Controller for the main character - a Seraph.
// Handles gliding, flight, and ground movement mechanics.
// Requires a box collider to be attached to the entity 
// for collisions to properly work.

using UnityEngine;

namespace cst
{
    public class SeraphController : MonoBehaviour
    {
        // Inspector fields
        [SerializeField] private float m_restingSpeed = 2.5f;
        [SerializeField] private float m_maxSpeed = 3.5f;

        private float m_maxRollAngle = 37.5f;
        private float m_maxPitchAngle = 65.0f;
        private float m_turnTightness = 2.0f;
        private float m_incrementTurnSpeed = 35.0f;
        private float m_returnTurnSpeed = 30.0f;
        private float m_incrementPitchSpeed = 45.0f;
        private float m_returnPitchSpeed = 30.0f;

        private Vector3 m_position;
        private Vector3 m_rotation;
        private float m_forwardSpeed;

        // Initialise the flight variables
        public void Start()
        {
            m_position = transform.position;
            m_rotation = transform.eulerAngles;
            m_forwardSpeed = m_restingSpeed;
        }

        // Update the position
        public void Update()
        {
            float delta = Time.deltaTime;

            handleOrientationChange(delta);
            handleMoveForward(delta);
        }

        public void OnCollisionEnter(Collision other)
        {
            Debug.Log("Collision ENTER!");
        }

        // Emergency!
        public void OnCollisionStay(Collision other)
        {
            Debug.Log("Collision STAY!");
        }

        public void OnTriggerEnter(Collider other)
        {
            Debug.Log("Trigger ENTER!");
        }

        private void handleOrientationChange(float delta)
        {
            handlePitchChange(delta);
            handleYawChange(delta);
            handleRollChange(delta);

            transform.eulerAngles = m_rotation;
        }

        // Handles the change in pitch based on the user input
        private void handlePitchChange(float delta)
        {
            if (Input.GetKey(KeyCode.W))
            {
                turnVerticalDown(delta);
            }
            else if (Input.GetKey(KeyCode.S))
            {
                turnVerticalUp(delta);
            }
            else
            {
                turnVerticalNone(delta);
            }
        }

        // Handles the change in yaw based on the current roll
        private void handleYawChange(float delta)
        {
            m_rotation.y = Helpers.wrapAngle(
                m_rotation.y - (delta * m_rotation.z * m_turnTightness));
        }

        // Handles the change in roll based on the user input
        private void handleRollChange(float delta)
        {
            if (Input.GetKey(KeyCode.A))
            {
                if (m_rotation.z < 0.0f)
                {
                    turnHorizontalLeft(delta * 1.25f);
                }
                else
                {
                    turnHorizontalLeft(delta);
                }
            }
            else if (Input.GetKey(KeyCode.D))
            {
                if (m_rotation.z > 0.0f)
                {
                    turnHorizontalRight(delta * 1.25f);
                }
                else
                {
                    turnHorizontalRight(delta);
                }
            }
            else
            {
                turnHorizontalNone(delta);
            }
        }

        // Changes pitch based on up user input
        private void turnVerticalUp(float delta)
        {
            if (-(m_rotation.x - 360.0f) < m_maxPitchAngle)
                m_rotation.x -= (delta * m_incrementPitchSpeed);
        }

        // Changes pitch based on down user input
        private void turnVerticalDown(float delta)
        {
            if (-(m_rotation.x - 360.0f) > -m_maxPitchAngle)
                m_rotation.x += (delta*m_incrementPitchSpeed);
        }

        // Returns pitch to neutral on no user input
        private void turnVerticalNone(float delta)
        {
            // Intentionally left empty - no return pitch behaviour
        }

        // Changes roll based on left user input
        private void turnHorizontalLeft(float delta)
        {
            m_rotation.z = Helpers.high(
                m_rotation.z + (delta * m_incrementTurnSpeed), 
                m_maxRollAngle);
        }

        // Changes roll based on right user input
        private void turnHorizontalRight(float delta)
        {
            m_rotation.z = Helpers.low(
                m_rotation.z - (delta * m_incrementTurnSpeed), 
                -m_maxRollAngle);
        }

        // Returns roll to neutral on no user input
        private void turnHorizontalNone(float delta)
        {
            if (m_rotation.z > 0.0f)
            {
                m_rotation.z -= delta * m_returnTurnSpeed;

                if (m_rotation.z < 0.0f)
                    m_rotation.z = 0.0f;
            }
            else
            {
                m_rotation.z += delta * m_returnTurnSpeed;

                if (m_rotation.z > 0.0f)
                    m_rotation.z = 0.0f;
            }

        }

        // Handles moving forward
        private void handleMoveForward(float delta)
        {
            m_forwardSpeed += delta / 2.0f;

            if (m_forwardSpeed > m_maxSpeed)
                m_forwardSpeed = m_maxSpeed;

            if (m_forwardSpeed < 0.0f)
                m_forwardSpeed = 0.0f;

            m_position += transform.forward * m_forwardSpeed;

            // Update the game object
            transform.position = m_position;
        }
    }
}