//==========================================================
// Author: Sean Vieira
// Version: 1.0
// Function: Stores data for the individual keys
// on the keypad.
//==========================================================

using UnityEngine;
using System.Collections;

namespace sv
{
    public class KeypadKeyClass : MonoBehaviour
    {
        [SerializeField] public GameObject m_parentObject;
        [SerializeField] private int m_keyValue;
        
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

        public void SetKeyValue(int k)
        {
            m_keyValue = k;
        }

        public int GetKeyValue()
        {
            return m_keyValue;
        }

        public GameObject GetParent()
        {
            return m_parentObject;
        }
    }
}