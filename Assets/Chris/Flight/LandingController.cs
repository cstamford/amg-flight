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

using System;
using cst.Common;
using UnityEngine;

namespace cst.Flight
{
    public class LandingController : SharedGroundControls
    {
        private const float LANDING_TRANSITION_MAX_VELOCITY      = 7.5f;
        private const float LANDING_TRANSITION_MAX_TIME          = 1.0f;
        private const float LANDING_TRANSITION_RETURN_ROLL_SPEED = 180.0f;

        private float m_forwardTransitionTimer;
        private float m_forwardTransitionSpeed;
        private float m_forwardInitialSpeed;

        public LandingController(SeraphController controller)
            : base(controller)
        {}

        public override void start(TransitionData data)
        {
            Debug.Log(GetType().Name + " received transition data: " + data);
            m_forwardTransitionSpeed = data.velocity > LANDING_TRANSITION_MAX_VELOCITY
                ? LANDING_TRANSITION_MAX_VELOCITY
                : data.velocity;

            m_forwardInitialSpeed = m_forwardTransitionSpeed;
            m_forwardTransitionTimer = LANDING_TRANSITION_MAX_TIME;
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
        }

        public override void triggerStay(Collider other)
        {
        }

        public override void triggerExit(Collider other)
        {
        }

        public override void collisionEnter(Collision other)
        {
        }

        public override void collisionStay(Collision other)
        {
        }

        public override void collisionExit(Collision other)
        {
        }

        public override TransitionData transitionData()
        {
            return new TransitionData { direction = transform.forward, velocity = m_forwardTransitionSpeed };
        }

        private void handleLandingTransitionSpeed()
        {
            m_forwardTransitionTimer -= Time.deltaTime;

            if (m_forwardTransitionTimer < 0.0f)
                m_forwardTransitionTimer = 0.0f;

            m_forwardTransitionSpeed = Helpers.quadraticInterpOut(0.0f, m_forwardInitialSpeed,
                m_forwardTransitionTimer, LANDING_TRANSITION_MAX_TIME);

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

        private void handleTransition()
        {
            const float EPSILON = 0.000001f;

            if (Math.Abs(m_rotation.z) < EPSILON && Math.Abs(m_forwardTransitionSpeed) < EPSILON)
                state = SeraphState.GROUNDED;
        }
    }
}