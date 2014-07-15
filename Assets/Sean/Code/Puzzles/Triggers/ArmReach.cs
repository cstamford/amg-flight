//=================================================================
// Author: Sean Vieira
// Version: 1.0
// Function: Plays the animation of the arm model when toggled
//=================================================================


using UnityEngine;
using System.Collections;

namespace sv.Triggers
{
    public enum AnimState
    {
        Idle,
        Extending,
        Retracting,
    }
    
    public class ArmReach : MonoBehaviour
    {
        // Inspector Fields
        [SerializeField] private AnimState m_state;
        [SerializeField] private GameObject m_animatedMesh;

        // Event flags
        private bool m_isTriggered;
        private bool m_finished;
        
        // Animation flags
        private bool m_animIsReversed;
        private bool m_animStarted;
        private bool m_idleStarted;
        private bool m_animFinished;
        private bool m_animRetractStarted;

        // Misc
        private AnimState m_lastState;

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
                PlayAnimation();
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
                    m_animatedMesh.SetActive(true);
                }
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
                    m_animatedMesh.animation["Reach"].speed = 1.5f;
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

        public bool IsFinished
        {
            get { return m_finished; }
            set
            {
                m_finished = value;
                if (m_finished)
                {
                    Reset();
                }
            }
        }

        public AnimState State
        {
            get { return m_state; }
            set
            {
                m_lastState = m_state;
                m_state = value;
            }
        }

        // Returns false until animation cycle has finished
        private bool PlayAnimation()
        {
            switch (m_state)
            {
                case AnimState.Idle:
                    {
                        PlayIdle();
                    } break;
                case AnimState.Extending:
                    {
                        IsReversed = false;
                        m_animRetractStarted = false;
                        PlayAnim();
                    } break;
                case AnimState.Retracting:
                    {
                        IsReversed = true;
                        PlayAnim();
                    } break;
                
                default:
                    {
                        PlayIdle();
                    } break;
            }

            return false;
        } 
        
        // Returns false until animation cycle has finished
        private void PlayIdle()
        {
            if (!m_idleStarted)
            {
                if (!m_animatedMesh.animation.IsPlaying("Idle"))
                {
                    m_animatedMesh.animation.Play("Idle");
                    m_idleStarted = true;
                }
            }
        }
        
        // Returns false until animation cycle has finished        
        private void PlayAnim()
        {
            if (!m_animStarted)
            {
                if (!m_animatedMesh.animation.IsPlaying("Reach"))
                {
                    m_animatedMesh.animation.Play("Reach");
                    m_animStarted = true;
                }
            }
            else
            {
                if (m_state == AnimState.Extending)
                {
                    if (!m_animFinished)
                    {
                        if (!m_animatedMesh.animation.IsPlaying("Reach"))
                        {
                            m_animFinished = true;
                        }
                    }
                    else
                    {
                        m_state = AnimState.Idle;
                    }
                }
                else
                {
                    if (!m_animRetractStarted)
                    {
                        m_animStarted = false;
                        m_animFinished = false;
                        m_animRetractStarted = true;
                    }
                    else
                    {
                        if (!m_animFinished)
                        {
                            if (!m_animatedMesh.animation.IsPlaying("Reach"))
                            {
                                m_animFinished = true;
                            }
                        }
                        else
                        {
                            IsFinished = true;
                        }
                    }
                    
                }
            }   
        }       

        private void Reset()
        {
            //if (m_state == null) m_state = AnimState.Extending;
            m_lastState = m_state;
            m_isTriggered = false;
            m_animIsReversed = false;
            m_animStarted = false;
            m_idleStarted = false;
            m_animFinished = false;
            m_animRetractStarted = false;
            m_animatedMesh.SetActive(false);
        }
    }
}