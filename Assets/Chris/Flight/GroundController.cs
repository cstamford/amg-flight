using cst.Common;
using UnityEngine;

namespace cst.Flight
{
    public class GroundController : ControllerBase, IControllerBase
    {
        public bool walking { get; set; }
        public bool falling { get; set; }

        private const float DEFAULT_HEIGHT                       = 32.0f;
        private const float START_FALL_VELOCITY                  = 50.0f;
        private const float GLIDE_TRANSITION_MIN_VELOCITY        = 100.0f;
        private const float LANDING_TRANSITION_MAX_VELOCITY      = 150.0f;
        private const float LANDING_TRANSITION_TIME              = 1.0f;
        private const float LANDING_TRANSITION_RETURN_ROLL_SPEED = 180.0f;
        private const float MAX_FALL_VELOCTY                     = 250.0f;
        private const float MAX_FALL_TIME                        = 3.0f;
        private const float FORWARD_SPEED                        = 50.0f;
        private const float STRAFE_SPEED                         = 50.0f;
        private const float LOOK_SENSITIVITY                     = 100.0f;
        private const bool  CAMERA_PITCH                         = true;

        private Vector3              m_position;
        private Vector3              m_rotation;
        private float                m_fallSpeed;
        private float                m_forwardTransitionSpeed;
        private float                m_fallTimer;
        private readonly float       m_height;

        public GroundController(SeraphController controller)
            : base(controller)
        {
            BoxCollider collider = transform.collider as BoxCollider;

            if (collider)
                m_height = collider.size.y + 1.0f;
            else
                m_height = DEFAULT_HEIGHT + 1.0f;

            walking = false;
            falling = false;
        }

        public void start(TransitionData data)
        {
            Debug.Log(GetType().Name + " received transition data: " + data);

            if (state == SeraphState.LANDING)
                m_forwardTransitionSpeed = data.velocity;
        }

        public void update()
        {
            m_position = transform.position;
            m_rotation = transform.eulerAngles;
            falling = checkFalling();

            handleCamera();

            if (state == SeraphState.LANDING)
                handleLandingTransition();

            if (falling)
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
            return new TransitionData
            {
                direction = new Vector3(0.0f, -1.0f, 0.0f), 
                velocity = m_fallSpeed
            };
        }

        // Camera-mouse movement - only runs inside the editor
        private void handleCamera()
        {
            Vector3 rotation = m_rotation;

            if (CAMERA_PITCH)
            {
                if (inputManager.actionFired(Action.LOOK_UP))
                {
                    float normalisedPitch = Helpers.getNormalizedAngle(rotation.x);
                    float change = Time.deltaTime * inputManager.actionDelta(Action.LOOK_UP) * LOOK_SENSITIVITY;

                    if (normalisedPitch - change > -85.0f)
                        rotation.x -= change;
                }
                else if (inputManager.actionFired(Action.LOOK_DOWN))
                {
                    float normalisedPitch = Helpers.getNormalizedAngle(rotation.x);
                    float change = Time.deltaTime * inputManager.actionDelta(Action.LOOK_DOWN) * LOOK_SENSITIVITY;

                    if (normalisedPitch + change < 85.0f)
                        rotation.x += change;
                }
            }

            if (inputManager.actionFired(Action.LOOK_LEFT))
            {
                float change = Time.deltaTime * inputManager.actionDelta(Action.LOOK_LEFT) * LOOK_SENSITIVITY;
                rotation.y -= change;
            }
            else if (inputManager.actionFired(Action.LOOK_RIGHT))
            {
                float change = Time.deltaTime * inputManager.actionDelta(Action.LOOK_RIGHT) * LOOK_SENSITIVITY;
                rotation.y += change;
            }

            m_rotation = rotation;
        }

        private void handleLandingTransition()
        {
            handleLandingTransitionSpeed();
            handleLandingTransitionRoll();

            if (m_rotation.z == 0.0f && m_forwardTransitionSpeed == 0.0f)
                state = SeraphState.GROUNDED;
        }

        private void handleLandingTransitionSpeed()
        {
            if (m_forwardTransitionSpeed > LANDING_TRANSITION_MAX_VELOCITY)
                m_forwardTransitionSpeed = LANDING_TRANSITION_MAX_VELOCITY;

            float speedStep = (LANDING_TRANSITION_MAX_VELOCITY /
                LANDING_TRANSITION_TIME) * Time.deltaTime;

            m_forwardTransitionSpeed -= speedStep;

            if (m_forwardTransitionSpeed < speedStep)
                m_forwardTransitionSpeed = 0.0f;

            Vector3 delta = transform.forward * m_forwardTransitionSpeed *
                Time.deltaTime;

            // Strip the height component
            delta.y = 0.0f;

            m_position += delta;
        }

        private void handleLandingTransitionRoll()
        {
            float angleStep = LANDING_TRANSITION_RETURN_ROLL_SPEED *
                Time.deltaTime;

            float angle = m_rotation.z - 180.0f;

            if (angle > 0.0f)
            {
                m_rotation.z = Helpers.wrapAngle(m_rotation.z + angleStep);

                if (m_rotation.z - 180.0f < 0.0f)
                    m_rotation.z = 0.0f;
            }
            else if (angle < 0.0f)
            {
                m_rotation.z = Helpers.wrapAngle(m_rotation.z - angleStep);

                if (m_rotation.z - 180.0f > 0.0f)
                    m_rotation.z = 0.0f;
            }
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
            Vector3 rightVector = transform.right;
            forwardVector.y = 0.0f;
            rightVector.y = 0.0f;

            bool walkedThisFrame = false;

            if (inputManager.actionFired(Action.MOVE_FORWARD))
            {
                m_position += forwardVector * FORWARD_SPEED * Time.deltaTime * inputManager.actionDelta(Action.MOVE_FORWARD);
                walkedThisFrame = true;
            }

            if (inputManager.actionFired(Action.MOVE_BACKWARD))
            {
                m_position -= forwardVector * FORWARD_SPEED * Time.deltaTime * inputManager.actionDelta(Action.MOVE_BACKWARD);
                walkedThisFrame = true;
            }

            if (inputManager.actionFired(Action.MOVE_LEFT))
            {
                m_position -= rightVector * STRAFE_SPEED * Time.deltaTime * inputManager.actionDelta(Action.MOVE_LEFT);
                walkedThisFrame = true;
            }

            if (inputManager.actionFired(Action.MOVE_RIGHT))
            {
                m_position += rightVector * STRAFE_SPEED * Time.deltaTime * inputManager.actionDelta(Action.MOVE_RIGHT);
                walkedThisFrame = true;
            }

            walking = walkedThisFrame;
            transform.eulerAngles = m_rotation;
            transform.position = m_position;
        }

        // Handles transitioning between falling and gliding
        private void handleTransition()
        {
            if (!inputManager.actionFired(Action.INTERACT))
                return;

            switch (capability)
            {
                case SeraphCapability.GLIDE:

                    if (m_fallSpeed >= GLIDE_TRANSITION_MIN_VELOCITY)
                        state = SeraphState.GLIDING;

                    // TODO pick up items
                    //else


                    break;

                case SeraphCapability.FLIGHT:

                    state = SeraphState.GLIDING;

                    break;
            }
        }
    }
}