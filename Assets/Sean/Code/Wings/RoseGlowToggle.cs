//===============================================================
// Author: Sean Vieira
// Function: If the player has selected the wings, turn glow off
//===============================================================

using Louis.Common;
using UnityEngine;
using System;

namespace sv
{
    public class RoseGlowToggle : MonoBehaviour 
    {
        [SerializeField] private GameObject m_wingsObject;
        private Light m_glow;
        private WingsController m_wCtrl;

	    // Use this for initialization
	    void Start () 
        {
            if (!m_wingsObject)
            {
                enabled = false;
                throw new Exception("No Wings object attached to script.");
            }
            else
            {
                m_wCtrl = m_wingsObject.GetComponent<WingsController>();
            }

            m_glow = GetComponentInChildren<Light>();
	    }
	
	    // Update is called once per frame
	    void Update () 
        {
            if (m_wCtrl.IsAttachedToPlayer)
            {
                m_glow.enabled = false;
                RotatingObject rotation = GetComponent<RotatingObject>();
                rotation.enabled = false;
            }
	    }
    }
}