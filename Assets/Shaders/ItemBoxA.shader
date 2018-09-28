Shader "Custom/ItemBoxA"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_TintColor ("Tint Color", color) = (1,1,1,1)
		_FrontTransparency("Front Transparency", Range(0.0,1.0)) = 1.0
		_BackTransparency("BackTransparency", Range(0.0,1.0)) = 1.0
		_RimColor("Rim Color", Color) = (0.12, 0.31, 0.47, 1.0)
		_RimPower("Rim Power", Range(0.5,8.0)) = 3.0
	}
	SubShader
	{
		Tags { "Queue"="Transparent" "RenderType"="Transparent" }
		LOD 100

		Lighting On

		ZWrite Off
		Blend SrcAlpha OneMinusSrcAlpha

		Cull Front
		CGPROGRAM
		#pragma surface surf Standard fullforwardshadows alpha vertex:vert

		#pragma target 3.0
		// make fog work
		#pragma multi_compile_fog

		#include "UnityCG.cginc"

		struct Input {
			float2 uv_MainTex;
			float2 uv_BumpMap;
			half3 viewDir;
			// For Detail Texture
			float2 uv_Detail;
			float3 worldPos;
			float3 worldNormal;
			float3 worldRefl;
			INTERNAL_DATA
			half fog;
		};

		sampler2D _MainTex;
		//float4 _MainTex_ST;
		float4 _TintColor;
		float _FrontTransparency;
		float _BackTransparency;

		fixed4 _RimColor;
		half _RimPower;


		void vert(inout appdata_full v, out Input data)
		{
			// For vertex expantion
			//v.vertex.xyz += v.normal * _Amount;

			UNITY_INITIALIZE_OUTPUT(Input, data);
			float4 hpos = UnityObjectToClipPos(v.vertex);
		}

		void surf(Input IN, inout SurfaceOutputStandard o)
		{
			fixed4 c = tex2D(_MainTex, IN.uv_MainTex);
			o.Albedo = c.rgb * _TintColor;
			o.Alpha = c.a * _BackTransparency;

			/*fixed rim = 1.0 - saturate(dot(normalize(IN.viewDir), o.Normal));
			o.Emission = (_RimColor.rgb * pow(rim, _RimPower));*/
		}
		ENDCG

		Cull Back
		CGPROGRAM
		#pragma surface surf Standard fullforwardshadows alpha vertex:vert

		#pragma target 3.0
			// make fog work
		#pragma multi_compile_fog

		#include "UnityCG.cginc"

		struct Input {
			float2 uv_MainTex;
			float2 uv_BumpMap;
			half3 viewDir;
			// For Detail Texture
			float2 uv_Detail;
			float3 worldPos;
			float3 worldNormal;
			float3 worldRefl;
			INTERNAL_DATA
			half fog;
		};

		sampler2D _MainTex;
		//float4 _MainTex_ST;
		float4 _TintColor;
		float _FrontTransparency;
		float _BackTransparency;

		fixed4 _RimColor;
		half _RimPower;


		void vert(inout appdata_full v, out Input data)
		{
			// For vertex expantion
			//v.vertex.xyz += v.normal * _Amount;

			UNITY_INITIALIZE_OUTPUT(Input, data);
			float4 hpos = UnityObjectToClipPos(v.vertex);
		}

		void surf(Input IN, inout SurfaceOutputStandard o)
		{
			fixed4 c = tex2D(_MainTex, IN.uv_MainTex);
			o.Albedo = c.rgb * _TintColor;
			o.Alpha = c.a * _FrontTransparency;

			fixed rim = 1.0 - saturate(dot(normalize(IN.viewDir), o.Normal));
			o.Emission = (_RimColor.rgb * pow(rim, _RimPower));
		}
		ENDCG

		UsePass "Legacy Shaders/VertexLit/SHADOWCASTER"
	}
}
