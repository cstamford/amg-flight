// Authored by Sean

using UnityEngine;
using System.Collections;

namespace sv
{
    public class CrosshairGUI : MonoBehaviour
    {
        public Texture2D reticuleImage;

        private Vector3 crosshairPos;
        private float width, height;

        // Initialise the variables;
        void Start()
        {
            crosshairPos.x = 0;
            crosshairPos.y = 0;
            crosshairPos.z = 0;

            width = reticuleImage.width / 2.0f;
            height = reticuleImage.height / 2.0f;
        }

        // Draws the crosshair onto the screen in the centre of the screen
        // TODO: Possibly set reticule to move with mouse/controller stick?
        void OnGUI()
        {
            crosshairPos.x = (Screen.width / 2) - (reticuleImage.width / 2);
            crosshairPos.y = (Screen.height / 2) - (reticuleImage.height / 2);

            GUI.DrawTexture(new Rect(crosshairPos.x, crosshairPos.y, width, height), reticuleImage);
        }
    }
}
