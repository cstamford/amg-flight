﻿//==========================================================
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
        [SerializeField] private string m_keypadPassword;
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
            if (m_userPassword.Length == m_keypadPassword.Length)
            {
                if (!m_trigger)
                {
                    if (ComparePasswords())
                    {
                        m_trigger = true;

                        DisplayCorrectPasswordText(true);
                    }
                    else
                    {
                        DisplayIncorrectPasswordText(true);
                    }
                }
            }   
        }

        public void AddKeyToPassword(int k)
        {
            m_userPassword += k;
        }

        public bool ComparePasswords()
        {
            
            if (m_userPassword.Equals(m_keypadPassword))
            {
                Debug.Log("User entered " + m_userPassword);
                Debug.Log("Password is " + m_keypadPassword);                
                return true;
            }
            else
            {
                Debug.Log("User entered " + m_userPassword);
                Debug.Log("Password is " + m_keypadPassword);
                m_userPassword = "";
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

        public void DisplayIncorrectPasswordText(bool b)
        {
            m_keypadGUI.ShowIncorrectPasswordText(b);
        }

        public void DisplayCorrectPasswordText(bool b)
        {
            m_keypadGUI.ShowCorrectPasswordText(b);
        }

        public void DisplayGUI(bool b)
        {
            m_keypadGUI.ShowGUI(b);
        }
    }
}