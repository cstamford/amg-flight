// ==================================================================== \\
// File   : ForceGlide.cs                                               \\
// Author : Christopher Stamford									    \\
//                                                                      \\
// Forces the Seraph to enter glide mode when leaving a trigger.        \\
// ==================================================================== \\

using UnityEngine;

namespace cst.Flight
{
    public class ForceGlide : MonoBehaviour
    {
        private bool m_used = false;

        public void OnTriggerExit(Collider other)
        {
            if (m_used)
                return;

            SeraphController controller = other.GetComponent<SeraphController>();

            if (controller != null && controller.capability >= SeraphCapability.GLIDE)
            {
                m_used = true;
                controller.state = SeraphState.GLIDING;
            }
        }
    }
}
