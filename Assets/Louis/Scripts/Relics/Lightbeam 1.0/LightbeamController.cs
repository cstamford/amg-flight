//
// Filename : LightbeamController.cs
// Author : Louis Dimmock
// Date : 27th March 2014
//
// Version : 1.0
// Version Info : 
// 		Controls the relic lights
//		Applys a texture offset using a specified amount
//

using UnityEngine;
using System.Collections;

public class LightbeamControllerV1 : MonoBehaviour
{
	// Set up the speed that the texture will move at
	public float m_movementSpeed = 0.0f;

	// Total texture offset
	private Vector2 m_textureOffset = new Vector2(0.0f, 0.0f);

	// Use this for initialization
	void Start ()
	{
		// Nothing to do
	}
	
	// Update is called once per frame
	void Update ()
	{
		// Translate the texture by the specified amount
		m_textureOffset.x += m_movementSpeed;

		// Keep the texture offset between 0 and 1
		if (m_textureOffset.y > 1.0f)
			m_textureOffset.y -= 1.0f;
		
		renderer.material.SetTextureOffset ("_MainTex", m_textureOffset);
	}
}
