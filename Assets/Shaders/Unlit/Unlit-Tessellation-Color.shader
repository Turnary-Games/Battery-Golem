// Credits to the creator, GambinoInd
// http://forum.unity3d.com/threads/shader-moving-trees-grass-in-wind-outside-of-terrain.230911/

Shader "Unlit/Tessellation/Color" {
 
Properties {
    _Color ("Color", Color) = (1,0,0,1)

	[Header(Tessellation settings)]
	_EdgeLength("Edge length", Range(2,50)) = 5
	_Phong("Phong Strengh", Range(0,1)) = 0.5
}
 
SubShader {
    Tags {"Queue"="AlphaTest" "IgnoreProjector"="True"}
    LOD 200
	Cull Off
   
CGPROGRAM
#pragma target 3.0
#pragma surface surf NoLighting noforwardadd vertex:vert tessellate:tessEdge tessphong:_Phong nolightmap
#include "Tessellation.cginc"

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
};

void vert (inout appdata v) {}

void surf (Input IN, inout SurfaceOutput o) {
    fixed4 c = _Color;
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
 
Fallback "Unlit/Color"
}