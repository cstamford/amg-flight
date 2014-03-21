using UnityEngine;
using System.Collections;

[System.Serializable]
public class ShaderTexture
{
	// Variable to store the texture
	public Texture2D m_Texture = null;
	// Variable to store how fast to offset
	public float m_Speed = 0.0f;
	// Variable to store how many times to tile the texture
	public Vector2 m_Tile = new Vector2(1.0f, 1.0f);
}

public class AnimatedWater : MonoBehaviour
{
	// The color to tint the water (default to clear black)
	public Color m_Tint = new Color(0.0f, 0.0f, 0.0f, 0.0f);
	
	// The color to tint the water (default white)
	public Color m_specularColor = new Color(1.0f, 1.0f, 1.0f, 1.0f);
	
	// How shiny the water is
	public float m_specularAmount = 10.0f;

	// Angle that the water flows at
	public float m_waterAngle = 0.0f;

	// Objects to store data about each texture
	public ShaderTexture m_waterTexture = new ShaderTexture();
	public ShaderTexture m_normalTextureA = new ShaderTexture();
	public ShaderTexture m_normalTextureB = new ShaderTexture();
	
	// Vectors to store the texture offsets
	private Vector2 m_waterMovement = new Vector2(0.0f, 0.0f);
	private Vector2 m_normalMovementA = new Vector2(0.0f, 0.0f);
	private Vector2 m_normalMovementB = new Vector2(0.0f, 0.0f);
	
	void Start ()
	{
		// We set the angle in degrees in the inspector
		// To calculate movement, this needs to be in radians
		// So lets convert to radians
		m_waterAngle *= Mathf.Deg2Rad;

		// Providing the texture has been set in the inspector
		if(m_waterTexture.m_Texture != null)
		{
			// Set the texture and the tiling
			SetTexture("_MainTex", m_waterTexture);
		}

		// Normal maps
		if(m_normalTextureA.m_Texture != null)
		{
			// Set the texture and the tiling
			SetTexture("_BumpMapA", m_waterTexture);
		}
		if(m_normalTextureB.m_Texture != null)
		{
			// Set the texture and the tiling
			SetTexture("_BumpMapB", m_waterTexture);
		}

		// The color to tint the water
		SetColor("_Color", m_Tint);
		
		// The color to tint the water
		SetColor("_SpecularColor", m_specularColor);
		
		// How shiny the water is
		SetFloat("_SpecularAmount", m_specularAmount);

	}
	
	void Update ()
	{
		// Translate the texture offset based on speed
		UpdateOffset(ref m_waterMovement, m_waterTexture.m_Speed);
		UpdateOffset(ref m_normalMovementA, m_normalTextureA.m_Speed);
		UpdateOffset(ref m_normalMovementB, m_normalTextureB.m_Speed);

		// Apply the offsets to the texture
		SetOffset("_MainTex", m_waterMovement);
		SetOffset("_BumpMapA", m_normalMovementA);
		SetOffset("_BumpMapB", m_normalMovementB);
	}

	private void UpdateOffset(ref Vector2 Offset, float Speed)
	{
		Offset.x += Speed * Mathf.Cos( m_waterAngle );
		Offset.y += Speed * Mathf.Sin( m_waterAngle ); 

		// Keep the offset within a range of 0 and 1
		if(Offset.x > 1.0f)
			Offset.x -= 1.0f;
		if(Offset.y > 1.0f)
			Offset.y -= 1.0f;
	}

	private void SetTexture(string TextureName, ShaderTexture Tex)
	{
		// Set the texture in the shader
		renderer.material.SetTexture(TextureName, Tex.m_Texture);
		// Set the texture tiling in the shader
		renderer.material.SetTextureScale(TextureName, Tex.m_Tile);
	}

	private void SetOffset(string TextureName, Vector2 Offset)
	{
		renderer.material.SetTextureOffset (TextureName, Offset);
	}

	private void SetColor(string ColorName, Color Value)
	{
		renderer.material.SetColor(ColorName, Value);
	}

	private void SetFloat(string FloatName, float Value)
	{
		renderer.material.SetFloat(FloatName, Value);
	}
}