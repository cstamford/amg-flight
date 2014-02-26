// Controller for the main character - a Seraph.
// Handles gliding, flight, and ground movement mechanics.
//
// Requires a box collider and kinematic rigid body on script object.
// The rigid body needs constraints on all axes.

using System;
using UnityEngine;

namespace cst
{
    // Controller determines state if NONE.
    public enum SeraphState
    {
        NONE,
        GROUNDED,
        FALLING,
        GLIDING,
        FLYING
    }

    public enum SeraphCapability
    {
        NONE,
        GLIDE,
        FLIGHT
    }

    public interface IControllerBase
    {
        void update();
        void triggerEnter(Collider other);
        void collisionEnter(Collision other);
    }

    public abstract class ControllerBase
    {
        public SeraphController controller
        {
            get { return m_controller; }
        }

        protected Transform transform
        {
            get { return m_controller.getTransform(); }
        }

        protected SeraphState state
        {
            get { return m_controller.getState(); }
            set { m_controller.setState(value); }
        }

        protected SeraphCapability capability
        {
            get { return m_controller.getCapability(); }
            set { m_controller.setCapability(value); }
        }

        private readonly SeraphController m_controller;

        protected ControllerBase(SeraphController controller)
        {
            m_controller = controller;
        }
    }

    public class GroundController : ControllerBase, IControllerBase
    {
        public GroundController(SeraphController controller)
            : base(controller)
        {}

        public void update()
        {
        }

        public void triggerEnter(Collider other)
        {
            Debug.Log(GetType().Name + " triggerEnter()");
        }

        public void collisionEnter(Collision other)
        {
            Debug.Log(GetType().Name + " collisionEnter()");
        }
    }

    public class GlideController : ControllerBase, IControllerBase
    {
        private const float MAX_ROLL_ANGLE = 37.5f;
        private const float MAX_PITCH_ANGLE = 65.0f;
        private const float TURN_TIGHTNESS = 2.0f;
        private const float INCREMENT_TURN_SPEED = 35.0f;
        private const float RETURN_TURN_SPEED = 30.0f;
        private const float INCREMENT_PITCH_SPEED = 45.0f;
        private const float INCREMENT_FOWARD_SPEED = 15.0f;
        private const float RESTING_SPEED = 100.0f;
        private const float MAX_SPEED = 250.0f;

        private Vector3 m_position;
        private Vector3 m_rotation;

        private float m_forwardSpeed = RESTING_SPEED;

        public GlideController(SeraphController controller)
            : base(controller)
        {}

        public void update()
        {
            m_rotation = transform.eulerAngles;
            m_position = transform.position;

            handleOrientationChange(Time.deltaTime);
            handleMoveForward(Time.deltaTime);
        }

        public void triggerEnter(Collider other)
        {
            Debug.Log(GetType().Name + " triggerEnter()");
        }

        public void collisionEnter(Collision other)
        {
            Debug.Log(GetType().Name + " collisionEnter()");

            transform.position -= transform.forward * m_forwardSpeed 
                * Time.deltaTime * 2.0f;
        }

        private void handleOrientationChange(float delta)
        {
            handlePitchChange(delta);
            handleYawChange(delta);
            handleRollChange(delta);

            transform.eulerAngles = m_rotation;
        }

        // Handles the change in pitch based on the user input
        private void handlePitchChange(float delta)
        {
            if (Input.GetKey(KeyCode.W))
            {
                turnVerticalDown(delta);
            }
            else if (Input.GetKey(KeyCode.S))
            {
                turnVerticalUp(delta);
            }
            else
            {
                turnVerticalNone(delta);
            }
        }

        // Handles the change in yaw based on the current roll
        private void handleYawChange(float delta)
        {
            float angle = m_rotation.z;

            if (angle > 180.0f)
                angle -= 360.0f;

            m_rotation.y = Helpers.wrapAngle(m_rotation.y 
                - (delta * angle * TURN_TIGHTNESS));
        }

        // Handles the change in roll based on the user input
        private void handleRollChange(float delta)
        {
            if (Input.GetKey(KeyCode.A))
            {
                if (m_rotation.z < 0.0f)
                {
                    turnHorizontalLeft(delta * 1.25f);
                }
                else
                {
                    turnHorizontalLeft(delta);
                }
            }
            else if (Input.GetKey(KeyCode.D))
            {
                if (m_rotation.z > 0.0f)
                {
                    turnHorizontalRight(delta * 1.25f);
                }
                else
                {
                    turnHorizontalRight(delta);
                }
            }
            else
            {
                turnHorizontalNone(delta);
            }
        }

        // Changes pitch based on up user input
        private void turnVerticalUp(float delta)
        {
            float angle = Helpers.getNormalizedAngle(m_rotation.x);

            if (angle > -MAX_PITCH_ANGLE)
            {
                m_rotation.x = Helpers.wrapAngle(m_rotation.x 
                    - (delta * INCREMENT_PITCH_SPEED));
            }
        }

        // Changes pitch based on down user input
        private void turnVerticalDown(float delta)
        {
            float angle = Helpers.getNormalizedAngle(m_rotation.x);

            if (angle < MAX_PITCH_ANGLE)
            {
                m_rotation.x = Helpers.wrapAngle(m_rotation.x 
                    + (delta * INCREMENT_PITCH_SPEED));
            }
        }

        // Returns pitch to neutral on no user input
        private void turnVerticalNone(float delta)
        {
            // Intentionally left empty - no return pitch behaviour
        }

        // Changes roll based on left user input
        private void turnHorizontalLeft(float delta)
        {
            float angle = Helpers.getNormalizedAngle(m_rotation.z);

            if (angle < MAX_ROLL_ANGLE)
            {
                m_rotation.z = Helpers.wrapAngle(m_rotation.z 
                    + (delta * INCREMENT_TURN_SPEED));
            }
        }

        // Changes roll based on right user input
        private void turnHorizontalRight(float delta)
        {
            float angle = Helpers.getNormalizedAngle(m_rotation.z);

            if (angle > -MAX_ROLL_ANGLE)
            {
                m_rotation.z = Helpers.wrapAngle(
                    m_rotation.z - (delta * INCREMENT_TURN_SPEED));
            }
        }

        // Returns roll to neutral on no user input
        private void turnHorizontalNone(float delta)
        {
            float angle = m_rotation.z - 180.0f;

            if (angle > 0.0f)
            {
                m_rotation.z = Helpers.wrapAngle(m_rotation.z 
                    + delta * RETURN_TURN_SPEED);

                if (m_rotation.z - 180.0f < 0.0f)
                    m_rotation.z = 0.0f;
            }
            else
            {
                m_rotation.z = Helpers.wrapAngle(m_rotation.z 
                    - delta * RETURN_TURN_SPEED);

                if (m_rotation.z - 180.0f > 0.0f)
                    m_rotation.z = 0.0f;
            }

        }

        // Handles moving forward
        private void handleMoveForward(float delta)
        {
            if (m_forwardSpeed > MAX_SPEED)
                m_forwardSpeed = MAX_SPEED;

            if (m_forwardSpeed < 0.0f)
                m_forwardSpeed = 0.0f;

            m_position += transform.forward * m_forwardSpeed 
                * delta;

            transform.position = m_position;
        }
    }

    public class FlightController : ControllerBase, IControllerBase
    {
        private readonly SeraphController m_parent;

        public FlightController(SeraphController controller)
            : base(controller)
        {}

        public void update()
        {
        }

        public void triggerEnter(Collider other)
        {
            Debug.Log(GetType().Name + " triggerEnter()");
        }

        public void collisionEnter(Collision other)
        {
            Debug.Log(GetType().Name + " collisionEnter()");
        }
    }

    public class SeraphController : MonoBehaviour
    {
        [SerializeField] private SeraphCapability m_capability 
            = SeraphCapability.NONE;

        private SeraphState m_state = SeraphState.NONE;

        private GroundController m_groundController;
        private GlideController m_glideController;
        private FlightController m_flightController;
        private IControllerBase m_activeController;

        public void Start()
        {
            if (rigidbody == null)
            {
                enabled = false;
                throw new Exception("No rigidbody attached.");
            }

            if (rigidbody.isKinematic)
            {
                enabled = false;
                throw new Exception("Attached rigidbody is kinematic.");
            }

            if (rigidbody.useGravity)
            {
                Debug.LogWarning("Gravity enabled on rigidbody - Seraph" +
                                 "Controller has its own implementation.");
            }

            m_groundController = new GroundController(this);
            m_glideController = new GlideController(this);
            m_flightController = new FlightController(this);
        }

        public void Update()
        {
            switch (m_state)
            {
                case SeraphState.NONE:
                    // TODO Determine new state automatically.
                    m_activeController = m_glideController;
                    break;

                case SeraphState.GLIDING:
                    m_activeController = m_glideController;
                    break;

                case SeraphState.FLYING:
                    m_activeController = m_flightController;
                    break;

                default:
                    m_activeController = m_groundController;
                    break;
            }

            m_activeController.update();
        }

        public void OnCollisionEnter(Collision other)
        {
            m_activeController.collisionEnter(other);
        }

        public void OnCollisionStay(Collision other)
        {
            // Something very bad has happened here. 
            // TODO Teleport player to nearest safe point.
            Debug.LogWarning("Seraph is inside an object");
        }

        public void OnTriggerEnter(Collider other)
        {
            m_activeController.triggerEnter(other);
        }

        public SeraphState getState()
        {
            return m_state;
        }

        public void setState(SeraphState state)
        {
            m_state = state;
        }

        public SeraphCapability getCapability()
        {
            return m_capability;
        }

        public void setCapability(SeraphCapability capability)
        {
            m_capability = capability;
        }

        public Transform getTransform()
        {
            return transform;
        }
    }
}