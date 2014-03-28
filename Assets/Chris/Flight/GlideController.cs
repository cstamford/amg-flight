// ==================================================================== \\
// File   : GlideController.cs                                          \\
// Author : Christopher Stamford									    \\
//                                                                      \\
// GlideController.cs provides functionality for the GLIDING state.     \\
//                                                                      \\
// It is capable of changing pitch and roll. Yaw is modified based on   \\
// the Seraph's current roll, using constants defined in the class.     \\
//                                                                      \\
// Preserves momentum from the previous state's transition data.        \\
//                                                                      \\
// This controller can transition to the following states:              \\
//   - FALLING                                                          \\
//   - LANDING                                                          \\
//   - FLYING                                                           \\
// ==================================================================== \\

using cst.Common;
using UnityEngine;
using Action = cst.Common.Action;

namespace cst.Flight
{
    public class GlideController : ControllerBase, IControllerBase
    {
        public float forwardSpeed { get; private set; }

        private const float MAX_ROLL_ANGLE        = 37.5f;
        private const float MAX_PITCH_ANGLE       = 65.0f;
        private const float TURN_TIGHTNESS        = 2.0f;
        private const float INCREMENT_TURN_SPEED  = 35.0f;
        private const float MIN_RETURN_TURN_SPEED = 10.0f;
        private const float MAX_RETURN_TURN_SPEED = 30.0f;
        private const float INCREMENT_PITCH_SPEED = 45.0f;
        private const float INCREMENT_VELOCITY    = 40.0f;
        private const float DECREMENT_VELOCITY    = 25.0f;
        private const float RESTING_VELOCITY      = 125.0f;
        private const float MAX_VELOCITY          = RESTING_VELOCITY * 2.0f;
        private const float MIN_VELOCITY          = 0.0f;

        private Vector3 m_position;
        private Vector3 m_rotation;

        public GlideController(SeraphController controller)
            : base(controller)
        { }

        public void start(TransitionData data)
        {
            Debug.Log(GetType().Name + " received transition data: " + data);

            forwardSpeed = data.velocity < RESTING_VELOCITY ? RESTING_VELOCITY : data.velocity;
        }

        public void update()
        {
            m_rotation = transform.eulerAngles;
            m_position = transform.position;

            handleOrientationChange();
            handleMoveForward();
            handleTransition();

            transform.eulerAngles = m_rotation;
            transform.position    = m_position;
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
            state = SeraphState.FALLING;
        }

        public void collisionExit(Collision other)
        {
            Debug.Log(GetType().Name + " collisionExit()");
        }

        public TransitionData transitionData()
        {
            return new TransitionData { direction = transform.forward, velocity = forwardSpeed };
        }

        private void handleOrientationChange()
        {
            handlePitchChange();
            handleYawChange();
            handleRollChange();
        }

        // Handles the change in pitch based on the user input
        private void handlePitchChange()
        {
            if (inputManager.actionFired(Action.MOVE_FORWARD))
            {
                turnVerticalDown(inputManager.actionDelta(Action.MOVE_FORWARD));
            }
            else if (inputManager.actionFired(Action.MOVE_BACKWARD))
            {
                turnVerticalUp(inputManager.actionDelta(Action.MOVE_BACKWARD));
            }
            else
            {
                turnVerticalNone();
            }
        }

        // Handles the change in yaw based on the current roll
        private void handleYawChange()
        {
            float angle = m_rotation.z;

            if (angle > 180.0f)
                angle -= 360.0f;

            m_rotation.y = Helpers.wrapAngle(m_rotation.y - (Time.deltaTime * angle * TURN_TIGHTNESS));
        }

        // Handles the change in roll based on the user input
        private void handleRollChange()
        {
            if (inputManager.actionFired(Action.MOVE_LEFT))
            {
                if (m_rotation.z < 0.0f)
                {
                    turnHorizontalLeft(inputManager.actionDelta(Action.MOVE_LEFT) * 1.5f);
                }
                else
                {
                    turnHorizontalLeft(inputManager.actionDelta(Action.MOVE_LEFT));
                }
            }
            else if (inputManager.actionFired(Action.MOVE_RIGHT))
            {
                if (m_rotation.z > 0.0f)
                {
                    turnHorizontalRight(inputManager.actionDelta(Action.MOVE_RIGHT) * 1.5f);
                }
                else
                {
                    turnHorizontalRight(inputManager.actionDelta(Action.MOVE_RIGHT));
                }
            }
            else
            {
                turnHorizontalNone();
            }
        }

        // Changes pitch based on up user input
        private void turnVerticalUp(float delta)
        {
            float angle = Helpers.getNormalizedAngle(m_rotation.x);

            if (angle > -MAX_PITCH_ANGLE)
                m_rotation.x = Helpers.wrapAngle(m_rotation.x - (delta * Time.deltaTime * INCREMENT_PITCH_SPEED));
        }

        // Changes pitch based on down user input
        private void turnVerticalDown(float delta)
        {
            float angle = Helpers.getNormalizedAngle(m_rotation.x);

            if (angle < MAX_PITCH_ANGLE)
                m_rotation.x = Helpers.wrapAngle(m_rotation.x + (delta * Time.deltaTime * INCREMENT_PITCH_SPEED));
        }

        // Returns pitch to neutral on no user input
        private void turnVerticalNone()
        {
            // Intentionally left empty - no return pitch behaviour
        }

        // Changes roll based on left user input
        private void turnHorizontalLeft(float delta)
        {
            float angle = Helpers.getNormalizedAngle(m_rotation.z);

            if (angle < MAX_ROLL_ANGLE)
                m_rotation.z = Helpers.wrapAngle(m_rotation.z + (delta * Time.deltaTime * INCREMENT_TURN_SPEED));
        }

        // Changes roll based on right user input
        private void turnHorizontalRight(float delta)
        {
            float angle = Helpers.getNormalizedAngle(m_rotation.z);

            if (angle > -MAX_ROLL_ANGLE)
                m_rotation.z = Helpers.wrapAngle(m_rotation.z - (delta * Time.deltaTime * INCREMENT_TURN_SPEED));
        }

        // Returns roll to neutral on no user input
        private void turnHorizontalNone()
        {
            float angle = Helpers.getNormalizedAngle(m_rotation.z);
            float posAngle = angle < 0.0f ? -angle : angle;

            if (posAngle > MAX_RETURN_TURN_SPEED)
                posAngle = MAX_RETURN_TURN_SPEED;

            else if (posAngle < MIN_RETURN_TURN_SPEED)
                posAngle = MIN_RETURN_TURN_SPEED;

            if (angle > 0.0f)
            {
                m_rotation.z -= posAngle * Time.deltaTime;

                if (Helpers.getNormalizedAngle(m_rotation.z) < 0.0f)
                    m_rotation.z = 0.0f;
            }
            else if (angle < 0.0f)
            {
                m_rotation.z += posAngle * Time.deltaTime;

                if (Helpers.getNormalizedAngle(m_rotation.z) > 0.0f)
                    m_rotation.z = 0.0f;
            }
        }

        // Handles moving forward
        private void handleMoveForward()
        {
            // Get the normalized pitch
            float pitch = Helpers.getNormalizedAngle(m_rotation.x);

            // Get rest speed based on pitch
            float restSpeed = RESTING_VELOCITY + ((pitch * MAX_VELOCITY - RESTING_VELOCITY) / MAX_PITCH_ANGLE);

            // Convert pitch to a unit vector
            pitch /= MAX_PITCH_ANGLE;

            // Calculate the step each frame
            float step = pitch * Time.deltaTime;

            // Don't go above the calculated rest speed
            if (forwardSpeed > restSpeed && pitch > 0.0f)
                step = -step;

            if (step > 0.0f)
                step *= INCREMENT_VELOCITY;
            else
                step *= DECREMENT_VELOCITY;

            forwardSpeed += step;

            // Drop out of flight if we stall
            if (forwardSpeed < MIN_VELOCITY)
                state = SeraphState.GROUNDED;

            m_position += transform.forward * forwardSpeed * Time.deltaTime;
        }

        private void handleTransition()
        {
            if (Helpers.nearestHit(transform.position, Vector3.down, height).HasValue)
                state = SeraphState.LANDING;

            if (inputManager.actionFired(Action.CLEAR_STATE))
                state = SeraphState.FALLING;

            if (inputManager.actionFired(Action.FLIGHT_STATE) && capability >= SeraphCapability.FLIGHT)
                state = SeraphState.FLYING;
        }
    }
}