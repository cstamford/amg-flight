//==========================================================
// Author: Sean Vieira
// Version: 1.0
// Function: Stores data for the individual objects 
// in a puzzle that requires collection
//==========================================================

using UnityEngine;
using System.Collections;

namespace sv
{
    public class PuzzleCollectObject : MonoBehaviour
    {
        [SerializeField] public GameObject m_parentObject;
        [SerializeField] private bool m_isCollected;
        private int m_index;

        // Use this for initialization
        void Start()
        {
            if (!m_parentObject)
            {
                Debug.Log(this.name + " does not have a parent object");
            }
            else
            {

            }
        }

        // Update is called once per frame
        void Update()
        {
            /* Empty */
        }

        public void SetCollectedState(bool b)
        {
            m_isCollected = b;
        }

        public bool IsCollected()
        {
            return m_isCollected;
        }

        public void SetIndex(int i)
        {
            m_index = i;
        }

        public int GetIndex()
        {
            return m_index;
        }

        public GameObject GetParent()
        {
            return m_parentObject;
        }
    }
}