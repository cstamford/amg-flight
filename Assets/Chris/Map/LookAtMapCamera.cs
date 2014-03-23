// ==================================================================== \\
// File   : LookAtMapCamera.cs                                          \\
// Author : Christopher Stamford									    \\
//                                                                      \\
// LookAtMapCamera.cs forces the parent GameObject to look at the       \\
// provided GameObject. If one is not provided, it will scan the scene  \\
// for an object with the Map Camera tag.                               \\
// ==================================================================== \\

using System;
using UnityEngine;

namespace cst.Map
{
    public class LookAtMapCamera : MonoBehaviour
    {
        [SerializeField] private GameObject m_mapCamera;

        void Start()
        {
            // If no camera has been defined in properties, try to auto-find one
            if (m_mapCamera == null)
                m_mapCamera = GameObject.FindWithTag("Map Camera");

            if (m_mapCamera == null)
                throw new Exception("No map camera detected in the scene.");
        }

        void Update()
        {
            transform.rotation = Quaternion.LookRotation(transform.position - m_mapCamera.transform.position);
        }
    }
}