//
// Filename : Glow.shader
// Author : Louis Dimmock
// Date : 2nd April 2014
//
// Current Version : 1.1
// Version Info : 
// 		Removed the ability to use transparency for the actual object
//
// Previous Versions:
//		1.0 : Simple surface shader that adds an inside glow effect to the attached object. Main use is for relics.
//

Shader "Flight/Glow/1.1"
{
	// Define our properties, accessed my the inspector
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_BumpMap ("Bumpmap", 2D) = "bump" {}
		_Transparency ("Transparency", Range(0.0, 1.0)) = 1.0
		_GlowColor ("Glow Color", Color) = (1, 1, 1, 1)
		_GlowAmount ("Glow Power", Range(0.0, 10.0)) = 5.0
	}

	SubShader
	{
     	// Set the shader tags
		Tags
		{
        	"Queue" = "Transparent" // We want to render this after solid objects
			"RenderType" = "Opaque" // Categorise the shader
		}

		CGPROGRAM
		
        // Define the shader function and the light model to use
		#pragma surface GlowSurfaceShader BlinnPhong
		
		// Texture samplers
		sampler2D _MainTex;
		sampler2D _BumpMap;
		
        // Color tint
		float _Transparency;
		
		// Glow
		float4 _GlowColor;
		float _GlowAmount;
		
    	// Define our data struct
		struct Input
		{
			float2 uv_MainTex;
			float2 uv_BumpMap;
			float3 viewDir;
		};
		
		// Surface Shader
		void GlowSurfaceShader (Input IN, inout SurfaceOutput output)
		{
			// Sample the textures
			float4 textureColor = tex2D (_MainTex, IN.uv_MainTex);
			float4 normalMap = tex2D (_BumpMap, IN.uv_BumpMap);
			
			// Set the output color
			output.Albedo = textureColor.rgb;
			
			// Unpack the normal
			output.Normal = UnpackNormal ( normalMap );
			
			// Calculate glow amount based on view direction
			half Glow = 1.0 - saturate( dot (normalize(IN.viewDir), output.Normal) );
			
			// Calculate the final glow output
			output.Emission = _GlowColor * pow (Glow, _GlowAmount) * _GlowColor.a;
			
            // Set the transparency using the texture alpha channel and the color tint
            output.Alpha = textureColor.a * _Transparency;
		}
		
		ENDCG
	}
	
	// Define which shader to use if this shader fails to run
	Fallback "Diffuse"
}