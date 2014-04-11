//===============================================
// Author: Sean Vieira
// Version: 1.0
// Function: Restricts the character model's 
// movement to stop unnatural rotations
//===============================================

using UnityEngine;
using System.Collections;
namespace sv
{
    public class CharacterMovementRestrictor : MonoBehaviour
    {
        public GameObject m_playerObject;
        private Vector3 m_newPos;

        void Start()
        {
            if (this.m_playerObject != null)
            {
                m_newPos = new Vector3(this.m_playerObject.transform.position.x, this.m_playerObject.transform.position.y, this.m_playerObject.transform.position.z);
            }
            this.transform.position = m_newPos;
        }

        void Update()
        {
            m_newPos = new Vector3(this.m_playerObject.transform.position.x, this.m_playerObject.transform.position.y, this.m_playerObject.transform.position.z);
            this.transform.position = m_newPos;
        }
    }
}