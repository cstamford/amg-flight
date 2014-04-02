//
//	Filename : Water.shader
//	Author : Louis Dimmock
//	Date : 6th March 2014
//
//	Version : 3.0
//	Versions Notes :
//		The shader has been rewritten to use a surface shader, this allows the material to use transparency.
//		After testing/discussions with the group, bump map intensity was removed as it was felt it was an unnecessary feature.
//
//	Previous Versions : 			  
//		2.0 : Advanced version 1.0 by adding an additional normal map.
//		1.0 : Basic fragment shader utilising a normal map and specular lighting.
//

Shader "Flight/Water/3.0"
{
	// Set up the shader properties that the user can customise
    Properties
    {
		// Water Tint
		_Color ("Color Tint", Color) = (1, 1, 1, 1)
		
		// Water Shininess
		_SpecularColor ("Specular Color", Color) = (1, 1, 1, 1)
		_SpecularAmount ("Specular Amount", Float) = 10
		
		// Textures
		_MainTex ("Water Texture", 2D) = "white" { }
		_BumpMapA ("Bump Map 1", 2D) = "bump" { }
		_BumpMapB ("Bump Map 2", 2D) = "bump" { }
    }
    
    SubShader
     {
     	// Set the shader tags
        Tags
        {
        	"Queue" = "AlphaTest" // We want to render this after solid objects
        	"IgnoreProjector" = "True"
        	"RenderType" = "Transparent" // Categorise the shader
		}
        
        // Set the level of detail distance
        LOD 400 // This will need to be modified once we have added it to the game
        
        CGPROGRAM
        
        // Define the shader function and the light model to use
        #pragma surface WaterSurfaceShader BlinnPhong alpha

		// Texture samplers
        sampler2D _MainTex;
        sampler2D _BumpMapA;
        sampler2D _BumpMapB;
        
        // Color tint
        float4 _Color;
        
        // Texture shininess
        float _SpecularAmount;
    
    	// Define our data struct
        struct Input
        {
            float2 uv_MainTex;
            float2 uv_BumpMapA;
            float2 uv_BumpMapB;
            INTERNAL_DATA
        };
		
		// Surface Shader
        void WaterSurfaceShader(Input IN, inout SurfaceOutput output)
        {            
        	// Sample the main texture
            float4 textureColor = tex2D (_MainTex, IN.uv_MainTex);
            
            // Sample the two normal maps
            float4 normalA = tex2D(_BumpMapA, IN.uv_BumpMapA);
            float4 normalB = tex2D(_BumpMapB, IN.uv_BumpMapB);
            
            // Expand the normals, combine them and apply to the output
            output.Normal = normalize( UnpackNormal( normalA ) + UnpackNormal( normalB ) );
            
            // Set the specular
            output.Specular = _SpecularAmount;
            output.Gloss = textureColor.a;
            
            // Set the texture color
            output.Albedo = textureColor.rgb * _Color;
            
            // Set the transparency using the texture alpha channel and the color tint
            output.Alpha = textureColor.a * _Color.a;
        }
        
        ENDCG
    }
    
    // Define which shader to use if this shader fails to run
    FallBack "Transparent/Specular"
} 