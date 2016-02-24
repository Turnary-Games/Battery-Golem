Shader "Hidden/SilhouetteImageEffect" {
	Properties {
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_Silhouette ("Silhouette color", Color) = (1,1,1,.5)
	}
	SubShader {
		Blend SrcAlpha OneMinusSrcAlpha
		Stencil {
			Ref 1
			Comp equal
		}

		Pass {
			CGPROGRAM
			#pragma vertex vert_img
			#pragma fragment frag
 
			#include "UnityCG.cginc"
 
			uniform float4 _Silhouette;
 
			float4 frag(v2f_img i) : COLOR {
				return _Silhouette;
			}
			ENDCG
		}
	}
}