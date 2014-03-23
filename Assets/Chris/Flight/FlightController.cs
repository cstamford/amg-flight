using cst.Common;
using UnityEngine;

namespace cst.Flight
{
    public class FlightController : ControllerBase, IControllerBase
    {
        public FlightController(SeraphController controller)
            : base(controller)
        {}

        public void start(TransitionData data)
        {
            Debug.Log(GetType().Name + " received transition data: " + data);
        }

        public void update()
        {
            if (inputManager.actionFired(Action.CLEAR_STATE))
                state = SeraphState.FALLING;
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
            return new TransitionData { direction = Vector3.zero, velocity = 0.0f };
        }
    }
}