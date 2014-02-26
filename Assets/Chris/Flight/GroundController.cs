using UnityEngine;

namespace cst.Flight
{
    public class GroundController : ControllerBase, IControllerBase
    {
        public GroundController(SeraphController controller)
            : base(controller)
        {}

        public void start()
        {
        }

        public void update()
        {
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
    }
}