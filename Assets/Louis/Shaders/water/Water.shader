//
//	Filename : Water.shader
//	Author : Louis Dimmock
//	Date : 31st March 2014
//
//	Current Version : 3.1
//	Version Information :
//		After testing, the water texture has been removed. 
//		The shader now uses the Water Color variable to control the colour of the water.
//
//	Previous Versions : 			  
//		3.0 : The shader has been rewritten to use a surface shader, this allows the material to use transparency.
//		2.0 : The shader utilises multiple normal maps to provide a better visual effect
//		1.0 : The shader utilises a normal map and specular lighting to provide a water ripple effect
//

Shader "Flight/Water/3.1"
{
	// Set up the shader properties that the user can customise
    Properties
    {
		// Water Tint
		_Color ("Water Color", Color) = (1, 1, 1, 1)
		
		// Water Shininess
		_SpecularColor ("Specular Color", Color) = (1, 1, 1, 1)
		_SpecularAmount ("Specular Amount", Float) = 10
		
		// Textures
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
        sampler2D _BumpMapA;
        sampler2D _BumpMapB;
        
        // Color tint
        float4 _Color;
        
        // Texture shininess
        float _SpecularAmount;
    
    	// Define our data struct
        struct Input
        {
            float2 uv_BumpMapA;
            float2 uv_BumpMapB;
            INTERNAL_DATA
        };
		
		// Surface Shader
        void WaterSurfaceShader(Input IN, inout SurfaceOutput output)
        {            
            // Sample the two normal maps
            float4 normalA = tex2D(_BumpMapA, IN.uv_BumpMapA);
            float4 normalB = tex2D(_BumpMapB, IN.uv_BumpMapB);
            
            // Expand the normals, combine them and apply to the output
            output.Normal = normalize( UnpackNormal( normalA ) + UnpackNormal( normalB ) );
            
            // Set the specular
            output.Specular = _SpecularAmount;
            
            // Set the texture color
            output.Albedo = _Color;
            
            // Set the transparency using the texture alpha channel and the color tint
            output.Alpha = _Color.a;
        }
        
		ENDCG
    }
    
    // Define which shader to use if this shader fails to run
    FallBack "Transparent/Specular"
} 