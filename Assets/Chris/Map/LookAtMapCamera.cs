// Looks at the map camera

using System;
using UnityEngine;

namespace cst
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
            // Basic billboarding. Not sure if this is needed.
            transform.rotation = Quaternion.LookRotation(transform.position - m_mapCamera.transform.position);
        }
    }
}