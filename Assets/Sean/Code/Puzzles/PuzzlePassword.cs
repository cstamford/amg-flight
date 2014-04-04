//==========================================================
// Author: Sean Vieira
// Version: 1.1
// Function: Handles puzzles that require a code to
// be entered in a specific order, which then triggers
// and event
//
// Revision History ----------------------------------------
// 1.1: Stores length of target password, number of keys,
//      how many times the keys have been hit and 
//
// 1.0: Ability to compare passwords, and add keys to the
//      user password
//==========================================================

using UnityEngine;
using System;
using sv.Triggers;

namespace sv.Puzzles
{
    public class PuzzlePassword : MonoBehaviour
    {
        // Trigger stuff
        [SerializeField] private GameObject m_triggerTarget;
        private bool m_trigger;
        private TriggerController m_triggerController;

        // Passwords
        [SerializeField] private int m_passwordLength;
        private string m_targetPassword;
        private string m_userPassword;

        // Misc
        private int m_lastKeyValue;
        
        // Use this for initialization
        void Start()
        {
            m_trigger = false;
            m_userPassword = "";
            m_targetPassword = "";
            
            m_triggerController = GetComponent<TriggerController>();
            if (!m_triggerController)
            {
                enabled = false;
                throw new Exception("No trigger object script attached to GameObject");
            }
        }

        void Update()
        {
            if (!m_trigger)
            {
                if (ComparePasswords())
                {
                    ActivateTrigger();
                }
            }
        }

        public bool RemoveLastKeyFromUserPassword()
        {
            //Debug.Log("Removing last key from userpassword");
            if (m_userPassword.Length > 0)
            {
                m_userPassword = m_userPassword.Remove(m_userPassword.Length - 1);
                
                // If the new password is greater than 0, edit the last key value
                if (m_userPassword.Length > 0)
                {
                    m_lastKeyValue = m_userPassword[m_userPassword.Length - 1] - '0';
                }


                return true;
            }

            return false;
        }

        public void AddKeyToUserPassword(int k)
        {
            m_lastKeyValue = k;
            m_userPassword += k; 
            Debug.Log("Added " + k + " to user password. New password is " + m_userPassword);     
        }

        public void AddKeyToTargetPassword(int k)
        {
            if (m_targetPassword.Length < m_passwordLength)
            {
                m_targetPassword += k; 
                Debug.Log("Added " + k + " to target password. New password is " + m_targetPassword);
            }
        }

        public bool ComparePasswords()
        {
            if (m_userPassword.Length >= m_passwordLength)
            {
                if (m_userPassword.Equals(m_targetPassword))
                {
                    return true;
                }
            }
            return false;
        }
        
        public string UserPassword
        {
            get { return m_userPassword; }
        }

        public string TargetPassword
        {
            get { return m_targetPassword; }
        }

        public int LastKeyEntered
        {
            get { return m_lastKeyValue; }
        }

        public int TargetPasswordLength
        {
            get { return m_passwordLength; }
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