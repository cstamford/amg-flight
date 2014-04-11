//==========================================================
// Author: Sean Vieira
// Version: 1.2
// Function: 
// 1.2 Now has Rift Enabled raycasting
// 1.1 Handles the selection of objects, through the 
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
        [SerializeField] private bool m_isRiftEnabled;

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
        private ArmReach m_cursorController;    
        private int[] m_relicOrder;

		// Relic collection (audio)
        private bool m_collected;
        private bool m_relicPlaced;
        private String m_collectedName;
        private int m_totalNumOfRelicsCollected;
        private int m_numOfObelisksPressed;

        // Wing collection (audio + animation)
        private bool m_narrationIsDone;
        private bool m_animationIsDone;

        // Rift raycasting
        private GameObject m_OVRCameraRight;

        // Use this for initialization
        void Start()
        {
            if (!m_cursor)
            {
                Debug.Log(this.name + " has no cursor attached.");
            }
            else
            {
                m_cursorController = m_cursor.GetComponent<ArmReach>();
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

            m_OVRCameraRight = GameObject.Find("CameraRight");

            m_puzzleTypePassword = GameObject.Find("Level End Puzzle").GetComponent<PuzzlePassword>();
            m_relicOrder = new int[m_puzzleTypePassword.TargetPasswordLength];

            m_totalNumOfRelicsCollected = 0;
            m_numOfObelisksPressed = 0;
        }

        // Update is called once per frame
        void Update()
        {
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
                    CheckForObjectAcquirement(); // acquire objects based on ray hits                    
                    CheckForObjectInteraction(); // test for user button presses                   
                }
            }
            else
            {
                if (m_inputManager.actionFired(Action.INTERACT))
                {
                    Debug.Log("Ray has no intersections");
                }

                if (m_cursorController != null)
                {
                    if (m_cursorController.State != AnimState.Retracting)
                    {
                        m_cursorController.State = AnimState.Retracting;
                    }
                } 
            }   
        }

        // Casts a ray forward from the camera, returns true if intersection
        bool CastRay()
        {
            if (!m_isRiftEnabled)
            {

                m_crosshairRay = new Ray(transform.position, transform.forward);

                Debug.DrawRay(transform.position, transform.forward * m_rayLength);

                if (Physics.Raycast(m_crosshairRay, out m_hit, m_rayLength))
                {
                    return true;
                }
            }
            else
            {
                m_crosshairRay = new Ray(transform.position, m_OVRCameraRight.transform.forward);

                Debug.DrawRay(transform.position, m_OVRCameraRight.transform.forward * m_rayLength);                
                
                if (Physics.Raycast(m_crosshairRay, out m_hit, m_rayLength))
                {
                    return true;
                }
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
                    if (m_cursorController != null)
                    {
                        m_cursorController.ActivateTrigger = true;
                        if (m_cursorController.State != AnimState.Idle)
                        {
                            m_cursorController.State = AnimState.Extending;
                        }
                    }  

                    m_puzzleTypeCollect = AcquireObjectComponent<PuzzleCollect>(m_selected, m_puzzleTypeCollect);
                } break;
                case "PuzzleCollectObject":
                {
                    if (m_cursorController != null)
                    {
                        m_cursorController.ActivateTrigger = true;
                        if (m_cursorController.State != AnimState.Idle)
                        {
                            m_cursorController.State = AnimState.Extending;
                        }
                    } 

                    m_puzzleCollectable = AcquireObjectComponent<PuzzleCollectObject>(m_selected, m_puzzleCollectable);                    
   
                    if (m_puzzleTypeCollect != AcquireObjectComponent<PuzzleCollect>(m_puzzleCollectable.GetParent(), m_puzzleTypeCollect))
                    {
                        m_puzzleTypeCollect = AcquireObjectComponent<PuzzleCollect>(m_puzzleCollectable.GetParent(), m_puzzleTypeCollect);
                    }
                } break;
                case "PuzzlePassword":
                {
                    if (m_cursorController != null)
                    {
                        m_cursorController.ActivateTrigger = true;
                        if (m_cursorController.State != AnimState.Idle)
                        {
                            m_cursorController.State = AnimState.Extending;
                        }
                    } 

                    m_puzzleTypePassword = AcquireObjectComponent<PuzzlePassword>(m_selected, m_puzzleTypePassword);
                } break;
                case "PuzzlePasswordObject":
                {
                    if (m_cursorController != null)
                    {
                        m_cursorController.ActivateTrigger = true;
                        if (m_cursorController.State != AnimState.Idle)
                        {
                            m_cursorController.State = AnimState.Extending;
                        }
                    } 

                    m_puzzlePasswordKey = AcquireObjectComponent<PuzzlePasswordKey>(m_selected, m_puzzlePasswordKey);

                    if (!m_puzzleTypePassword != AcquireObjectComponent<PuzzlePassword>(m_puzzlePasswordKey.GetParent(), m_puzzleTypePassword))
                    {
                        m_puzzleTypePassword = AcquireObjectComponent<PuzzlePassword>(m_puzzlePasswordKey.GetParent(), m_puzzleTypePassword);
                    }
                } break;
                case "Wings":
                {
                    if (m_cursorController != null)
                    {
                        m_cursorController.ActivateTrigger = true;
                        if (m_cursorController.State != AnimState.Idle)
                        {
                            m_cursorController.State = AnimState.Extending;
                        }
                    } 

                    m_wingsController = AcquireObjectComponent<WingsController>(m_selected, m_wingsController);
                        
                    // As wings are part of 'puzzle' to open door
                    m_puzzleCollectable = AcquireObjectComponent<PuzzleCollectObject>(m_selected, m_puzzleCollectable);

                    if (!m_puzzleTypeCollect)
                    {
                        m_puzzleTypeCollect = AcquireObjectComponent<PuzzleCollect>(m_puzzleCollectable.GetParent(), m_puzzleTypeCollect);
                    }
                } break;
                default:
                    {
                        m_cursorController.State = AnimState.Retracting;
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

                if (m_cursorController.State != AnimState.Retracting)
                {
                    switch (m_selected.tag)
                    {
                        case "PuzzleCollectObject":
                            {
                                int passwordValueOfRelic = m_puzzleCollectable.GetIndex() + 1;
                                m_puzzleTypePassword.AddKeyToTargetPassword(passwordValueOfRelic);

                                m_puzzleTypeCollect.SetPuzzleObjectCollectedState(m_puzzleCollectable.GetIndex(), true);
                                m_selected.SetActive(false);
                                m_collected = true;

                                m_relicOrder[m_totalNumOfRelicsCollected] = passwordValueOfRelic;
                                m_totalNumOfRelicsCollected++;


                                m_cursorController.State = AnimState.Retracting;
                            } break;

                        case "PuzzlePasswordObject":
                            {
                                
                                if (m_puzzlePasswordKey.GetParent().name == "Level End Puzzle")
                                {
                                    HandleEndPuzzle();
                                }
                                else
                                {
                                    if (m_puzzlePasswordKey.IsEntered)
                                    {
                                        m_puzzleTypePassword.AddKeyToUserPassword(m_puzzlePasswordKey.Value);
                                        m_puzzlePasswordKey.IsEntered = true;
                                    }
                                    else if (m_puzzlePasswordKey.Value == m_puzzleTypePassword.LastKeyEntered)
                                    {
                                        m_puzzleTypePassword.RemoveLastKeyFromUserPassword();
                                        m_puzzlePasswordKey.IsEntered = false;
                                    }
                                }

                                m_cursorController.State = AnimState.Retracting;
                            } break;

                        case "Wings":
                            {
                                //if (m_narrationIsDone)
                                {
                                    if (!m_wingsController.IsAttachedToPlayer)
                                    {
                                        m_wingsController.ParentObject = m_wingsController.WingPosition;
                                        m_seraph.capability = SeraphCapability.GLIDE;
                                        m_collected = true;
                                        m_puzzleTypeCollect.SetPuzzleObjectCollectedState(m_puzzleCollectable.GetIndex(), true);
                                    }
                                }

                                m_cursorController.State = AnimState.Retracting;
                            } break;                            
                    }
                                
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

        private void HandleEndPuzzle()
        {
            // variables to store material and trigger object
            TriggerController trigger = m_puzzlePasswordKey.GetComponent<TriggerController>();

            if (!m_puzzlePasswordKey.IsEntered)
            {
                m_puzzlePasswordKey.OrderPressed = m_numOfObelisksPressed;
                if (m_relicOrder[m_puzzlePasswordKey.OrderPressed] != 0)
                {
                    m_puzzleTypePassword.AddKeyToUserPassword(m_puzzlePasswordKey.Value);
                    m_puzzlePasswordKey.IsEntered = true;
                    m_numOfObelisksPressed++;

                    // Trigger the relic in the obelisk
                    if (trigger)
                    {
                        Material newMat = ChooseNewMaterial();
                        trigger.ActivateTrigger<Material>(m_puzzlePasswordKey.gameObject, newMat);
                        m_relicPlaced = true;
                    }
                }
            }
            else if (m_puzzlePasswordKey.Value == m_puzzleTypePassword.LastKeyEntered)
            {
                m_puzzleTypePassword.RemoveLastKeyFromUserPassword();
                m_numOfObelisksPressed--;
                m_puzzlePasswordKey.IsEntered = false;

                if (trigger)
                {
                    trigger.DeactivateTrigger(m_puzzlePasswordKey.gameObject);
                }
            }
        }

        private Material ChooseNewMaterial()
        {
            Material newMat;

            Debug.Log("The order this obelisk was pressed was : " + m_numOfObelisksPressed + ". The value of this obelisk is : " + m_relicOrder[m_puzzlePasswordKey.OrderPressed]);

            switch (m_relicOrder[m_puzzlePasswordKey.OrderPressed])
            {
                case 1:
                    {
                        newMat = Resources.Load<Material>("Sean Materials/End Relic Red");
                    } break;
                case 2:
                    {
                        newMat = Resources.Load<Material>("Sean Materials/End Relic Orange");
                    } break;
                case 3:
                    {
                        newMat = Resources.Load<Material>("Sean Materials/End Relic Yellow");
                    } break;
                case 4:
                    {
                        newMat = Resources.Load<Material>("Sean Materials/End Relic Green");
                    } break;
                case 5:
                    {
                        newMat = Resources.Load<Material>("Sean Materials/End Relic Blue");
                    } break;
                case 6:
                    {
                        newMat = Resources.Load<Material>("Sean Materials/End Relic Purple");
                    } break;
                case 7:
                    {
                        newMat = Resources.Load<Material>("Sean Materials/End Relic White");
                    } break;
                default:
                    {
                        newMat = Resources.Load<Material>("Sean Materials/lambert1");
                    } break;
            }

            return newMat;
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

        public int GetCollectionCount()
        {
            return m_totalNumOfRelicsCollected;
        }

        public bool RelicPlaced()
        {
            bool temp = m_relicPlaced;
            m_relicPlaced = false;
            return temp;
        }

        // End Marc Stuff =================
    }
}