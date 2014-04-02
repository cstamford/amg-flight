//
// Filename : Cartoon.shader
// Author : Louis Dimmock
// Date : 20th March 2014
//
// Version : 1.0
// Version Info : 
// 		A basic cartoon shader created for use with the map
//

Shader "Flight/Cartoon/1.0"
{
	// Set up the shader properties that the user can customise
    Properties
    {
		// Textures
        _MainTex ("Main Texure", 2D) = "white" {}
        _BumpMap ("Bump Map", 2D) = "bump" {}
        
		// How much should we toon-ify the object
        _CartoonPower ("Cartoon Amount", Range(0.1,20)) = 4
        
        // How much to blur the colours together
        _ColorBlendAmount ("Color Merge", Range(0.1,20)) = 8
        
        // How thick the stroke should be
        _Stroke ("Stroke", Range(0,1)) = 0.5
    }
    SubShader
    {
     	// Set the shader tags
        Tags
        {
        	"RenderType" = "Opaque"  // Categorise the shader
        }
        
        // Set the level of detail distance
        LOD 200
 
        CGPROGRAM
        
        // Define the shader function and the light model to use
        #pragma surface CartoonSurfaceShader Toon
 
		// Texture samplers
        sampler2D _MainTex;
        sampler2D _BumpMap;
        
        // How much we want to toon-ify the object
        float _CartoonPower;
        
        // How much to blend the colors
        float _ColorBlendAmount;
        
        // How thick we want the stroke to be
        float _Stroke;
 
    	// Define our data struct
        struct Input
        {
            float2 uv_MainTex;
            float2 uv_BumpMap;
            float3 viewDir;
        };
 
		// Surface Shader
        void CartoonSurfaceShader(Input IN, inout SurfaceOutput output)
        {
        	// Sample the texture
            float4 textureColor = tex2D (_MainTex, IN.uv_MainTex);
            
            // Sample the bump map
            float4 normal = tex2D(_BumpMap, IN.uv_BumpMap);
            
            // Expand the normal and apply to the output
            output.Normal = UnpackNormal( normal );
            
            // Calculate the stroke
            float edge = saturate( dot ( output.Normal, normalize( IN.viewDir ) ) ); 
            edge = edge < _Stroke ? edge/4 : 1;
            
            // Set the texture color
            output.Albedo = ( floor( textureColor.rgb * _ColorBlendAmount) / _ColorBlendAmount ) * edge;
            
            // Set the transparency using the texture alpha channel
            output.Alpha = textureColor.a;
        }
 
 		
        float4 LightingToon(SurfaceOutput input, half3 lightDir, half lightIntensity )
        {
            float4 textureColor;
            
            // Calculate the lighting
            float Lighting = dot(input.Normal, lightDir); 
            Lighting = floor(Lighting * _CartoonPower)/_CartoonPower;
            
            // Calculate the final pixel color
            textureColor.rgb = input.Albedo * _LightColor0.rgb * Lighting * lightIntensity * 2;
            textureColor.a = input.Alpha;
            
            return textureColor;
        }
 
        ENDCG
    }
    
    // Define which shader to use if this shader fails to run
    FallBack "Diffuse"
}