//=================================================================
// Author: Sean Vieira
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
    
    public class TriggerController <T>
    {
        private T m_activeTrigger;
        private TriggerType m_triggerType;

        public T ActiveTrigger
        {
            get { return m_activeTrigger; }
            set
            {
                Debug.Log("Active trigger set to type: " + value);
                m_activeTrigger = value;
            }
        }

        public TriggerType Type
        {
            get { return m_triggerType; }
            set
            {
                Debug.Log("Active Trigger type has been set to " + value);
                m_triggerType = value;
            }

        }

        public void SetActiveTrigger(TriggerType type, T trigger)
        {
            m_triggerType = type;
            m_activeTrigger = trigger;
        }

        public bool ActivateTrigger()
        {
            

            return true;
        }
    }
}