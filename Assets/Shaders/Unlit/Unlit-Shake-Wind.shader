Shader "Unlit/Shake/Wind" {
	Properties {
		_Color ("Tint", Color) = (1,1,1,1)
		_MainTex ("Texture (RGB)", 2D) = "white" {}

		[Header(Wind simulation)]
		_MinY("Minimum Y Value", float) = 0.0

		_xScale("X Amount", Range(-1,1)) = 0.5
		_yScale("Z Amount", Range(-1,1)) = 0.5

		_Scale("Effect Scale", float) = 1.0
		_Speed("Effect Speed", float) = 1.0

		_WorldScale("World Scale", float) = 1.0
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		Cull Off
		
		CGPROGRAM
		#pragma surface surf NoLighting noforwardadd vertex:vert nolightmap
		#pragma target 3.0

		sampler2D _MainTex;
		fixed4 _Color;

		float _MinY;
		float _xScale;
		float _yScale;
		float _Scale;
		float _WorldScale;
		float _Speed;
		float _Amount;

		struct Input {
			float2 uv_MainTex;
		};

		struct appdata {
			float4 vertex : POSITION;
			float3 normal : NORMAL;
			float2 texcoord : TEXCOORD0;
			float4 color : COLOR;
		};

		// https://www.reddit.com/r/Unity3D/comments/349as7/vertex_shader_help_please_wind_simulation/
		void vert(inout appdata v) {
			const float PI = 3.14159;

			float num = v.vertex.y - _MinY;
			float weight = sin(v.texcoord.y*PI*0.5);

			if ((num) > 0.0) {
				float3 worldPos = v.vertex.xyz;//mul(_Object2World, v.vertex).xyz;
				float x = sin(worldPos.x / _WorldScale + (_Time.y*_Speed)) * weight*(num - _MinY) * _Scale * 0.01;
				float y = sin(worldPos.y / _WorldScale + (_Time.y*_Speed)) * weight*(num - _MinY) * _Scale * 0.01;

				v.vertex.x += x * _xScale;
				v.vertex.y += y * _yScale;
			}

		}

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
		ENDCG
	}

	FallBack "Unlit/Texture"
}
