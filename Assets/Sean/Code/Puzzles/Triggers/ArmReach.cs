//=================================================================
// Author: Sean Vieira
// Version: 1.0
// Function: Plays the animation of the arm model when toggled
//=================================================================


using UnityEngine;
using System.Collections;

namespace sv.Triggers
{
    public class ArmReach : MonoBehaviour
    {

        [SerializeField]
        private GameObject m_animatedMesh;
        private bool m_isTriggered;
        private bool m_animIsReversed;
        private bool m_animIsDone;
        private bool m_animStarted;


        // Use this for initialization
        void Start()
        {
            m_isTriggered = false;
            m_animIsReversed = false;
            m_animIsDone = false;
            m_animStarted = false;
            m_animatedMesh.SetActive(false);
        }

        // Update is called once per frame
        void Update()
        {
            if (m_isTriggered)
            {
                if (PlayingAnimation())
                {
                    m_isTriggered = false;
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

        // Sets animation variables depending on if the animation is to be played forwards or backwards
        public bool IsReversed
        {
            get { return m_animIsReversed; }
            set
            {
                m_animIsReversed = value;
                if (m_animIsReversed)
                {
                    m_animatedMesh.animation["Reach"].speed = 1;
                    if (!m_animatedMesh.animation.IsPlaying("Reach"))
                    {
                        m_animatedMesh.animation["Reach"].time = 0;
                    }
                }
                else
                {
                    m_animatedMesh.animation["Reach"].speed = -1;
                    if (!m_animatedMesh.animation.IsPlaying("Reach"))
                    {
                        m_animatedMesh.animation["Reach"].time = m_animatedMesh.animation["Reach"].length;
                    }
                }
            }
        }

        public bool IsDone
        {
            get { return m_animIsDone; }
            set
            {
                m_animIsDone = value;
            }
        } 

        // Returns false until animation cycle has finished
        private bool PlayingAnimation()
        {
            if (m_animIsDone)
            {
                return true;
            }
            else
            {
                if (!m_animatedMesh.animation.IsPlaying("Reach"))
                {
                    if (m_animStarted)
                    {
                        m_animIsDone = true;
                    }
                }
                else
                {
                    if (!m_animStarted)
                    {
                        m_animatedMesh.animation.Play("Reach");
                        m_animStarted = true;
                    }                    
                }                
            }

            return false;
        }

    }
}