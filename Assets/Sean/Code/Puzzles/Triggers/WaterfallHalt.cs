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
        [SerializeField] private float m_stopSpeed;
        private bool m_isTriggered;
        private ParticleEmitter m_emitter;
        private float m_deactivateTimer;

        // Use this for initialization
        void Start()
        {
            m_isTriggered = false;
            m_emitter = GetComponent<ParticleEmitter>();
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

                    if (m_deactivateTimer >= 5.0f)
                    {
                        this.gameObject.SetActive(false);
                    }
                }
                else
                {

                }
            }

            m_deactivateTimer += m_deactivateTimer * Time.deltaTime;
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
            if (m_emitter.maxEmission > 0.0f)
            {
                m_emitter.maxEmission -= m_stopSpeed * Time.deltaTime;
                return false;
            }

            return true;
        }
    }
}