Shader "Custom/UISurfShader" {
	Properties{
		[PerRendererData]
		_MainTex("Texture", 2D) = "white" {}
		_MapTex("Texture", 2D) = "white" {}
		[HDR]
		_Color("Color", Color) = (1,1,1,1)
	}
	SubShader {
		Tags { "RenderType"="Transparent" "Queue"="Transparent" }
		ZWrite Off
		Blend SrcAlpha OneMinusSrcAlpha
		LOD 200
		
		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Lambert alpha

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		sampler2D _MainTex;
		sampler2D _MapTex;
		fixed4 _Color;

		struct Input {
			float2 uv_MainTex;
		};

		void surf (Input IN, inout SurfaceOutput o) {
			fixed4 col = tex2D(_MainTex, IN.uv_MainTex);
			fixed4 e = tex2D(_MapTex, IN.uv_MainTex);

			o.Albedo = !(e.rgb || 0.0f) * col.rgb;
			o.Emission = !(e.rgb || 0.0f) * col.rgb + (e.rgb && 1.0f) * _Color;
			o.Alpha = (e.rgb && 1.0f) + !(e.rgb || 0.0f) * col.a * 0.25f;
			o.Gloss = 1.0f;
			o.Specular = 1.0f;
		}
		ENDCG
	} 
	FallBack "Diffuse"
}
