using UnityEngine;
using System.Collections;

public class GravityFixer : MonoBehaviour {

    Vector3 position;

	// Use this for initialization
	void Start () {
        position = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
        transform.position = position;
	}
}
