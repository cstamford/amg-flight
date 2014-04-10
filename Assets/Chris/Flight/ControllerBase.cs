// ==================================================================== \\
// File   : ControllerBase.cs                                           \\
// Author : Christopher Stamford									    \\
//                                                                      \\
// ControllerBase.cs contains several pure virtual classes.             \\
//                                                                      \\
// TransitionData is a struct that each controller is required to       \\
// provide on-demand. This allows other controllers to retrieve data    \\
// from the previous controller on a transition change.                 \\
//                                                                      \\
// IControllerBase is an interface defining the public interface each   \\
// controller should implement. This interface provides a way for the   \\
// SeraphController to forward events on to the specialised controller. \\
//                                                                      \\
// ControllerBase should be inherited by all specialised controllers.   \\
// Because only the SeraphController inherits from MonoBehaviour, there \\
// must be a way to dispatch important Unity information regarding the  \\
// Seraph to the specialised controllers. ControllerBase provides this  \\
// functionality.                                                       \\
//                                                                      \\
// SharedGroundControls is an abstract class which inherits from        \\
// ControllerBase and implements IControllerBase. Because C# does not   \\
// support multiple inheritance, creating this class is the easiest way \\
// to share common functionality (ground controls) between any classes  \\
// which use that functionality.                                        \\
// ==================================================================== \\

using System;
using cst.Common;
using UnityEngine;
using Action = cst.Common.Action;

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
        void triggerStay(Collider other);
        void triggerExit(Collider other);
        void collisionEnter(Collision other);
        void collisionStay(Collision other);
        void collisionExit(Collision other);
        TransitionData transitionData();
    }

    public abstract class ControllerBase
    {
        protected const float DEFAULT_HEIGHT = 2.0f;
        private readonly SeraphController m_controller;

        protected ControllerBase(SeraphController controller)
        {
            if (controller == null)
                throw new ArgumentNullException("Provided controller is null.");

            m_controller = controller;
        }

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

        protected float height
        {
            get 
            { 
                BoxCollider coll = transform.collider as BoxCollider;

                if (coll != null) 
                    return coll.size.y + 0.1f;

                return DEFAULT_HEIGHT + 0.1f;
            }
        }
    }

    // Hack because C# doesn't support multiple inheritance
    public abstract class SharedGroundControls : ControllerBase, IControllerBase
    {
        public bool moved     { get; private set; }
        public bool sprinting { get; private set; }

        protected Vector3 m_rotation;
        protected Vector3 m_position;
        protected float   m_desiredHeight;

        protected const float  HEIGHT_PADDING = 1.5f;

        private const float    FORWARD_SPEED = 5.0f;
        private const float    STRAFE_SPEED = 5.0f;
        private const float    LOOK_SENSITIVITY = 100.0f;
        private const float    INTERP_VALUE = 12.5f;
        private readonly float MOVEMENT_DELTA;

        protected SharedGroundControls(SeraphController controller, float movementDelta = 1.0f)
            : base(controller)
        {
            MOVEMENT_DELTA = movementDelta;
        }

        protected void handleMovement()
        {
            sprinting = false;
            moved     = false;
            const string WATER_TAG = "WaterObject";
            float movementDelta = MOVEMENT_DELTA;

            if (Helpers.nearestHitDistance(transform.position, Vector3.down, height + 0.1f, WATER_TAG).HasValue)
                movementDelta /= 1.5f;

            if (inputManager.actionFired(Action.SPRINT))
            {
                movementDelta *= 1.75f;
                sprinting = true;
            }

            handleMoving(movementDelta);
            handleStrafing(movementDelta);
        }

        protected void handleFacing()
        {
            Vector3 rotation = m_rotation;

            if (inputManager.actionFired(Action.LOOK_UP))
            {
                const float MIN_PITCH = -85.0f;
                float normalisedPitch = Helpers.getNormalizedAngle(rotation.x);
                float change = Time.deltaTime * inputManager.actionDelta(Action.LOOK_UP) * LOOK_SENSITIVITY;

                if (normalisedPitch - change > MIN_PITCH)
                    rotation.x -= change;
            }

            else if (inputManager.actionFired(Action.LOOK_DOWN))
            {
                const float MAX_PITCH = 85.0f;
                float normalisedPitch = Helpers.getNormalizedAngle(rotation.x);
                float change = Time.deltaTime * inputManager.actionDelta(Action.LOOK_DOWN) * LOOK_SENSITIVITY;

                if (normalisedPitch + change < MAX_PITCH)
                    rotation.x += change;
            }

            if (inputManager.actionFired(Action.LOOK_LEFT))
            {
                float change = Time.deltaTime * inputManager.actionDelta(Action.LOOK_LEFT) * LOOK_SENSITIVITY;
                rotation.y -= change;
            }

            else if (inputManager.actionFired(Action.LOOK_RIGHT))
            {
                float change = Time.deltaTime * inputManager.actionDelta(Action.LOOK_RIGHT) * LOOK_SENSITIVITY;
                rotation.y += change;
            }

            m_rotation = rotation;
        }

        protected void interpolateHeight()
        {
            float? distanceToGround = Helpers.nearestHitDistance(transform.position, Vector3.down, height + HEIGHT_PADDING);

            if (distanceToGround.HasValue)
                m_desiredHeight = m_position.y - (distanceToGround.Value - height);

            m_position = Vector3.Lerp(m_position, new Vector3(m_position.x, m_desiredHeight, m_position.z),
                Time.deltaTime * INTERP_VALUE);
        }


        private void handleMoving(float delta)
        { 
            Vector3 movementVec = getMovementVector(transform.forward);

            if (inputManager.actionFired(Action.MOVE_FORWARD))
            {
                m_position += movementVec * FORWARD_SPEED * Time.deltaTime *
                              inputManager.actionDelta(Action.MOVE_FORWARD) *
                              delta;

                moved = true;
            }

            if (inputManager.actionFired(Action.MOVE_BACKWARD))
            {
                m_position -= movementVec * FORWARD_SPEED * Time.deltaTime *
                              inputManager.actionDelta(Action.MOVE_BACKWARD) *
                              delta;

                moved = true;
            }
        }

        private void handleStrafing(float delta)
        {
            Vector3 movementVec = getMovementVector(transform.right);

            if (inputManager.actionFired(Action.MOVE_LEFT))
            {
                m_position -= movementVec * STRAFE_SPEED * Time.deltaTime *
                              inputManager.actionDelta(Action.MOVE_LEFT) *
                              delta;

                moved = true;
            }

            if (inputManager.actionFired(Action.MOVE_RIGHT))
            {
                m_position += movementVec * STRAFE_SPEED * Time.deltaTime *
                              inputManager.actionDelta(Action.MOVE_RIGHT) *
                              delta;

                moved = true;
            }
        }

        private Vector3 getMovementVector(Vector3 originalVector)
        {
            // Strip the height component from the movement vector and normalise it
            originalVector.y = 0.0f;
            originalVector = Vector3.Normalize(originalVector);

            // Check if we're near terrain
            RaycastHit? surface = Helpers.nearestHit(transform.position, Vector3.down, height + HEIGHT_PADDING);

            // If we are, we need to get the vector along the surface of the plane below us
            if (surface.HasValue)
            {
                // Get the normal vector from terrain below us
                Vector3 surfaceNormalVec = surface.Value.normal;

                // MATHS, HOW DOES IT WORK?
                originalVector = Vector3.Cross(
                    Vector3.Cross(surfaceNormalVec, originalVector),
                    surfaceNormalVec);
            }

            return originalVector;
        }

        public abstract void start(TransitionData data);
        public abstract void update();
        public abstract void triggerEnter(Collider other);
        public abstract void triggerStay(Collider other);
        public abstract void triggerExit(Collider other);
        public abstract void collisionEnter(Collision other);
        public abstract void collisionStay(Collision other);
        public abstract void collisionExit(Collision other);
        public abstract TransitionData transitionData();
    }
}