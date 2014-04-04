//=================================================================
// Author: Sean Vieira
// Version: 1.0
// Function: Cause a door gameobject to slide from a closed 
// position to an open one
//=================================================================

using UnityEngine;
using System.Collections;

namespace sv.Triggers
{
    public class ToggleMaterial : MonoBehaviour
    {
        private Material m_newMaterial;
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
                if (ChangeMaterial())
                {
                    m_isTriggered = false;
                }
                else
                {
                    
                }
            }
        }

        public Material TargetMaterial
        {
            get { return m_newMaterial; }
            set
            {
                m_newMaterial = value;
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
        private bool ChangeMaterial()
        {
            MeshRenderer renderer = GetComponent<MeshRenderer>();

            if (renderer != null)
            {
                renderer.material = m_newMaterial;
                return true;
            }

            Debug.Log("GameObject does not have a Mesh Renderer component attached.");
            return false;
        }
        
    }
}