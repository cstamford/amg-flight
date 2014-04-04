//==========================================================
// Author: Sean Vieira
// Version: 1.2
// Function: Handles puzzles that require certain objects
// to be collected, and in no specific order
//==========================================================

using UnityEngine;
using System;
using sv.Triggers;

namespace sv.Puzzles
{
    public class PuzzleCollect : MonoBehaviour
    {
        [SerializeField] private GameObject[] m_puzzleObjects;
        [SerializeField] private GameObject m_triggerTarget;
        private TriggerController m_triggerController;
        private PuzzleCollectObject m_selectedObject;
        private int m_numOfObjects; 
        private bool m_trigger;
        
        // Use this for initialization
        void Start()
        {
            m_trigger = false;
            m_triggerController = GetComponent<TriggerController>();
            if (!m_triggerController)
            {
                enabled = false;
                throw new Exception("No trigger object script attached to GameObject");
            }

            m_numOfObjects = m_puzzleObjects.GetLength(0);
            for (int i = 0; i < m_numOfObjects; i++)
            {
                m_selectedObject = m_puzzleObjects[i].GetComponent<PuzzleCollectObject>();
                m_selectedObject.SetIndex(i);
            }
        }

        // Update is called once per frame
        void Update()
        {
            // Prints a statement in debug
            if (!m_trigger)
            {
                if (IsCompleted())
                {
                    ActivateTrigger();
                }
            }
        }    

        public void SetPuzzleObjectCollectedState(int i, bool b)
        {
            m_selectedObject = m_puzzleObjects[i].GetComponent<PuzzleCollectObject>();
            m_selectedObject.SetCollectedState(b);
        }

        // Check to see if all puzzle objects have been collected
        public bool IsCompleted()
        {
            for (int i = 0; i < m_numOfObjects; i++)
            {
                m_selectedObject = m_puzzleObjects[i].GetComponent<PuzzleCollectObject>();

                if (!m_selectedObject.IsCollected())
                {
                    return false;
                }
            }

            return true;
        }

        public void ActivateTrigger()
        {
            m_trigger = true;

            if (m_triggerTarget)
            {
                //Debug.Log("Activating trigger...");
                if (!m_triggerController.ActivateTrigger(m_triggerTarget))
                {
                    Debug.Log("Error activating trigger");
                }
            }
        }
    }
}