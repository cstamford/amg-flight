//=======================================
//
// Filename: WingShader.shader
// Original Author: Sean Vieira
// Revisions made by: Louis Dimmock
//
// Version: 1.1
//
// Description: A simple shader that is based off of the in-built diffuse shader
//
//=======================================

Shader "Flight/Wings/1.0" 
{	
	Properties 
	{
		_Colour ("Main Colour", Color) = (1.0, 1.0, 1.0, 1.0)
		_MainTex ("Wing Texture", 2D) = "white" {}
		_AlphaMask ("Alpha Mask", 2D) = "white" {}
      	_Cutoff ("Alpha cutoff", Range (0,1)) = 0.1
	}
	
	SubShader
	{			
		Pass
		{
			// Define our tags
			Tags
			{
				"Queue" = "Transparent"
			}
			
			Cull Front
		
			CGPROGRAM
			
			// Define the functions we are going to use
			#pragma vertex WingVertexShader
			#pragma fragment WingFragmentShader

			// Texture Samplers
			sampler2D _MainTex;	
			sampler2D _AlphaMask;
			
			// Texture tiling	
			float4 _MainTex_ST;
			float4 _AlphaMask_ST;
			
			// Colour Tint
			float4 _Colour;
			
			// Define our input and output structs
			struct vertexInput
			{
				float4 vertex : POSITION;
				float4 texcoord : TEXCOORD0;

			};
			
			struct vertexOutput
			{
				float4 pos : SV_POSITION;
				float4 tex : TEXCOORD0;
			};

			vertexOutput WingVertexShader(vertexInput input) 
			{
				vertexOutput output;
				
				output.pos = mul(UNITY_MATRIX_MVP, input.vertex);
				output.tex = input.texcoord;

				return output;
			}

			float4 WingFragmentShader(vertexOutput input) : COLOR
			{
				float4 textureColor, textureAlpha;
				
				// Sample the texture
				textureColor = tex2D(_MainTex, input.tex.xy * _MainTex_ST.xy + _MainTex_ST.zw);
				textureAlpha = tex2D(_AlphaMask, input.tex.xy * _AlphaMask_ST.xy + _AlphaMask_ST.zw);
				
				float4 output = textureColor * _Colour;
				//output.a = saturate(textureAlpha.r * textureAlpha.g * textureAlpha.b);
				
				return output;
			}

			ENDCG
		}
		
		Pass
		{
			// Define our tags
			Tags
			{
				"Queue" = "Transparent"
			}
			
			Cull Back
		
			CGPROGRAM
			
			// Define the functions we are going to use
			#pragma vertex WingVertexShader
			#pragma fragment WingFragmentShader

			// Texture Samplers
			sampler2D _MainTex;	
			sampler2D _AlphaMask;
			
			// Texture tiling	
			float4 _MainTex_ST;
			float4 _AlphaMask_ST;
			
			// Colour Tint
			float4 _Colour;
			
			// Define our input and output structs
			struct vertexInput
			{
				float4 vertex : POSITION;
				float4 texcoord : TEXCOORD0;

			};
			
			struct vertexOutput
			{
				float4 pos : SV_POSITION;
				float4 tex : TEXCOORD0;
			};

			vertexOutput WingVertexShader(vertexInput input) 
			{
				vertexOutput output;
				
				output.pos = mul(UNITY_MATRIX_MVP, input.vertex);
				output.tex = input.texcoord;

				return output;
			}

			float4 WingFragmentShader(vertexOutput input) : COLOR
			{
				float4 textureColor, textureAlpha;
				
				// Sample the texture
				textureColor = tex2D(_MainTex, input.tex.xy * _MainTex_ST.xy + _MainTex_ST.zw);
				textureAlpha = tex2D(_AlphaMask, input.tex.xy * _AlphaMask_ST.xy + _AlphaMask_ST.zw);
				
				float4 output = textureColor * _Colour;
				//output.a = (textureAlpha.r * textureAlpha.g * textureAlpha.b);
				
				return output;
			}

			ENDCG
		}
	} 

	FallBack "Diffuse"
}
