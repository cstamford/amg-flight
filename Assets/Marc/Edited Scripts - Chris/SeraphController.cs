// Controller for the main character - a Seraph.
// Handles gliding, flight, and ground movement mechanics.
//
// Requires a box collider and kinematic rigid body on script object.
// The rigid body needs constraints on all rotational axes.

using System;
using UnityEngine;

namespace cst.Flight
{
    public enum SeraphState
    {
        GROUNDED,
        LANDING,
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
        [SerializeField] private SeraphState      m_state      = SeraphState.GROUNDED;

        private GroundController m_groundController;
        private GlideController  m_glideController;		
        private FlightController m_flightController;
        private IControllerBase  m_activeController;
        private bool             m_ambientSoundPlaying;
        private AudioSource      m_ambientSound;

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
            m_glideController  = new GlideController(this);
            m_flightController = new FlightController(this);

            m_ambientSound      = (AudioSource)gameObject.AddComponent("AudioSource");
            m_ambientSound.clip = (AudioClip)Resources.Load("windy ambience");
        }

        public void Update()
        {
            IControllerBase lastController = m_activeController;

            switch (m_state)
            {
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

            if (lastController != m_activeController)
            {
                TransitionData data = lastController == null 
                    ? new TransitionData 
                    { direction = new Vector3(), velocity = 0.0f } 
                    : lastController.transitionData();

                m_activeController.start(data);
            }

            handleAudio();
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

        public void handleAudio()
        {
            if (!m_ambientSoundPlaying)
            {
                m_ambientSound.loop = true;
                m_ambientSound.Play();
				m_ambientSound.volume=0.5f;
                m_ambientSoundPlaying = true;
            }
        }

		public float SeraphGlideVelocity
		{
			get { return m_glideController.ForwardSpeed; }
		}
    }
}