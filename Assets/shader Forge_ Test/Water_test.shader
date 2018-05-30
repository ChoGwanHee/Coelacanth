// Shader created with Shader Forge v1.38 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.38;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,cgin:,lico:0,lgpr:1,limd:1,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,imps:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:2,bsrc:0,bdst:1,dpts:2,wrdp:True,dith:0,atcv:False,rfrpo:True,rfrpn:Refraction,coma:15,ufog:False,aust:True,igpj:False,qofs:0,qpre:1,rntp:1,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,atwp:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:False,fnfb:False,fsmp:False;n:type:ShaderForge.SFN_Final,id:4013,x:34513,y:32797,varname:node_4013,prsc:2|diff-8382-OUT,spec-6506-OUT,gloss-6470-OUT,normal-2272-RGB,transm-6024-OUT,lwrap-6024-OUT,voffset-639-OUT;n:type:ShaderForge.SFN_Color,id:1304,x:33033,y:33042,ptovrint:False,ptlb:Color,ptin:_Color,varname:node_1304,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:0.5220588,c2:0.7,c3:1,c4:1;n:type:ShaderForge.SFN_Append,id:7026,x:32131,y:32974,varname:node_7026,prsc:2|A-5371-OUT,B-7496-OUT;n:type:ShaderForge.SFN_Vector1,id:5371,x:31950,y:32955,varname:node_5371,prsc:2,v1:-0.1;n:type:ShaderForge.SFN_Vector1,id:7496,x:31962,y:33076,varname:node_7496,prsc:2,v1:-0.5;n:type:ShaderForge.SFN_Time,id:8005,x:32270,y:33018,varname:node_8005,prsc:2;n:type:ShaderForge.SFN_Multiply,id:7197,x:32464,y:32953,varname:node_7197,prsc:2|A-7026-OUT,B-8005-TTR;n:type:ShaderForge.SFN_Tex2d,id:6837,x:32919,y:32869,ptovrint:False,ptlb:water_fall,ptin:_water_fall,varname:node_6837,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:7bdba3ec569d11e4a9d32ca1f718175a,ntxv:0,isnm:False|UVIN-6663-OUT;n:type:ShaderForge.SFN_Add,id:4469,x:32688,y:33019,varname:node_4469,prsc:2|A-7197-OUT,B-8366-UVOUT;n:type:ShaderForge.SFN_TexCoord,id:8366,x:32337,y:33233,varname:node_8366,prsc:2,uv:0,uaff:True;n:type:ShaderForge.SFN_Multiply,id:2776,x:33327,y:32814,varname:node_2776,prsc:2|A-6837-R,B-1304-RGB;n:type:ShaderForge.SFN_ValueProperty,id:6024,x:34186,y:33085,ptovrint:False,ptlb:light,ptin:_light,varname:node_6024,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:5;n:type:ShaderForge.SFN_Tex2d,id:3854,x:32893,y:33361,ptovrint:False,ptlb:node_3854,ptin:_node_3854,varname:node_3854,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:5a5513296fc9cc647802d9548f823da7,ntxv:0,isnm:False|UVIN-4469-OUT;n:type:ShaderForge.SFN_Clamp01,id:9610,x:32893,y:33177,varname:node_9610,prsc:2|IN-613-OUT;n:type:ShaderForge.SFN_SwitchProperty,id:3710,x:33148,y:33198,ptovrint:False,ptlb:node_3710,ptin:_node_3710,varname:node_3710,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,on:False|A-9610-OUT,B-3854-RGB;n:type:ShaderForge.SFN_Multiply,id:6791,x:33350,y:33284,varname:node_6791,prsc:2|A-3710-OUT,B-3854-R;n:type:ShaderForge.SFN_RemapRange,id:6687,x:33527,y:33167,varname:node_6687,prsc:2,frmn:0,frmx:3,tomn:-1,tomx:30|IN-6791-OUT;n:type:ShaderForge.SFN_Clamp01,id:3973,x:33509,y:32992,varname:node_3973,prsc:2|IN-6687-OUT;n:type:ShaderForge.SFN_Add,id:2248,x:33657,y:32735,varname:node_2248,prsc:2|A-2776-OUT,B-3973-OUT;n:type:ShaderForge.SFN_Clamp01,id:1695,x:33839,y:32745,varname:node_1695,prsc:2|IN-2248-OUT;n:type:ShaderForge.SFN_OneMinus,id:613,x:32667,y:33188,varname:node_613,prsc:2|IN-8366-V;n:type:ShaderForge.SFN_SwitchProperty,id:8382,x:33977,y:32713,ptovrint:False,ptlb:node_8382,ptin:_node_8382,varname:node_8382,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,on:False|A-1695-OUT,B-1304-RGB;n:type:ShaderForge.SFN_Multiply,id:6294,x:33103,y:33529,varname:node_6294,prsc:2|A-3854-R,B-8366-Z;n:type:ShaderForge.SFN_SwitchProperty,id:1953,x:33557,y:33431,ptovrint:False,ptlb:node_1953,ptin:_node_1953,varname:node_1953,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,on:False|A-6791-OUT,B-6294-OUT;n:type:ShaderForge.SFN_NormalVector,id:439,x:33752,y:33534,prsc:2,pt:False;n:type:ShaderForge.SFN_Multiply,id:639,x:34070,y:33408,varname:node_639,prsc:2|A-8132-OUT,B-439-OUT,C-3703-OUT;n:type:ShaderForge.SFN_Vector1,id:3703,x:33752,y:33694,varname:node_3703,prsc:2,v1:2;n:type:ShaderForge.SFN_Multiply,id:6663,x:32731,y:32852,varname:node_6663,prsc:2|A-4469-OUT,B-3738-OUT;n:type:ShaderForge.SFN_Vector1,id:3738,x:32537,y:32847,varname:node_3738,prsc:2,v1:1.5;n:type:ShaderForge.SFN_Add,id:8132,x:33875,y:33335,varname:node_8132,prsc:2|A-7120-OUT,B-1953-OUT;n:type:ShaderForge.SFN_Multiply,id:7120,x:33712,y:33219,varname:node_7120,prsc:2|A-2776-OUT,B-2057-OUT;n:type:ShaderForge.SFN_Vector1,id:2057,x:33568,y:33344,varname:node_2057,prsc:2,v1:0.1;n:type:ShaderForge.SFN_Tex2d,id:2272,x:34055,y:32910,ptovrint:False,ptlb:node_2272,ptin:_node_2272,varname:node_2272,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:120bf9966cdfd7b428e213137d4e6cff,ntxv:3,isnm:True|UVIN-4947-OUT;n:type:ShaderForge.SFN_Vector1,id:6506,x:34158,y:32800,varname:node_6506,prsc:2,v1:65;n:type:ShaderForge.SFN_Multiply,id:4947,x:33889,y:32895,varname:node_4947,prsc:2|A-4469-OUT,B-3749-OUT;n:type:ShaderForge.SFN_Vector1,id:3749,x:33710,y:32953,varname:node_3749,prsc:2,v1:0.5;n:type:ShaderForge.SFN_Vector1,id:6470,x:34202,y:32855,varname:node_6470,prsc:2,v1:0.8;proporder:1304-6837-6024-3854-3710-8382-1953-2272;pass:END;sub:END;*/

Shader "Shader Forge/Water_test" {
    Properties {
        _Color ("Color", Color) = (0.5220588,0.7,1,1)
        _water_fall ("water_fall", 2D) = "white" {}
        _light ("light", Float ) = 5
        _node_3854 ("node_3854", 2D) = "white" {}
        [MaterialToggle] _node_3710 ("node_3710", Float ) = 1
        [MaterialToggle] _node_8382 ("node_8382", Float ) = 0
        [MaterialToggle] _node_1953 ("node_1953", Float ) = 0
        _node_2272 ("node_2272", 2D) = "bump" {}
    }
    SubShader {
        Tags {
            "RenderType"="Opaque"
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
            #pragma only_renderers d3d9 d3d11 glcore gles 
            #pragma target 3.0
            uniform float4 _LightColor0;
            uniform float4 _Color;
            uniform sampler2D _water_fall; uniform float4 _water_fall_ST;
            uniform float _light;
            uniform sampler2D _node_3854; uniform float4 _node_3854_ST;
            uniform fixed _node_3710;
            uniform fixed _node_8382;
            uniform fixed _node_1953;
            uniform sampler2D _node_2272; uniform float4 _node_2272_ST;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float4 tangent : TANGENT;
                float4 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float4 uv0 : TEXCOORD0;
                float4 posWorld : TEXCOORD1;
                float3 normalDir : TEXCOORD2;
                float3 tangentDir : TEXCOORD3;
                float3 bitangentDir : TEXCOORD4;
                LIGHTING_COORDS(5,6)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                o.tangentDir = normalize( mul( unity_ObjectToWorld, float4( v.tangent.xyz, 0.0 ) ).xyz );
                o.bitangentDir = normalize(cross(o.normalDir, o.tangentDir) * v.tangent.w);
                float4 node_8005 = _Time;
                float2 node_4469 = ((float2((-0.1),(-0.5))*node_8005.a)+o.uv0);
                float2 node_6663 = (node_4469*1.5);
                float4 _water_fall_var = tex2Dlod(_water_fall,float4(TRANSFORM_TEX(node_6663, _water_fall),0.0,0));
                float3 node_2776 = (_water_fall_var.r*_Color.rgb);
                float4 _node_3854_var = tex2Dlod(_node_3854,float4(TRANSFORM_TEX(node_4469, _node_3854),0.0,0));
                float3 node_6791 = (lerp( saturate((1.0 - o.uv0.g)), _node_3854_var.rgb, _node_3710 )*_node_3854_var.r);
                v.vertex.xyz += (((node_2776*0.1)+lerp( node_6791, (_node_3854_var.r*o.uv0.b), _node_1953 ))*v.normal*2.0);
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                float3 lightColor = _LightColor0.rgb;
                o.pos = UnityObjectToClipPos( v.vertex );
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
                float4 node_8005 = _Time;
                float2 node_4469 = ((float2((-0.1),(-0.5))*node_8005.a)+i.uv0);
                float2 node_4947 = (node_4469*0.5);
                float3 _node_2272_var = UnpackNormal(tex2D(_node_2272,TRANSFORM_TEX(node_4947, _node_2272)));
                float3 normalLocal = _node_2272_var.rgb;
                float3 normalDirection = normalize(mul( normalLocal, tangentTransform )); // Perturbed normals
                float3 lightDirection = normalize(_WorldSpaceLightPos0.xyz);
                float3 lightColor = _LightColor0.rgb;
                float3 halfDirection = normalize(viewDirection+lightDirection);
////// Lighting:
                float attenuation = LIGHT_ATTENUATION(i);
                float3 attenColor = attenuation * _LightColor0.xyz;
///////// Gloss:
                float gloss = 0.8;
                float specPow = exp2( gloss * 10.0 + 1.0 );
////// Specular:
                float NdotL = saturate(dot( normalDirection, lightDirection ));
                float node_6506 = 65.0;
                float3 specularColor = float3(node_6506,node_6506,node_6506);
                float3 directSpecular = attenColor * pow(max(0,dot(halfDirection,normalDirection)),specPow)*specularColor;
                float3 specular = directSpecular;
/////// Diffuse:
                NdotL = dot( normalDirection, lightDirection );
                float3 w = float3(_light,_light,_light)*0.5; // Light wrapping
                float3 NdotLWrap = NdotL * ( 1.0 - w );
                float3 forwardLight = max(float3(0.0,0.0,0.0), NdotLWrap + w );
                float3 backLight = max(float3(0.0,0.0,0.0), -NdotLWrap + w ) * float3(_light,_light,_light);
                NdotL = max(0.0,dot( normalDirection, lightDirection ));
                float3 directDiffuse = (forwardLight+backLight) * attenColor;
                float3 indirectDiffuse = float3(0,0,0);
                indirectDiffuse += UNITY_LIGHTMODEL_AMBIENT.rgb; // Ambient Light
                float2 node_6663 = (node_4469*1.5);
                float4 _water_fall_var = tex2D(_water_fall,TRANSFORM_TEX(node_6663, _water_fall));
                float3 node_2776 = (_water_fall_var.r*_Color.rgb);
                float4 _node_3854_var = tex2D(_node_3854,TRANSFORM_TEX(node_4469, _node_3854));
                float3 node_6791 = (lerp( saturate((1.0 - i.uv0.g)), _node_3854_var.rgb, _node_3710 )*_node_3854_var.r);
                float3 diffuseColor = lerp( saturate((node_2776+saturate((node_6791*10.33333+-1.0)))), _Color.rgb, _node_8382 );
                float3 diffuse = (directDiffuse + indirectDiffuse) * diffuseColor;
/// Final Color:
                float3 finalColor = diffuse + specular;
                return fixed4(finalColor,1);
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
            #pragma only_renderers d3d9 d3d11 glcore gles 
            #pragma target 3.0
            uniform float4 _Color;
            uniform sampler2D _water_fall; uniform float4 _water_fall_ST;
            uniform sampler2D _node_3854; uniform float4 _node_3854_ST;
            uniform fixed _node_3710;
            uniform fixed _node_1953;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float4 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                V2F_SHADOW_CASTER;
                float4 uv0 : TEXCOORD1;
                float4 posWorld : TEXCOORD2;
                float3 normalDir : TEXCOORD3;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                float4 node_8005 = _Time;
                float2 node_4469 = ((float2((-0.1),(-0.5))*node_8005.a)+o.uv0);
                float2 node_6663 = (node_4469*1.5);
                float4 _water_fall_var = tex2Dlod(_water_fall,float4(TRANSFORM_TEX(node_6663, _water_fall),0.0,0));
                float3 node_2776 = (_water_fall_var.r*_Color.rgb);
                float4 _node_3854_var = tex2Dlod(_node_3854,float4(TRANSFORM_TEX(node_4469, _node_3854),0.0,0));
                float3 node_6791 = (lerp( saturate((1.0 - o.uv0.g)), _node_3854_var.rgb, _node_3710 )*_node_3854_var.r);
                v.vertex.xyz += (((node_2776*0.1)+lerp( node_6791, (_node_3854_var.r*o.uv0.b), _node_1953 ))*v.normal*2.0);
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                o.pos = UnityObjectToClipPos( v.vertex );
                TRANSFER_SHADOW_CASTER(o)
                return o;
            }
            float4 frag(VertexOutput i, float facing : VFACE) : COLOR {
                float isFrontFace = ( facing >= 0 ? 1 : 0 );
                float faceSign = ( facing >= 0 ? 1 : -1 );
                i.normalDir = normalize(i.normalDir);
                i.normalDir *= faceSign;
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float3 normalDirection = i.normalDir;
                SHADOW_CASTER_FRAGMENT(i)
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
