//=================================================================
// Author: Sean Vieira
// Version: 1.0
// Function: Handles different types of triggers through a generic
// method that can be called from any puzzle type
//=================================================================

using UnityEngine;
using System.Collections;

namespace sv.Triggers
{
    public enum TriggerType
    {
        OPEN_DOOR,
        WATERFALL_STOP
    }
    
    public class TriggerController : MonoBehaviour
    {
        private TriggerType m_triggerType;

        public TriggerType Type
        {
            get { return m_triggerType; }
            set
            {
                Debug.Log("Active Trigger type has been set to " + value);
                m_triggerType = value;
            }

        }

        public bool ActivateTrigger()
        {
            switch (m_triggerType)
            {
                case TriggerType.OPEN_DOOR:
                    {
                        DoorOpen trigger = GetComponent<DoorOpen>();
                        trigger.ActivateTrigger = true;

                        return true;
                    };
                case TriggerType.WATERFALL_STOP:
                    {
                        DoorOpen trigger = GetComponent<DoorOpen>();
                        trigger.ActivateTrigger = true;
                        return true;
                    };
            }

            return false ;
        }
    }
}