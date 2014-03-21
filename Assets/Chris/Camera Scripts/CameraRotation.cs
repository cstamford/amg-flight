// Basic camera rotation script for testing.
// Adapted from Unity's built-in script by Chris Stamford.

using UnityEngine;

namespace cst.Camera
{
    public class CameraRotation : MonoBehaviour
    {
        private enum RotationAxes
        {
            MOUSE_X = 0,
            MOUSE_Y,
            MOUSE_X_AND_Y
        }

        private RotationAxes m_axes = RotationAxes.MOUSE_X_AND_Y;
        [SerializeField] private float m_sensitivityX = 7.5f;
        [SerializeField] private float m_sensitivityY = 7.5f;
        [SerializeField] private float m_minimumX = -360.0f;
        [SerializeField] private float m_maximumX = 360.0f;
        [SerializeField] private float m_minimumY = -60.0f;
        [SerializeField] private float m_maximumY = 60.0f;
        [SerializeField] private float m_rotationY = 0.0f;

        // Use this for initialization
        private void Start()
        {
        }

        // Update is called once per frame
        private void Update()
        {
            // Rotate camera
            if (m_axes == RotationAxes.MOUSE_X_AND_Y)
            {
                float rotationX = transform.localEulerAngles.y 
                    + Input.GetAxis("Mouse X") * m_sensitivityX;

                m_rotationY += Input.GetAxis("Mouse Y") * m_sensitivityY;
                m_rotationY = Mathf.Clamp(m_rotationY, m_minimumY, m_maximumY);

                transform.localEulerAngles = new Vector3(-m_rotationY, 
                    rotationX, 0);
            }
            else if (m_axes == RotationAxes.MOUSE_X)
            {
                transform.Rotate(0, Input.GetAxis("Mouse X") * m_sensitivityX, 0);
            }
            else if (m_axes == RotationAxes.MOUSE_Y)
            {
                m_rotationY += Input.GetAxis("Mouse Y") * m_sensitivityY;
                m_rotationY = Mathf.Clamp(m_rotationY, m_minimumY, m_maximumY);

                transform.localEulerAngles = new Vector3(-m_rotationY, 
                    transform.localEulerAngles.y, 0);
            }
        }
    }
}