//=================================================================
// Author: Sean Vieira
// Version: 1.0
// Function: Toggles the material of a relic to a new
// one when triggered
//=================================================================

using UnityEngine;
using System.Collections;

namespace sv.Triggers
{
   public class ToggleMaterial : MonoBehaviour
    {
        [SerializeField] private GameObject m_relic;
        private Material m_newMaterial;
        private bool m_isTriggered;


        // Use this for initialization
        void Start()
        {
            m_isTriggered = false;
            m_relic.SetActive(false);
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
                if (m_isTriggered)
                {
                    m_relic.SetActive(true);
                }
                else
                {
                    m_relic.SetActive(false);
                }
            }
        }

        // Opens the door until reached max height, then returns true
        private bool ChangeMaterial()
        {
            MeshRenderer renderer = m_relic.GetComponent<MeshRenderer>();

            if (renderer != null)
            {
                if (renderer.material != m_newMaterial)
                {
                    renderer.material = m_newMaterial;
                }

                return true;
            }

            Debug.Log("GameObject does not have a Mesh Renderer component attached.");
            return false;
        }
        
    }
}