// Disabled for on pre-render, re-enabled on post-render. Used to selectively disable
// fog on one camera.

using UnityEngine;

namespace cst
{
    public class CameraDisableFog : MonoBehaviour
    {
        private bool m_defaultState;

        public void OnStart()
        {
            m_defaultState = RenderSettings.fog;
        }

        private void OnPreRender()
        {
            RenderSettings.fog = false;
        }

        private void OnPostRender()
        {
            RenderSettings.fog = m_defaultState;
        }
    }
}