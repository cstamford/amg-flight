using UnityEngine;
using System.Collections;

public class GUITest2 : MonoBehaviour {
    void OnGUI()
    {
        Texture2D icon;

        icon = new Texture2D(100, 50);

        float w = 100, h = 50;

        //  Left Eye

        //  Place text on corners, edges and center
        GUI.Box(new Rect(0, 0, w, h), "Top-left"); GUI.Box(new Rect((Screen.width / 2 - w) / 2, 0, w, h), "Top-midddle"); GUI.Box(new Rect(Screen.width / 2 - w, 0, w, h), "Top-right");
        GUI.Box(new Rect(0, Screen.height / 2 - h / 2, w, h), "Middle-left"); GUI.Box(new Rect(Screen.width / 4 - w / 2, Screen.height / 2 - h / 2, w, h), "Middle-midddle"); GUI.Box(new Rect(Screen.width / 2 - w, Screen.height / 2 - h / 2, w, h), "Middle-right");
        GUI.Box(new Rect(0, Screen.height - h, w, h), "Bottom-left"); GUI.Box(new Rect(Screen.width / 4 - w / 2, Screen.height - h, w, h), "Bottom-middle"); GUI.Box(new Rect(Screen.width / 2 - w, Screen.height - h, w, h), "Bottom-right");

        //  Place images in gaps
        GUI.Box(new Rect(Screen.width / 8 - w / 4, 0, w, h), new GUIContent(icon)); GUI.Box(new Rect(Screen.width * 3 / 8 - w * 3 / 4, 0, w, h), new GUIContent(icon));
        GUI.Box(new Rect(0, (Screen.height - h) / 4, w, h), new GUIContent(icon)); GUI.Box(new Rect(Screen.width / 2 - w, (Screen.height - h) / 4, w, h), new GUIContent(icon)); GUI.Box(new Rect(Screen.width / 4 - w / 2, (Screen.height - h) / 4, w, h), new GUIContent(icon));
        GUI.Box(new Rect(Screen.width / 8 - w / 4, (Screen.height - h) / 2, w, h), new GUIContent(icon)); GUI.Box(new Rect(Screen.width * 3 / 8 - w * 7 / 8, (Screen.height - h) / 2, w, h), new GUIContent(icon));
        GUI.Box(new Rect(0, Screen.height * 3 / 4 - h * 3 / 4, w, h), new GUIContent(icon)); GUI.Box(new Rect(Screen.width / 4 - w / 2, (Screen.height - h) * 3 / 4, w, h), new GUIContent(icon)); GUI.Box(new Rect((Screen.width) / 2 - w, (Screen.height - h) * 3 / 4, w, h), new GUIContent(icon));
        GUI.Box(new Rect(Screen.width / 8 - w / 4, Screen.height - h, w, h), new GUIContent(icon)); GUI.Box(new Rect(Screen.width * 3 / 8 - w * 3 / 4, Screen.height - h, w, h), new GUIContent(icon));

        //  Right Eye
        
        //  Place text on corners, edges and center
        GUI.Box(new Rect(0 + Screen.width / 2, 0, w, h), "Top-left"); GUI.Box(new Rect((Screen.width / 2 - w) / 2 + Screen.width / 2, 0, w, h), "Top-midddle"); GUI.Box(new Rect(Screen.width / 2 - w + Screen.width / 2, 0, w, h), "Top-right");
        GUI.Box(new Rect(0 + Screen.width / 2, Screen.height / 2 - h / 2, w, h), "Middle-left"); GUI.Box(new Rect(Screen.width / 4 - w / 2 + Screen.width / 2, Screen.height / 2 - h / 2, w, h), "Middle-midddle"); GUI.Box(new Rect(Screen.width / 2 - w + Screen.width / 2, Screen.height / 2 - h / 2, w, h), "Middle-right");
        GUI.Box(new Rect(0 + Screen.width / 2, Screen.height - h, w, h), "Bottom-left"); GUI.Box(new Rect(Screen.width / 4 - w / 2 + Screen.width / 2, Screen.height - h, w, h), "Bottom-middle"); GUI.Box(new Rect(Screen.width / 2 - w + Screen.width / 2, Screen.height - h, w, h), "Bottom-right");

        //  Place images in gaps
        GUI.Box(new Rect(Screen.width / 8 - w / 4 + Screen.width / 2, 0, w, h), new GUIContent(icon)); GUI.Box(new Rect(Screen.width * 3 / 8 - w * 3 / 4 + Screen.width / 2, 0, w, h), new GUIContent(icon));
        GUI.Box(new Rect(0 + Screen.width / 2, (Screen.height - h) / 4 + Screen.width / 2, w, h), new GUIContent(icon)); GUI.Box(new Rect(Screen.width / 2 - w + Screen.width / 2, (Screen.height - h) / 4, w, h), new GUIContent(icon)); GUI.Box(new Rect(Screen.width / 4 - w / 2 + Screen.width / 2, (Screen.height - h) / 4, w, h), new GUIContent(icon));
        GUI.Box(new Rect(Screen.width / 8 - w / 4 + Screen.width / 2, (Screen.height - h) / 2, w, h), new GUIContent(icon)); GUI.Box(new Rect(Screen.width * 3 / 8 - w * 7 / 8 + Screen.width / 2, (Screen.height - h) / 2, w, h), new GUIContent(icon));
        GUI.Box(new Rect(0 + Screen.width / 2, Screen.height * 3 / 4 - h * 3 / 4, w, h), new GUIContent(icon)); GUI.Box(new Rect(Screen.width / 4 - w / 2 + Screen.width / 2, (Screen.height - h) * 3 / 4, w, h), new GUIContent(icon)); GUI.Box(new Rect((Screen.width) / 2 - w + Screen.width / 2, (Screen.height - h) * 3 / 4, w, h), new GUIContent(icon));
        GUI.Box(new Rect(Screen.width / 8 - w / 4 + Screen.width / 2, Screen.height - h, w, h), new GUIContent(icon)); GUI.Box(new Rect(Screen.width * 3 / 8 - w * 3 / 4 + Screen.width / 2, Screen.height - h, w, h), new GUIContent(icon));

    }
}
