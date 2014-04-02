//
// Filename : RotatingObject.cs
// Author : Louis Dimmock
// Date : 10th Feburary 2014
//
// Version : 1.0
// Version Info : 
// 		Simple script that rotates an object around its center pivot at a set speed
//

using UnityEngine;
using System.Collections;

public class RotatingObject : MonoBehaviour
{
	// Speed that the relic rotates at
	public float m_rotationSpeed = 15.0f;

	// Current rotation
	private Vector3 m_rotation = new Vector3(0.0f, 0.0f, 0.0f);

	// Use this for initialization
	void Start ()
	{
		m_rotation = transform.eulerAngles;
	}
	
	// Update is called once per frame
	void Update ()
	{
		// Apply rotation
		m_rotation.y += m_rotationSpeed * Time.deltaTime;

		// Make sure rotation remains between 0 and 360
		Wrap(ref m_rotation.y);

		// Apply transformation to wings
		transform.eulerAngles = m_rotation;
	}

	private void Wrap(ref float value)
	{
		// Wrap around to 360
		if (value < 0)
		{
			value += 360.0f;
		}
		else if (value > 360.0f) // Wrap around to 0
		{
			value -= 360.0f;
		}
	}
}
