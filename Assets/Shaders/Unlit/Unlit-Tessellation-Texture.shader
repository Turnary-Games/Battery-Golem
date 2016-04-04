// Credits to the creator, GambinoInd
// http://forum.unity3d.com/threads/shader-moving-trees-grass-in-wind-outside-of-terrain.230911/

Shader "Unlit/Tessellation/Texture" {
 
Properties {
    _Color ("Tint", Color) = (1,1,1,1)
    _MainTex ("Base (RGB) Trans (A)", 2D) = "white" {}
    _Cutoff ("Alpha cutoff", Range(0,1)) = 0.5

	[Header(Tessellation settings)]
	_EdgeLength("Edge length", Range(2,50)) = 5
	_Phong("Phong Strengh", Range(0,1)) = 0.5
}
 
SubShader {
    Tags {"Queue"="AlphaTest" "IgnoreProjector"="True" "RenderType"="TransparentCutout"}
    LOD 200
	Cull Off
   
CGPROGRAM
#pragma target 3.0
#pragma surface surf NoLighting noforwardadd alphatest:_Cutoff vertex:vert tessellate:tessEdge tessphong:_Phong nolightmap
#include "Tessellation.cginc"

sampler2D _MainTex;
fixed4 _Color;

float _Phong;
float _EdgeLength;

struct Input {
    float2 uv_MainTex;
};

struct appdata {
	float4 vertex : POSITION;
	float3 normal : NORMAL;
	float2 texcoord : TEXCOORD0;
	float4 color : COLOR;
};

void vert (inout appdata v) {}

void surf (Input IN, inout SurfaceOutput o) {
    fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
    o.Albedo = c.rgb;
    o.Alpha = c.a;
}

fixed4 LightingNoLighting(SurfaceOutput s, fixed3 lightDir, fixed atten) {
	fixed4 c;
	c.rgb = s.Albedo;
	c.a = s.Alpha;
	return c;
}

float4 tessEdge(appdata v0, appdata v1, appdata v2) {
	return UnityEdgeLengthBasedTess(v0.vertex, v1.vertex, v2.vertex, _EdgeLength);
}
ENDCG
}
 
Fallback "Unlit/Texture"
}