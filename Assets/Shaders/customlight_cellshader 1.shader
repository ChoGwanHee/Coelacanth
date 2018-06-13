Shader "Custom/customlight_cellshader1 " {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}

	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200

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
			o.Albedo = c.rgb*1;
			o.Emission = c.rgb;
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
				NdotL = 0.2;
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
