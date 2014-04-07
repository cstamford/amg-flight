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

using System;
using cst.Common;
using UnityEngine;
using Action = cst.Common.Action;

namespace cst.Flight
{
    public class GroundController : SharedGroundControls
    {
        public GroundController(SeraphController controller)
            : base(controller)
        { }

        public override void start(TransitionData data)
        {
            Debug.Log(GetType().Name + " received transition data: " + data);

            const float EPSILON = 0.000001f;
            if (Math.Abs(m_rotation.z) > EPSILON)
                transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, 0.0f);
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

        private void handleTransition()
        {
            if (!Helpers.nearestHitDistance(transform.position, Vector3.down, height + HEIGHT_PADDING).HasValue)
                state = SeraphState.FALLING;

            if ((inputManager.actionFired(Action.FLIGHT_STATE) && capability >= SeraphCapability.FLIGHT))
                state = SeraphState.FLYING;
        }
    }
}