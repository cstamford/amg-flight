//===========================================================
// Author: Sean
// Version: 1.0
// Function: if applied to GUIText object, will cause the
// text to fade out for the specified length of time
//===========================================================

using UnityEngine;
using System.Collections;

namespace sv
{
    public class TextFade : MonoBehaviour
    {
        [SerializeField] private float fadeLength; 
        private float m_timer;
        private Color m_color;
        

        // Use this for initialization
        void Start()
        {
            m_timer = 0.0f;            
            fadeLength = cst.Helpers.low(fadeLength, 0.5f);

            m_color = Color.white;
            guiText.color = m_color;

            guiText.enabled = false;
        }

        // Update is called once per frame
        void Update()
        {
            // Fade the text out if enabled
            if (guiText.enabled)
            {
                if (m_timer >= 0.01f)
                {
                    m_color.a -= (m_timer / fadeLength);
                    m_timer = 0.0f;
                }

                if (m_color.a <= 0.0f)
                {
                    guiText.enabled = false;
                }

                guiText.color = m_color;

                m_timer += Time.deltaTime;
            }
            else
            {
                m_color.a = 1.0f;
            }
        }
    }
    
}