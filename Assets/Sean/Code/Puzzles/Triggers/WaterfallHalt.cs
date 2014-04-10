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
        [SerializeField] private GameObject m_partnerFoam;
        [SerializeField] private float m_stopSpeed;
        private bool m_isTriggered;
        private ParticleEmitter m_emitter, m_emitterFoam;
        private float m_deactivateTimer;

        // Use this for initialization
        void Start()
        {
            m_isTriggered = false;
            m_emitter = GetComponent<ParticleEmitter>();
            if (m_partnerFoam != null)
            {
                m_emitterFoam = m_partnerFoam.GetComponent<ParticleEmitter>();
            }
            m_deactivateTimer = 0.0f;
        }

        // Update is called once per frame
        void Update()
        {
            if (m_isTriggered)
            {
                if (GradualReduction())
                {
                    m_isTriggered = false;

                    Collider collider = GetComponent<Collider>();
                    collider.enabled = false;

                    if (m_deactivateTimer >= 50.0f)
                    {
                        this.gameObject.SetActive(false);
                        if (m_partnerFoam != null)
                        { 
                            m_partnerFoam.SetActive(false); 
                        }

                        m_deactivateTimer = 0.0f;
                        m_isTriggered = false;
                    }
                }
            }

            m_deactivateTimer += Time.deltaTime;
        }

        public bool ActivateTrigger
        {
            get { return m_isTriggered; }
            set
            {
                m_isTriggered = value;
            }
        }

        private bool GradualReduction()
        {
            if (m_emitter.maxEmission > 0.0f || m_emitterFoam.maxEmission > 0.0f)
            {
                m_emitter.maxEmission -= m_stopSpeed * Time.deltaTime;
                if (m_emitter.minEmission < m_emitter.maxEmission)
                {
                    m_emitter.minEmission = m_emitter.maxEmission;
                }
                
                m_emitterFoam.maxEmission -= m_stopSpeed * Time.deltaTime;
                if (m_emitterFoam.minEmission < m_emitterFoam.maxEmission)
                {
                    m_emitterFoam.minEmission = m_emitterFoam.maxEmission;
                }

                return false;
            }

            return true;
        }

        public bool GetWaterfallHalt()
        {
            return m_isTriggered && !GradualReduction();
        }
    }
}