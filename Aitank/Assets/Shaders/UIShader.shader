Shader "Custom/UIShader"
{
	Properties
	{
		[PerRendererData]
		_MainTex("Texture", 2D) = "white" {}
		_MapTex("Texture", 2D) = "white" {}
		_Color("Color", Color) = (1,1,1,1)
	}
	SubShader
	{
		Tags {"Queue" = "Transparent" "RenderType" = "Transparent"}
		// No culling or depth
		ZWrite Off
		Blend SrcAlpha OneMinusSrcAlpha

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
			};

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
				o.uv = v.uv;
				return o;
			}
			
			sampler2D _MainTex;
			sampler2D _MapTex;
			fixed4 _Color;

			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 col = tex2D(_MainTex, i.uv);
				fixed4 e = tex2D(_MapTex, i.uv);
				float2 pos = i.uv;

				//col.a = col.a + 0.25f + e;
				e.rgb = !(e.rgb || 0) * col.rgb + (e.rgb && 1) * _Color;
				// just invert the colors
				//col = 1 - col;
				return e;
			}
			ENDCG
		}
	}
}
