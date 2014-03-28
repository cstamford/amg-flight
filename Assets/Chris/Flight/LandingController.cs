// ==================================================================== \\
// File   : LandingController.cs                                        \\
// Author : Christopher Stamford									    \\
//                                                                      \\
// LandingController.cs provides functionality for the LANDING state.   \\
//                                                                      \\
// It provides a smooth transition between the GLIDING state and the    \\
// GROUNDED state. This ensures that the transition between high speed  \\
// gliding and hitting the ground is not jarring for the player.        \\
//                                                                      \\
// This controller can transition to the following states:              \\
//   - GROUNDED                                                         \\
// ==================================================================== \\

using cst.Common;
using UnityEngine;

namespace cst.Flight
{
    public class LandingController : SharedGroundControls
    {
        private const float HEIGHT_INTERP_STEP                   = 25.0f;
        private const float LANDING_TRANSITION_MAX_VELOCITY      = 150.0f;
        private const float LANDING_TRANSITION_TIME              = 1.0f;
        private const float LANDING_TRANSITION_RETURN_ROLL_SPEED = 180.0f;

       	public float m_forwardTransitionSpeed;
        private float m_desiredHeight;

        public LandingController(SeraphController controller)
            : base(controller)
        {}

        public override void start(TransitionData data)
        {
            Debug.Log(GetType().Name + " received transition data: " + data);
            m_forwardTransitionSpeed = data.velocity;
        }

        public override void update()
        {
            m_position      = transform.position;
            m_rotation      = transform.eulerAngles;
            m_desiredHeight = m_position.y;

            handleFacing();
            handleLandingTransitionSpeed();
            handleLandingTransitionRoll();
            handleTransition();
            interpolateHeight();

            transform.position    = m_position;
            transform.eulerAngles = m_rotation;
        }

        public override void triggerEnter(Collider other)
        {
            Debug.Log(GetType().Name + " triggerEnter()");
        }

        public override void triggerExit(Collider other)
        {
            Debug.Log(GetType().Name + " triggerExit()");
        }

        public override void collisionEnter(Collision other)
        {
            Debug.Log(GetType().Name + " collisionEnter()");
        }

        public override void collisionExit(Collision other)
        {
            Debug.Log(GetType().Name + " collisionExit()");
        }

        public override TransitionData transitionData()
        {
            return new TransitionData { direction = transform.forward, velocity = m_forwardTransitionSpeed };
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

            float? distanceToGround = Helpers.nearestHit(transform.position, Vector3.down, height * 1.5f);

            if (distanceToGround.HasValue)
                m_desiredHeight = m_position.y - (distanceToGround.Value - height);
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

        private void handleTransition()
        {
            if (m_rotation.z == 0.0f && m_forwardTransitionSpeed == 0.0f)
                state = SeraphState.GROUNDED;
        }

        private void interpolateHeight()
        {
            if (m_position.y > m_desiredHeight)
            {
                m_position.y -= HEIGHT_INTERP_STEP * Time.deltaTime;

                if (m_position.y <= m_desiredHeight)
                    m_position.y = m_desiredHeight;
            }
            else if (m_position.y < m_desiredHeight)
            {
                m_position.y += HEIGHT_INTERP_STEP * Time.deltaTime;

                if (m_position.y >= m_desiredHeight)
                    m_position.y = m_desiredHeight;
            }
        }
    }
}