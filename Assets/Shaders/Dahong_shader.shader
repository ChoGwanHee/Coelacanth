// Shader created with Shader Forge v1.38 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.38;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,cgin:,lico:0,lgpr:1,limd:0,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,imps:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:2,bsrc:0,bdst:1,dpts:2,wrdp:True,dith:0,atcv:False,rfrpo:True,rfrpn:Refraction,coma:15,ufog:True,aust:True,igpj:False,qofs:0,qpre:1,rntp:1,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,atwp:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:False,fnfb:False,fsmp:False;n:type:ShaderForge.SFN_Final,id:1810,x:32852,y:32401,varname:node_1810,prsc:2|normal-6496-OUT,emission-4053-OUT,custl-7396-RGB;n:type:ShaderForge.SFN_LightVector,id:5257,x:31740,y:32417,varname:node_5257,prsc:2;n:type:ShaderForge.SFN_NormalVector,id:4529,x:31740,y:32559,prsc:2,pt:False;n:type:ShaderForge.SFN_Dot,id:306,x:31916,y:32468,varname:node_306,prsc:2,dt:0|A-5257-OUT,B-4529-OUT;n:type:ShaderForge.SFN_Step,id:3199,x:32124,y:32512,varname:node_3199,prsc:2|A-306-OUT,B-7225-OUT;n:type:ShaderForge.SFN_Vector1,id:7225,x:31933,y:32619,varname:node_7225,prsc:2,v1:0.4;n:type:ShaderForge.SFN_Tex2d,id:7396,x:32518,y:32529,ptovrint:False,ptlb:node_7396,ptin:_node_7396,varname:node_7396,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:37122323e8427d64481022664893a3a8,ntxv:0,isnm:False|MIP-3327-OUT;n:type:ShaderForge.SFN_Vector1,id:4179,x:32399,y:32787,varname:node_4179,prsc:2,v1:0.008;n:type:ShaderForge.SFN_Fresnel,id:3402,x:32124,y:32945,varname:node_3402,prsc:2;n:type:ShaderForge.SFN_Color,id:5197,x:32533,y:32889,ptovrint:False,ptlb:node_5197,ptin:_node_5197,varname:node_5197,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:0,c2:0,c3:0.1,c4:1;n:type:ShaderForge.SFN_Vector1,id:1908,x:32241,y:32077,varname:node_1908,prsc:2,v1:-0.04;n:type:ShaderForge.SFN_Multiply,id:221,x:32947,y:32121,varname:node_221,prsc:2;n:type:ShaderForge.SFN_ValueProperty,id:5992,x:32241,y:32150,ptovrint:False,ptlb:node_5992,ptin:_node_5992,varname:node_5992,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0.9;n:type:ShaderForge.SFN_Multiply,id:8605,x:32124,y:32796,varname:node_8605,prsc:2;n:type:ShaderForge.SFN_Vector1,id:6131,x:32124,y:32631,varname:node_6131,prsc:2,v1:0.5;n:type:ShaderForge.SFN_Multiply,id:4053,x:32446,y:32106,varname:node_4053,prsc:2|A-1908-OUT,B-5992-OUT;n:type:ShaderForge.SFN_Add,id:3327,x:32296,y:32546,varname:node_3327,prsc:2|A-3199-OUT,B-6131-OUT;n:type:ShaderForge.SFN_Fresnel,id:200,x:31911,y:32812,varname:node_200,prsc:2;n:type:ShaderForge.SFN_LightVector,id:177,x:31922,y:32991,varname:node_177,prsc:2;n:type:ShaderForge.SFN_Tex2d,id:4381,x:32411,y:32301,ptovrint:False,ptlb:node_4381,ptin:_node_4381,varname:node_4381,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:06e125ab0b6e4014bb3675593eeb47a4,ntxv:3,isnm:True;n:type:ShaderForge.SFN_Slider,id:3609,x:31877,y:32277,ptovrint:False,ptlb:node_3609,ptin:_node_3609,varname:node_3609,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0,max:1;n:type:ShaderForge.SFN_Add,id:6685,x:32640,y:32004,varname:node_6685,prsc:2;n:type:ShaderForge.SFN_ValueProperty,id:4382,x:32409,y:32029,ptovrint:False,ptlb:node_4382,ptin:_node_4382,varname:node_4382,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0;n:type:ShaderForge.SFN_Add,id:6496,x:32654,y:32368,varname:node_6496,prsc:2|A-4381-RGB,B-272-OUT;n:type:ShaderForge.SFN_Vector1,id:272,x:32488,y:32443,varname:node_272,prsc:2,v1:0.5;n:type:ShaderForge.SFN_Vector1,id:664,x:32603,y:32711,varname:node_664,prsc:2,v1:0;proporder:7396-5197-5992-4381-3609-4382;pass:END;sub:END;*/

Shader "Custom/Dahong_shader" {
    Properties {
        _node_7396 ("node_7396", 2D) = "white" {}
        _node_5197 ("node_5197", Color) = (0,0,0.1,1)
        _node_5992 ("node_5992", Float ) = 0.9
        _node_4381 ("node_4381", 2D) = "bump" {}
        _node_3609 ("node_3609", Range(0, 1)) = 0
        _node_4382 ("node_4382", Float ) = 0
    }
    SubShader {
        Tags {
            "RenderType"="Opaque"
        }
        LOD 200
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
            uniform sampler2D _node_7396; uniform float4 _node_7396_ST;
            uniform float _node_5992;
            uniform sampler2D _node_4381; uniform float4 _node_4381_ST;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float4 tangent : TANGENT;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 posWorld : TEXCOORD1;
                float3 normalDir : TEXCOORD2;
                float3 tangentDir : TEXCOORD3;
                float3 bitangentDir : TEXCOORD4;
                LIGHTING_COORDS(5,6)
                UNITY_FOG_COORDS(7)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                o.tangentDir = normalize( mul( unity_ObjectToWorld, float4( v.tangent.xyz, 0.0 ) ).xyz );
                o.bitangentDir = normalize(cross(o.normalDir, o.tangentDir) * v.tangent.w);
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
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
                float3x3 tangentTransform = float3x3( i.tangentDir, i.bitangentDir, i.normalDir);
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float3 _node_4381_var = UnpackNormal(tex2D(_node_4381,TRANSFORM_TEX(i.uv0, _node_4381)));
                float3 normalLocal = (_node_4381_var.rgb+0.5);
                float3 normalDirection = normalize(mul( normalLocal, tangentTransform )); // Perturbed normals
                float3 lightDirection = normalize(_WorldSpaceLightPos0.xyz);
////// Lighting:
////// Emissive:
                float node_4053 = ((-0.04)*_node_5992);
                float3 emissive = float3(node_4053,node_4053,node_4053);
                float4 _node_7396_var = tex2Dlod(_node_7396,float4(TRANSFORM_TEX(i.uv0, _node_7396),0.0,(step(dot(lightDirection,i.normalDir),0.4)+0.5)));
                float3 finalColor = emissive + _node_7396_var.rgb;
                fixed4 finalRGBA = fixed4(finalColor,1);
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
            struct VertexInput {
                float4 vertex : POSITION;
            };
            struct VertexOutput {
                V2F_SHADOW_CASTER;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.pos = UnityObjectToClipPos( v.vertex );
                TRANSFER_SHADOW_CASTER(o)
                return o;
            }
            float4 frag(VertexOutput i, float facing : VFACE) : COLOR {
                float isFrontFace = ( facing >= 0 ? 1 : 0 );
                float faceSign = ( facing >= 0 ? 1 : -1 );
                SHADOW_CASTER_FRAGMENT(i)
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
