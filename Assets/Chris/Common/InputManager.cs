// ==================================================================== \\
// File   : InputManager.cs                                             \\
// Author : Christopher Stamford, additions by Sean Vieira              \\
//                                                                      \\
// InputManager.cs defines the an InputManager class, which is used     \\
// throughout the project for device-independent input.                 \\
//                                                                      \\
// The InputManager should be attached to a GameObject in the scene.    \\
//                                                                      \\
// It provides functionality to query the state of an action (based on  \\
// user input), and the delta of that action - for example, how far the \\
// controller stick has been moved, or how hard a controller button has \\
// been pressed.                                                        \\
// ==================================================================== \\

//==============================================================
// Revision 1.1 by Sean Vieira
//
// - Added in new TOGGLE_STATE action        
// - When Space/RB is pressed switches from FALLING to GLIDING
// - INTERACT now casts a ray for object selection
// - INTERACT buttons are Keyboard-E/360 Pad-A
//==============================================================

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

        ASCEND,
        DESCEND,

        FLIGHT_STATE,
        GLIDE_STATE,
        CLEAR_STATE,

        INTERACT,
        SHOW_MAP,
        EXIT
    }

    public class InputManager : MonoBehaviour
    {
        public struct InputAxisTags
        {
            public const string CONTROLLER_LEFT_STICK_X_AXIS  = "ControllerHorizontal";
            public const string CONTROLLER_LEFT_STICK_Y_AXIS  = "ControllerVertical";
            public const string CONTROLLER_RIGHT_STICK_X_AXIS = "CameraHorizontal";
            public const string CONTROLLER_RIGHT_STICK_Y_AXIS = "CameraVertical";
            public const string MOUSE_X_AXIS                  = "Mouse X";
            public const string MOUSE_Y_AXIS                  = "Mouse Y";
        }

        private readonly Dictionary<Action, bool> m_actions       = new Dictionary<Action, bool>();
        private readonly Dictionary<Action, float> m_actionDeltas = new Dictionary<Action, float>();
        private const float MOUSE_SENSITIVITY = 5.0f;

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

            m_actions[Action.ASCEND]  = Input.GetKey(KeyCode.Q);
            m_actions[Action.DESCEND] = Input.GetKey(KeyCode.Z);

            m_actions[Action.INTERACT]     = Input.GetKey(KeyCode.E)         || Input.GetKey(KeyCode.JoystickButton0); // A
            m_actions[Action.SHOW_MAP]     = Input.GetKeyDown(KeyCode.M)     || Input.GetKeyDown(KeyCode.JoystickButton1); // X
            m_actions[Action.CLEAR_STATE]  = Input.GetKey(KeyCode.Return)    || Input.GetKey(KeyCode.JoystickButton3); // Y
            m_actions[Action.GLIDE_STATE]  = Input.GetKey(KeyCode.Space)     || Input.GetKey(KeyCode.JoystickButton4); // Left bumper
            m_actions[Action.FLIGHT_STATE] = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.JoystickButton5); // Right bumper
            m_actions[Action.EXIT]         = Input.GetKey(KeyCode.Escape)    || Input.GetKey(KeyCode.JoystickButton6); // Back button

            // Action deltas
            m_actionDeltas[Action.MOVE_FORWARD]  = (m_actions[Action.MOVE_FORWARD]  ? (leftStickY < 0.0f ? -leftStickY : 1.0f) : 0.0f);
            m_actionDeltas[Action.MOVE_BACKWARD] = (m_actions[Action.MOVE_BACKWARD] ? (leftStickY > 0.0f ? leftStickY  : 1.0f) : 0.0f);
            m_actionDeltas[Action.MOVE_LEFT]     = (m_actions[Action.MOVE_LEFT]     ? (leftStickX < 0.0f ? -leftStickX : 1.0f) : 0.0f);
            m_actionDeltas[Action.MOVE_RIGHT]    = (m_actions[Action.MOVE_RIGHT]    ? (leftStickX > 0.0f ? leftStickX  : 1.0f) : 0.0f);

            m_actionDeltas[Action.LOOK_UP]    = (m_actions[Action.LOOK_UP]    ? (rightStickY < 0.0f ? -rightStickY : mouseY > 0.0f ? mouseY  : 1.0f) : 0.0f);
            m_actionDeltas[Action.LOOK_DOWN]  = (m_actions[Action.LOOK_DOWN]  ? (rightStickY > 0.0f ? rightStickY  : mouseY < 0.0f ? -mouseY : 1.0f) : 0.0f);
            m_actionDeltas[Action.LOOK_LEFT]  = (m_actions[Action.LOOK_LEFT]  ? (rightStickX < 0.0f ? -rightStickX : mouseX < 0.0f ? -mouseX : 1.0f) : 0.0f);
            m_actionDeltas[Action.LOOK_RIGHT] = (m_actions[Action.LOOK_RIGHT] ? (rightStickX > 0.0f ? rightStickX  : mouseX > 0.0f ? mouseX  : 1.0f) : 0.0f);

            m_actionDeltas[Action.ASCEND]       = m_actions[Action.ASCEND]       ? 1.0f : 0.0f;
            m_actionDeltas[Action.DESCEND]      = m_actions[Action.DESCEND]      ? 1.0f : 0.0f;
            m_actionDeltas[Action.INTERACT]     = m_actions[Action.INTERACT]     ? 1.0f : 0.0f;
            m_actionDeltas[Action.SHOW_MAP]     = m_actions[Action.SHOW_MAP]     ? 1.0f : 0.0f;
            m_actionDeltas[Action.CLEAR_STATE]  = m_actions[Action.CLEAR_STATE]  ? 1.0f : 0.0f;
            m_actionDeltas[Action.GLIDE_STATE]  = m_actions[Action.GLIDE_STATE]  ? 1.0f : 0.0f;
            m_actionDeltas[Action.FLIGHT_STATE] = m_actions[Action.FLIGHT_STATE] ? 1.0f : 0.0f;
            m_actionDeltas[Action.EXIT]         = m_actions[Action.EXIT]         ? 1.0f : 0.0f;
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