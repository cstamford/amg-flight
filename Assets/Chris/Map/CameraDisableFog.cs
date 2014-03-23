// ==================================================================== \\
// File   : CameraDisableFog.cs                                         \\
// Author : Christopher Stamford									    \\
//                                                                      \\
// CameraDisableFog.cs disables fog selectively for one camera.         \\
// ==================================================================== \\

using UnityEngine;

namespace cst.Map
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