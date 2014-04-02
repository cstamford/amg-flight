//==========================================================
// Author: Sean Vieira
// Version: 2.0
// Function: Handles puzzles that require a code to
// be entered in a specific order, which then triggers
// and event
//==========================================================

using UnityEngine;
using System;
using sv.Triggers;

namespace sv.Puzzles
{
    public class PuzzlePassword : MonoBehaviour
    {
        [SerializeField] private string m_keypadPassword;
        [SerializeField] private GameObject m_triggerTarget;
        private TriggerController m_triggerController;
        private string m_userPassword;
        private bool m_trigger;
        
        // Use this for initialization
        void Start()
        {
            m_trigger = false;
            m_userPassword = ""; 
            
            m_triggerController = GetComponent<TriggerController>();
            if (!m_triggerController)
            {
                enabled = false;
                throw new Exception("No trigger object script attached to GameObject");
            }          
        }

        void Update()
        {
            if (m_userPassword.Length == m_keypadPassword.Length)
            {
                if (!m_trigger)
                {
                    if (ComparePasswords())
                    {
                       ActivateTrigger();
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

        private void ActivateTrigger()
        {
            m_trigger = true;

            if (m_triggerTarget)
            {
                Debug.Log("Activating trigger...");
                if (!m_triggerController.ActivateTrigger(m_triggerTarget))
                {
                    Debug.Log("Error activating trigger");
                }
            }
        }
    }
}