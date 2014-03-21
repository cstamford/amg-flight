//==========================================================
// Author: Sean Vieira
// Version: 1.1
// Function: Handles the selection of objects, through the 
// use of a ray that is cast from the centre of the screen 
// in the direction the camera is facing
//==========================================================

using UnityEngine;
using System.Collections;

namespace sv
{
    public class ObjectSelection : MonoBehaviour
    {
        [SerializeField] private float m_rayLength;
        [SerializeField] private GameObject m_cursor;
        private RaycastHit m_hit;
        private Ray m_crosshairRay;
        private GameObject m_selected;
        private PuzzleCollect m_puzzleTypeCollect;
        private PuzzlePassword m_puzzleTypePassword;
        private PuzzleCollectObject m_puzzleCollectable;
        private PuzzlePasswordKey m_puzzlePasswordKey;

        // Use this for initialization
        void Start()
        {
            if (!m_cursor)
            {
                Debug.Log(this.name + " has no cursor attached.");
            }
            else
            {
                m_cursor.SetActive(false);
            }          
        }

        // Update is called once per frame
        void Update()
        {
            m_cursor.SetActive(false);          

            // If the ray collides with an object
            if (CastRay())
            {
                m_selected = m_hit.collider.gameObject;
                  
                if (!m_selected)
                {
                    if (Input.GetMouseButtonDown(0))
                    {
                        Debug.Log("Nothing has been selected");
                    }
                }
                else
                {                   
                    
                    // Check to see if selectable objects have been acquired
                    if (m_selected.tag == "PuzzleCollect")
                    {
                        m_cursor.SetActive(true);
                        m_puzzleTypeCollect = AcquireObjectComponent<PuzzleCollect>(m_selected, m_puzzleTypeCollect);                        
                    }
                    else if (m_selected.tag == "PuzzleCollectObject")
                    {
                        m_cursor.SetActive(true);
                        m_puzzleCollectable = AcquireObjectComponent<PuzzleCollectObject>(m_selected, m_puzzleCollectable);

                        // Acquire the puzzle controller if it isn't already acquired. 
                        // ie it's necessary to do here since it may not necessarily have a mesh
                        if (!m_puzzleTypeCollect)
                        {
                            m_puzzleTypeCollect = AcquireObjectComponent<PuzzleCollect>(m_puzzleCollectable.GetParent(), m_puzzleTypeCollect);
                        }
                    }
                    
                    if (Input.GetMouseButtonDown(0))
                    {
                        Debug.Log("Ray intersects with " + m_selected.name);

                        if (m_selected.tag == "PuzzleCollectObject")
                        {
                            // Set the object state as collected, and de-activate it (so it no longer affects the scene)
                            m_puzzleTypeCollect.SetPuzzleObjectCollectedState(m_puzzleCollectable.GetIndex(), true);
                            m_selected.SetActive(false);
                        }
                    }
                    else
                    {
                        /* Any other input method here */
                    }
                }
            }
            else
            {
                if (Input.GetMouseButtonDown(0))
                {
                    Debug.Log("Ray has no intersections");
                }
            }           
        }

        // Casts a ray forward from the camera, returns true if intersection
        bool CastRay()
        {
            m_crosshairRay = new Ray(transform.position, transform.forward);

            Debug.DrawRay(transform.position, transform.forward * m_rayLength);

            if (Physics.Raycast(m_crosshairRay, out m_hit, m_rayLength))
            {
                return true;
            }

            return false;
        }


        // Legacy code -----------------------------------------
        // Acquire the keypad component from target GameObject            
        /*void AcquireKeypadObject(GameObject target)
        {
            PuzzlePassword temp = target.GetComponent<PuzzlePassword>();

            if (!m_keypad)
            {
                m_keypad = temp;
            }
            else
            {
                if (m_keypad != temp)
                {
                    m_keypad = temp;
                }
            }
        }

        // Acquire the keypad key component from target GameObject  
        void AcquireKeypadButtonObject(GameObject target)
        {
            PuzzlePasswordKey temp = target.GetComponent<PuzzlePasswordKey>();

            if (!m_keyPressed)
            {
                m_keyPressed = temp;
            }
            else
            {
                if (m_keyPressed != temp)
                {
                    m_keyPressed = temp;
                }
            }
            
            if (!m_keypad)
            {
                AcquireKeypadObject(m_keyPressed.GetParent());
            }
        }*/
        //--------------------------------------------------------

        // Generic function for acquiring/storing a specific type of component
        T AcquireObjectComponent<T>(GameObject target, T component) where T : Component
        {
            T temp = target.GetComponent<T>();
            
            if (!component)
            {
                component = temp;
            }
            else
            {
                if (component != temp)
                {
                    component = temp;
                }
            }

            return component;
        }
    }
}