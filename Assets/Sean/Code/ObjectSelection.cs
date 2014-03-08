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
        [SerializeField] private float rayLength;

        private RaycastHit hit;
        private Ray crosshairRay;
        private PuzzleKeypad keypad;
        private GameObject selected;

        // Use this for initialization
        void Start()
        {
            /* Empty */
        }

        // Update is called once per frame
        void Update()
        {
            if (CastRay())
            {
                selected = hit.collider.gameObject;

                if (!selected)
                {
                    if (Input.GetMouseButtonDown(0))
                    {
                        Debug.Log("Nothing has been selected");
                    }
                }
                else
                {

                    if (selected.tag == "keypad")
                    {
                        if (Input.GetMouseButtonDown(0))
                        {
                            Debug.Log("Ray intersects with " + selected.name);

                            if (!keypad)
                            {
                                AcquireKeypadObject(selected);
                            }

                            keypad.DisplayTextTip(true);
                        }
                        else if (Input.GetKeyDown(KeyCode.E))
                        {
                            if (!keypad)
                            {
                                AcquireKeypadObject(selected);
                            }

                            keypad.DisplayGUI(true);
                        }
                    }
                    else
                    {
                        if (Input.GetMouseButtonDown(0))
                        {
                            Debug.Log("Ray intersects with " + selected.name);
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
            crosshairRay = new Ray(transform.position, transform.forward);

            Debug.DrawRay(transform.position, transform.forward * rayLength);

            if (Physics.Raycast(crosshairRay, out hit, rayLength))
            {
                return true;
            }

            return false;
        }

        // Acquire the keypad component from target GameObject            
        void AcquireKeypadObject(GameObject target)
        {
            if (target.tag == "keypad")
            {
                keypad = target.GetComponent<PuzzleKeypad>();
                Debug.Log("Keypad variable acquired");
            }
        }
    }
}