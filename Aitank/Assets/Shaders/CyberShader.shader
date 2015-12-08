Shader "Custom/CyberShader" {
	Properties {
			_Color ("Color", Color) = (1,1,1,1)
			[NoScaleOffset]_MainTex ("Albedo (RGB)", 2D) = "white" {}
			_Glossiness ("Smoothness", Range(0,1)) = 0.5
			_Metallic ("Metallic", Range(0,1)) = 0.0
			[HDR]_EmissionColor("Emission", Color) = (0,0,0,1)
			[HDR]_SubEmissionColor("SubEmission", Color) = (0, 0, 0, 1)
			[NoScaleOffset]_EmissionMap("Emission", 2D) = "white" { }
	}
	SubShader {
		Tags { "RenderType"="Opaque"}
		LOD 200
		
		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Standard fullforwardshadows

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		sampler2D _MainTex;
		sampler2D _EmissionMap;

		struct Input {
			float2 uv_MainTex;
			float3 worldPos;
		};

		half _Glossiness;
		half _Metallic;
		fixed4 _Color;
		fixed4 _EmissionColor;
		fixed4 _SubEmissionColor;

		void surf (Input IN, inout SurfaceOutputStandard o) {
			// Albedo comes from a texture tinted by color
			fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
			fixed4 e = tex2D(_EmissionMap, IN.uv_MainTex);
			float distFromCam = length(IN.worldPos - _WorldSpaceCameraPos);

			o.Albedo = c.rgb;
			// Metallic and smoothness come from slider variables
			o.Metallic = _Metallic;
			o.Smoothness = _Glossiness;
			o.Alpha = c.a;
			o.Emission = _EmissionColor * e * (distFromCam / 120 + 1.0f) + _SubEmissionColor * (!e * 0.01f);
		}
		ENDCG
	} 
	FallBack "Diffuse"
}
