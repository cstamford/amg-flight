﻿//=================================================================
// Author: Sean Vieira
// Version: 1.0
// Function: Cause a door gameobject to slide from a closed 
// position to an open one
//=================================================================

using UnityEngine;
using System.Collections;

namespace sv.Triggers
{
    public class DoorOpen : MonoBehaviour
    {
        [SerializeField] private float m_openedHeight;
        [SerializeField] private float m_moveSpeed;
        private float m_closedHeight; 
        private float m_currentHeight;
        private bool m_isTriggered;


        // Use this for initialization
        void Start()
        {
            m_isTriggered = false;
            m_closedHeight = this.transform.position.y;
            m_currentHeight = m_closedHeight;
        }

        // Update is called once per frame
        void Update()
        {
            if (m_isTriggered)
            {
                if (OpenDoor())
                {
                    m_isTriggered = false;
                }
                else
                {
                    Transform newPos = transform;
                    newPos.position = new Vector3(transform.position.x, m_currentHeight, transform.position.z);

                    transform.position = newPos.position;
                }
            }
        }

        public bool ActivateTrigger
        {
            get { return m_isTriggered; }
            set
            {
                m_isTriggered = value;
            }
        }

        // Opens the door until reached max height, then returns true
        private bool OpenDoor()
        {
            if (m_currentHeight != m_openedHeight)
            {
                m_currentHeight += m_moveSpeed * Time.deltaTime;
                return false;
            }              
        
            return true;
        }

        // Opens the door until reached max height, then returns true
        private bool CloseDoor()
        {
            if (m_currentHeight != m_closedHeight)
            {
                m_currentHeight -= m_moveSpeed * Time.deltaTime;
                return false;
            }

            return true;
        }
        
    }
}