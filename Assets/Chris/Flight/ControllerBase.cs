﻿using UnityEngine;

namespace cst.Flight
{
    public interface IControllerBase
    {
        void start();
        void update();
        void triggerEnter(Collider other);
        void triggerExit(Collider other);
        void collisionEnter(Collision other);
        void collisionExit(Collision other);
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
}