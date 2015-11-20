Shader "Custom/EmissionShader" {
	Properties {
		_MainTex("Albedo (RGB)", 2D) = "white" {}
		_EmissionTex("Emission Texture", 2D) = "white" {}
		//_EmissionColor("Emission Color", Color) = (1.0, 1.0, 1.0, 1.0)
		_Intensity("Intensity", Range(0.0, 3.0)) = 1.0
		_BendScale("Bend Scale", Range(0.0, 1.0)) = 0.2
	}

	SubShader {
		Tags {"RenderType" = "Opaque"}
		
		CGPROGRAM

		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Lambert alpha
		#define PI 3.14159

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		//half4 _EmissionColor;
		sampler2D _MainTex;
		sampler2D _EmissionTex;
		float _Intensity;
		float _BendScale;

		struct Input {
			float2 uv_MainTex;
			float4 col : COLOR;
		};

		void surf (Input IN, inout SurfaceOutput o)
		{
			half4 c = tex2D(_MainTex, IN.uv_MainTex);
			float e = tex2D(_EmissionTex, IN.uv_MainTex).a;

			o.Albedo = c.rgb;
			o.Alpha = c.a * IN.col.a;
			//o.Emission = _EmissionColor * _Intensity * e;
			o.Emission = IN.col * _Intensity * e;
		}
		ENDCG
	} 

	Fallback "Diffuse"
}
