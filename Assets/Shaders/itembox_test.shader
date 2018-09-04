Shader "Unlit/itembox_test"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_TintColor ("Tint Color", color) = (1,1,1,1)
		_Transparency("Transparency", Range(0.0,1.0)) = 1.0
		_RimColor("Rim Color", Color) = (0.12, 0.31, 0.47, 1.0)
		_RimPower("Rim Power", Range(0.5,8.0)) = 3.0
	}
	SubShader
	{
		Tags { "Queue"="Transparent" "RenderType"="Transparent" }
		LOD 100

		Lighting On

		//ZWrite Off
		//Blend SrcAlpha OneMinusSrcAlpha

		/*Pass
		{
			Cull Front
			CGPROGRAM
			#pragma vertex vert Lambert
			#pragma fragment frag
			// make fog work
			#pragma multi_compile_fog
			
			#include "UnityCG.cginc"
			

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				UNITY_FOG_COORDS(1)
				float4 vertex : SV_POSITION;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			float4 _TintColor;
			float _Transparency;

			float4 _LightColor0;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				UNITY_TRANSFER_FOG(o,o.vertex);

				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				// sample the texture
				fixed4 col = tex2D(_MainTex, i.uv) * _TintColor;
				col.a *= _Transparency;

				// apply fog
				UNITY_APPLY_FOG(i.fogCoord, col);
				return col;
			}
			ENDCG
		}*/
		//Pass
		//{
			//Cull Off
			CGPROGRAM
			//#pragma vertex vert
			//#pragma fragment frag
			#pragma surface surf Standard fullforwardshadows alpha vertex:vert

			#pragma target 3.0
			// make fog work
			#pragma multi_compile_fog

			#include "UnityCG.cginc"
			/*
			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				UNITY_FOG_COORDS(1)
				float4 vertex : SV_POSITION;
			};*/


			struct Input {
				float2 uv_MainTex;
				float2 uv_BumpMap;
				// Third Adding
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
			float _Transparency;

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
				fixed4 c = tex2D(_MainTex, IN.uv_MainTex); // Second Modifiacation
				o.Albedo = c.rgb;
				o.Alpha = _Transparency;

				fixed rim = 1.0 - saturate(dot(normalize(IN.viewDir), o.Normal));
				o.Emission = (_RimColor.rgb * pow(rim, _RimPower));
			}


			/*v2f vert(appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				UNITY_TRANSFER_FOG(o,o.vertex);
				return o;
			}

			fixed4 frag(v2f i) : SV_Target
			{
				// sample the texture
				fixed4 col = tex2D(_MainTex, i.uv) * _TintColor;
				col.a *= _Transparency;

				// apply fog
				UNITY_APPLY_FOG(i.fogCoord, col);
				return col;
			}*/
			ENDCG
		//}

		UsePass "Legacy Shaders/VertexLit/SHADOWCASTER"
	}
}
