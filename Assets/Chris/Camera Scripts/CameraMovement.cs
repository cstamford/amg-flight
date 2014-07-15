// ==================================================================== \\
// File   : CameraMovement.cs                                           \\
// Author : Christopher Stamford                                        \\
//                                                                      \\
// CameraMovement.cs provides basic debug movement when attached to an  \\
// object.                                                              \\
//                                                                      \\
// This script should only be used for debug purposes.                  \\
// ==================================================================== \\

using UnityEngine;

namespace cst.Camera
{
    public class CameraMovement : MonoBehaviour
    {
        // Use this for initialization
        private void Start()
        {

        }

        // Update is called once per frame
        private void Update()
        {
            Vector3 position = transform.position;

            // Move position
            if (Input.GetKey(KeyCode.W))
                position += transform.forward;

            if (Input.GetKey(KeyCode.A))
                position -= transform.right;

            if (Input.GetKey(KeyCode.S))
                position -= transform.forward;

            if (Input.GetKey(KeyCode.D))
                position += transform.right;

            transform.position = position;
        }
    }
}