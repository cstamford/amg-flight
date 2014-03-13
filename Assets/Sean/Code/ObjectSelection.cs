//==========================================================
// Author: Sean Vieira
// Version: 1.0
// Function: Handles the selection of objects, through the 
// use of a ray that is cast from the centre of the screen 
// in the direction the camera is facing
//==========================================================

using UnityEngine;
using System.Collections;

namespace sv
{
    public class ObjectSelection : MonoBehaviour
    {
        [SerializeField] private float m_rayLength;
        [SerializeField] private GameObject m_cursor;

        private RaycastHit m_hit;
        private Ray m_crosshairRay;
        private PuzzleKeypad m_keypad;
        private KeypadKeyClass m_keyPressed;
        private GameObject m_selected;

        // Use this for initialization
        void Start()
        {
            if (!m_cursor)
            {
                Debug.Log(this.name + " has no cursor attached.");
            }
            else
            {
                m_cursor.SetActive(false);
            }


        }

        // Update is called once per frame
        void Update()
        {
            m_cursor.SetActive(false);
            
            if (CastRay())
            {
                m_selected = m_hit.collider.gameObject;

                
                
                if (!m_selected)
                {
                    if (Input.GetMouseButtonDown(0))
                    {
                        Debug.Log("Nothing has been selected");
                    }
                }
                else
                {                    
                    
                    // Check for keypad/keypad key objects
                    if (m_selected.tag == "keypad")
                    {
                        m_cursor.SetActive(true);
                        AcquireKeypadObject(m_selected);
                    }
                    else if (m_selected.tag == "keypad key")
                    {
                        m_cursor.SetActive(true);
                        AcquireKeypadButtonObject(m_selected);
                    }
                    
                    if (Input.GetMouseButtonDown(0))
                    {
                        Debug.Log("Ray intersects with " + m_selected.name);
                        
                        if (m_selected.tag == "keypad key")
                        {
                            m_keypad.AddKeyToPassword(m_keyPressed.GetKeyValue());
                        }
                        
                    }
                    else
                    {
                        if (Input.GetKeyDown(KeyCode.E))
                        {
                            if (m_selected.tag == "keypad")
                            {
                                AcquireKeypadObject(m_selected);

                                m_keypad.DisplayTextTip(true);
                            }
                            else if (m_selected.tag == "keypad key")
                            {
                                AcquireKeypadObject(m_keyPressed.GetParent());

                                m_keypad.DisplayTextTip(true);
                            }
                        }
                    }
                }
            }
            else
            {
                if (Input.GetMouseButtonDown(0))
                {
                    Debug.Log("Ray has no intersections");
                }
            }           
        }

        // Casts a ray forward from the camera, returns true if intersection
        bool CastRay()
        {
            m_crosshairRay = new Ray(transform.position, transform.forward);

            Debug.DrawRay(transform.position, transform.forward * m_rayLength);

            if (Physics.Raycast(m_crosshairRay, out m_hit, m_rayLength))
            {
                return true;
            }

            return false;
        }

        // Acquire the keypad component from target GameObject            
        void AcquireKeypadObject(GameObject target)
        {
            PuzzleKeypad temp = target.GetComponent<PuzzleKeypad>();

            if (!m_keypad)
            {
                m_keypad = temp;
            }
            else
            {
                if (m_keypad != temp)
                {
                    m_keypad = temp;
                }
            }
        }

        // Acquire the keypad key component from target GameObject  
        void AcquireKeypadButtonObject(GameObject target)
        {
            KeypadKeyClass temp = target.GetComponent<KeypadKeyClass>();

            if (!m_keyPressed)
            {
                m_keyPressed = temp;
            }
            else
            {
                if (m_keyPressed != temp)
                {
                    m_keyPressed = temp;
                }
            }

            
            if (!m_keypad)
            {
                AcquireKeypadObject(m_keyPressed.GetParent());
            }
        }
    }
}