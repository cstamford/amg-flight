// Controller for the main character - a Seraph.
// Handles gliding, flight, and ground movement mechanics.
//
// Requires a box collider and kinematic rigid body on script object.
// The rigid body needs constraints on all rotational axes.

using System;
using cst.Common;
using UnityEngine;

namespace cst.Flight
{
    public enum SeraphState
    {
        GROUNDED,
        LANDING,
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

    public class SeraphController : MonoBehaviour
    {
        [SerializeField] private SeraphCapability m_capability = SeraphCapability.GLIDE;
        [SerializeField] private SeraphState      m_state      = SeraphState.FALLING;
        [SerializeField] private GameObject       m_inputManagerObject;
        
        private InputManager      m_inputManager;
        private GroundController  m_groundController;
        private FallingController m_fallingController;
        private LandingController m_landingController;
        private GlideController   m_glideController;		
        private FlightController  m_flightController;
        private IControllerBase   m_activeController;

        public void Start()
        {
            if (collider == null)
            {
                enabled = false;
                throw new Exception("No collider attached.");
            }

            if (m_inputManagerObject == null)
            {
                enabled = false;
                throw new Exception("No input manager added to the Seraph. Please define one in the inspector.");
            }

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

            m_groundController  = new GroundController(this);
            m_fallingController = new FallingController(this);
            m_landingController = new LandingController(this);
            m_glideController   = new GlideController(this);
            m_flightController  = new FlightController(this);

            m_inputManager = m_inputManagerObject.GetComponent<InputManager>();

            if (m_inputManager == null)
            {
                enabled = false;
                throw new Exception("No InputManager script detected on the provided InputManagerObject.");
            }
        }

        public void Update()
        {
            IControllerBase lastController = m_activeController;

            switch (m_state)
            {
                case SeraphState.GROUNDED:
                    m_activeController = m_groundController;
                    break;

                case SeraphState.FALLING:
                    m_activeController = m_fallingController;
                    break;

                case SeraphState.LANDING:
                    m_activeController = m_landingController;
                    break;

                case SeraphState.GLIDING:
                    m_activeController = m_glideController;
                    break;

                case SeraphState.FLYING:
                    m_activeController = m_flightController;
                    break;
            }

            if (lastController != m_activeController)
            {
                TransitionData data = lastController == null 
                    ? new TransitionData { direction = Vector3.zero, velocity = 0.0f } 
                    : lastController.transitionData();

                m_activeController.start(data);
            }

            m_activeController.update();
        }

        public void OnCollisionEnter(Collision other)
        {
            m_activeController.collisionEnter(other);
        }

        public void OnCollisionExit(Collision other)
        {
            m_activeController.collisionExit(other);

            // Hack - remove rigidbody velocity
            rigidbody.velocity = Vector3.zero;
        }

        public void OnTriggerEnter(Collider other)
        {
            m_activeController.triggerEnter(other);
        }

        public void OnTriggerExit(Collider other)
        {
            m_activeController.triggerExit(other);
        }

        public InputManager inputManager
        {
            get { return m_inputManager; }
        }

        public SeraphState state
        {
            get { return m_state; }
            set
            {
                Debug.Log("Seraph state set to " + value);
                m_state = value;
            }
        }

        public SeraphCapability capability
        {
            get { return m_capability; }
            set
            {
                Debug.Log("Seraph capability set to " + value);
                m_capability = value;
            }
        }

        public new Transform transform
        {
            get { return base.transform; }
        }

        public new GameObject gameObject
        {
            get { return base.gameObject; }
        }

		public float SeraphGlideVelocity
		{
			get { return m_glideController.forwardSpeed; }
		}
    }
}