// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/ItemOutline" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_Glossiness ("Smoothness", Range(0,1)) = 0.5
		_Metallic ("Metallic", Range(0,1)) = 0.0
		_OutlineColor("Outline Color", Color) = (0,0,0,1)
		_Outline("Outline width", Range(0.0, 0.15)) = .005
		_OutlineOffset("Outline Offset", Vector) = (0, 0, 0)
	}

	CGINCLUDE
	#include "UnityCG.cginc"

	struct appdata {
		half4 vertex : POSITION;
		half3 normal : NORMAL;
		half2 texcoord : TEXCOORD0;
	};

	struct v2f {
		half4 pos : SV_POSITION;
		half2 uv : TEXCOORD0;
		half3 normalDir : NORMAL;
	};

	uniform half4 _Color;
	uniform half _Outline;
	uniform half4 _OutlineColor;

	ENDCG

	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200

		Pass {
			ZWrite Off
			ZTest Always
			ColorMask 0

			Stencil{
				Ref 2
				Comp always
				Pass replace
				ZFail decrWrap
			}

			CGPROGRAM

			#pragma vertex vert2
			#pragma fragment frag

			v2f vert2(appdata v)
			{
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);

				return o;
			}

			half4 frag(v2f i) : COLOR
			{
				return _Color;
			}

			ENDCG
		}

		Pass {
			Cull Off
			ZWrite Off
			ColorMask RGB

			Stencil{
				Ref 2
				Comp NotEqual
				Pass replace
				ZFail decrWrap
			}

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag


			float4 vert(appdata_base v) : SV_POSITION{
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
				float3 normal = mul((float3x3) UNITY_MATRIX_MV, v.normal);
				normal.x *= UNITY_MATRIX_P[0][0];
				normal.y *= UNITY_MATRIX_P[1][1];
				o.pos.xy += normal.xy * _Outline;
				return o.pos;
			}

			half4 frag(v2f i) : COLOR{
				return _OutlineColor;
			}
			ENDCG
		}

		ZWrite On
		ZTest LEqual
		Blend SrcAlpha OneMinusSrcAlpha


		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Standard fullforwardshadows

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		sampler2D _MainTex;

		struct Input {
			float2 uv_MainTex;
		};

		half _Glossiness;
		half _Metallic;
		//fixed4 _Color;

		// Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
		// See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
		// #pragma instancing_options assumeuniformscaling
		UNITY_INSTANCING_BUFFER_START(Props)
		// put more per-instance properties here
		UNITY_INSTANCING_BUFFER_END(Props)

		void surf(Input IN, inout SurfaceOutputStandard o) {
			// Albedo comes from a texture tinted by color
			fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
			o.Albedo = c.rgb;
			// Metallic and smoothness come from slider variables
			o.Metallic = _Metallic;
			o.Smoothness = _Glossiness;
			o.Alpha = c.a;
		}
		ENDCG

	}
	FallBack "Diffuse"
}
