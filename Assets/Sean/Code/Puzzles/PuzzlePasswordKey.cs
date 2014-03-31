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
        
        // Use this for initialization
        void Start()
        {
            if (!m_parentObject)
            {
                Debug.Log(this.name + " does not have a parent object");
            }
        }
        
        // Update is called once per frame
        void Update()
        {
            /* Empty */
        }

        public void SetValue(int k)
        {
            m_value = k;
        }

        public int GetValue()
        {
            return m_value;
        }

        public GameObject GetParent()
        {
            return m_parentObject;
        }
    }
}