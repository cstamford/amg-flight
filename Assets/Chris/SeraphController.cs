// Controller for the main character - a Seraph.
// Handles gliding, flight, and ground movement mechanics.
// Requires a box collider and non-kinematic rigid body for collisions.

using UnityEngine;

namespace cst
{
    public class SeraphController : MonoBehaviour
    {
        // Inspector fields
        [SerializeField] private float RESTING_SPEED = 100.0f;
        [SerializeField] private float MAX_SPEED = 250.0f;

        private const float MAX_ROLL_ANGLE = 37.5f;
        private const float MAX_PITCH_ANGLE = 65.0f;
        private const float TURN_TIGHTNESS = 2.0f;
        private const float INCREMENT_TURN_SPEED = 35.0f;
        private const float RETURN_TURN_SPEED = 30.0f;
        private const float INCREMENT_PITCH_SPEED = 45.0f;
        private const float INCREMENT_FOWARD_SPEED = 15.0f;

        private Vector3 m_position;
        private Vector3 m_rotation;
        private float m_forwardSpeed;

        // Initialise the flight variables
        public void Start()
        {
            m_position = transform.position;
            m_rotation = transform.eulerAngles;
            m_forwardSpeed = RESTING_SPEED;
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
            float angle = m_rotation.z;

            if (angle > 180.0f)
                angle -= 360.0f;

            m_rotation.y = Helpers.wrapAngle(
                m_rotation.y - (delta * angle * TURN_TIGHTNESS));
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
            float angle = Helpers.getNormalizedAngle(m_rotation.x);

            if (angle > -MAX_PITCH_ANGLE)
            {
                m_rotation.x = Helpers.wrapAngle(
                    m_rotation.x - (delta*INCREMENT_PITCH_SPEED));
            }
        }

        // Changes pitch based on down user input
        private void turnVerticalDown(float delta)
        {
            float angle = Helpers.getNormalizedAngle(m_rotation.x);

            if (angle < MAX_PITCH_ANGLE)
            {
                m_rotation.x = Helpers.wrapAngle(
                    m_rotation.x + (delta*INCREMENT_PITCH_SPEED));
            }
        }

        // Returns pitch to neutral on no user input
        private void turnVerticalNone(float delta)
        {
            // Intentionally left empty - no return pitch behaviour
        }

        // Changes roll based on left user input
        private void turnHorizontalLeft(float delta)
        {
            float angle = Helpers.getNormalizedAngle(m_rotation.z);

            if (angle < MAX_ROLL_ANGLE)
            {
                m_rotation.z = Helpers.wrapAngle(
                    m_rotation.z + (delta * INCREMENT_PITCH_SPEED));
            }
        }

        // Changes roll based on right user input
        private void turnHorizontalRight(float delta)
        {
            float angle = Helpers.getNormalizedAngle(m_rotation.z);

            if (angle > -MAX_ROLL_ANGLE)
            {
                m_rotation.z = Helpers.wrapAngle(
                    m_rotation.z - (delta * INCREMENT_PITCH_SPEED));
            }
        }

        // Returns roll to neutral on no user input
        private void turnHorizontalNone(float delta)
        {
            float angle = m_rotation.z - 180.0f;

            if (angle > 0.0f)
            {
                m_rotation.z = Helpers.wrapAngle(
                    m_rotation.z + delta * RETURN_TURN_SPEED);

                if (m_rotation.z - 180.0f < 0.0f)
                    m_rotation.z = 0.0f;
            }
            else
            {
                m_rotation.z = Helpers.wrapAngle(
                    m_rotation.z - delta * RETURN_TURN_SPEED);

                if (m_rotation.z - 180.0f > 0.0f)
                    m_rotation.z = 0.0f;
            }

        }

        // Handles moving forward
        private void handleMoveForward(float delta)
        {
            if (m_forwardSpeed > MAX_SPEED)
                m_forwardSpeed = MAX_SPEED;

            if (m_forwardSpeed < 0.0f)
                m_forwardSpeed = 0.0f;

            m_position += transform.forward*m_forwardSpeed*delta;

            // Update the game object
            transform.position = m_position;
        }
    }
}