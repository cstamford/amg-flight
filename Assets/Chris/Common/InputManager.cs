// This is ugly code.

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace cst.Common
{
    public enum Action
    {
        MOVE_FORWARD,
        MOVE_BACKWARD,
        MOVE_LEFT,
        MOVE_RIGHT,

        LOOK_UP,
        LOOK_DOWN,
        LOOK_LEFT,
        LOOK_RIGHT,

        INTERACT
    }

    public class InputManager : MonoBehaviour
    {
        private struct InputAxisTags
        {
            public const string CONTROLLER_LEFT_STICK_X_AXIS  = "ControllerHorizontal";
            public const string CONTROLLER_LEFT_STICK_Y_AXIS  = "ControllerVertical";
            public const string CONTROLLER_RIGHT_STICK_X_AXIS = "CameraHorizontal";
            public const string CONTROLLER_RIGHT_STICK_Y_AXIS = "CameraVertical";
            public const string MOUSE_X_AXIS                  = "Mouse X";
            public const string MOUSE_Y_AXIS                  = "Mouse Y";
        }

        private readonly Dictionary<Action, bool>  m_actions;
        private readonly Dictionary<Action, float> m_actionDeltas;
        private const float MOUSE_SENSITIVITY = 25.0f;

        public InputManager()
        {
            m_actions      = new Dictionary<Action, bool>();
            m_actionDeltas = new Dictionary<Action, float>();
        }

        public bool actionFired(Action action)
        {
            return m_actions.ContainsKey(action) && m_actions[action];
        }

        public float actionDelta(Action action)
        {
            return m_actionDeltas.ContainsKey(action) ? m_actionDeltas[action] : 0.0f;
        }

        private void Update()
        {
            float leftStickX  = Input.GetAxis(InputAxisTags.CONTROLLER_LEFT_STICK_X_AXIS);
            float leftStickY  = Input.GetAxis(InputAxisTags.CONTROLLER_LEFT_STICK_Y_AXIS);
            float rightStickX = Input.GetAxis(InputAxisTags.CONTROLLER_RIGHT_STICK_X_AXIS);
            float rightStickY = Input.GetAxis(InputAxisTags.CONTROLLER_RIGHT_STICK_Y_AXIS);
            float mouseX      = Input.GetAxis(InputAxisTags.MOUSE_X_AXIS) * MOUSE_SENSITIVITY;
            float mouseY      = Input.GetAxis(InputAxisTags.MOUSE_Y_AXIS) * MOUSE_SENSITIVITY;

            // Action states
            m_actions[Action.MOVE_FORWARD]  = Input.GetKey(KeyCode.W) || leftStickY < 0.0f;
            m_actions[Action.MOVE_BACKWARD] = Input.GetKey(KeyCode.S) || leftStickY > 0.0f;
            m_actions[Action.MOVE_LEFT]     = Input.GetKey(KeyCode.A) || leftStickX < 0.0f;
            m_actions[Action.MOVE_RIGHT]    = Input.GetKey(KeyCode.D) || leftStickX > 0.0f;

            m_actions[Action.LOOK_UP]    = Input.GetKey(KeyCode.UpArrow)    || mouseY > 0.0f || rightStickY < 0.0f;
            m_actions[Action.LOOK_DOWN]  = Input.GetKey(KeyCode.DownArrow)  || mouseY < 0.0f || rightStickY > 0.0f;
            m_actions[Action.LOOK_LEFT]  = Input.GetKey(KeyCode.LeftArrow)  || mouseX < 0.0f || rightStickX < 0.0f;
            m_actions[Action.LOOK_RIGHT] = Input.GetKey(KeyCode.RightArrow) || mouseX > 0.0f || rightStickX > 0.0f;

            m_actions[Action.INTERACT] = Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.JoystickButton0);

            // Action deltas
            m_actionDeltas[Action.MOVE_FORWARD]  = (m_actions[Action.MOVE_FORWARD]  ? (leftStickY < 0.0f ? -leftStickY : 1.0f) : 0.0f);
            m_actionDeltas[Action.MOVE_BACKWARD] = (m_actions[Action.MOVE_BACKWARD] ? (leftStickY > 0.0f ? leftStickY  : 1.0f) : 0.0f);
            m_actionDeltas[Action.MOVE_LEFT]     = (m_actions[Action.MOVE_LEFT]     ? (leftStickX < 0.0f ? -leftStickX : 1.0f) : 0.0f);
            m_actionDeltas[Action.MOVE_RIGHT]    = (m_actions[Action.MOVE_RIGHT]    ? (leftStickX > 0.0f ? leftStickX  : 1.0f) : 0.0f);

            m_actionDeltas[Action.LOOK_UP]    = (m_actions[Action.LOOK_UP]    ? (rightStickY < 0.0f ? -rightStickY : mouseY > 0.0f ? mouseY : 1.0f) : 0.0f);
            m_actionDeltas[Action.LOOK_DOWN]  = (m_actions[Action.LOOK_DOWN]  ? (rightStickY > 0.0f ? rightStickY  : mouseY < 0.0f ? -mouseY  : 1.0f) : 0.0f);
            m_actionDeltas[Action.LOOK_LEFT]  = (m_actions[Action.LOOK_LEFT]  ? (rightStickX < 0.0f ? -rightStickX : mouseX < 0.0f ? -mouseX : 1.0f) : 0.0f);
            m_actionDeltas[Action.LOOK_RIGHT] = (m_actions[Action.LOOK_RIGHT] ? (rightStickX > 0.0f ? rightStickX  : mouseX > 0.0f ? mouseX  : 1.0f) : 0.0f);

            // TODO Get controller pressure.
            m_actionDeltas[Action.INTERACT] = m_actions[Action.INTERACT] ? 1.0f : 0.0f;
        }

        private void dumpDebug()
        {
            foreach(Action act in Enum.GetValues(typeof(Action))
                .Cast<Action>()
                .Where(act => m_actions.ContainsKey(act) && m_actionDeltas.ContainsKey((act))))
            {
                Debug.Log(String.Format("{0}: [{1} :: {2}]", act, m_actions[act], m_actionDeltas[act]));
            }
        }
    }
}