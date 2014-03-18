using System;
using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace cst.Flight
{
    public class GroundController : ControllerBase, IControllerBase
    {
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

        private Vector3              m_position;
        private Vector3              m_rotation;
        private float                m_fallSpeed;
        private float                m_forwardTransitionSpeed;
        private float                m_fallTimer;
        private readonly float       m_height;
        private bool                 m_walking;
        private bool                 m_falling;
        private bool                 m_walkSoundPlaying;
        private readonly AudioSource m_walkSound;

        public GroundController(SeraphController controller)
            : base(controller)
        {
            BoxCollider collider = transform.collider as BoxCollider;

            if (collider)
                m_height = collider.size.y + 1.0f;
            else
                m_height = DEFAULT_HEIGHT + 1.0f;

            m_walkSound      = (AudioSource)controller.getGameObject().AddComponent("AudioSource");
            m_walkSound.clip = (AudioClip)Resources.Load("footsteps-1");
        }

        public void start(TransitionData data)
        {
            Debug.Log(GetType().Name + " received transition data: " + data);

            if (controller.getState() == SeraphState.LANDING)
                m_forwardTransitionSpeed = data.velocity;
        }

        public void update()
        {
            m_position = transform.position;
            m_rotation = transform.eulerAngles;
            m_falling = checkFalling();

            handleCamera();

            if (controller.getState() == SeraphState.LANDING)
                handleLandingTransition();

            if (m_falling)
            {
                handleFalling();
                handleTransition();
            }
            else
            {
                m_fallTimer = 0.0f;
            }

            handleMovement();
            handleAudio();
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

        private void handleAudio()
        {
            if (!m_falling)
            {
                if (m_walking)
                {
                    if (!m_walkSoundPlaying)
                    {
                        m_walkSound.loop = true;
                        m_walkSound.playOnAwake = false;
                        m_walkSound.Play();
                        m_walkSoundPlaying = true;
                    }
                }
                else
                {
                    if (m_walkSoundPlaying)
                    {
                        m_walkSound.loop = false;
                        m_walkSound.playOnAwake = false;
                        m_walkSound.Stop();
                        m_walkSoundPlaying = false;
                    }
                }
            }

            if (m_falling)
            {
                m_walkSound.Stop();
            }
        }

        // Camera-mouse movement - only runs inside the editor
        //[Conditional("UNITY_EDITOR")]
        private void handleCamera()
        {
            float rotationX = m_rotation.x - (Input.GetAxis("Mouse Y") * 5.0f);
            float rotationY = m_rotation.y + (Input.GetAxis("Mouse X") * 5.0f);

            m_rotation.x = rotationX;
            m_rotation.y = rotationY;
        }

        private void handleLandingTransition()
        {
            handleLandingTransitionSpeed();
            handleLandingTransitionRoll();

            if (m_rotation.z == 0.0f && m_forwardTransitionSpeed == 0.0f)
                controller.setState(SeraphState.GROUNDED);
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
            forwardVector.y = 0.0f;

            Vector3 rightVector = transform.right;
            rightVector.y = 0.0f;

            bool walkedThisFrame = false;

            if (Input.GetKey(KeyCode.W) || Input.GetAxis("ControllerVertical") < 0)
            {
                m_position += forwardVector*FORWARD_SPEED*Time.deltaTime;
                walkedThisFrame = true;
            }

            if (Input.GetKey(KeyCode.S) || Input.GetAxis("ControllerVertical") > 0)
            {
                m_position -= forwardVector*FORWARD_SPEED*Time.deltaTime;
                walkedThisFrame = true;
            }

            if (Input.GetKey(KeyCode.D) || Input.GetAxis("ControllerHorizontal") > 0)
            {
                m_position += rightVector*STRAFE_SPEED*Time.deltaTime;
                walkedThisFrame = true;
            }

            if (Input.GetKey(KeyCode.A) || Input.GetAxis("ControllerHorizontal") < 0)
            {
                m_position -= rightVector*STRAFE_SPEED*Time.deltaTime;
                walkedThisFrame = true;
            }

            m_walking = walkedThisFrame;
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

                    if (m_fallSpeed >= GLIDE_TRANSITION_MIN_VELOCITY)
                        controller.setState(SeraphState.GLIDING);

                    break;

                case SeraphCapability.FLIGHT:

                    controller.setState(SeraphState.GLIDING);

                    break;
            }
        }
    }
}