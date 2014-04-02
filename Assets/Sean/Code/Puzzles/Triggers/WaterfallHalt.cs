//=================================================================
// Author: Sean Vieira
// Version: 1.0
// Function: Cause a waterfall particle system to slow to a stop
//=================================================================

using UnityEngine;
using System.Collections;

namespace sv.Triggers
{
    public class WaterfallHalt : MonoBehaviour
    {
        private bool m_isTriggered;

        // Use this for initialization
        void Start()
        {
            m_isTriggered = false;
            
        }

        // Update is called once per frame
        void Update()
        {
            if (m_isTriggered)
            {

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
    }
}