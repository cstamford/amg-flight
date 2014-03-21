using System;
using cst.Common;
using UnityEngine;

namespace cst.Flight
{
    public struct TransitionData
    {
        public float velocity { get; set; }
        public Vector3 direction { get; set; }

        public override string ToString()
        {
            return "Direction: " + direction + " Velocity: " + velocity;
        }
    }

    public interface IControllerBase
    {
        void start(TransitionData data);
        void update();
        void triggerEnter(Collider other);
        void triggerExit(Collider other);
        void collisionEnter(Collision other);
        void collisionExit(Collision other);
        TransitionData transitionData();
    }

    public abstract class ControllerBase
    {
        public SeraphController controller
        {
            get { return m_controller; }
        }

        protected InputManager inputManager
        {
            get { return m_controller.inputManager; }
        }

        protected SeraphState state
        {
            get { return m_controller.state; }
            set { m_controller.state = value; }
        }

        protected SeraphCapability capability
        {
            get { return m_controller.capability; }
            set { m_controller.capability = value; }
        }

        protected Transform transform
        {
            get { return m_controller.transform; }
        }

        protected GameObject gameObject
        {
            get { return m_controller.gameObject; }
        }

        private readonly SeraphController m_controller;

        protected ControllerBase(SeraphController controller)
        {
            if (controller == null)
                throw new ArgumentNullException("Provided controller is null.");

            m_controller = controller;
        }
    }
}