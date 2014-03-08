//==========================================================
// Author: Sean Vieira
// Version: 1.0
// Function: Handles the GUI relating to the keypad, 
// including the text tip that is shown when the keypad
// is pressed and the input of the password to a textfield
//==========================================================

using UnityEngine;
using System.Collections;
using System.Text.RegularExpressions;

namespace sv
{
    public class KeypadGUI : MonoBehaviour
    {
        public GUIText textTip;
        [SerializeField] private float guiWidth;
        [SerializeField] private float guiHeight;
        [SerializeField] private Vector3 guiPosition;

        private bool m_isEnabled;
        private bool m_passwordEntered;
        private string m_input;
        private string m_name;
        private Vector3 m_pos;

        void Start()
        {
            m_isEnabled = false;
            m_passwordEntered = false;
            m_pos.x = (Screen.width * guiPosition.x) - (guiWidth / 2);
            m_pos.y = Screen.height - ((Screen.height * guiPosition.y) - (guiHeight / 2));
            m_pos.z = 0.0f;

            m_input = "";
            m_name = "inputPW";
        }

        // Main function - all others are helpers
        void OnGUI()
        {
            Event e = Event.current;

            // Show GUI if it has been enabled
            if (IsEnabled())
            {
                GUI.SetNextControlName(m_name);
                GUI.FocusControl(m_name);

                if (e.keyCode == KeyCode.Return)
                {
                    m_isEnabled = false;
                    m_passwordEntered = true;
                }
                else
                {                    
                    // Limit text field to numbers only
                    m_input = GUI.TextField(new Rect(m_pos.x, m_pos.y, guiWidth, guiHeight), m_input, 10);
                    m_input = Regex.Replace(m_input, @"[^0-9]", "");

                    m_passwordEntered = false;
                }
            }
        }

        public bool IsEnabled()
        {
            return m_isEnabled;
        }

        public void ShowGUI(bool b)
        {
            m_isEnabled = b;
        }

        public void ShowTextTip(bool b)
        {
            textTip.enabled = b;
        }

        public string GetInput()
        {
            return m_input;
        }

        public bool PasswordIsEntered()
        {
            return m_passwordEntered;
        }
    }
}