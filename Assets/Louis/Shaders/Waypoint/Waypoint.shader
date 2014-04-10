//
// Filename : Waypoint.shader
// Author : Louis Dimmock
// Date : 9th April 2014
//
// Current Version : 1.0
// Version Info : 
//		An altered version of my Waypoint.shader
//

Shader "Flight/Waypoint/1.0"
{
	// Define our properties
    Properties 
    {
        _MainTex ("Waypoint Texture", 2D) = "white" {}
        _Color("Color Tint", Color) = (1, 1, 1, 1)
    }

	// Define our shader
    SubShader
	{
		// Define our shader tags
        Tags
		{
			"Queue" = "Transparent"
			"RenderType" = "Transparent"
		}

		// Define the pass
        Pass
		{
			// Turn on back face culling
            Cull Back
			
			// Disable writing to the z buffer
            ZWrite Off
			
			// Define the alpha blending type
            Blend srcAlpha OneMinusSrcAlpha

            CGPROGRAM

			// Define our shader functions
            #pragma vertex WaypointVertexShader
            #pragma fragment WaypointFragmentShader
            #pragma fragmentoption ARB_precision_hint_fastest 

			// Add access to global shader functions and variables
            #include "UnityCG.cginc"

			// Texture
            sampler2D _MainTex;
            float4 _MainTex_ST;
            
			// Color tint
            float4 _Color;

			// Define our structs
            struct vertexInput
			{
                half4 vertex : POSITION;
                half2 texcoord : TEXCOORD0;
                fixed4 color : COLOR; 
            };

            struct vertexOutput
			{
                half4 pos : POSITION;
                fixed4 color : COLOR;
                half2 texcoord : TEXCOORD0;
            };

            vertexOutput WaypointVertexShader(vertexInput input)
            {
                vertexOutput output;
                output.pos = mul (UNITY_MATRIX_MVP, input.vertex);
                output.color = input.color;
                output.texcoord = TRANSFORM_TEX(input.texcoord, _MainTex);

                return output;
            }

            float4 WaypointFragmentShader(vertexOutput input) : COLOR
            {
				// Sample the texture
                float4 textureColor = tex2D(_MainTex, input.texcoord);
				
				// Combine input color with the texture color and the color tint
				float4 finalColor = input.color * textureColor * _Color;

                return finalColor;
            }

            ENDCG           
        }
    } 

	// Define the shader that should be used if this one cant be used
    FallBack "Particles/Alpha Blended"
}