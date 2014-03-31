//=================================================================
// Author: Sean Vieira
// Function: Cause a door gameobject to slide from one position
// to another
//=================================================================

using UnityEngine;
using System.Collections;

namespace sv.Triggers
{
    public class DoorOpen : MonoBehaviour
    {
        private float m_startHeight;
        private float m_endHeight;
        private float m_moveTimer;
        private float m_moveSpeed;
        private bool m_isTriggered;


        // Use this for initialization
        void Start()
        {
            m_isTriggered = false;
            m_moveTimer = 0.0f;
            m_moveSpeed = 1.0f;
            m_startHeight = this.transform.position.y;
            m_endHeight = this.transform.position.y + 5.0f;
        }

        // Update is called once per frame
        void Update()
        {
            m_moveTimer += Time.deltaTime * m_moveSpeed;

            if (m_isTriggered)
            {
                if (m_startHeight != m_endHeight)
                {
                    Mathf.Lerp(m_startHeight, m_endHeight, m_moveTimer);
                }
                else
                {
                    m_isTriggered = false;
                }
            }
        }
    }
}