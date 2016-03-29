Shader "Projector/Texture" {
	Properties {
		_Tex ("Cookie", 2D) = "gray" {}
		_FalloffTex ("FallOff", 2D) = "white" {}
		_Color("Tint", Color) = (1,1,1,1)
	}
	Subshader {
		Tags {"Queue"="Transparent"}
		Pass {
			ZWrite Off
			Blend SrcAlpha OneMinusSrcAlpha
			Offset -1, -1

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"
			
			struct v2f {
				float4 uvTex : TEXCOORD0;
				float4 uvFalloff : TEXCOORD1;
				float4 pos : SV_POSITION;
			};
			
			float4x4 _Projector;
			float4x4 _ProjectorClip;
			
			v2f vert (float4 vertex : POSITION)
			{
				v2f o;
				o.pos = mul (UNITY_MATRIX_MVP, vertex);
				o.uvTex = mul (_Projector, vertex);
				o.uvFalloff = mul(_ProjectorClip, vertex);
				return o;
			}
			
			fixed4 _Color;
			sampler2D _Tex;
			sampler2D _FalloffTex;
			
			fixed4 frag (v2f i) : SV_Target
			{
				// Take color from texture
				fixed4 tex = tex2Dproj (_Tex, UNITY_PROJ_COORD(i.uvTex));
				
				// Add tint
				tex *= _Color;

				// Add falloff
				fixed4 texF = tex2Dproj(_FalloffTex, UNITY_PROJ_COORD(i.uvFalloff));
				fixed4 res = lerp(fixed4(1, 1, 1, 0), tex, texF.a);

				return res;
			}
			ENDCG
		}
	}
}
