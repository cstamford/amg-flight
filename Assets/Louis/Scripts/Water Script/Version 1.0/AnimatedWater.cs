//
// Filename : AnimatedWater.cs
// Author : Louis Dimmock
// Date : 10th Feburary 2014
//
// Version : 1.0
// Version Info : 
// 		Simple script that provides material editing functionality.
//		Reduces the amount of materials needed.
//		The angle that the water flows at can be set.
//		Allows a textures, texture tiling and texture offset to be set
//

using UnityEngine;
using System.Collections;

public class AnimatedWaterV1 : MonoBehaviour
{
	// The color to tint the water
	public Color m_Tint = new Color(1.0f, 1.0f, 1.0f, 1.0f);
	
	// Specular
	public Color m_specularColor = new Color(1.0f, 1.0f, 1.0f, 1.0f);
	public float m_specularAmount = 10.0f;

	// Angle that the water flows at
	public float m_waterAngle = 0.0f;
	
	// Define which textures to use
	public Texture2D m_waterTexture = null;
	public Texture2D m_normalTexture = null;
	
	// Define the texture tiling
	public Vector2 m_waterTiling = new Vector2(1.0f, 1.0f);
	public Vector2 m_normalTiling = new Vector2(1.0f, 1.0f);
	
	// Define how fast the water will move
	public float m_offsetSpeed = 0.0f;
	
	// Store total offset for textures
	private Vector2 m_waterOffset = new Vector2(0.0f, 0.0f);
	private Vector2 m_normalOffset = new Vector2(0.0f, 0.0f);
	
	void Start ()
	{
		// We set the angle in degrees in the inspector
		// To calculate movement, this needs to be in radians
		// So lets convert to radians
		m_waterAngle *= Mathf.Deg2Rad;
		
		// Set the textures
		renderer.material.SetTexture("_MainTex", m_waterTexture);
		renderer.material.SetTexture("_BumpMap", m_normalTexture);
		
		// Set the texture tiling
		renderer.material.SetTextureScale("_MainTex", m_waterTiling);
		renderer.material.SetTextureScale("_BumpMap", m_normalTiling);
		
		// Set the water tint
		renderer.material.SetColor("_Color", m_Tint);
		
		// Set the specular
		renderer.material.SetColor("_SpecularColor", m_specularColor);
		renderer.material.SetFloat("_SpecularAmount", m_specularAmount);
	}
	
	void Update ()
	{
		UpdateOffset( ref m_waterOffset, m_offsetSpeed );
		UpdateOffset( ref m_normalOffset, m_offsetSpeed );
	
		// Translate the texture offset based on speed
		renderer.material.SetTextureOffset ("_MainTex", m_waterOffset);
		renderer.material.SetTextureOffset ("_BumpMap", m_normalOffset);
	}

	private void UpdateOffset(ref Vector2 Offset, float Speed)
	{
		// Offset the texture based on water speed and angle of water flow
		Offset.x += Speed * Mathf.Cos( m_waterAngle );
		Offset.y += Speed * Mathf.Sin( m_waterAngle ); 

		// Keep the offset within a range of 0 and 1
		if(Offset.x > 1.0f)
			Offset.x -= 1.0f;
		if(Offset.y > 1.0f)
			Offset.y -= 1.0f;
	}
}