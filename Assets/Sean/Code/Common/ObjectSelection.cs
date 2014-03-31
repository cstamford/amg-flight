//==========================================================
// Author: Sean Vieira
// Version: 1.1
// Function: Handles the selection of objects, through the 
// use of a ray that is cast from the centre of the screen 
// in the direction the camera is facing
//==========================================================

using UnityEngine;
using System;
using sv.Puzzles;
using sv.Triggers;
using cst.Common;
using cst.Flight;
using Action = cst.Common.Action;

namespace sv
{
    public class ObjectSelection : MonoBehaviour
    {
        [SerializeField] private float m_rayLength;
        [SerializeField] private GameObject m_inputManagerObject;
        [SerializeField] private GameObject m_cursor;

        private RaycastHit m_hit;
        private Ray m_crosshairRay;

        private InputManager m_inputManager;
        private GameObject m_selected;
        private SeraphController m_seraph;
        private PuzzleCollect m_puzzleTypeCollect;
        private PuzzlePassword m_puzzleTypePassword;
        private PuzzleCollectObject m_puzzleCollectable;
        private PuzzlePasswordKey m_puzzlePasswordKey;
        private WingsController m_wingsController;

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

            if (!m_inputManagerObject)
            {
                enabled = false;
                Debug.Log("No input manager added to the Seraph. Please define one in the inspector.");
            }
            else
            {
                m_inputManager = m_inputManagerObject.GetComponent<InputManager>();

                if (!m_inputManager)
                {
                    enabled = false;
                    throw new Exception("No InputManager script detected on the provided InputManagerObject.");
                }
            }

            m_seraph = GetComponent<SeraphController>();
            if (!m_seraph)
            {
                enabled = false;
                throw new Exception("No Seraph Controller script detected on game object.");
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
                    if (m_inputManager.actionFired(Action.INTERACT))
                    {
                        Debug.Log("Nothing has been selected");
                    }
                }
                else
                {
                    CheckForObjectAcquirement();
                    
                    CheckForObjectInteraction();
                }
            }
            else
            {
                if (m_inputManager.actionFired(Action.INTERACT))
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
       
        // Check to see if selectable objects have been acquired
        void CheckForObjectAcquirement()
        {
            switch (m_selected.tag)
            {
                case "PuzzleCollect":
                    {
                        m_cursor.SetActive(true);
                        m_puzzleTypeCollect = AcquireObjectComponent<PuzzleCollect>(m_selected, m_puzzleTypeCollect);
                    } break;
                case "PuzzleCollectObject":
                    {
                        m_cursor.SetActive(true);
                        m_puzzleCollectable = AcquireObjectComponent<PuzzleCollectObject>(m_selected, m_puzzleCollectable);

                        // This is necessary to do here since puzzle contreller may not necessarily have a mesh
                        if (!m_puzzleTypeCollect)
                        {
                            m_puzzleTypeCollect = AcquireObjectComponent<PuzzleCollect>(m_puzzleCollectable.GetParent(), m_puzzleTypeCollect);
                        }
                    } break;
                case "PuzzlePassword":
                    {
                        m_cursor.SetActive(true);
                        m_puzzleTypePassword = AcquireObjectComponent<PuzzlePassword>(m_selected, m_puzzleTypePassword);
                    } break;
                case "PuzzlePasswordObject":
                    {
                        m_cursor.SetActive(true);
                        m_puzzlePasswordKey = AcquireObjectComponent<PuzzlePasswordKey>(m_selected, m_puzzlePasswordKey);

                        if (!m_puzzleTypePassword)
                        {
                            m_puzzleTypePassword = AcquireObjectComponent<PuzzlePassword>(m_puzzlePasswordKey.GetParent(), m_puzzleTypePassword);
                        }
                    } break;
                case "Wings":
                    {
                        m_cursor.SetActive(true);
                        m_wingsController = AcquireObjectComponent<WingsController>(m_selected, m_wingsController);
                    } break;
            }
        }

        void CheckForObjectInteraction()
        {
            // Check to see if objects are interacted with
            if (m_inputManager.actionFired(Action.INTERACT))
            {
                Debug.Log("Ray intersects with " + m_selected.name);

                switch (m_selected.tag)
                {
                    case "PuzzleCollectObject":
                    {
                        // Set the object state as collected, and de-activate it (so it no longer affects the scene)
                        m_puzzleTypeCollect.SetPuzzleObjectCollectedState(m_puzzleCollectable.GetIndex(), true);
                        m_selected.SetActive(false);
                    } break;
                    case "PuzzlePasswordObject":
                    {
                        // Set the object state as entered into the password
                        m_puzzleTypePassword.AddKeyToPassword(m_puzzlePasswordKey.GetValue());
                    } break;
                    case "Wings":
                        {
                            if (!m_wingsController.IsAttachedToPlayer)
                            {                                
                                m_wingsController.IsAttachedToPlayer = true;
                                m_wingsController.ParentObject = this.gameObject;
                                m_seraph.capability = SeraphCapability.FLIGHT;
                            }
                            else
                            {
                                m_wingsController.IsAttachedToPlayer = false;
                                m_wingsController.ParentObject = null;
                                m_seraph.capability = SeraphCapability.NONE;
                                m_seraph.state = SeraphState.FALLING;
                            }
                        } break;
                }
            }
        }
    }
}