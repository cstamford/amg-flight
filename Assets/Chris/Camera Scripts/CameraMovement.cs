// Basic camera movement script for testing.
// Adapted from Unity's built-in script by Chris Stamford.

using UnityEngine;

namespace cst
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