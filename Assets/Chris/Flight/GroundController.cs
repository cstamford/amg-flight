// ==================================================================== \\
// File   : GroundController.cs                                         \\
// Author : Christopher Stamford									    \\
//                                                                      \\
// GroundController.cs provides functionality for the GROUNDED state.   \\
//                                                                      \\
// It allows ground movement and deals with collision detection, and    \\
// ensuring that the Seraph remains at the correct height above the     \\
// ground at all times.                                                 \\
//                                                                      \\
// This controller can transition to the following states:              \\
//   - FALLING                                                          \\
//   - FLYING                                                           \\
// ==================================================================== \\

using cst.Common;
using UnityEngine;

namespace cst.Flight
{
    public class GroundController : SharedGroundControls
    {
        private const float HEIGHT_INTERP_STEP = 25.0f;
        private float m_desiredHeight;

        public GroundController(SeraphController controller)
            : base(controller)
        { }

        public override void start(TransitionData data)
        {
            Debug.Log(GetType().Name + " received transition data: " + data);
        }

        public override void update()
        {
            m_position      = transform.position;
            m_rotation      = transform.eulerAngles;
            m_desiredHeight = m_position.y;

            handleFacing();
            handleMovement();
            interpolateHeight();
            handleTransition();

            transform.eulerAngles = m_rotation;
            transform.position    = m_position;
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
            return new TransitionData { direction = Vector3.zero, velocity = 0.0f };
        }

        private void interpolateHeight()
        {
            float? distanceToGround = Helpers.nearestHit(transform.position, Vector3.down, height * 2.0f);

            if (distanceToGround.HasValue)
                m_desiredHeight = m_position.y - (distanceToGround.Value - height);

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

        private void handleTransition()
        {
            if (!Helpers.nearestHit(transform.position, Vector3.down, height + 5.0f).HasValue)
                state = SeraphState.FALLING;

            if ((inputManager.actionFired(Action.FLIGHT_STATE) && capability >= SeraphCapability.FLIGHT))
                state = SeraphState.FLYING;
        }
    }
}