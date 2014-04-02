//==========================================================
// Author: Sean Vieira
// Version: 2.0
// Function: Handles puzzles that require a code to
// be entered in a specific order, which then triggers
// and event
//==========================================================

using UnityEngine;
using System.Collections;
using sv.Triggers;

namespace sv.Puzzles
{
    public class PuzzlePassword : MonoBehaviour
    {
        [SerializeField] private string m_keypadPassword;
        [SerializeField] private GameObject m_triggerTarget;
        [SerializeField] private TriggerType m_triggerType;
        private string m_userPassword;
        private bool m_trigger;
        
        // Use this for initialization
        void Start()
        {
            m_trigger = false;
            m_userPassword = "";            
        }

        // Update is called once per frame
        void Update()
        {
            if (m_userPassword.Length == m_keypadPassword.Length)
            {
                Debug.Log("Passwords are the same length!");

                if (!m_trigger)
                {
                    if (ComparePasswords())
                    {
                        m_trigger = true;
                        Debug.Log("Trigger has been activated!");
                        ActivateTrigger();
                    }
                }
                else
                {
                    Debug.Log("Trigger has already been activated!");
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
            if (m_triggerTarget)
            {
                /* Cause an event to fire */
                m_triggerTarget.SetActive(false); // <- Temp
            }
        }
    }
}