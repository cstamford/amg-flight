//===============================================
// Author: Sean Vieira
// Version: 1.0
// Function: Controls the orbs used in puzzles,
// currently only rotates them
//===============================================

using UnityEngine;
using System.Collections;

namespace sv
{
    public class PuzzleOrbController : MonoBehaviour
    {
        private const float ROTATION_SPEED = 25.0f;
        private Vector3 m_rotation;

        // Use this for initialization
        void Start()
        {
            /* Empty */
        }

        // Update is called once per frame
        void Update()
        {
            m_rotation = transform.eulerAngles;
            
            RotateOrb(Time.deltaTime);
        }

        void RotateOrb(float delta)
        {
            m_rotation.y = cst.Common.Helpers.wrapAngle(m_rotation.y + (ROTATION_SPEED * delta));

            transform.eulerAngles = m_rotation;
        }
    }
}