// ==================================================================== \\
// File   : SeraphController.cs                                         \\
// Author : Christopher Stamford									    \\
//                                                                      \\
// SeraphController.cs acts as a controller for all game states.        \\
//                                                                      \\
// It defines all valid states and capabilities for a Seraph.           \\
//                                                                      \\
// It forwards messages from Unity to the currently active specialised  \\
// controller, and it handles transitioning between the controllers in  \\
// a smooth way. It also provides exposes private properties of the     \\
// underlying GameObject for the ControllerBase to use.                 \\
//                                                                      \\
// Requires a collider, non-kinematic rigidbody, and an InputManager to \\
// be attached to the underlying GameObject to function correctly.      \\
// ==================================================================== \\

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

            m_inputManager = m_inputManagerObject.GetComponent<InputManager>();

            if (m_inputManager == null)
            {
                enabled = false;
                throw new Exception("No InputManager script detected on the provided InputManagerObject.");
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
        }

        public void Update()
        {
            IControllerBase lastController = activeController;

            switch (m_state)
            {
                case SeraphState.GROUNDED:
                    activeController = m_groundController;
                    break;

                case SeraphState.FALLING:
                    activeController = m_fallingController;
                    break;

                case SeraphState.LANDING:
                    activeController = m_landingController;
                    break;

                case SeraphState.GLIDING:
                    activeController = m_glideController;
                    break;

                case SeraphState.FLYING:
                    activeController = m_flightController;
                    break;
            }

            if (lastController != activeController)
            {
                TransitionData data = lastController == null 
                    ? new TransitionData { direction = Vector3.zero, velocity = 0.0f } 
                    : lastController.transitionData();

                activeController.start(data);
            }

            activeController.update();
        }

        public void OnCollisionEnter(Collision other)
        {
            activeController.collisionEnter(other);
        }

        public void OnCollisionExit(Collision other)
        {
            activeController.collisionExit(other);

            // Hack - remove rigidbody velocity
            rigidbody.velocity = Vector3.zero;
        }

        public void OnTriggerEnter(Collider other)
        {
            activeController.triggerEnter(other);
        }

        public void OnTriggerExit(Collider other)
        {
            activeController.triggerExit(other);
        }
        public IControllerBase activeController 
        { 
            get; 
            private set; 
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
    }
}