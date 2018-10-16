// Shader created with Shader Forge v1.38 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.38;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,cgin:,lico:1,lgpr:1,limd:1,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,imps:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:0,bsrc:0,bdst:1,dpts:2,wrdp:True,dith:0,atcv:False,rfrpo:True,rfrpn:Refraction,coma:15,ufog:True,aust:True,igpj:False,qofs:0,qpre:1,rntp:1,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,atwp:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:False,fnfb:False,fsmp:False;n:type:ShaderForge.SFN_Final,id:3130,x:32910,y:32568,varname:node_3130,prsc:2|diff-8028-RGB,diffpow-1829-OUT,spec-5053-OUT,gloss-9482-OUT,emission-4450-OUT,transm-4961-RGB,amdfl-5345-RGB,olwid-3550-OUT,olcol-1282-RGB;n:type:ShaderForge.SFN_Tex2d,id:8028,x:32251,y:32529,ptovrint:False,ptlb:Texture,ptin:_Texture,varname:node_8028,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:98924265563ae6f419dc5f536f5de8a7,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Tex2d,id:5345,x:32360,y:32713,ptovrint:False,ptlb:Emission_Light,ptin:_Emission_Light,varname:node_5345,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:98924265563ae6f419dc5f536f5de8a7,ntxv:0,isnm:False|MIP-9774-OUT;n:type:ShaderForge.SFN_Tex2d,id:4961,x:32360,y:32887,ptovrint:False,ptlb:ossclusion,ptin:_ossclusion,varname:node_4961,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:43a7117832a95a7449e6798d23177bf4,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Slider,id:3550,x:32463,y:33077,ptovrint:False,ptlb:Outline,ptin:_Outline,varname:node_3550,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0.01949422,max:1;n:type:ShaderForge.SFN_Color,id:1282,x:32562,y:32906,ptovrint:False,ptlb:outlinecolor,ptin:_outlinecolor,varname:node_1282,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:0,c2:0,c3:0,c4:1;n:type:ShaderForge.SFN_ValueProperty,id:9774,x:32040,y:32737,ptovrint:False,ptlb:Emission_Light_value,ptin:_Emission_Light_value,varname:node_9774,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:2;n:type:ShaderForge.SFN_Slider,id:5053,x:32518,y:32470,ptovrint:False,ptlb:specular,ptin:_specular,varname:node_5053,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:1.204187,max:5;n:type:ShaderForge.SFN_Slider,id:9482,x:32501,y:32654,ptovrint:False,ptlb:gloss,ptin:_gloss,varname:node_9482,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:1.149456,max:5;n:type:ShaderForge.SFN_Vector1,id:1829,x:32579,y:32582,varname:node_1829,prsc:2,v1:5;n:type:ShaderForge.SFN_Tex2d,id:1201,x:32360,y:32356,ptovrint:False,ptlb:Emission,ptin:_Emission,varname:node_1201,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:98924265563ae6f419dc5f536f5de8a7,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Vector1,id:1070,x:32106,y:32356,varname:node_1070,prsc:2,v1:0.1;n:type:ShaderForge.SFN_Multiply,id:4450,x:32628,y:32261,varname:node_4450,prsc:2|A-1201-RGB,B-3845-OUT;n:type:ShaderForge.SFN_ValueProperty,id:3845,x:32188,y:32232,ptovrint:False,ptlb:Emission_value,ptin:_Emission_value,varname:node_3845,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0.05;proporder:8028-5345-4961-3550-1282-9774-5053-9482-1201-3845;pass:END;sub:END;*/

Shader "Custom/food" {
    Properties {
        _Texture ("Texture", 2D) = "white" {}
        _Emission_Light ("Emission_Light", 2D) = "white" {}
        _ossclusion ("ossclusion", 2D) = "white" {}
        _Outline ("Outline", Range(0, 1)) = 0.01949422
        _outlinecolor ("outlinecolor", Color) = (0,0,0,1)
        _Emission_Light_value ("Emission_Light_value", Float ) = 2
        _specular ("specular", Range(0, 5)) = 1.204187
        _gloss ("gloss", Range(0, 5)) = 1.149456
        _Emission ("Emission", 2D) = "white" {}
        _Emission_value ("Emission_value", Float ) = 0.05
    }
    SubShader {
        Tags {
            "RenderType"="Opaque"
        }
        LOD 200
        Pass {
            Name "Outline"
            Tags {
            }
            Cull Front
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            #pragma fragmentoption ARB_precision_hint_fastest
            #pragma multi_compile_shadowcaster
            #pragma multi_compile_fog
            #pragma only_renderers d3d9 d3d11 glcore gles 
            #pragma target 3.0
            uniform float _Outline;
            uniform float4 _outlinecolor;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                UNITY_FOG_COORDS(0)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.pos = UnityObjectToClipPos( float4(v.vertex.xyz + v.normal*_Outline,1) );
                UNITY_TRANSFER_FOG(o,o.pos);
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                return fixed4(_outlinecolor.rgb,0);
            }
            ENDCG
        }
        Pass {
            Name "FORWARD"
            Tags {
                "LightMode"="ForwardBase"
            }
            
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #include "UnityCG.cginc"
            #include "AutoLight.cginc"
            #pragma multi_compile_fwdbase_fullshadows
            #pragma multi_compile_fog
            #pragma only_renderers d3d9 d3d11 glcore gles 
            #pragma target 3.0
            uniform float4 _LightColor0;
            uniform sampler2D _Texture; uniform float4 _Texture_ST;
            uniform sampler2D _Emission_Light; uniform float4 _Emission_Light_ST;
            uniform sampler2D _ossclusion; uniform float4 _ossclusion_ST;
            uniform float _Emission_Light_value;
            uniform float _specular;
            uniform float _gloss;
            uniform sampler2D _Emission; uniform float4 _Emission_ST;
            uniform float _Emission_value;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 posWorld : TEXCOORD1;
                float3 normalDir : TEXCOORD2;
                LIGHTING_COORDS(3,4)
                UNITY_FOG_COORDS(5)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                float3 lightColor = _LightColor0.rgb;
                o.pos = UnityObjectToClipPos( v.vertex );
                UNITY_TRANSFER_FOG(o,o.pos);
                TRANSFER_VERTEX_TO_FRAGMENT(o)
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                i.normalDir = normalize(i.normalDir);
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float3 normalDirection = i.normalDir;
                float3 lightDirection = normalize(_WorldSpaceLightPos0.xyz);
                float3 lightColor = _LightColor0.rgb;
                float3 halfDirection = normalize(viewDirection+lightDirection);
////// Lighting:
                float attenuation = LIGHT_ATTENUATION(i);
                float3 attenColor = attenuation * _LightColor0.xyz;
///////// Gloss:
                float gloss = _gloss;
                float specPow = exp2( gloss * 10.0 + 1.0 );
////// Specular:
                float NdotL = saturate(dot( normalDirection, lightDirection ));
                float3 specularColor = float3(_specular,_specular,_specular);
                float3 directSpecular = attenColor * pow(max(0,dot(halfDirection,normalDirection)),specPow)*specularColor;
                float3 specular = directSpecular;
/////// Diffuse:
                NdotL = dot( normalDirection, lightDirection );
                float3 forwardLight = pow(max(0.0, NdotL ), 5.0);
                float4 _ossclusion_var = tex2D(_ossclusion,TRANSFORM_TEX(i.uv0, _ossclusion));
                float3 backLight = pow(max(0.0, -NdotL ), 5.0) * _ossclusion_var.rgb;
                NdotL = max(0.0,dot( normalDirection, lightDirection ));
                float3 directDiffuse = (forwardLight+backLight) * attenColor;
                float3 indirectDiffuse = float3(0,0,0);
                indirectDiffuse += UNITY_LIGHTMODEL_AMBIENT.rgb; // Ambient Light
                float4 _Emission_Light_var = tex2Dlod(_Emission_Light,float4(TRANSFORM_TEX(i.uv0, _Emission_Light),0.0,_Emission_Light_value));
                indirectDiffuse += _Emission_Light_var.rgb; // Diffuse Ambient Light
                float4 _Texture_var = tex2D(_Texture,TRANSFORM_TEX(i.uv0, _Texture));
                float3 diffuseColor = _Texture_var.rgb;
                float3 diffuse = (directDiffuse + indirectDiffuse) * diffuseColor;
////// Emissive:
                float4 _Emission_var = tex2D(_Emission,TRANSFORM_TEX(i.uv0, _Emission));
                float3 emissive = (_Emission_var.rgb*_Emission_value);
/// Final Color:
                float3 finalColor = diffuse + specular + emissive;
                fixed4 finalRGBA = fixed4(finalColor,1);
                UNITY_APPLY_FOG(i.fogCoord, finalRGBA);
                return finalRGBA;
            }
            ENDCG
        }
        Pass {
            Name "FORWARD_DELTA"
            Tags {
                "LightMode"="ForwardAdd"
            }
            Blend One One
            
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDADD
            #include "UnityCG.cginc"
            #include "AutoLight.cginc"
            #pragma multi_compile_fwdadd_fullshadows
            #pragma multi_compile_fog
            #pragma only_renderers d3d9 d3d11 glcore gles 
            #pragma target 3.0
            uniform float4 _LightColor0;
            uniform sampler2D _Texture; uniform float4 _Texture_ST;
            uniform sampler2D _ossclusion; uniform float4 _ossclusion_ST;
            uniform float _specular;
            uniform float _gloss;
            uniform sampler2D _Emission; uniform float4 _Emission_ST;
            uniform float _Emission_value;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 posWorld : TEXCOORD1;
                float3 normalDir : TEXCOORD2;
                LIGHTING_COORDS(3,4)
                UNITY_FOG_COORDS(5)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                float3 lightColor = _LightColor0.rgb;
                o.pos = UnityObjectToClipPos( v.vertex );
                UNITY_TRANSFER_FOG(o,o.pos);
                TRANSFER_VERTEX_TO_FRAGMENT(o)
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                i.normalDir = normalize(i.normalDir);
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float3 normalDirection = i.normalDir;
                float3 lightDirection = normalize(lerp(_WorldSpaceLightPos0.xyz, _WorldSpaceLightPos0.xyz - i.posWorld.xyz,_WorldSpaceLightPos0.w));
                float3 lightColor = _LightColor0.rgb;
                float3 halfDirection = normalize(viewDirection+lightDirection);
////// Lighting:
                float attenuation = LIGHT_ATTENUATION(i);
                float3 attenColor = attenuation * _LightColor0.xyz;
///////// Gloss:
                float gloss = _gloss;
                float specPow = exp2( gloss * 10.0 + 1.0 );
////// Specular:
                float NdotL = saturate(dot( normalDirection, lightDirection ));
                float3 specularColor = float3(_specular,_specular,_specular);
                float3 directSpecular = attenColor * pow(max(0,dot(halfDirection,normalDirection)),specPow)*specularColor;
                float3 specular = directSpecular;
/////// Diffuse:
                NdotL = dot( normalDirection, lightDirection );
                float3 forwardLight = pow(max(0.0, NdotL ), 5.0);
                float4 _ossclusion_var = tex2D(_ossclusion,TRANSFORM_TEX(i.uv0, _ossclusion));
                float3 backLight = pow(max(0.0, -NdotL ), 5.0) * _ossclusion_var.rgb;
                NdotL = max(0.0,dot( normalDirection, lightDirection ));
                float3 directDiffuse = (forwardLight+backLight) * attenColor;
                float4 _Texture_var = tex2D(_Texture,TRANSFORM_TEX(i.uv0, _Texture));
                float3 diffuseColor = _Texture_var.rgb;
                float3 diffuse = directDiffuse * diffuseColor;
/// Final Color:
                float3 finalColor = diffuse + specular;
                fixed4 finalRGBA = fixed4(finalColor * 1,0);
                UNITY_APPLY_FOG(i.fogCoord, finalRGBA);
                return finalRGBA;
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
