//==========================================================
// Author: Sean Vieira
// Version: 2.0
// Function: Handles the GUI relating to the puzzle 
// objects, including text tips that are displayed if
// the objective of the puzzle is complete or not,
// and an [old] gui that appears if a password is
// needing entered
//==========================================================

using UnityEngine;
using System.Collections;
using System.Text.RegularExpressions;

namespace sv.Puzzles
{
    public class PuzzleGUI : MonoBehaviour
    {
        [SerializeField] private GUIText m_textTip, m_objectiveIncompleteText, m_objectiveCompletedText;
        [SerializeField] private float m_guiWidth;
        [SerializeField] private float m_guiHeight;
        [SerializeField] private Vector3 m_guiPosition;

        private bool m_isEnabled;
        private bool m_passwordEntered;
        private string m_input;
        private string m_name;
        private Vector3 m_pos;

        void Start()
        {
            m_isEnabled = false;
            m_passwordEntered = false;
            m_pos.x = (Screen.width * m_guiPosition.x) - (m_guiWidth / 2);
            m_pos.y = Screen.height - ((Screen.height * m_guiPosition.y) - (m_guiHeight / 2));
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
                    m_input = GUI.TextField(new Rect(m_pos.x, m_pos.y, m_guiWidth, m_guiHeight), m_input, 10);
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
            m_textTip.enabled = b;
        }

        public void ShowIncompletedText(bool b)
        {
            m_objectiveIncompleteText.enabled = b;
        }

        public void ShowCompletedText(bool b)
        {
            m_objectiveCompletedText.enabled = b;
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