// Authored by Sean

using UnityEngine;
using System.Collections;

namespace sv
{
    public class ObjectSelection : MonoBehaviour
    {


        private float rayLength = 175.0f;
        private RaycastHit hit;
        private Ray crosshairRay;

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
                // Currently only in for debug purposes
                // TODO: Create a keycoded puzzle
                if (Input.GetMouseButtonDown(0))
                {
                    Debug.Log("Ray intersects with " + hit.collider.name);
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

        // Casts a ray forward from the camera
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
    }
}