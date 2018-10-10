// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/customlight_cellshaderd" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_Outline("Outline", Range(0,0.1)) = 0
		_OutlineColor("Outline Color", Color) = (1, 1, 1, 1)
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200

		Pass {
            Tags { "RenderType"="Opaque" }
            //Cull Off
			ZWrite Off
			ZTest Always
 
            CGPROGRAM
 
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
 
            struct v2f {
                float4 pos : SV_POSITION;
            };
 
            float _Outline;
            float4 _OutlineColor;
 
            float4 vert(appdata_base v) : SV_POSITION {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                float3 normal = mul((float3x3) UNITY_MATRIX_MV, v.normal);
                normal.x *= UNITY_MATRIX_P[0][0];
                normal.y *= UNITY_MATRIX_P[1][1];
                o.pos.xy += normal.xy * _Outline;
                return o.pos;
            }
 
            half4 frag(v2f i) : COLOR {
                return _OutlineColor;
            }
 
            ENDCG
        }

		CGPROGRAM

		#pragma surface surf k noshadow noambient
		#pragma target 3.0

		sampler2D _MainTex;

		struct Input {
			float2 uv_MainTex;
		};

		
		fixed4 _Color;


		void surf(Input IN, inout SurfaceOutput o) {

			fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
			o.Albedo = c.rgb*2.5-0.3;
			o.Emission = c.rgb*1 - 0.15;
			o.Alpha = c.a;


			
			


		}

		float4 Lightingk(SurfaceOutput s, float3 viewDir, float3 lightDir, float atten) {

			//float3 diff;
			float NdotL = saturate(dot(s.Normal, lightDir));
			//diff = (NdotL * _LightColor0.rgb * atten);
			//diff *= s.Albedo;


			if (NdotL > 0.8) {
				NdotL = 0.9;
			}

			/*else if (NdotL > 0.8) {
				NdotL = 1;
			}*/
			else if (NdotL > 0.5) {
				NdotL = 1;
			}
			else {
				NdotL = 0.4;
			}
			
			float4 c;
			c.rgb = s.Albedo * NdotL * _LightColor0.rgb*atten;
			c.a = s.Alpha;
			return c;

			//스펙큘러
			/*float3 H = normalize(lightDir + viewDir);
			float NdotH = saturate(dot(H, s.Normal));

			if (NdotH > 0.98) {
				NdotH = 1;
			}
			else {
				NdotH = 0;
			}

			float4 final;
			final.rgb = (s.Albedo * _LightColor0.rgb) + NdotH;
			final.a = 1;

			return final;*/

		}
		ENDCG
	}
	FallBack "Diffuse"
}
