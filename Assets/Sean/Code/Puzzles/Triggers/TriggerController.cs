//=================================================================
// Author: Sean Vieira
// Version: 1.0
// Function: Determines which type of trigger is set off based on 
// the trigger type of the puzzle
//=================================================================

using UnityEngine;
using System.Collections;

namespace sv.Triggers
{
    public enum TriggerType
    {
        OPEN_DOOR,
        CLOSE_DOOR,
        WATERFALL_HALT,
        TOGGLE_MATERIAL,
        WINGS_CLOSE,
        WINGS_OPEN,
    }
    
    public class TriggerController : MonoBehaviour
    {
        [SerializeField] TriggerType m_triggerType;

        public TriggerType Type
        {
            get { return m_triggerType; }
            set
            {
                //Debug.Log("Active Trigger type has been set to " + value);
                m_triggerType = value;
            }

        }

        public bool ActivateTrigger(GameObject targetTrigger)
        {
            switch (m_triggerType)
            {
                case TriggerType.OPEN_DOOR:
                    {
                        DoorOpen trigger = targetTrigger.GetComponent<DoorOpen>();
                        trigger.ActivateTrigger = true;
                    } break;

                case TriggerType.CLOSE_DOOR:
                    {
                        DoorClose trigger = targetTrigger.GetComponent<DoorClose>();
                        trigger.ActivateTrigger = true;
                    } break;

                case TriggerType.WATERFALL_HALT:
                    {
                        WaterfallHalt trigger = targetTrigger.GetComponent<WaterfallHalt>();
                        trigger.ActivateTrigger = true;
                    } break;

                case TriggerType.TOGGLE_MATERIAL:
                    {
                        ToggleMaterial trigger = targetTrigger.GetComponent<ToggleMaterial>();
                        trigger.ActivateTrigger = true;
                    } break;

                case TriggerType.WINGS_CLOSE:
                    {
                        ToggleMaterial trigger = targetTrigger.GetComponent<ToggleMaterial>();
                        trigger.ActivateTrigger = true;
                    } break;

                case TriggerType.WINGS_OPEN:
                    {
                        ToggleMaterial trigger = targetTrigger.GetComponent<ToggleMaterial>();
                        trigger.ActivateTrigger = true;
                    } break;

                default :
                    {
                        return false;
                    }
            }

            return true ;
        }

        public bool ActivateTrigger<T>(GameObject targetTrigger, T param) where T : Material  
        {
            switch (m_triggerType)
            {
                case TriggerType.OPEN_DOOR:
                    {
                        DoorOpen trigger = targetTrigger.GetComponent<DoorOpen>();
                        trigger.ActivateTrigger = true;
                    } break;

                case TriggerType.CLOSE_DOOR:
                    {
                        DoorClose trigger = targetTrigger.GetComponent<DoorClose>();
                        trigger.ActivateTrigger = true;
                    } break;

                case TriggerType.WATERFALL_HALT:
                    {
                        WaterfallHalt trigger = targetTrigger.GetComponent<WaterfallHalt>();
                        trigger.ActivateTrigger = true;
                    } break;

                case TriggerType.TOGGLE_MATERIAL:
                    {
                        ToggleMaterial trigger = targetTrigger.GetComponent<ToggleMaterial>();
                        trigger.TargetMaterial = param;
                        trigger.ActivateTrigger = true;
                    } break;

                case TriggerType.WINGS_CLOSE:
                    {
                        ToggleMaterial trigger = targetTrigger.GetComponent<ToggleMaterial>();
                        trigger.ActivateTrigger = true;
                    } break;

                case TriggerType.WINGS_OPEN:
                    {
                        ToggleMaterial trigger = targetTrigger.GetComponent<ToggleMaterial>();
                        trigger.ActivateTrigger = true;
                    } break;

                default:
                    {
                        return false;
                    }
            }

            return true;
        }

        public bool DeactivateTrigger(GameObject targetTrigger)
        {
            switch (m_triggerType)
            {
                case TriggerType.OPEN_DOOR:
                    {
                        DoorOpen trigger = targetTrigger.GetComponent<DoorOpen>();
                        trigger.ActivateTrigger = false;
                    } break;

                case TriggerType.CLOSE_DOOR:
                    {
                        DoorClose trigger = targetTrigger.GetComponent<DoorClose>();
                        trigger.ActivateTrigger = false;
                    } break;

                case TriggerType.WATERFALL_HALT:
                    {
                        WaterfallHalt trigger = targetTrigger.GetComponent<WaterfallHalt>();
                        trigger.ActivateTrigger = false;
                    } break;

                case TriggerType.TOGGLE_MATERIAL:
                    {
                        ToggleMaterial trigger = targetTrigger.GetComponent<ToggleMaterial>();
                        trigger.ActivateTrigger = false;
                    } break;

                case TriggerType.WINGS_CLOSE:
                    {
                        ToggleMaterial trigger = targetTrigger.GetComponent<ToggleMaterial>();
                        trigger.ActivateTrigger = false;
                    } break;

                case TriggerType.WINGS_OPEN:
                    {
                        ToggleMaterial trigger = targetTrigger.GetComponent<ToggleMaterial>();
                        trigger.ActivateTrigger = false;
                    } break;

                default:
                    {
                        return false;
                    }
            }          

            return true;
        }

        public bool TriggerIsActive(GameObject targetTrigger)
        {
            switch (m_triggerType)
            {
                case TriggerType.OPEN_DOOR:
                    {
                        DoorOpen trigger = targetTrigger.GetComponent<DoorOpen>();
                        return trigger.ActivateTrigger;
                    };

                case TriggerType.CLOSE_DOOR:
                    {
                        DoorClose trigger = targetTrigger.GetComponent<DoorClose>();
                        return trigger.ActivateTrigger;
                    };

                case TriggerType.WATERFALL_HALT:
                    {
                        WaterfallHalt trigger = targetTrigger.GetComponent<WaterfallHalt>();
                        return trigger.ActivateTrigger;
                    };

                case TriggerType.TOGGLE_MATERIAL:
                    {
                        ToggleMaterial trigger = targetTrigger.GetComponent<ToggleMaterial>();
                        return trigger.ActivateTrigger;
                    };

                case TriggerType.WINGS_CLOSE:
                    {
                        ToggleMaterial trigger = targetTrigger.GetComponent<ToggleMaterial>();
                        return trigger.ActivateTrigger;
                    };

                case TriggerType.WINGS_OPEN:
                    {
                        ToggleMaterial trigger = targetTrigger.GetComponent<ToggleMaterial>();
                        return trigger.ActivateTrigger;
                    };
                default:
                    {
                        return false;
                    }
            }  
        }
    }
}