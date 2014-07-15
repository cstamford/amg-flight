// ==================================================================== \\
// File   : FollowPlayer.cs                                             \\
// Author : Christopher Stamford									    \\
//                                                                      \\
// FollowPlayer.cs forces the parent GameObject to share its position   \\
// with another GameObject. If no GameObject attached, scans the scene  \\
// for one with the Player tag.                                         \\
// ==================================================================== \\

using System;
using UnityEngine;

namespace cst.Map
{
    public class FollowPlayer : MonoBehaviour
    {
        [SerializeField] private GameObject m_seraph;

        private void Start()
        {
            // If no camera has been defined in properties, try to auto-find one
            if (m_seraph == null)
                m_seraph = GameObject.FindWithTag("Player");

            if (m_seraph == null)
                throw new Exception("No seraph detected in the scene.");
        }

        private void Update()
        {
            transform.position = m_seraph.transform.position;
        }
    }
}