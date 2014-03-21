using cst.Common;
using UnityEngine;

namespace cst.Flight
{
    public class FallingController : SharedGroundControls
    {
        private const float MIN_GLIDE_VELOCITY  = 100.0f;
        private const float START_FALL_VELOCITY = 50.0f;
        private const float MAX_FALL_VELOCTY    = 350.0f;
        private const float MAX_FALL_TIME       = 3.0f;

        private float m_fallSpeed;
        private float m_fallTimer;

        public FallingController(SeraphController controller)
            : base(controller)
        {}

        public override void start(TransitionData data)
        {
            Debug.Log(GetType().Name + " received transition data: " + data);
            m_fallTimer = 0.0f;
        }

        public override void update()
        {
            m_position = transform.position;
            m_rotation = transform.eulerAngles;

            handleFacing();
            handleMovement();
            handleFalling();
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
            return new TransitionData { direction = Vector3.down, velocity = m_fallSpeed };
        }

        private void handleFalling()
        {
            m_fallTimer += Time.deltaTime;

            if (m_fallTimer > MAX_FALL_TIME)
                m_fallTimer = MAX_FALL_TIME;

            m_fallSpeed = Helpers.quadraticInterp(START_FALL_VELOCITY,
                MAX_FALL_VELOCTY, m_fallTimer, MAX_FALL_TIME);

            m_position.y -= m_fallSpeed * Time.deltaTime;
        }

        private void handleTransition()
        {
            float? distanceToGround = Helpers.nearestHit(transform.position, Vector3.down, height);

            if (distanceToGround.HasValue)
            {
                state = SeraphState.GROUNDED;
            }
            else if (inputManager.actionFired(Action.INTERACT) && 
                     capability >= SeraphCapability.GLIDE &&
                     m_fallSpeed > MIN_GLIDE_VELOCITY)
            {
                state = SeraphState.GLIDING;
            }
        }
    }
}