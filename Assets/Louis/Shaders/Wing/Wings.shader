//
// Filename: Wings.shader
// Original Author: Sean Vieira
// Revisions made by: Louis Dimmock
//
// Current Version: 2.0
// Version information : 
//		Rewrote version 1.0 to add culling, normals, specular lighting
//
// Previous Versions : 
//		1.0 : A simple shader that is based off of the in-built diffuse shader
//

Shader "Flight/Wings/2.0"
{
	// Set up variables so we can access them in inspector mode
	Properties
	{
		_Color("Texture Tint", Color) = (1,1,1,1)
		// Textures
		_MainTex("Main Texture", 2D) = "white" { }
		_BumpMap("Bump Map", 2D) = "bump" { }
		
		// Specular Lighting
		_SpecularColor ("Specular Color", Color) = (1, 1, 1, 1)
		_SpecularAmount ("Shininess", Float) = 10
	}
	
	SubShader
	{
		// Define our pass
		pass
		{
			// Set the shader tags
			Tags
			{
				"RenderType" = "Opaque" // Categorise the shader
			}
			
			// Allow us to draw the back of the wings
			Cull off
			
			CGPROGRAM
			
			// Define the shader functions to use
			#pragma vertex WingsVertexShader
			#pragma fragment WingsFragmentShader
			
			// Texture samplers
			sampler2D _MainTex;
			sampler2D _BumpMap;
			
			// Texture scale factors
			float4 _MainTex_ST;
			float4 _BumpMap_ST;
			
			// Texture tint
			float4 _Color;
			
			// Specular
			float4 _SpecularColor;
			float _SpecularAmount;
			
			// Scene light
			float4 _LightColor0;

			// Structs to define the input and output of the shader
			struct vertexInput
			{
				float4 vertex : POSITION;
				float3 normal : NORMAL;
				float4 texcoord : TEXCOORD0;
				float4 tangent : TANGENT;
			};
			
			struct vertexOutput
			{
				float4 pos : SV_POSITION;
				float4 tex : TEXCOORD0;
				float4 posWorld : TEXCOORD1;
				float3 normalWorld : TEXCOORD2;
				float3 tangentWorld : TEXCOORD3;
				float3 binormalWorld : TEXCOORD4;
			};
			
			// Vertex shader
			vertexOutput WingsVertexShader(vertexInput input)
			{
				vertexOutput output;
				
				output.pos = mul(UNITY_MATRIX_MVP, input.vertex);
				output.tex = input.texcoord;
				output.posWorld = mul(_Object2World, input.vertex);
				
				// Calculate normal
				output.normalWorld = normalize( mul( float4( input.normal, 0.0f ), _World2Object ).xyz );
				
				// Calculate tangent
				output.tangentWorld = normalize( mul( _Object2World, input.tangent ).xyz );
				
				// Calculate binormal
				output.binormalWorld = normalize( cross( output.normalWorld, output.tangentWorld) * input.tangent.w );
				
				return output;
			}
			
			// Fragment shader
			float4 WingsFragmentShader(vertexOutput input) : COLOR
			{
				// Define the variables we will use
				float3 viewDirection;
				float3 lightDirection;
				float3 normalDirection;
				float lightIntensity;
				float3 normalLocal;
				float3x3 normalWorld;
				float4 textureColor;
				float4 normalColor;
				float3 diffuseColor;
				float3 specularColor;
				float3 lightColor;
				float4 finalColor;
				
				// Calculate the direction that the pixel is compared to us
				viewDirection = normalize( _WorldSpaceCameraPos.xyz - input.posWorld.xyz );
				
				// Check if the pixel is behind us
				if(_WorldSpaceLightPos0.w == 0.0) 
				{
					lightIntensity = 1.0;
					lightDirection = normalize(_WorldSpaceLightPos0.xyz);
				}
				else // We can see the pixel
				{
					// Calculate the direction vector to the light source
					float3 fragmentToLightSource = _WorldSpaceLightPos0.xyz - input.posWorld.xyz;
					
					// Calculate the distance
					float distance = length(fragmentToLightSource);
					
					// Calculate the light intensity
					lightIntensity = 1.0 / distance;
					lightDirection = normalize(fragmentToLightSource);
				}
				
				// Sample the textures
				textureColor = tex2D(_MainTex, input.tex.xy * _MainTex_ST.xy + _MainTex_ST.zw);
				normalColor = tex2D(_BumpMap, input.tex.xy * _BumpMap_ST.xy + _BumpMap_ST.zw);
				
				// Expand the normal to -1/+1
				normalLocal = float3(2.0 * normalColor.ag - float2(1.0, 1.0), 0.0);
				
				// Calculate the normal in the world
				normalWorld = float3x3( input.tangentWorld, input.binormalWorld, input.normalWorld );
				normalDirection = normalize( mul( normalLocal, normalWorld ) );
				
				// Calculate lighting
				diffuseColor = lightIntensity * _LightColor0.xyz * saturate( dot( normalDirection, lightDirection ) );
				specularColor = diffuseColor * _SpecularColor.xyz * pow( saturate( dot( reflect( -lightDirection, normalDirection ), viewDirection ) ), _SpecularAmount );
				
				// Combine lighting
				lightColor = UNITY_LIGHTMODEL_AMBIENT.xyz + diffuseColor + specularColor;
				
				// Apply lighting to the texture color
				textureColor = float4( textureColor.xyz * _Color * lightColor, 1.0);
				
				return textureColor;
			}
			
			ENDCG
		}
	}
	
    // Define which shader to use if this shader fails to run
	FallBack "Specular"
}