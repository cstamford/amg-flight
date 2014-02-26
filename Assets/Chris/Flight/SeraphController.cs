// Controller for the main character - a Seraph.
// Handles gliding, flight, and ground movement mechanics.
//
// Requires a box collider and kinematic rigid body on script object.
// The rigid body needs constraints on all rotational axes.

using System;
using UnityEngine;

namespace cst.Flight
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

    public class SeraphController : MonoBehaviour
    {
        [SerializeField] private SeraphCapability m_capability 
            = SeraphCapability.NONE;
        [SerializeField] private SeraphState m_state = SeraphState.NONE;

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

            if (collider == null)
            {
                enabled = false;
                throw new Exception("No collider attached.");
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

        public void OnCollisionExit(Collision other)
        {
            m_activeController.collisionExit(other);

            // Hack - remove rigidbody velocity
            rigidbody.velocity = new Vector3();
        }

        public void OnTriggerEnter(Collider other)
        {
            m_activeController.triggerEnter(other);
        }

        public void OnTriggerExit(Collider other)
        {
            m_activeController.triggerExit(other);
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