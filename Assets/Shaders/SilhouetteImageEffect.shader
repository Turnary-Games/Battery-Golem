Shader "Hidden/SilhouetteImageEffect" {
	Properties {
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_Silhouette ("Silhouette color", Color) = (1,1,1,.5)
	}
	SubShader {
		Tags { "Queue" = "Overlay" }
		
		Pass {

			CGPROGRAM
			#pragma vertex vert_img
			#pragma fragment frag
 
			#include "UnityCG.cginc"
 
			uniform float4 _Silhouette;
			sampler2D _MainTex;
 
			float4 frag(v2f_img i) : COLOR {
				float4 c = tex2D(_MainTex, i.uv);
				return c;
			}
			ENDCG
		}

		Pass {
				
			Stencil {
				Ref 1
				Comp Equal
			}

			CGPROGRAM
			#pragma vertex vert_img
			#pragma fragment frag
 
			#include "UnityCG.cginc"
 
			uniform float4 _Silhouette;
			sampler2D _MainTex;
 
			float4 frag(v2f_img i) : COLOR {
				float4 o = tex2D(_MainTex, i.uv);
				float4 s = _Silhouette;
				s.a = 1;

				float4 c = lerp(o, s, _Silhouette.a);

				return c;
			}
			ENDCG
		}
	}
}