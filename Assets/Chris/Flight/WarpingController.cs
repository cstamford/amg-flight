using UnityEngine;

namespace cst.Flight
{
    public class WarpingController : SharedGroundControls
    {
        public WarpingController(SeraphController controller)
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

            handleFacing();

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
    }
}
