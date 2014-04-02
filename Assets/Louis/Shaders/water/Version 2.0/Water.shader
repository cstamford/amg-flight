//
//	Filename : Water.shader
//	Author : Louis Dimmock
//	Date : 2nd March 2014
//
//	Version : 2.0
//	Version Notes :
//		In order to provide a better visual effect, an additional normal map was added to the shader.
//		By doing so, this provides a ripple on ripple effect.
//		Due to the increase in normal maps, an additional depth control variable has been added.
//
//	Previous Versions : 			  
//		1.0 : Basic fragment shader utilising a normal map and specular lighting.
//

Shader "Flight/Water/2.0"
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
		_BumpMapA ("Normal Map", 2D) = "bump" { }
		_BumpDepthA ("Depth", Range(0.25, 10.0)) = 1 
		// Variables to set the normal map 2
		_BumpMapB ("Normal Map", 2D) = "bump" { }
		_BumpDepthB ("Depth", Range(0.25, 10.0)) = 1 
	}
	
	SubShader
	{
     	// Set the shader tags
		Tags
		{
			"RenderType" = "Opaque"
		}
			
		// Define our first pass, we only need this due to web player issues
	    pass 
	    {
	        Tags { "LightMode" = "ForwardAdd" }    
	    }
		
		// Define our second pass, this is where we actually render
		pass
		{
			CGPROGRAM
			
			#pragma vertex vert
			#pragma fragment frag
			#pragma exclude_renderers flash
			
			// Variables to store textures
			sampler2D _MainTex;
			sampler2D _BumpMapA;
			sampler2D _BumpMapB;
			
			// Variables to store texture tile amounts
			float4 _MainTex_ST;
			float4 _BumpMapA_ST;
			float4 _BumpMapB_ST;
			
			// Variables to set the density of normal maps
			float _BumpDepthA;
			float _BumpDepthB;
			
			// Color variables
			float4 _Color;
			float4 _SpecularColor;
			float _SpecularAmount;
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
			vertexOutput vert(vertexInput input)
			{
				vertexOutput output;
				
				output.pos = mul(UNITY_MATRIX_MVP, input.vertex);
				output.tex = input.texcoord;
				output.posWorld = mul(_Object2World, input.vertex);
				
				output.normalWorld = normalize( mul( float4( input.normal, 0.0f ), _World2Object ).xyz );
				output.tangentWorld = normalize( mul( _Object2World, input.tangent ).xyz );
				output.binormalWorld = normalize( cross( output.normalWorld, output.tangentWorld) * input.tangent.w );
				
				return output;
			}
			
			// Pixel shader
			float4 frag(vertexOutput input) : COLOR
			{
				// Set up variables
				float3 viewDirection;
				float3 lightDirection;
				float3 normalDirection;
				
				float lightIntensity;
								
				float4 normalColorA;
				float4 normalColorB;
				float4 normalColor;
				
				float3 normalLocalA;
				float3 normalLocalB;
				float3 normalLocal;
				
				float3x3 normalWorld;
				
				float4 textureColor;
				float3 diffuseColor;
				float3 specularColor;
				float3 lightColor;
				float4 finalColor;
				
				// Begin calculations
			
				// Calculate the angle we are looking at the pixel
				viewDirection = normalize(_WorldSpaceCameraPos.xyz - input.posWorld.xyz );
								
				if(_WorldSpaceLightPos0.w == 0.0) 
				{
					lightIntensity = 1.0;
					lightDirection = normalize(_WorldSpaceLightPos0.xyz);
				}
				else
				{
					float3 vertexToLightSource = _WorldSpaceLightPos0.xyz - input.posWorld.xyz;
					float distance = length(vertexToLightSource);
					lightIntensity = 1.0 / distance;
					lightDirection = normalize(vertexToLightSource);
				}
				
				// Sample the main texture
				textureColor = tex2D(_MainTex, input.tex.xy * _MainTex_ST.xy + _MainTex_ST.zw);		
				
				// Sample the normal maps
				normalColorA = tex2D(_BumpMapA, input.tex.xy * _BumpMapA_ST.xy + _BumpMapA_ST.zw);
				normalColorB = tex2D(_BumpMapB, input.tex.xy * _BumpMapB_ST.xy + _BumpMapB_ST.zw);
				
				// Expand the normals and set the intensity of the normal map
				normalLocalA = float3(2.0 * normalColorA.ag - float2(1.0, 1.0), 0.0);
				normalLocalA.z = _BumpDepthA;
				
				normalLocalB = float3(2.0 * normalColorB.ag - float2(1.0, 1.0), 0.0);
				normalLocalB.z = _BumpDepthB;
			
				// Combine the two normals	
				normalLocal = normalize(normalLocalA + normalLocalB);
				
				// Calculate the normal in the world
				normalWorld = float3x3( input.tangentWorld, input.binormalWorld, input.normalWorld );
				normalDirection = normalize( mul( normalLocal, normalWorld ) );
				
				// Calculate lighting
				diffuseColor = lightIntensity * _LightColor0.xyz * saturate( dot(normalDirection, lightDirection) );
				
				// Check the specular amount
				if (dot(normalDirection, lightDirection) < 0.0)
	            {
					// No specular light as we are on the wrong side to the light source
					specularColor = float3(0.0, 0.0, 0.0);
	            }
	            else // Theres specular to add, we are on the same side as the light source
	            {
	            	// Calculate the reflection vector
	            	float3 reflection = reflect( -lightDirection, normalDirection );
	            	
	            	// Calculate the specular amount
					specularColor = lightIntensity * _LightColor0 * _SpecularColor * pow( max( 0.0, dot( reflection, viewDirection) ), _SpecularAmount);
	            }
				
				// Combine lighting
				lightColor = UNITY_LIGHTMODEL_AMBIENT.xyz + diffuseColor + specularColor;
				
				// Apply lighting to the texture color
				textureColor = float4( textureColor.xyz * lightColor * _Color.xyz, 1.0f);
				
				return textureColor;
			}
			
			ENDCG
		}
	} 
	//FallBack "Specular"
}
