Shader "Unlit/StencilWriter"
{
	Properties
	{}
	SubShader
	{
		Tags { "RenderType"="Opaque" }
		
		// Just write to the stencil
		Pass {
			Cull Off
			ZWrite Off
			ZTest Greater
			ColorMask 0
			Stencil {
				Ref 1
				Comp always
				Pass replace
			}
		}
	}
}
