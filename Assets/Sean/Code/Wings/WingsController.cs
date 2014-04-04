//=================================================================
// Author: Sean Vieira
// Function: Helper function so that controls a couple of booleans
// relating to wing state, as well as a method to change parent 
// object
//=================================================================

using UnityEngine;
using System;
using cst.Flight;

namespace sv
{
    public class WingsController : MonoBehaviour
    {
        [SerializeField] private GameObject m_wingPosition;
        private bool m_isAttachedToPlayer;
        private bool m_playerIsFlying;
        private GameObject m_parentObject;
        private Animation m_animation;

        // Use this for initialization
        void Start()
        {
            m_isAttachedToPlayer = false;
            m_playerIsFlying = false;
            m_parentObject = this.transform.parent.gameObject;
            m_animation = GetComponent<Animation>();

            if (!m_wingPosition)
            {
                enabled = false;
                throw new Exception("No Wing Position GameObject attached to script.");
            }
        }

        void Update()
        {
            if (IsParentSeraph())
            {
                if (!m_isAttachedToPlayer)
                {
                    SetWingsOnSeraph();
                }
                else
                {
                    SeraphController seraph = m_parentObject.GetComponent<SeraphController>();

                    if (seraph.state == SeraphState.GLIDING || seraph.state == SeraphState.FLYING)
                    {
                        if (!m_playerIsFlying)
                        {
                            SetWingsFlyingPosition();
                        }
                    }
                    else
                    {
                        if (m_playerIsFlying)
                        {
                            m_playerIsFlying = false;
                            SetWingsOnSeraph();
                        }

                    }
                }
            }
        }

        public bool IsAttachedToPlayer 
        {
            get { return m_isAttachedToPlayer;}
            set 
            {
                m_isAttachedToPlayer = value;
            }
        }

        public GameObject ParentObject
        {
            get { return m_parentObject; }
            set
            {
                m_parentObject = value; 
                if (value != null)
                {
                    Debug.Log("The parent object of the wings has been set to " + value);

                    this.transform.parent = value.transform;
                }
                else
                {
                    this.transform.parent = null;
                }               
            }
        }

        private void SetWingsOnSeraph()
        {
            IsAttachedToPlayer = true;

            Transform wingPos = m_wingPosition.transform;
            Transform newPos = this.transform;

            newPos.localPosition = new Vector3(wingPos.localPosition.x, wingPos.localPosition.y - 1.0f, wingPos.localPosition.z);
            newPos.localRotation = Quaternion.Euler(90, 0, 0);

            m_animation.Play("Idle Closed");
        }

        private void SetWingsFlyingPosition()
        {
            m_playerIsFlying = true;

            Transform wingPos = m_wingPosition.transform;
            Transform newPos = this.transform;

            newPos.localPosition = new Vector3(wingPos.localPosition.x, wingPos.localPosition.y + 0.25f, wingPos.localPosition.z - 1.25f);
            newPos.localRotation = Quaternion.Euler(180, 0, 0);

            m_animation.Play("Idle Open");
        }

        private bool IsParentSeraph()
        {
            if (m_parentObject != null)
            {
                if (m_parentObject.tag == "Player")
                {
                    Collider box = GetComponent<Collider>();

                    box.enabled = false;

                    return true;
                }
            }
            
            return false;
        }
    }
}