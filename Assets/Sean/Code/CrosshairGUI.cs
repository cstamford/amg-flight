//==========================================================
// Author: Sean Vieira
// Version: 1.0
// Function: Positions a crosshair in the centre of the 
// screen. This is purely to aid with raycasting and 
// object selection
//==========================================================

using UnityEngine;
using System.Collections;

namespace sv
{
    public class CrosshairGUI : MonoBehaviour
    {
        [SerializeField] public Texture2D m_reticuleImage;

        private Vector3 m_crosshairPos;
        private float m_width, m_height;

        // Initialise the variables;
        void Start()
        {
            m_crosshairPos.x = 0;
            m_crosshairPos.y = 0;
            m_crosshairPos.z = 0;

            m_width = m_reticuleImage.width / 2.0f;
            m_height = m_reticuleImage.height / 2.0f;
        }

        // Draws the crosshair onto the screen in the centre of the screen
        // TODO: Possibly set reticule to move with mouse/controller stick?
        void OnGUI()
        {
            m_crosshairPos.x = (Screen.width / 2);
            m_crosshairPos.y = (Screen.height / 2);

            GUI.DrawTexture(new Rect(m_crosshairPos.x, m_crosshairPos.y, m_width, m_height), m_reticuleImage);
        }
    }
}
