//=================================================================
// Author: Sean Vieira
// Function: Helper function so that controls a couple of booleans
// relating to wing state, as well as a method to change parent 
// object
//=================================================================

using UnityEngine;
using System.Collections;

namespace sv
{
    public class WingsController : MonoBehaviour
    {

        private bool m_isAttachedToPlayer;
        private GameObject m_parentObject;
        private GameObject m_transformsParentObject;
        private Animation m_animation;
        private Rigidbody m_rBody;

        // Use this for initialization
        void Start()
        {
            m_isAttachedToPlayer = false;
            m_parentObject = this.transform.parent.gameObject;
            m_transformsParentObject = m_parentObject;
            m_animation = GetComponent<Animation>();
            m_rBody = GetComponent<Rigidbody>();
            m_rBody.Sleep();
        }

        void Update()
        {
            if (m_isAttachedToPlayer)
            {
                m_rBody.constraints = RigidbodyConstraints.FreezeAll;
                m_rBody.useGravity = false;
            }
            else 
            {
                if (m_parentObject == null)
                {
                    m_rBody.constraints = RigidbodyConstraints.None;
                    m_rBody.useGravity = true;
                }
            }
        }

        public bool IsAttachedToPlayer 
        {
            get { return m_isAttachedToPlayer;}
            set 
            {
                if (value != null)
                {
                    Debug.Log("Wing attachment to player has been set to " + value);
                }
                else Debug.Log("Wing attachment to player has been set to null");
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
                    Debug.Log("Wing attachment to player has been set to null");
                    this.transform.parent = null;
                }               
            }
        }
    }
}