//==========================================================
// Author: Sean Vieira
// Version: 1.1
// Function: Stores data for the individual objects 
// in a puzzle that requires a code to be entered
//==========================================================

using UnityEngine;
using System.Collections;

namespace sv.Puzzles
{
    public class PuzzlePasswordKey : MonoBehaviour
    {
        [SerializeField] public GameObject m_parentObject;
        [SerializeField] private int m_value;
        private bool m_isEntered;
        private int m_indexOrder;
        
        // Use this for initialization
        void Start()
        {
            if (!m_parentObject)
            {
                Debug.Log(this.name + " does not have a parent object");
            }

            m_isEntered = false;
        }
        
        // Update is called once per frame
        void Update()
        {
            /* Empty */
        }

        public int Value
        {
            get { return m_value; }
            set
            {
                m_value = value;
            }
        }

        public bool IsEntered
        {
            get { return m_isEntered; }
            set
            {
                m_isEntered = value;
            }
        }

        public int OrderPressed
        {
            get { return m_indexOrder; }
            set
            {
                m_indexOrder = value;
            }
        }

        public GameObject GetParent()
        {
            return m_parentObject;
        }
    }
}