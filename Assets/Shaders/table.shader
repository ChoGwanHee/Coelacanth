Shader "Custom/table" {
	Properties {
		
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_BumpMap ("노말맵",2D) = "" {}
	}
	SubShader {
		Tags { "RenderType" = "Transparent" "Queue" = "Transparent" }
		LOD 200
		
		CGPROGRAM
		
		#pragma surface surf navii noambient alpha:fade
		float4 Lightingnavii(SurfaceOutput s, float3 lightDir, float atten) {
			//디퓨즈칼라
			float NdotL = dot(s.Normal, lightDir) * 0.6 + 0.4;
			float3 difColor = NdotL * _LightColor0.rgb * atten;
			//텍스쳐적용
			float4 k;
			k.rgb = difColor * s.Albedo;
			k.a = s.Alpha;
			return k;
		}
		
		#pragma target 3.0

		sampler2D _MainTex;
		sampler2D _BumpMap;

		struct Input {
			float2 uv_MainTex;
			float2 uv_BumpMap;
		};

		

		

		void surf (Input IN, inout SurfaceOutput o) {
			
			float4 c = tex2D (_MainTex, IN.uv_MainTex) ;
			o.Normal = UnpackNormal(tex2D(_BumpMap, IN.uv_BumpMap));
			o.Albedo = c.rgb;
			
			
			o.Alpha = c.a;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
