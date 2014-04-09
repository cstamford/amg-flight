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

        [SerializeField] private GameObject m_animatedMesh;
        private bool m_isTriggered;

        private bool m_started;
        private bool m_finished;

        private bool m_isIdle;
        private bool m_animIdleEnd;
        private bool m_animIsReversed;
        private bool m_animIsDone;


        // Use this for initialization
        void Start()
        {
            Reset();
        }

        // Update is called once per frame
        void Update()
        {
            if (m_isTriggered)
            {
                if (!m_finished)
                {
                    PlayingAnimation();
                }
                else
                {
                    Reset();
                }
            }
            else
            {

            }
        }

        public bool ActivateTrigger
        {
            get { return m_isTriggered; }
            set
            {
                m_isTriggered = value;
                m_animIsReversed = !value;
                m_animatedMesh.SetActive(true);
            }
        }

        // Sets animation variables depending on if the animation is to be played forwards or backwards
        public bool IsReversed
        {
            get { return m_animIsReversed; }
            set
            {
                m_animIsReversed = value;
                if (!m_animIsReversed)
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
            get { return m_finished; }
            set
            {
                m_finished = value;
            }
        } 

        // Returns false until animation cycle has finished
        private bool PlayingAnimation()
        {
            if (!m_started)
            {
                Debug.Log("Playing animation...");
                PlayAnim();
                m_started = true;
            }
            else
            {

                if (!m_isIdle)
                {
                    if (!m_animIsDone)
                    {
                        if (!m_animatedMesh.animation.IsPlaying("Reach"))
                        {
                            m_animIsDone = true;
                        }
                        else
                        {

                        }
                    }
                    else
                    {
                        Debug.Log("Model is in an idle state...");
                        PlayIdle();
                        m_isIdle = true;
                    }
                }
                else
                {
                    if (!m_animatedMesh.animation.IsPlaying("Idle"))
                    {
                        m_animIdleEnd = true;
                        m_animIsDone = false;
                    }
                }
            }

            return false;
        }

        // Returns false until animation cycle has finished
        private void PlayIdle()
        {
            if (!m_animatedMesh.animation.IsPlaying("Idle"))
            {
                m_animatedMesh.animation.Play("Idle");
            }
        }
        
        // Returns false until animation cycle has finished        
        private void PlayAnim()
        {
            if (!m_animatedMesh.animation.IsPlaying("Reach"))
            {
                m_animatedMesh.animation.Play("Reach");
            }
        }

        private void Reset()
        {
            m_isTriggered = false;
            m_started = false;
            m_finished = false;
            m_animIsReversed = false;
            m_animIsDone = false;
            m_animIdleEnd = false;
            m_isIdle = false;
            m_animatedMesh.SetActive(false);
        }
    }
}