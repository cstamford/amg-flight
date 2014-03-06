using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace cst.Flight
{
    public class GroundController : ControllerBase, IControllerBase
    {
        private const float DEFAULT_HEIGHT = 32.0f;
        private const float START_FALL_VELOCITY = 50.0f;
        private const float GLIDE_TRANSIFITION_MIN_VELOCITY = 100.0f;
        private const float MAX_FALL_VELOCTY = 250.0f;
        private const float MAX_FALL_TIME = 3.0f;
        private const float FORWARD_SPEED = 50.0f;
        private const float STRAFE_SPEED = 50.0f;

        private Vector3 m_position;
        private Vector3 m_rotation;
        private float m_fallSpeed;
        private float m_fallTimer;
        private readonly float m_height;

        public GroundController(SeraphController controller)
            : base(controller)
        {
            BoxCollider collider = transform.collider as BoxCollider;

            if (collider)
                m_height = collider.size.y + 1.0f;
            else
                m_height = DEFAULT_HEIGHT + 1.0f;
        }

        public void start(TransitionData data)
        {
            Debug.Log(GetType().Name + " received transition data: " 
                + data.velocity);
        }

        public void update()
        {
            m_position = transform.position;
            m_rotation = transform.eulerAngles;

            // Make sure there is no roll
            m_rotation.z = 0.0f;

            cameraControl();

            if (checkFalling())
            {
                handleFalling();
                handleTransition();
            }
            else
            {
                m_fallTimer = 0.0f;
            }

            handleMovement();
        }

        public void triggerEnter(Collider other)
        {
            Debug.Log(GetType().Name + " triggerEnter()");
        }

        public void triggerExit(Collider other)
        {
            Debug.Log(GetType().Name + " triggerExit()");
        }

        public void collisionEnter(Collision other)
        {
            Debug.Log(GetType().Name + " collisionEnter()");
        }

        public void collisionExit(Collision other)
        {
            Debug.Log(GetType().Name + " collisionExit()");
        }

        public TransitionData transitionData()
        {
            return new TransitionData { velocity = new Vector3(0.0f, m_fallSpeed, 0.0f) };
        }

        // Camera-mouse movement - only runs inside the editor
        [Conditional("UNITY_EDITOR")]
        private void cameraControl()
        {
            float rotationX = m_rotation.x - (Input.GetAxis("Mouse Y") * 5.0f);
            float rotationY = m_rotation.y + (Input.GetAxis("Mouse X") * 5.0f);

            m_rotation.x = rotationX;
            m_rotation.y = rotationY;
        }

        // Checks if we are currently falling
        private bool checkFalling()
        {
            return !Physics.Raycast(transform.position,
                new Vector3(0.0f, -1.0f, 0.0f), m_height);
        }

        // Handles falling
        private void handleFalling()
        {
            m_fallTimer += Time.deltaTime;

            if (m_fallTimer > MAX_FALL_TIME)
                m_fallTimer = MAX_FALL_TIME;

            m_fallSpeed = Helpers.quadraticInterp(START_FALL_VELOCITY, 
                MAX_FALL_VELOCTY, m_fallTimer, MAX_FALL_TIME);

            m_position.y -= m_fallSpeed * Time.deltaTime;
        }

        // Handles movement (in air and ground)
        private void handleMovement()
        {
            // Strip the height component from our vectors
            Vector3 forwardVector = transform.forward;
            forwardVector.y = 0.0f;

            Vector3 rightVector = transform.right;
            rightVector.y = 0.0f;

            if (Input.GetKey(KeyCode.W))
                m_position += forwardVector * FORWARD_SPEED * Time.deltaTime;

            if (Input.GetKey(KeyCode.S))
                m_position -= forwardVector * FORWARD_SPEED * Time.deltaTime;

            if (Input.GetKey(KeyCode.D))
                m_position += rightVector * STRAFE_SPEED * Time.deltaTime;

            if (Input.GetKey(KeyCode.A))
                m_position -= rightVector * STRAFE_SPEED * Time.deltaTime;

            transform.eulerAngles = m_rotation;
            transform.position = m_position;
        }

        // Handles transitioning between falling and gliding
        private void handleTransition()
        {
            if (!Input.GetKey(KeyCode.Space))
                return;

            switch (capability)
            {
                case SeraphCapability.GLIDE:

                    if (m_fallSpeed >= GLIDE_TRANSIFITION_MIN_VELOCITY)
                        controller.setState(SeraphState.GLIDING);

                    break;

                case SeraphCapability.FLIGHT:

                    controller.setState(SeraphState.GLIDING);

                    break;
            }
        }
    }
}