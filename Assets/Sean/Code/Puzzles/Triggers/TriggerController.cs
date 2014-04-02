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
        WATERFALL_STOP
    }
    
    public class TriggerController : MonoBehaviour
    {
        [SerializeField] TriggerType m_triggerType;

        public TriggerType Type
        {
            get { return m_triggerType; }
            set
            {
                Debug.Log("Active Trigger type has been set to " + value);
                m_triggerType = value;
            }

        }
        public bool ActivateTrigger(GameObject targetTrigger)
        {
            Debug.Log("ACTIVATING TRIGGER MOFO");
			
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

                case TriggerType.WATERFALL_STOP:
                    {
                        WaterfallHalt trigger = targetTrigger.GetComponent<WaterfallHalt>();
                        trigger.ActivateTrigger = true;
                    } break;

                default :
                    {
                        return false;
                    }
            }

            return true ;
        }
    }
}