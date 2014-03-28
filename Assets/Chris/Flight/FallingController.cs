// ==================================================================== \\
// File   : FallingController.cs                                        \\
// Author : Christopher Stamford									    \\
//                                                                      \\
// FallingController.cs provides functionality for the FALLING state.   \\
//                                                                      \\
// Preserves momentum from the previous state's transition data, and    \\
// uses quadratic interpolation to increase fall speed and to decrease  \\
// forward momentum.                                                    \\
//                                                                      \\
// This controller can transition to the following states:              \\
//   - GROUNDED                                                         \\
//   - GLIDING                                                          \\
//   - FLYING                                                           \\
// ==================================================================== \\

using System;
using cst.Common;
using UnityEngine;
using Action = cst.Common.Action;

namespace cst.Flight
{
    public class FallingController : SharedGroundControls
    {
        private const float MIN_GLIDE_VELOCITY  = 8.0f;
        private const float START_FALL_VELOCITY = 1.0f;
        private const float MAX_FALL_VELOCTY    = 50.0f;
        private const float MAX_FALL_TIME       = 5.5f;
        private const float MAX_FORWARD_TIME    = 7.5f;
        private const float RETURN_ROLL_SPEED   = 180.0f;

        private float m_fallSpeed;
        private float m_fallTimer;

        private float m_initialForwardSpeed;
        private float m_forwardSpeed;
        private float m_forwardTimer;

        public FallingController(SeraphController controller)
            : base(controller)
        {}

        public override void start(TransitionData data)
        {
            Debug.Log(GetType().Name + " received transition data: " + data);
            m_fallTimer           = 0.0f;
            m_initialForwardSpeed = data.velocity;
            m_forwardSpeed        = data.velocity;
            m_forwardTimer        = MAX_FORWARD_TIME;
        }

        public override void update()
        {
            m_position = transform.position;
            m_rotation = transform.eulerAngles;

            handleFacing();
            handleMovement(0.75f);
            handleRoll();
            handleFalling();
            handleForwardVelocity();
            handleTransition();

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
            state = SeraphState.GROUNDED;
        }

        public override void collisionExit(Collision other)
        {
            Debug.Log(GetType().Name + " collisionExit()");
        }

        public override TransitionData transitionData()
        {
            return new TransitionData { direction = Vector3.down, velocity = getActualSpeed() };
        }

        private void handleFalling()
        {
            m_fallTimer += Time.deltaTime;

            if (m_fallTimer > MAX_FALL_TIME)
                m_fallTimer = MAX_FALL_TIME;

            m_fallSpeed = Helpers.quadraticInterpIn(START_FALL_VELOCITY, MAX_FALL_VELOCTY, m_fallTimer, MAX_FALL_TIME);

            m_position.y -= m_fallSpeed * Time.deltaTime;
        }

        private void handleForwardVelocity()
        {
            m_forwardTimer -= Time.deltaTime;

            if (m_forwardTimer < 0.0f)
                m_forwardTimer = 0.0f;

            m_forwardSpeed = Helpers.quadraticInterpIn(0.0f, m_initialForwardSpeed, m_forwardTimer, MAX_FORWARD_TIME);

            m_position += transform.forward * m_forwardSpeed * Time.deltaTime;
        }

        private void handleRoll()
        {
            float angleStep = RETURN_ROLL_SPEED * Time.deltaTime;
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
            if (Helpers.nearestHit(transform.position, Vector3.down, height).HasValue)
                state = SeraphState.GROUNDED;

            if (inputManager.actionFired(Action.GLIDE_STATE) && capability >= SeraphCapability.GLIDE && getActualSpeed() > MIN_GLIDE_VELOCITY)
                state = SeraphState.GLIDING;

            if (inputManager.actionFired(Action.FLIGHT_STATE) && capability >= SeraphCapability.FLIGHT)
                state = SeraphState.FLYING;
        }

        private float getActualSpeed()
        {
            return (float) Math.Sqrt(Math.Pow(m_fallSpeed, 2.0f) + Math.Pow(m_forwardSpeed, 2.0f));
        }
    }
}