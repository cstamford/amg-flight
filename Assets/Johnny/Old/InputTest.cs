using UnityEngine;
using System.Collections;

public class InputTest : MonoBehaviour {

    bool hitTrigger;
    bool keyPressed;
    string output;
    bool paused;

	// Use this for initialization
	void Start () {
        hitTrigger = false;
        keyPressed = false;
        paused = false;
	}
	
	// Update is called once per frame
	void Update () {
        //print("Updating");
        if (Input.GetKey(KeyCode.E))
        {
            keyPressed = true;
        }
        else
        {
            keyPressed = false;
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            paused = true;
            Time.timeScale = 0;
        }
	}

    void OnTriggerEnter(Collider other)
    {
        print("Exiting Trigger");
        if (other.gameObject.tag == "TestTrigger")
        {
            hitTrigger = true;
            output = other.gameObject.tag;
        }
    }

    void OnTriggerStay(Collider other)
    {
        print("Entered Trigger");
    }

    void OnTriggerExit(Collider other)
    {
        print("Exiting Trigger");
        if (other.gameObject.tag == "TestTrigger")
        {
            hitTrigger = false;
            output = "";
        }
    }

    void OnGUI()
    {
        GUI.Box(new Rect(0, 0, 150, 25), "ht: " + hitTrigger);
        GUI.Box(new Rect(0, 25, 150, 25), "kp: " + keyPressed);
        GUI.Box(new Rect(0, 50, 150, 25), "collider: " + output);
        
        if (hitTrigger && keyPressed)
        {
            GUI.Box(new Rect(0, Screen.height / 2, Screen.width, Screen.height / 2), "Lorem Ipsum scroll crap stuff");
        }

        if (paused)
        {
            // Make a group on the center of the screen
            GUI.BeginGroup(new Rect(Screen.width / 2 - 50, Screen.height / 2 - 50, 100, 100));
            // All rectangles are now adjusted to the group. (0,0) is the topleft corner of the group.

            // We'll make a box so you can see where the group is on-screen.
            GUI.Box(new Rect(0, 0, 100, 100), "Game Paused");

            if (GUI.Button(new Rect(10, 40, 80, 30), "Click me"))
            {
                paused = false;
                Time.timeScale = 1;
            }

            // End the group we started above. This is very important to remember!
            GUI.EndGroup();
        }
    }
}
