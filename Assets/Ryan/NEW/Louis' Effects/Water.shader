Shader "Flight/Water With Reflection"
{
	Properties
	{
		_BumpMap ("Bumpmap", 2D) = "bump" {}
		_Cube ("Cubemap", CUBE) = "" {}
		_Opacity ("Opacity", Range(0,1)) = 1
		_WaterSpeed ("Water Speed", Float) = 1
		_WaveSize ("Wave Size", Float) = 1
	}

	SubShader 
	{
		Tags
		{
			"RenderType" = "Opaque"
		}

		CGPROGRAM

		#pragma surface WaterSurface Lambert alpha

		struct Input
		{
			float2 uv_BumpMap;
			float3 worldRefl;
			INTERNAL_DATA
		};

		sampler2D _BumpMap;
		samplerCUBE _Cube;
		
		float _Opacity;
		float _WaterSpeed;
		float _WaveSize;

		void WaterSurface (Input IN, inout SurfaceOutput output)
		{
			float Speed = _Time.x * _WaterSpeed;
			
			IN.uv_BumpMap += float2( Speed, Speed);
			
			output.Albedo = tex2D (_BumpMap, IN.uv_BumpMap).rgb;
			output.Alpha = _Opacity;
			output.Normal = UnpackNormal ( tex2D (_BumpMap, IN.uv_BumpMap) );
			output.Emission = texCUBE (_Cube, WorldReflectionVector (IN, output.Normal)).rgb;
		}
		ENDCG
	} 

	Fallback "Diffuse"
}