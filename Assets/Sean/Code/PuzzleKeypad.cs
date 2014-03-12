//==========================================================
// Author: Sean Vieira
// Version: 1.0
// Function: Handles the keypad object. This stores the
// required password (that can be edited in the editor)
// and a copy of the keypadGUI class
//==========================================================

using UnityEngine;
using System.Collections;

namespace sv
{
    public class PuzzleKeypad : MonoBehaviour
    {
        [SerializeField] private string keypadPassword;

        private const int m_numOfKeys = 10;
        public GameObject[] m_keys;

        private string m_userPassword;
        private KeypadGUI m_keypadGUI;
        private bool m_trigger;
        
        // Use this for initialization
        void Start()
        {
            m_trigger = false;
            m_keypadGUI = GetComponent<KeypadGUI>();
            m_userPassword = "";

        }

        // Update is called once per frame
        void Update()
        {
            if (m_keypadGUI.PasswordIsEntered())
            {
                SetUserPassword();                
            }

            // Prints a statement in debug
            // TODO: Set off a trigger

            if (ComparePasswords())
            {
                if (!m_trigger)
                {
                    m_trigger = true;
                    Debug.Log("It's a match!");
                }
            }

            
            
        }

        public bool ComparePasswords()
        {
            if (m_userPassword.Equals(keypadPassword))
            {
                return true;
            }

            return false;
        }

        public string GetUserPassword()
        {
            return m_userPassword;
        }

        public void SetUserPassword()
        {
            m_userPassword = m_keypadGUI.GetInput();
        }

        public void DisplayTextTip(bool b)
        {
            m_keypadGUI.ShowTextTip(b);
        }

        public void DisplayGUI(bool b)
        {
            m_keypadGUI.ShowGUI(b);
        }

        public int GetNumOfKeys()
        {
            return m_numOfKeys;
        }
    }
}