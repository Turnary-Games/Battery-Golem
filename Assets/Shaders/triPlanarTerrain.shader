Shader "Hidden/TerrainEngine/Splatmap/Lightmap-FirstPass" {
	Properties {
		_Control ("Control (RGBA)", 2D) = "red" {}
		_Splat3 ("Layer 3 (A)", 2D) = "white" {}
		_Splat2 ("Layer 2 (B)", 2D) = "white" {}
		_Splat1 ("Layer 1 (G)", 2D) = "white" {}
		_Splat0 ("Layer 0 (R)", 2D) = "white" {}
		// used in fallback on old cards
		_Color ("Main Color", Color) = (1,1,1,1)
		_MainTex ("Base (RGB)", 2D) = "white" {}
	}
	SubShader {
		Tags {"SplatCount" = "4" "Queue" = "Geometry-100" "RenderType" = "Opaque"}
		
		CGPROGRAM
			#pragma surface surf Terrain vertex:vert exclude_path:prepass
			#pragma target 3.0
	
			sampler2D _Control;

			struct SurfaceOutputTerrain {
				fixed3 Albedo;
				fixed3 Normal;
				fixed4 Light;
				fixed3 Emission;
				fixed Specular;
				fixed Alpha;
			};

			inline fixed4 LightingTerrain (SurfaceOutputTerrain s, fixed3 lightDir, fixed3 viewDir, fixed atten)
			{
				float4 result;
				result.rgb = (s.Albedo * s.Light.x) + ( pow(s.Light.y, s.Light.w * 128) * s.Light.z );
				result.rgb *= _LightColor0.rgb * atten * 2;
				result.a = 0.0;
				return result;
			}
									
			struct Input {
				float2	uv_Control;
				float3	worldPos;
				float3	worldNormal;
				float3	lightDir;
				float3	viewDir;
			};
	
			void vert(inout appdata_full v, out Input o) {
				UNITY_INITIALIZE_OUTPUT(Input, o);
				o.worldPos = mul(_Object2World, v.vertex).xyz;
				o.worldNormal = mul(_Object2World, float4(v.normal, 0.0)).xyz;
				o.lightDir = normalize(ObjSpaceLightDir(v.vertex));
				o.viewDir = normalize(ObjSpaceViewDir(v.vertex));
			}

			sampler2D _Splat0, _Splat1, _Splat2, _Splat3;
			sampler2D _BumpMap0, _BumpMap1, _BumpMap2, _BumpMap3;
			sampler2D _SpecMap0, _SpecMap1, _SpecMap2, _SpecMap3;
			float _TerrainTexScale0, _TerrainTexScale1, _TerrainTexScale2, _TerrainTexScale3;
	
			void surf (Input IN, inout SurfaceOutputTerrain o) {
			// INITIALISE THE CONTROL SPLAT FOR TERRAIN
				fixed4 splat_control = tex2D(_Control, IN.uv_Control);
	
			// INITIALISE THE VARIABLES FOR TRIPLANAR PROJECTION
				float3 worldNormal = normalize(IN.worldNormal);
				float3 projNormal = saturate(pow(worldNormal * 1.4, 4));
	
			// INITIALISE VARIABLES WE'LL NEED
				float3 tangent;
				float3 binormal;
				float3x3 rotation;
				float3 lightDirT;
				float3 viewDirT;
				
				float2 uv;
				float2 uv0;
				float2 uv1;
				float2 uv2;
				float2 uv3;
				
				float3 h;
				float4 tempNormal;
				float3 normal;
				
				fixed3 albedoX;
				fixed3 albedoY;
				fixed3 albedoZ;
				fixed4 lightX;
				fixed4 lightY;
				fixed4 lightZ;
	
			// CALCULATE X FOR ALL
			
				// UV
				uv = IN.worldPos.zy;
				uv0 = uv * _TerrainTexScale0;
				uv1 = uv * _TerrainTexScale1;
				uv2 = uv * _TerrainTexScale2;
				uv3 = uv * _TerrainTexScale3;
	
				// Tangent
				tangent = float3(0, 0, 1);
				
				// Light and View Vectors
				binormal = cross(worldNormal, tangent) * (step(worldNormal.x, 0) * 2 - 1);
				rotation = float3x3(tangent, binormal, worldNormal);
				lightDirT = mul(rotation, IN.lightDir);
				viewDirT = mul(rotation, IN.viewDir);
				
				// Albedo
				albedoX = tex2D(_Splat0, uv0).rgb * splat_control.r;
				albedoX += tex2D(_Splat1, uv1).rgb * splat_control.g;
				albedoX += tex2D(_Splat2, uv2).rgb * splat_control.b;
				albedoX += tex2D(_Splat3, uv3).rgb * splat_control.a;
				
				// Normal
				tempNormal = tex2D (_BumpMap0, uv0) * splat_control.r;
				tempNormal += tex2D (_BumpMap1, uv1) * splat_control.g;
				tempNormal += tex2D (_BumpMap2, uv2) * splat_control.b;
				tempNormal += tex2D (_BumpMap3, uv3) * splat_control.a;
				normal = UnpackNormal(tempNormal);
				
				// Specular
				lightX.zw = tex2D (_SpecMap0, uv0).rg * splat_control.r;
				lightX.zw += tex2D (_SpecMap1, uv1).rg * splat_control.g;
				lightX.zw += tex2D (_SpecMap2, uv2).rg * splat_control.b;
				lightX.zw += tex2D (_SpecMap3, uv3).rg * splat_control.a;
				
				// Lighting Values
				lightX.x = saturate(dot(normal, lightDirT));
				h = normalize(lightDirT + viewDirT);
				lightX.y = saturate(dot(normal, h));
	
			// CALCULATE Y FOR ALL
			
				// UV
				uv = IN.worldPos.xz;
				uv0 = uv * _TerrainTexScale0;
				uv1 = uv * _TerrainTexScale1;
				uv2 = uv * _TerrainTexScale2;
				uv3 = uv * _TerrainTexScale3;
	
				// Tangent
				tangent = float3(1, 0, 0);
				
				// Light and View Vectors
				binormal = cross(worldNormal, tangent) * (step(worldNormal.y, 0) * 2 - 1);
				rotation = float3x3(tangent, binormal, worldNormal);
				lightDirT = mul(rotation, IN.lightDir);
				viewDirT = mul(rotation, IN.viewDir);
				
				// Albedo
				albedoY = tex2D(_Splat0, uv0).rgb * splat_control.r;
				albedoY += tex2D(_Splat1, uv1).rgb * splat_control.g;
				albedoY += tex2D(_Splat2, uv2).rgb * splat_control.b;
				albedoY += tex2D(_Splat3, uv3).rgb * splat_control.a;
				
				// Normal
				tempNormal = tex2D (_BumpMap0, uv0) * splat_control.r;
				tempNormal += tex2D (_BumpMap1, uv1) * splat_control.g;
				tempNormal += tex2D (_BumpMap2, uv2) * splat_control.b;
				tempNormal += tex2D (_BumpMap3, uv3) * splat_control.a;
				normal = UnpackNormal(tempNormal);
				
				// Specular
				lightY.zw = tex2D (_SpecMap0, uv0).rg * splat_control.r;
				lightY.zw += tex2D (_SpecMap1, uv1).rg * splat_control.g;
				lightY.zw += tex2D (_SpecMap2, uv2).rg * splat_control.b;
				lightY.zw += tex2D (_SpecMap3, uv3).rg * splat_control.a;
				
				// Lighting Values
				lightY.x = saturate(dot(normal, lightDirT));
				h = normalize(lightDirT + viewDirT);
				lightY.y = saturate(dot(normal, h));
	
			// CALCULATE Z FOR ALL
			
				// UV
				uv = IN.worldPos.xy;
				uv.x *= -1;
				uv0 = uv * _TerrainTexScale0;
				uv1 = uv * _TerrainTexScale1;
				uv2 = uv * _TerrainTexScale2;
				uv3 = uv * _TerrainTexScale3;
	
				// Tangent
				tangent = float3(-1, 0, 0);
				
				// Light and View Vectors
				binormal = cross(worldNormal, tangent) * (step(worldNormal.z, 0) * 2 - 1);
				rotation = float3x3(tangent, binormal, worldNormal);
				lightDirT = mul(rotation, IN.lightDir);
				viewDirT = mul(rotation, IN.viewDir);
				
				// Albedo
				albedoZ = tex2D(_Splat0, uv0).rgb * splat_control.r;
				albedoZ += tex2D(_Splat1, uv1).rgb * splat_control.g;
				albedoZ += tex2D(_Splat2, uv2).rgb * splat_control.b;
				albedoZ += tex2D(_Splat3, uv3).rgb * splat_control.a;
				
				// Normal
				tempNormal = tex2D (_BumpMap0, uv0) * splat_control.r;
				tempNormal += tex2D (_BumpMap1, uv1) * splat_control.g;
				tempNormal += tex2D (_BumpMap2, uv2) * splat_control.b;
				tempNormal += tex2D (_BumpMap3, uv3) * splat_control.a;
				normal = UnpackNormal(tempNormal);
				
				// Specular
				lightZ.zw = tex2D (_SpecMap0, uv0).rg * splat_control.r;
				lightZ.zw += tex2D (_SpecMap1, uv1).rg * splat_control.g;
				lightZ.zw += tex2D (_SpecMap2, uv2).rg * splat_control.b;
				lightZ.zw += tex2D (_SpecMap3, uv3).rg * splat_control.a;
				
				// Lighting Values
				lightZ.x = saturate(dot(normal, lightDirT));
				h = normalize(lightDirT + viewDirT);
				lightZ.y = saturate(dot(normal, h));
	
				o.Albedo = albedoZ;
				o.Albedo = lerp(o.Albedo, albedoY, projNormal.y);
				o.Albedo = lerp(o.Albedo, albedoX, projNormal.x);
				
				o.Light = lightZ;
				o.Light = lerp(o.Light, lightY, projNormal.y);
				o.Light = lerp(o.Light, lightX, projNormal.x);
			}
	
		ENDCG
	}
	Fallback "VertexLit"
}