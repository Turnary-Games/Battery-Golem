Shader "Projector/Texture" {
	Properties {
		_Tex ("Cookie", 2D) = "gray" {}
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
				float4 uv : TEXCOORD0;
				float4 pos : SV_POSITION;
			};
			
			float4x4 _Projector;
			float4x4 _ProjectorClip;
			
			v2f vert (float4 vertex : POSITION)
			{
				v2f o;
				o.pos = mul (UNITY_MATRIX_MVP, vertex);
				o.uv = mul (_Projector, vertex);
				return o;
			}
			
			fixed4 _Color;
			sampler2D _Tex;
			
			fixed4 frag (v2f i) : SV_Target
			{
				// Take texture from cookie
				fixed4 c = tex2Dproj (_Tex, UNITY_PROJ_COORD(i.uv));
				
				// Add tint
				c *= _Color;

				return c;
			}
			ENDCG
		}
	}
}
