// Shader created with Shader Forge v1.38 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.38;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,cgin:,lico:1,lgpr:1,limd:1,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,imps:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:2,bsrc:0,bdst:1,dpts:2,wrdp:True,dith:0,atcv:False,rfrpo:True,rfrpn:Refraction,coma:15,ufog:True,aust:True,igpj:False,qofs:0,qpre:2,rntp:3,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.1086721,fgcg:0.1086721,fgcb:0.1102941,fgca:0,fgde:0.04,fgrn:0,fgrf:300,stcl:False,atwp:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:False,fnfb:False,fsmp:False;n:type:ShaderForge.SFN_Final,id:8017,x:32859,y:32618,varname:node_8017,prsc:2|diff-8309-OUT,emission-4255-RGB,clip-3662-OUT,olwid-8534-OUT;n:type:ShaderForge.SFN_RemapRange,id:3135,x:31886,y:32914,varname:node_3135,prsc:2,frmn:0,frmx:1,tomn:-0.5,tomx:0.5|IN-9316-OUT;n:type:ShaderForge.SFN_Add,id:3662,x:32180,y:33034,varname:node_3662,prsc:2|A-3135-OUT,B-100-R;n:type:ShaderForge.SFN_OneMinus,id:9316,x:31686,y:32914,varname:node_9316,prsc:2|IN-9096-OUT;n:type:ShaderForge.SFN_Tex2d,id:100,x:31817,y:33100,ptovrint:False,ptlb:noise,ptin:_noise,varname:node_100,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:5a5513296fc9cc647802d9548f823da7,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Slider,id:9096,x:31338,y:32866,ptovrint:False,ptlb:slider,ptin:_slider,varname:node_9096,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0,max:1;n:type:ShaderForge.SFN_RemapRange,id:142,x:31801,y:32596,varname:node_142,prsc:2,frmn:0,frmx:1,tomn:-12,tomx:12|IN-3662-OUT;n:type:ShaderForge.SFN_OneMinus,id:7538,x:32095,y:32373,varname:node_7538,prsc:2|IN-5117-OUT;n:type:ShaderForge.SFN_Clamp01,id:286,x:31703,y:32272,varname:node_286,prsc:2|IN-5651-OUT;n:type:ShaderForge.SFN_RemapRange,id:5651,x:31495,y:32272,varname:node_5651,prsc:2,frmn:0,frmx:1,tomn:-1,tomx:1;n:type:ShaderForge.SFN_Clamp01,id:5117,x:31981,y:32521,varname:node_5117,prsc:2|IN-142-OUT;n:type:ShaderForge.SFN_Vector1,id:2670,x:32604,y:32462,varname:node_2670,prsc:2,v1:1.5;n:type:ShaderForge.SFN_Tex2d,id:4255,x:32485,y:32718,varname:node_4255,prsc:2,tex:ecea72e053581ea479d5c1820911af60,ntxv:0,isnm:False|UVIN-6980-OUT,TEX-4481-TEX;n:type:ShaderForge.SFN_Tex2dAsset,id:4481,x:32121,y:32750,ptovrint:False,ptlb:Emission noise,ptin:_Emissionnoise,varname:node_4481,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:ecea72e053581ea479d5c1820911af60,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Tex2d,id:6329,x:32466,y:32339,ptovrint:False,ptlb:node_6329,ptin:_node_6329,varname:node_6329,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:d6d1bd460b747a946b6585d1e54f483c,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Append,id:6980,x:32298,y:32507,varname:node_6980,prsc:2|A-7538-OUT,B-4375-OUT;n:type:ShaderForge.SFN_ValueProperty,id:4375,x:32121,y:32607,ptovrint:False,ptlb:node_4375,ptin:_node_4375,varname:node_4375,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0;n:type:ShaderForge.SFN_Add,id:1031,x:32747,y:32364,varname:node_1031,prsc:2|B-8546-OUT;n:type:ShaderForge.SFN_ValueProperty,id:8546,x:32517,y:32265,ptovrint:False,ptlb:node_8546,ptin:_node_8546,varname:node_8546,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0.1;n:type:ShaderForge.SFN_Multiply,id:8309,x:32990,y:32435,varname:node_8309,prsc:2|A-6329-RGB,B-8560-RGB,C-2670-OUT;n:type:ShaderForge.SFN_ValueProperty,id:2332,x:32792,y:32260,ptovrint:False,ptlb:node_2332,ptin:_node_2332,varname:node_2332,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:2;n:type:ShaderForge.SFN_ValueProperty,id:8534,x:32621,y:33003,ptovrint:False,ptlb:outline,ptin:_outline,varname:node_8534,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0.01;n:type:ShaderForge.SFN_Color,id:8560,x:32622,y:32555,ptovrint:False,ptlb:diffuse color,ptin:_diffusecolor,varname:node_8560,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:1,c2:1,c3:1,c4:1;proporder:100-9096-4481-6329-4375-8546-2332-8534-8560;pass:END;sub:END;*/

Shader "Custom/noise_test" {
    Properties {
        _noise ("noise", 2D) = "white" {}
        _slider ("slider", Range(0, 1)) = 0
        _Emissionnoise ("Emission noise", 2D) = "white" {}
        _node_6329 ("node_6329", 2D) = "white" {}
        _node_4375 ("node_4375", Float ) = 0
        _node_8546 ("node_8546", Float ) = 0.1
        _node_2332 ("node_2332", Float ) = 2
        _outline ("outline", Float ) = 0.01
        _diffusecolor ("diffuse color", Color) = (1,1,1,1)
        [HideInInspector]_Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
    }
    SubShader {
        Tags {
            "Queue"="AlphaTest"
            "RenderType"="TransparentCutout"
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
            uniform sampler2D _noise; uniform float4 _noise_ST;
            uniform float _slider;
            uniform float _outline;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                UNITY_FOG_COORDS(1)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.pos = UnityObjectToClipPos( float4(v.vertex.xyz + v.normal*_outline,1) );
                UNITY_TRANSFER_FOG(o,o.pos);
                return o;
            }
            float4 frag(VertexOutput i, float facing : VFACE) : COLOR {
                float isFrontFace = ( facing >= 0 ? 1 : 0 );
                float faceSign = ( facing >= 0 ? 1 : -1 );
                float4 _noise_var = tex2D(_noise,TRANSFORM_TEX(i.uv0, _noise));
                float node_3662 = (((1.0 - _slider)*1.0+-0.5)+_noise_var.r);
                clip(node_3662 - 0.5);
                return fixed4(float3(0,0,0),0);
            }
            ENDCG
        }
        Pass {
            Name "FORWARD"
            Tags {
                "LightMode"="ForwardBase"
            }
            Cull Off
            
            
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
            uniform sampler2D _noise; uniform float4 _noise_ST;
            uniform float _slider;
            uniform sampler2D _Emissionnoise; uniform float4 _Emissionnoise_ST;
            uniform sampler2D _node_6329; uniform float4 _node_6329_ST;
            uniform float _node_4375;
            uniform float4 _diffusecolor;
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
            float4 frag(VertexOutput i, float facing : VFACE) : COLOR {
                float isFrontFace = ( facing >= 0 ? 1 : 0 );
                float faceSign = ( facing >= 0 ? 1 : -1 );
                i.normalDir = normalize(i.normalDir);
                i.normalDir *= faceSign;
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float3 normalDirection = i.normalDir;
                float4 _noise_var = tex2D(_noise,TRANSFORM_TEX(i.uv0, _noise));
                float node_3662 = (((1.0 - _slider)*1.0+-0.5)+_noise_var.r);
                clip(node_3662 - 0.5);
                float3 lightDirection = normalize(_WorldSpaceLightPos0.xyz);
                float3 lightColor = _LightColor0.rgb;
////// Lighting:
                float attenuation = LIGHT_ATTENUATION(i);
                float3 attenColor = attenuation * _LightColor0.xyz;
/////// Diffuse:
                float NdotL = max(0.0,dot( normalDirection, lightDirection ));
                float3 directDiffuse = max( 0.0, NdotL) * attenColor;
                float3 indirectDiffuse = float3(0,0,0);
                indirectDiffuse += UNITY_LIGHTMODEL_AMBIENT.rgb; // Ambient Light
                float4 _node_6329_var = tex2D(_node_6329,TRANSFORM_TEX(i.uv0, _node_6329));
                float3 diffuseColor = (_node_6329_var.rgb*_diffusecolor.rgb*1.5);
                float3 diffuse = (directDiffuse + indirectDiffuse) * diffuseColor;
////// Emissive:
                float2 node_6980 = float2((1.0 - saturate((node_3662*24.0+-12.0))),_node_4375);
                float4 node_4255 = tex2D(_Emissionnoise,TRANSFORM_TEX(node_6980, _Emissionnoise));
                float3 emissive = node_4255.rgb;
/// Final Color:
                float3 finalColor = diffuse + emissive;
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
            Cull Off
            
            
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
            uniform sampler2D _noise; uniform float4 _noise_ST;
            uniform float _slider;
            uniform sampler2D _Emissionnoise; uniform float4 _Emissionnoise_ST;
            uniform sampler2D _node_6329; uniform float4 _node_6329_ST;
            uniform float _node_4375;
            uniform float4 _diffusecolor;
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
            float4 frag(VertexOutput i, float facing : VFACE) : COLOR {
                float isFrontFace = ( facing >= 0 ? 1 : 0 );
                float faceSign = ( facing >= 0 ? 1 : -1 );
                i.normalDir = normalize(i.normalDir);
                i.normalDir *= faceSign;
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float3 normalDirection = i.normalDir;
                float4 _noise_var = tex2D(_noise,TRANSFORM_TEX(i.uv0, _noise));
                float node_3662 = (((1.0 - _slider)*1.0+-0.5)+_noise_var.r);
                clip(node_3662 - 0.5);
                float3 lightDirection = normalize(lerp(_WorldSpaceLightPos0.xyz, _WorldSpaceLightPos0.xyz - i.posWorld.xyz,_WorldSpaceLightPos0.w));
                float3 lightColor = _LightColor0.rgb;
////// Lighting:
                float attenuation = LIGHT_ATTENUATION(i);
                float3 attenColor = attenuation * _LightColor0.xyz;
/////// Diffuse:
                float NdotL = max(0.0,dot( normalDirection, lightDirection ));
                float3 directDiffuse = max( 0.0, NdotL) * attenColor;
                float4 _node_6329_var = tex2D(_node_6329,TRANSFORM_TEX(i.uv0, _node_6329));
                float3 diffuseColor = (_node_6329_var.rgb*_diffusecolor.rgb*1.5);
                float3 diffuse = directDiffuse * diffuseColor;
/// Final Color:
                float3 finalColor = diffuse;
                fixed4 finalRGBA = fixed4(finalColor * 1,0);
                UNITY_APPLY_FOG(i.fogCoord, finalRGBA);
                return finalRGBA;
            }
            ENDCG
        }
        Pass {
            Name "ShadowCaster"
            Tags {
                "LightMode"="ShadowCaster"
            }
            Offset 1, 1
            Cull Off
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_SHADOWCASTER
            #include "UnityCG.cginc"
            #include "Lighting.cginc"
            #pragma fragmentoption ARB_precision_hint_fastest
            #pragma multi_compile_shadowcaster
            #pragma multi_compile_fog
            #pragma only_renderers d3d9 d3d11 glcore gles 
            #pragma target 3.0
            uniform sampler2D _noise; uniform float4 _noise_ST;
            uniform float _slider;
            struct VertexInput {
                float4 vertex : POSITION;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                V2F_SHADOW_CASTER;
                float2 uv0 : TEXCOORD1;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.pos = UnityObjectToClipPos( v.vertex );
                TRANSFER_SHADOW_CASTER(o)
                return o;
            }
            float4 frag(VertexOutput i, float facing : VFACE) : COLOR {
                float isFrontFace = ( facing >= 0 ? 1 : 0 );
                float faceSign = ( facing >= 0 ? 1 : -1 );
                float4 _noise_var = tex2D(_noise,TRANSFORM_TEX(i.uv0, _noise));
                float node_3662 = (((1.0 - _slider)*1.0+-0.5)+_noise_var.r);
                clip(node_3662 - 0.5);
                SHADOW_CASTER_FRAGMENT(i)
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
