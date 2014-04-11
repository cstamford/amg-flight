// ==================================================================== \\
// File   : InputManager.cs                                             \\
// Author : Christopher Stamford										\\
// Revisions made by : Louis Dimmock & Sean Vieira			            \\
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

//=================================================================
// Revision 1.2 by Louis Dimmock
//
// - Added states for PAUSE, RESTART
// - Created an Xbox struct for easy access to controller keycodes
// - Applied Xbox struct to action states
//=================================================================

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

        GLIDE_STATE,
        CLEAR_STATE,

        SPRINT,

        INTERACT,
		RESTART,
		PAUSE,
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

		public struct Xbox
		{
#if UNITY_STANDALONE_OSX
            public const KeyCode BUTTON_A     = KeyCode.JoystickButton16;
			public const KeyCode BUTTON_B 	  = KeyCode.JoystickButton17;
			public const KeyCode BUTTON_X 	  = KeyCode.JoystickButton18;
			public const KeyCode BUTTON_Y     = KeyCode.JoystickButton19;
            public const KeyCode BUTTON_LB    = KeyCode.JoystickButton13;
            public const KeyCode BUTTON_RB    = KeyCode.JoystickButton14;
            public const KeyCode BUTTON_BACK  = KeyCode.JoystickButton10;
            public const KeyCode BUTTON_START = KeyCode.JoystickButton9;
            public const KeyCode LEFT_STICK   = KeyCode.JoystickButton11;
            public const KeyCode RIGHT_STICK  = KeyCode.JoystickButton12;
#else
			public const KeyCode BUTTON_A 		= KeyCode.JoystickButton0;
			public const KeyCode BUTTON_B 		= KeyCode.JoystickButton1;
			public const KeyCode BUTTON_X 		= KeyCode.JoystickButton2;
			public const KeyCode BUTTON_Y 		= KeyCode.JoystickButton3;
			public const KeyCode BUTTON_LB 		= KeyCode.JoystickButton4;
			public const KeyCode BUTTON_RB 		= KeyCode.JoystickButton5;
			public const KeyCode BUTTON_BACK 	= KeyCode.JoystickButton6;
			public const KeyCode BUTTON_START 	= KeyCode.JoystickButton7;
			public const KeyCode LEFT_STICK 	= KeyCode.JoystickButton8;
			public const KeyCode RIGHT_STICK 	= KeyCode.JoystickButton9;
#endif
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

            m_actions[Action.LOOK_UP]       = Input.GetKey(KeyCode.UpArrow)    || mouseY > 0.0f || rightStickY < 0.0f;
            m_actions[Action.LOOK_DOWN]     = Input.GetKey(KeyCode.DownArrow)  || mouseY < 0.0f || rightStickY > 0.0f;
            m_actions[Action.LOOK_LEFT]     = Input.GetKey(KeyCode.LeftArrow)  || mouseX < 0.0f || rightStickX < 0.0f;
            m_actions[Action.LOOK_RIGHT]    = Input.GetKey(KeyCode.RightArrow) || mouseX > 0.0f || rightStickX > 0.0f;

			m_actions[Action.SPRINT]        = Input.GetKey(KeyCode.LeftShift)  || Input.GetKey(Xbox.BUTTON_RB);

            m_actions[Action.CLEAR_STATE]   = Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(Xbox.BUTTON_Y);
            m_actions[Action.GLIDE_STATE]   = Input.GetKeyDown(KeyCode.Space)  || Input.GetKeyDown(Xbox.BUTTON_A); 
			
			m_actions[Action.INTERACT]      = Input.GetKeyDown(KeyCode.E)      || Input.GetKeyDown(Xbox.BUTTON_X);
			m_actions[Action.PAUSE]         = Input.GetKeyDown(KeyCode.P)      || Input.GetKeyDown(Xbox.BUTTON_START);
			m_actions[Action.SHOW_MAP]      = Input.GetKeyDown(KeyCode.M)      || Input.GetKeyDown(Xbox.BUTTON_B);
			m_actions[Action.EXIT]          = Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(Xbox.BUTTON_BACK); 

            // Action deltas
            m_actionDeltas[Action.MOVE_FORWARD]  = (m_actions[Action.MOVE_FORWARD]  ? (leftStickY < 0.0f ? -leftStickY : 1.0f) : 0.0f);
            m_actionDeltas[Action.MOVE_BACKWARD] = (m_actions[Action.MOVE_BACKWARD] ? (leftStickY > 0.0f ? leftStickY  : 1.0f) : 0.0f);
            m_actionDeltas[Action.MOVE_LEFT]     = (m_actions[Action.MOVE_LEFT]     ? (leftStickX < 0.0f ? -leftStickX : 1.0f) : 0.0f);
            m_actionDeltas[Action.MOVE_RIGHT]    = (m_actions[Action.MOVE_RIGHT]    ? (leftStickX > 0.0f ? leftStickX  : 1.0f) : 0.0f);

            m_actionDeltas[Action.LOOK_UP]       = (m_actions[Action.LOOK_UP]    ? (rightStickY < 0.0f ? -rightStickY : mouseY > 0.0f ? mouseY  : 1.0f) : 0.0f);
            m_actionDeltas[Action.LOOK_DOWN]     = (m_actions[Action.LOOK_DOWN]  ? (rightStickY > 0.0f ? rightStickY  : mouseY < 0.0f ? -mouseY : 1.0f) : 0.0f);
            m_actionDeltas[Action.LOOK_LEFT]     = (m_actions[Action.LOOK_LEFT]  ? (rightStickX < 0.0f ? -rightStickX : mouseX < 0.0f ? -mouseX : 1.0f) : 0.0f);
            m_actionDeltas[Action.LOOK_RIGHT]    = (m_actions[Action.LOOK_RIGHT] ? (rightStickX > 0.0f ? rightStickX  : mouseX > 0.0f ? mouseX  : 1.0f) : 0.0f);

            m_actionDeltas[Action.SPRINT]        = m_actions[Action.SPRINT]      ? 1.0f : 0.0f;

			m_actionDeltas[Action.CLEAR_STATE]   = m_actions[Action.CLEAR_STATE] ? 1.0f : 0.0f;
			m_actionDeltas[Action.GLIDE_STATE]   = m_actions[Action.GLIDE_STATE] ? 1.0f : 0.0f;

			m_actionDeltas[Action.INTERACT]      = m_actions[Action.INTERACT]    ? 1.0f : 0.0f;
			m_actionDeltas[Action.PAUSE]         = m_actions[Action.PAUSE]     	 ? 1.0f : 0.0f;
			m_actionDeltas[Action.SHOW_MAP]      = m_actions[Action.SHOW_MAP]    ? 1.0f : 0.0f;
			m_actionDeltas[Action.RESTART]       = m_actions[Action.RESTART]     ? 1.0f : 0.0f;
            m_actionDeltas[Action.EXIT]          = m_actions[Action.EXIT]        ? 1.0f : 0.0f;
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