//
// Filename : Water.shader
// Author : Louis Dimmock
// Date : 6th March 2014
//
// Version : 3.0
// Version Info : 
// 		The shader code has been completely rewritten to allow transparency
//

Shader "Flight/Water/3.0"
{
	// Set up variables so we can access them in inspector mode
    Properties
    {
		// Variable to control the colour tint
		_Color ("Color Tint", Color) = (1, 1, 1, 1)
		
		// Variables for specular
		_SpecularColor ("Specular Color", Color) = (1, 1, 1, 1)
		_SpecularAmount ("Specular Amount", Float) = 10
		
		// Variable for setting the base texture
		_MainTex ("Water Texture", 2D) = "white" { }
		
		// Variables to set the normal map 1
		_BumpMapA ("Bump Map 1", 2D) = "bump" { }
		_BumpDepthA ("Depth", Range(0.25, 10.0)) = 1 
		// Variables to set the normal map 2
		_BumpMapB ("Bump Map 2", 2D) = "bump" { }
		_BumpDepthB ("Depth", Range(0.25, 10.0)) = 1 
    }
    
    SubShader
     {
        Tags { "Queue" = "AlphaTest" "IgnoreProjector" = "True" "RenderType" = "Transparent" }
        
        LOD 400 // This will need to be modified once we have added it to the game
        
        CGPROGRAM
        
        #pragma surface surf BlinnPhong alpha
        #pragma target 3.0

		// Texture samplers
        sampler2D _MainTex;
        sampler2D _BumpMapA;
        sampler2D _BumpMapB;
        
		// Variables to set the density of normal maps
		float _BumpDepthA;
		float _BumpDepthB;
        
        // Color tint
        float4 _Color;
        
        // Texture shininess
        half _SpecularAmount;
    
    	// Struct to define the data that we will access
        struct Input
        {
            float2 uv_MainTex;
            float2 uv_BumpMapA;
            float2 uv_BumpMapB;
            INTERNAL_DATA
        };
		
		// Pixel Shader
        void surf (Input IN, inout SurfaceOutput output)
        {
        	// Define variables
            half4 textureColor;
            half4 normalA;
            half4 normalB;
            float3 normalFinal;
            
        	// Sample the main texture
            textureColor = tex2D (_MainTex, IN.uv_MainTex);
            
            // Sample the two normal maps
            normalA = tex2D(_BumpMapA, IN.uv_BumpMapA);
            normalB = tex2D(_BumpMapB, IN.uv_BumpMapB);
            
            // Set the density of the normal map
            normalA.z = _BumpDepthA;
            normalB.z = _BumpDepthB;
            
            // Combine the two normals and normalize
            normalFinal = normalize( UnpackNormal( normalA ).rgb + UnpackNormal( normalB ).rgb );
            
            // Apply the variables to the output
            output.Normal = normalFinal;
            output.Specular = _SpecularAmount;
            output.Gloss = textureColor.a;
            output.Albedo = textureColor.rgb * _Color;
            output.Alpha = textureColor.a * _Color.a;
        }
        
        ENDCG
    }
    //FallBack "Transparent/Specular"
} 