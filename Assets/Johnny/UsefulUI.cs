using UnityEngine;
using System.Collections;

public class UsefulUI : MonoBehaviour {

    Camera c;
    bool b;

    void Start()
    {
        c = GameObject.FindWithTag("MainCamera").camera;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            b = !b;
        }
    }

    void OnGUI()
    {
        print(c.transform.position.ToString());

        string output;


        if(b)
        {
            //FlightMovement fm = c.GetComponent<FlightMovement>();
            //output = "Velocity: " + fm.m_fo
        }
        else
        {
            output = "Boop";
        }
        
        //GUI.Box(new Rect(0, 0, 150, 25), output);
    }
}
