using UnityEngine;
using System.Collections;

public class CameraMovement : MonoBehaviour 
{
    public float height = 16.0f;

	// Use this for initialization
	void Start () 
    {
	
	}
	
	// Update is called once per frame
	void Update () 
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

        float terrainHeight = Terrain.activeTerrain.SampleHeight(transform.position);

        if (position.y < terrainHeight + height)
            position.y = terrainHeight + height;

        transform.position = position;
	}
}
