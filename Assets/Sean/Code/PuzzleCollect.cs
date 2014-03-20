//==========================================================
// Author: Sean Vieira
// Version: 1.0
// Function: Handles puzzles that require certain objects
// to be collected, and in no specific order
//==========================================================

using UnityEngine;
using System.Collections;

namespace sv
{
    public class PuzzleCollect : MonoBehaviour
    {
        [SerializeField] private GameObject[] m_puzzleObjects;
        [SerializeField] private GameObject m_triggerTarget;
        private PuzzleCollectObject m_selectedObject;
        private int m_numOfObjects; 
        private bool m_trigger; 
        private PuzzleGUI m_puzzleGUI;
        
        // Use this for initialization
        void Start()
        {
            m_trigger = false;
            m_puzzleGUI = GetComponent<PuzzleGUI>();

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
                if (CompletionCheck())
                {
                    m_trigger = true;

                    Debug.Log("Trigger has been activated!");
                    //DisplayCompletedText()
                }
            }
        }        

        public void DisplayTextTip(bool b)
        {
            m_puzzleGUI.ShowTextTip(b);
        }

        // Display text if puzzle is incomplete
        public void DisplayIncompletedText(bool b)
        {
            m_puzzleGUI.ShowIncompletedText(b);
        }

        // Display text if puzzle is complete
        public void DisplayCompletedText(bool b)
        {
            m_puzzleGUI.ShowCompletedText(b);
        }

        public void DisplayGUI(bool b)
        {
            m_puzzleGUI.ShowGUI(b);
        }

        public void SetPuzzleObjectCollectedState(int i, bool b)
        {
            m_selectedObject = m_puzzleObjects[i].GetComponent<PuzzleCollectObject>();
            m_selectedObject.SetCollectedState(b);
        }

        // Check to see if all puzzle objects have been collected
        private bool CompletionCheck()
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

        private void ActivateTrigger()
        {
            /* Empty */
        }
    }
}