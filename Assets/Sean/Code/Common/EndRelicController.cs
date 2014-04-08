// Filename: OldRelicController.cs  
// Author: Sean Vieira
// Date: 4th April 2014
//
// Version: 1.1
// Version Info:
//      Identical to Louis' original script, with the exception that
//      the relic now moves based on it's local position, rather than 
//      it's global position. Now contains a light that is equal to 
//      the materials colour
// ====================================================================
// Filename : RelicController.cs
// Author : Louis Dimmock
// Date : 10th Feburary 2014
//
// Version : 1.0
// Version Info : 
// 		Simple script that provides visual functionality to the relic.
//		Rotates the relic around its center pivot by a set speed.
//		Moves the relic up and down using a Sine wave based.
//

using UnityEngine;
using System.Collections;

namespace sv
{
    public class EndRelicController : MonoBehaviour
    {
        [SerializeField] private Light m_light;
        [SerializeField] private float m_glowRate;
        public float m_rotationSpeed = 15.0f;
        public float m_oscMagnitude = 1.0f;
        private Vector3 m_startPosition;
        private Vector3 m_offsetPosition;
        private Vector3 m_rotation = new Vector3(0.0f, 0.0f, 0.0f);
        private float m_frameCount = 0.0f;
        private float m_lightIntensityOriginal;

        // Use this for initialization
        void Start()
        {
            m_startPosition = transform.localPosition;
            m_rotation = transform.eulerAngles;
            m_lightIntensityOriginal = m_light.intensity;
        }

        // Update is called once per frame
        void Update()
        {
            HandleRotate();
            HandleOscillation();
            HandleColourChange();
            HandleLightGlow();

            m_frameCount++;
        }

        private void Wrap(ref float value)
        {
            // Wrap around to 360
            if (value < 0)
            {
                value += 360.0f;
            }
            else if (value > 360.0f) // Wrap around to 0
            {
                value -= 360.0f;
            }
        }

        // Rotate the object around the x-axis
        private void HandleRotate()
        {           
            m_rotation.x += m_rotationSpeed * Time.deltaTime;
            Wrap(ref m_rotation.x);
            transform.eulerAngles = m_rotation;
        }

        // Translate the object based on a sin curve, so that it appears to oscillate
        private void HandleOscillation()
        {
            m_offsetPosition.x = (m_oscMagnitude / 10.0f) * (Mathf.Sin(m_frameCount / 30) / 3.0f);
            transform.localPosition = m_startPosition + m_offsetPosition;
        }

        // Sets the specified light to be the specular colour of the relic
        private void HandleColourChange()
        {
            Color matColour;

            if (GetComponent<MeshRenderer>().material.HasProperty("_SpecColor"))
            {
                matColour = GetComponent<MeshRenderer>().material.GetColor("_SpecColor");
                if (m_light.color != matColour)
                {
                    m_light.color = matColour;
                }
            }
        }

        private void HandleLightGlow()
        {
            float lightIntensityOffset;

            float angle = (m_glowRate * m_frameCount) / 25.0f;
            lightIntensityOffset = Mathf.Cos(angle) * m_lightIntensityOriginal;
            m_light.intensity = m_lightIntensityOriginal + lightIntensityOffset;
        }

    }
}