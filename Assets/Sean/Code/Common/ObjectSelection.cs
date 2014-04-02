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

        // Raycasting
        private RaycastHit m_hit;
        private Ray m_crosshairRay;

        // Puzzle types
        private PuzzleCollect m_puzzleTypeCollect;
        private PuzzlePassword m_puzzleTypePassword;
        private PuzzleCollectObject m_puzzleCollectable;
        private PuzzlePasswordKey m_puzzlePasswordKey;
        private WingsController m_wingsController;

        // Misc.
        private InputManager m_inputManager;
        private GameObject m_selected;
        private SeraphController m_seraph;

		// Relic collection (audio)
		private bool m_collected;
		private String m_collectedName;

        // Wing collection (audio + animation)
        private bool m_narrationIsDone;
        private bool m_animationIsDone;

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
                        
                        if (m_puzzleTypeCollect != AcquireObjectComponent<PuzzleCollect>(m_puzzleCollectable.GetParent(), m_puzzleTypeCollect))
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
                        m_wingsController = AcquireObjectComponent<WingsController>(m_selected, m_wingsController);
                        
                        // As wings are aprt of 'puzzle' to open door
                        m_puzzleCollectable = AcquireObjectComponent<PuzzleCollectObject>(m_selected, m_puzzleCollectable);

                        if (!m_puzzleTypeCollect)
                        {
                            m_puzzleTypeCollect = AcquireObjectComponent<PuzzleCollect>(m_puzzleCollectable.GetParent(), m_puzzleTypeCollect);
                        }
                    } break;
            }
        }

        void CheckForObjectInteraction()
        {
            // Check to see if objects are interacted with
            if (m_inputManager.actionFired(Action.INTERACT))
            {
                Debug.Log("Ray intersects with " + m_selected.name);
				m_collectedName = m_selected.name;

                switch (m_selected.tag)
                {
                    case "PuzzleCollectObject":
                    {                        
						m_puzzleTypeCollect.SetPuzzleObjectCollectedState(m_puzzleCollectable.GetIndex(), true);
                        m_selected.SetActive(false);
						m_collected = true;
                    } break;

                    case "PuzzlePasswordObject":
                    {
                        // Set the object state as entered into the password
                        m_puzzleTypePassword.AddKeyToPassword(m_puzzlePasswordKey.GetValue());
                    } break;

                    case "Wings":
                        {
                            if (m_narrationIsDone)
                            {
                                if (!m_wingsController.IsAttachedToPlayer)
                                {
                                    m_wingsController.ParentObject = this.gameObject;
                                    m_seraph.capability = SeraphCapability.FLIGHT;
                                    m_collected = true;
                                    m_puzzleTypeCollect.SetPuzzleObjectCollectedState(m_puzzleCollectable.GetIndex(), true);
                                }
                            }
                        } break;
                }
            }
        }

        public bool IsWingNarrationDone
        {
            get { return m_narrationIsDone; }
            set
            {
                m_narrationIsDone = value;
            }
        }

        public bool IsWingAnimatonDone
        {
            get { return m_animationIsDone; }
            set
            {
                m_animationIsDone = value;
            }
        }

        // Marc Stuff =====================
		public bool GetCollection()
		{
			bool temp = m_collected;
			m_collected = false;
			return temp;
		}

		public String GetCollectionName()
		{
			return m_collectedName;
		}

        // End Marc Stuff =================
    }
}