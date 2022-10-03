// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "VaxKun/AnimeShader/Diffuse"
{
	Properties
	{
		_MainTexTint("MainTexTint", Color) = (1,1,1,1)
		_ShadowColor("ShadowColor", Color) = (0.6666667,0.654902,1,1)
		_ShadowRange("ShadowRange", Range( 0 , 1)) = 0.15
		_ShadowIntensity("ShadowIntensity", Range( 0 , 1.8)) = 0
		_SpecularRange("SpecularRange", Range( 0.9 , 1)) = 0
		_SpecularMult("SpecularMult", Range( 0 , 1)) = 0.226
		_PointLightIntensity("PointLightIntensity", Range( 0 , 1)) = 0.5
		_MainTex("MainTex", 2D) = "white" {}
		_SpecularTexture("Specular/Normal Map", 2D) = "gray" {}
		[Toggle(_ENVIRONMENTSHADOWS_ON)] _EnvironmentShadows("EnvironmentShadows", Float) = 1
		_OutlineColor("OutlineColor", 2D) = "white" {}
		_OutlineColorTint("OutlineColorTint", Color) = (1,1,1,1)
		_OutlineWidth("OutlineWidth", Float) = 0
		_OutlineWidthMultiplier("OutlineWidthMultiplier", Float) = 1
		[Toggle]_UseTexturedOutline("Textured Outline", Float) = 0
		[Toggle]_VCOutline("VC Outline", Float) = 0
		_EmisiveTexture("Emisive Texture", 2D) = "black" {}
		[HDR]_EmisiveColor("EmisiveColor", Color) = (1,1,1,1)
		_FresnelExponent("Fresnel Exponent", Float) = 5
		_FresnelColor("Fresnel Color", Color) = (0.4382885,1,0.4198113,1)
		_FresnelBias("Fresnel Bias", Float) = 0
		[Toggle]_Fresnel("Fresnel", Float) = 0
		_ShadowSharpness("ShadowSharpness", Range( 0 , 1)) = 0.9
		[Toggle(_DONTUSESPECULARTEXTURE_ON)] _DontUseSpecularTexture("DontUseSpecularTexture", Float) = 0
		_SpecularBaseValue("SpecularBaseValue", Range( 0 , 1)) = 0.5
		_TextureLightness("TextureLightness", Float) = 1
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ }
		Cull Front
		CGPROGRAM
		#pragma target 3.0
		#pragma surface outlineSurf Outline nofog  keepalpha noshadow noambient novertexlights nolightmap nodynlightmap nodirlightmap nometa noforwardadd vertex:outlineVertexDataFunc 
		void outlineVertexDataFunc( inout appdata_full v, out Input o )
		{
			UNITY_INITIALIZE_OUTPUT( Input, o );
			float3 ase_vertexNormal = v.normal.xyz;
			float temp_output_145_0 = ( _OutlineWidth * _OutlineWidthMultiplier );
			float3 outlineVar = (( _VCOutline )?( ( v.color * temp_output_145_0 ) ):( float4( ( ase_vertexNormal * temp_output_145_0 ) , 0.0 ) )).rgb;
			v.vertex.xyz += outlineVar;
		}
		inline half4 LightingOutline( SurfaceOutput s, half3 lightDir, half atten ) { return half4 ( 0,0,0, s.Alpha); }
		void outlineSurf( Input i, inout SurfaceOutput o )
		{
			float2 uv_OutlineColor = i.uv_texcoord * _OutlineColor_ST.xy + _OutlineColor_ST.zw;
			o.Emission = (( _UseTexturedOutline )?( ( _OutlineColorTint * tex2D( _OutlineColor, uv_OutlineColor ) ) ):( _OutlineColorTint )).rgb;
		}
		ENDCG
		

		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" "IsEmissive" = "true"  }
		Cull Off
		CGINCLUDE
		#include "UnityPBSLighting.cginc"
		#include "UnityShaderVariables.cginc"
		#include "UnityCG.cginc"
		#include "Lighting.cginc"
		#pragma target 3.0
		#pragma shader_feature _ENVIRONMENTSHADOWS_ON
		#pragma shader_feature_local _DONTUSESPECULARTEXTURE_ON
		struct Input
		{
			float2 uv_texcoord;
			float3 worldPos;
			float3 worldNormal;
			float3 viewDir;
		};

		struct SurfaceOutputCustomLightingCustom
		{
			half3 Albedo;
			half3 Normal;
			half3 Emission;
			half Metallic;
			half Smoothness;
			half Occlusion;
			half Alpha;
			Input SurfInput;
			UnityGIInput GIData;
		};

		uniform float4 _EmisiveColor;
		uniform sampler2D _EmisiveTexture;
		uniform float4 _EmisiveTexture_ST;
		uniform float _ShadowRange;
		uniform float _ShadowSharpness;
		uniform sampler2D _SpecularTexture;
		uniform float4 _SpecularTexture_ST;
		uniform float _SpecularBaseValue;
		uniform sampler2D _MainTex;
		uniform float4 _MainTex_ST;
		uniform float _PointLightIntensity;
		uniform float _TextureLightness;
		uniform float4 _MainTexTint;
		uniform float4 _ShadowColor;
		uniform float _ShadowIntensity;
		uniform float _SpecularMult;
		uniform float _SpecularRange;
		uniform float4 _FresnelColor;
		uniform float _Fresnel;
		uniform float _FresnelBias;
		uniform float _FresnelExponent;
		uniform float _VCOutline;
		uniform float _OutlineWidth;
		uniform float _OutlineWidthMultiplier;
		uniform float _UseTexturedOutline;
		uniform float4 _OutlineColorTint;
		uniform sampler2D _OutlineColor;
		uniform float4 _OutlineColor_ST;

		void vertexDataFunc( inout appdata_full v, out Input o )
		{
			UNITY_INITIALIZE_OUTPUT( Input, o );
			v.vertex.xyz += 0;
		}

		inline half4 LightingStandardCustomLighting( inout SurfaceOutputCustomLightingCustom s, half3 viewDir, UnityGI gi )
		{
			UnityGIInput data = s.GIData;
			Input i = s.SurfInput;
			half4 c = 0;
			#ifdef UNITY_PASS_FORWARDBASE
			float ase_lightAtten = data.atten;
			if( _LightColor0.a == 0)
			ase_lightAtten = 0;
			#else
			float3 ase_lightAttenRGB = gi.light.color / ( ( _LightColor0.rgb ) + 0.000001 );
			float ase_lightAtten = max( max( ase_lightAttenRGB.r, ase_lightAttenRGB.g ), ase_lightAttenRGB.b );
			#endif
			#if defined(HANDLE_SHADOWS_BLENDING_IN_GI)
			half bakedAtten = UnitySampleBakedOcclusion(data.lightmapUV.xy, data.worldPos);
			float zDist = dot(_WorldSpaceCameraPos - data.worldPos, UNITY_MATRIX_V[2].xyz);
			float fadeDist = UnityComputeShadowFadeDistance(data.worldPos, zDist);
			ase_lightAtten = UnityMixRealtimeAndBakedShadows(data.atten, bakedAtten, UnityComputeShadowFade(fadeDist));
			#endif
			#if defined(LIGHTMAP_ON) && ( UNITY_VERSION < 560 || ( defined(LIGHTMAP_SHADOW_MIXING) && !defined(SHADOWS_SHADOWMASK) && defined(SHADOWS_SCREEN) ) )//aselc
			float4 ase_lightColor = 0;
			#else //aselc
			float4 ase_lightColor = _LightColor0;
			#endif //aselc
			float3 clampResult13 = clamp( ase_lightColor.rgb , float3( 0,0,0 ) , float3( 1,1,1 ) );
			float3 temp_cast_1 = (1.0).xxx;
			float3 ase_worldPos = i.worldPos;
			#if defined(LIGHTMAP_ON) && UNITY_VERSION < 560 //aseld
			float3 ase_worldlightDir = 0;
			#else //aseld
			float3 ase_worldlightDir = normalize( UnityWorldSpaceLightDir( ase_worldPos ) );
			#endif //aseld
			float3 ase_worldNormal = i.worldNormal;
			float3 normalizeResult93 = normalize( ase_worldNormal );
			float dotResult74 = dot( ase_worldlightDir , normalizeResult93 );
			float2 uv_SpecularTexture = i.uv_texcoord * _SpecularTexture_ST.xy + _SpecularTexture_ST.zw;
			float temp_output_69_0 = ( dotResult74 - ( 0.5 - tex2D( _SpecularTexture, uv_SpecularTexture ).g ) );
			float smoothstepResult126 = smoothstep( _ShadowRange , ( (1.0 + (_ShadowSharpness - 0.0) * (0.0 - 1.0) / (1.0 - 0.0)) + _ShadowRange ) , temp_output_69_0);
			float smoothstepResult130 = smoothstep( (0.9 + (_ShadowSharpness - 0.0) * (0.01 - 0.9) / (1.0 - 0.0)) , 0.0 , ase_lightAtten);
			#ifdef _ENVIRONMENTSHADOWS_ON
				float staticSwitch65 = smoothstepResult130;
			#else
				float staticSwitch65 = 0.0;
			#endif
			float clampResult63 = clamp( staticSwitch65 , 0.0 , 1.0 );
			float clampResult60 = clamp( ( smoothstepResult126 - clampResult63 ) , 0.0 , 1.0 );
			float3 lerpResult8 = lerp( clampResult13 , temp_cast_1 , clampResult60);
			#ifdef _DONTUSESPECULARTEXTURE_ON
				float staticSwitch136 = _SpecularBaseValue;
			#else
				float staticSwitch136 = tex2D( _SpecularTexture, uv_SpecularTexture ).g;
			#endif
			float4 temp_cast_3 = (staticSwitch136).xxxx;
			float2 uv_MainTex = i.uv_texcoord * _MainTex_ST.xy + _MainTex_ST.zw;
			float temp_output_141_0 = ( _WorldSpaceLightPos0.w * (1.0 + (_PointLightIntensity - 0.0) * (0.0 - 1.0) / (1.0 - 0.0)) );
			float4 temp_cast_4 = (temp_output_141_0).xxxx;
			float4 blendOpSrc30 = temp_cast_3;
			float4 blendOpDest30 = ( ( ( tex2D( _MainTex, uv_MainTex ) - temp_cast_4 ) * _TextureLightness ) * _MainTexTint );
			float4 temp_output_30_0 = ( saturate( (( blendOpDest30 > 0.5 ) ? ( 1.0 - 2.0 * ( 1.0 - blendOpDest30 ) * ( 1.0 - blendOpSrc30 ) ) : ( 2.0 * blendOpDest30 * blendOpSrc30 ) ) ));
			float DiffuseIf61 = clampResult60;
			float4 lerpResult147 = lerp( float4( 0,0,0,0 ) , temp_output_30_0 , DiffuseIf61);
			float4 temp_output_24_0 = ( ( temp_output_30_0 * _ShadowColor * _ShadowIntensity ) * ( 1.0 - DiffuseIf61 ) );
			float4 Diffuse37 = ( temp_output_30_0 * DiffuseIf61 );
			float3 normalizeResult90 = normalize( ase_worldNormal );
			float dotResult53 = dot( i.viewDir , normalizeResult90 );
			float4 tex2DNode57 = tex2D( _SpecularTexture, uv_SpecularTexture );
			float clampResult50 = clamp( pow( dotResult53 , tex2DNode57.r ) , 0.0 , 1.0 );
			float ifLocalVar49 = 0;
			if( clampResult50 <= _SpecularRange )
				ifLocalVar49 = 0.0;
			else
				ifLocalVar49 = 1.0;
			float ifLocalVar42 = 0;
			if( ( ifLocalVar49 * tex2DNode57.b ) <= 0.1 )
				ifLocalVar42 = 0.0;
			else
				ifLocalVar42 = 1.0;
			float4 clampResult16 = clamp( ( ( lerpResult147 + temp_output_24_0 ) + ( DiffuseIf61 * ( ( Diffuse37 * _SpecularMult ) * ifLocalVar42 ) ) ) , float4( 0,0,0,0 ) , float4( 1,1,1,1 ) );
			float4 lerpResult9 = lerp( clampResult16 , temp_output_24_0 , clampResult63);
			float3 lerpResult3 = lerp( clampResult13 , lerpResult8 , clampResult63);
			float4 clampResult1 = clamp( ( float4( lerpResult8 , 0.0 ) * lerpResult9 * float4( lerpResult3 , 0.0 ) ) , float4( 0,0,0,0 ) , float4( 1,1,1,1 ) );
			float3 ase_worldViewDir = normalize( UnityWorldSpaceViewDir( ase_worldPos ) );
			float fresnelNdotV118 = dot( ase_worldNormal, ase_worldViewDir );
			float fresnelNode118 = ( _FresnelBias + 1.0 * pow( 1.0 - fresnelNdotV118, _FresnelExponent ) );
			float PontLightInt149 = temp_output_141_0;
			float clampResult123 = clamp( ( fresnelNode118 - PontLightInt149 ) , 0.0 , 1.0 );
			float4 lerpResult120 = lerp( clampResult1 , _FresnelColor , (( _Fresnel )?( clampResult123 ):( 0.0 )));
			c.rgb = lerpResult120.rgb;
			c.a = 1;
			return c;
		}

		inline void LightingStandardCustomLighting_GI( inout SurfaceOutputCustomLightingCustom s, UnityGIInput data, inout UnityGI gi )
		{
			s.GIData = data;
		}

		void surf( Input i , inout SurfaceOutputCustomLightingCustom o )
		{
			o.SurfInput = i;
			float2 uv_EmisiveTexture = i.uv_texcoord * _EmisiveTexture_ST.xy + _EmisiveTexture_ST.zw;
			o.Emission = ( _EmisiveColor * tex2D( _EmisiveTexture, uv_EmisiveTexture ) ).rgb;
		}

		ENDCG
		CGPROGRAM
		#pragma surface surf StandardCustomLighting keepalpha fullforwardshadows exclude_path:deferred vertex:vertexDataFunc 

		ENDCG
		Pass
		{
			Name "ShadowCaster"
			Tags{ "LightMode" = "ShadowCaster" }
			ZWrite On
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 3.0
			#pragma multi_compile_shadowcaster
			#pragma multi_compile UNITY_PASS_SHADOWCASTER
			#pragma skip_variants FOG_LINEAR FOG_EXP FOG_EXP2
			#include "HLSLSupport.cginc"
			#if ( SHADER_API_D3D11 || SHADER_API_GLCORE || SHADER_API_GLES || SHADER_API_GLES3 || SHADER_API_METAL || SHADER_API_VULKAN )
				#define CAN_SKIP_VPOS
			#endif
			#include "UnityCG.cginc"
			#include "Lighting.cginc"
			#include "UnityPBSLighting.cginc"
			struct v2f
			{
				V2F_SHADOW_CASTER;
				float2 customPack1 : TEXCOORD1;
				float3 worldPos : TEXCOORD2;
				float3 worldNormal : TEXCOORD3;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};
			v2f vert( appdata_full v )
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID( v );
				UNITY_INITIALIZE_OUTPUT( v2f, o );
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO( o );
				UNITY_TRANSFER_INSTANCE_ID( v, o );
				Input customInputData;
				vertexDataFunc( v, customInputData );
				float3 worldPos = mul( unity_ObjectToWorld, v.vertex ).xyz;
				half3 worldNormal = UnityObjectToWorldNormal( v.normal );
				o.worldNormal = worldNormal;
				o.customPack1.xy = customInputData.uv_texcoord;
				o.customPack1.xy = v.texcoord;
				o.worldPos = worldPos;
				TRANSFER_SHADOW_CASTER_NORMALOFFSET( o )
				return o;
			}
			half4 frag( v2f IN
			#if !defined( CAN_SKIP_VPOS )
			, UNITY_VPOS_TYPE vpos : VPOS
			#endif
			) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID( IN );
				Input surfIN;
				UNITY_INITIALIZE_OUTPUT( Input, surfIN );
				surfIN.uv_texcoord = IN.customPack1.xy;
				float3 worldPos = IN.worldPos;
				half3 worldViewDir = normalize( UnityWorldSpaceViewDir( worldPos ) );
				surfIN.viewDir = worldViewDir;
				surfIN.worldPos = worldPos;
				surfIN.worldNormal = IN.worldNormal;
				SurfaceOutputCustomLightingCustom o;
				UNITY_INITIALIZE_OUTPUT( SurfaceOutputCustomLightingCustom, o )
				surf( surfIN, o );
				#if defined( CAN_SKIP_VPOS )
				float2 vpos = IN.pos;
				#endif
				SHADOW_CASTER_FRAGMENT( IN )
			}
			ENDCG
		}
	}
	Fallback "Diffuse"
	CustomEditor "AnimeCelShaderEditor"
}
/*ASEBEGIN
Version=18301
-1895.2;84;1890;1011;4486.31;794.9153;1.390388;True;True
Node;AmplifyShaderEditor.WorldNormalVector;94;-2811.274,1099.314;Inherit;False;False;1;0;FLOAT3;0,0,1;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.RangedFloatNode;129;-2417.487,1295.112;Inherit;False;Property;_ShadowSharpness;ShadowSharpness;22;0;Create;True;0;0;False;0;False;0.9;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.TexturePropertyNode;38;-4537.845,720.1375;Inherit;True;Property;_SpecularTexture;Specular/Normal Map;8;0;Create;False;0;0;False;0;False;None;None;False;gray;Auto;Texture2D;-1;0;1;SAMPLER2D;0
Node;AmplifyShaderEditor.NormalizeNode;93;-2586.524,1114.012;Inherit;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;78;-2666.256,1247.523;Inherit;False;Constant;_Float7;Float 7;7;0;Create;True;0;0;False;0;False;0.5;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;135;-2979.949,901.0515;Inherit;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;0.9;False;4;FLOAT;0.01;False;1;FLOAT;0
Node;AmplifyShaderEditor.LightAttenuation;67;-2968.966,644.3253;Inherit;False;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;79;-2806.256,1349.523;Inherit;True;Property;_TextureSample2;Texture Sample 2;7;0;Create;True;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.WorldSpaceLightDirHlpNode;76;-2781.256,935.5228;Inherit;False;False;1;0;FLOAT;0;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.RangedFloatNode;68;-2807.41,837.414;Inherit;False;Constant;_Float4;Float 4;6;0;Create;True;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SmoothstepOpNode;130;-2564.26,606.2969;Inherit;True;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;70;-2350.256,955.5229;Inherit;False;Property;_ShadowRange;ShadowRange;2;0;Create;True;0;0;False;0;False;0.15;0.15;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;143;-3926.474,-182.135;Inherit;False;Property;_PointLightIntensity;PointLightIntensity;6;0;Create;True;0;0;False;0;False;0.5;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.DotProductOpNode;74;-2362.256,1054.523;Inherit;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;75;-2384.256,1178.523;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;128;-2110.487,1189.112;Inherit;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;1;False;4;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;144;-3592.474,-191.135;Inherit;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;1;False;4;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.WorldSpaceLightPos;142;-3774.474,-321.1349;Inherit;False;0;3;FLOAT4;0;FLOAT3;1;FLOAT;2
Node;AmplifyShaderEditor.StaticSwitch;65;-2194.21,808.9742;Inherit;False;Property;_EnvironmentShadows;EnvironmentShadows;9;0;Create;True;0;0;False;0;False;0;1;1;True;;Toggle;2;Key0;Key1;Create;False;9;1;FLOAT;0;False;0;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT;0;False;7;FLOAT;0;False;8;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;69;-2177.256,1083.523;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;127;-1888.487,1070.112;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;141;-3390.474,-350.1349;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SmoothstepOpNode;126;-1742.487,938.1119;Inherit;True;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;63;-1809.408,682.8741;Inherit;True;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;39;-3844.059,-512.325;Inherit;True;Property;_MainTex;MainTex;7;0;Create;True;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleSubtractOpNode;62;-1535.985,1259.208;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.WorldNormalVector;95;-3688.28,326.3729;Inherit;False;False;1;0;FLOAT3;0,0,1;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.RangedFloatNode;139;-3072.482,-264.1752;Inherit;False;Property;_TextureLightness;TextureLightness;25;0;Create;True;0;0;False;0;False;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;140;-3222.749,-437.9601;Inherit;False;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;40;-2950.218,-174.0552;Inherit;False;Property;_MainTexTint;MainTexTint;0;0;Create;True;0;0;False;0;False;1,1,1,1;0.8014535,0.3254717,1,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;138;-2812.374,-387.6309;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;137;-2987.105,-909.2247;Inherit;False;Property;_SpecularBaseValue;SpecularBaseValue;24;0;Create;True;0;0;False;0;False;0.5;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;60;-1351.224,1258.299;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.NormalizeNode;90;-3450.007,345.942;Inherit;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.ViewDirInputsCoordNode;54;-3518.288,104.8728;Inherit;False;World;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SamplerNode;31;-3215.236,-750.8021;Inherit;True;Property;_TextureSample0;Texture Sample 0;0;0;Create;True;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DotProductOpNode;53;-3215.288,205.8728;Inherit;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;61;-1139.481,1288.498;Inherit;False;DiffuseIf;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StaticSwitch;136;-2771.533,-644.4159;Inherit;False;Property;_DontUseSpecularTexture;DontUseSpecularTexture;23;0;Create;True;0;0;False;0;False;0;0;0;True;;Toggle;2;Key0;Key1;Create;True;9;1;FLOAT;0;False;0;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT;0;False;7;FLOAT;0;False;8;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;32;-2611.358,-334.7616;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;57;-3398.288,520.8728;Inherit;True;Property;_TextureSample1;Texture Sample 1;5;0;Create;True;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PowerNode;52;-2935.288,251.8728;Inherit;False;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.BlendOpsNode;30;-2404.358,-530.7616;Inherit;False;Overlay;True;3;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;2;FLOAT;1;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;36;-2088.122,-395.4861;Inherit;False;61;DiffuseIf;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;82;-2771.481,443.895;Inherit;False;Constant;_Float8;Float 8;9;0;Create;True;0;0;False;0;False;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;81;-2894.201,366.7054;Inherit;False;Property;_SpecularRange;SpecularRange;4;0;Create;True;0;0;False;0;False;0;0.9820514;0.9;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;83;-2771.481,518.895;Inherit;False;Constant;_Float9;Float 9;10;0;Create;True;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;50;-2736.288,246.8728;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;35;-1695.724,-635.2543;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.ConditionalIfNode;49;-2524.288,241.8728;Inherit;False;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.WireNode;58;-2431.096,469.3448;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;37;-1508.724,-637.2543;Inherit;False;Diffuse;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;43;-2336.764,39.41095;Inherit;False;37;Diffuse;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;47;-2283.288,403.8728;Inherit;False;Constant;_Float2;Float 2;5;0;Create;True;0;0;False;0;False;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;46;-2293.288,332.8728;Inherit;False;Constant;_Float1;Float 1;5;0;Create;True;0;0;False;0;False;0.1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;34;-2419.358,-333.7616;Inherit;False;Property;_ShadowColor;ShadowColor;1;0;Create;True;0;0;False;0;False;0.6666667,0.654902,1,1;0.6663285,0.6544118,1,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GetLocalVarNode;28;-2281.658,-72.36159;Inherit;False;61;DiffuseIf;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;48;-2281.288,473.8728;Inherit;False;Constant;_Float3;Float 3;5;0;Create;True;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;44;-2423.764,126.4109;Inherit;False;Property;_SpecularMult;SpecularMult;5;0;Create;True;0;0;False;0;False;0.226;0.102;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;80;-2454.8,-166.295;Inherit;False;Property;_ShadowIntensity;ShadowIntensity;3;0;Create;True;0;0;False;0;False;0;0.7956449;0;1.8;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;45;-2255.288,220.8728;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;26;-2036.358,-172.7616;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;27;-1815.358,-274.7616;Inherit;False;219;183;//Shadows;1;24;;1,1,1,1;0;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;25;-2019.358,-298.7616;Inherit;False;3;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;41;-2056.202,52.80859;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.LightColorNode;14;-1326.358,-275.7616;Inherit;False;0;3;COLOR;0;FLOAT3;1;FLOAT;2
Node;AmplifyShaderEditor.ConditionalIfNode;42;-2071.202,242.8086;Inherit;False;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;147;-1767.822,-431.114;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;21;-1839.059,-43.3616;Inherit;False;61;DiffuseIf;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;24;-1765.358,-224.7616;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;22;-1795.358,39.2384;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.CommentaryNode;20;-1556.358,-44.7616;Inherit;False;219;183;//Specular;1;19;;1,1,1,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;15;-1100.358,-137.7616;Inherit;False;Constant;_Float0;Float 0;0;0;Create;True;0;0;False;0;False;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;13;-1102.358,-252.7616;Inherit;False;3;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;1,1,1;False;1;FLOAT3;0
Node;AmplifyShaderEditor.LerpOp;8;-807.3584,10.23843;Inherit;False;3;0;FLOAT3;0,0,0;False;1;FLOAT3;1,1,1;False;2;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;19;-1506.358,5.238403;Inherit;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;18;-1504.358,-221.7616;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;119;-928.7585,-330.8292;Inherit;False;Property;_FresnelExponent;Fresnel Exponent;18;0;Create;True;0;0;False;0;False;5;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;149;-3284.289,-250.8841;Inherit;False;PontLightInt;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;124;-896.228,-408.9011;Inherit;False;Property;_FresnelBias;Fresnel Bias;20;0;Create;True;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;17;-1279.358,-57.7616;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;3;-534.8082,-119.5187;Inherit;False;3;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;146;-691.8406,899.5049;Inherit;False;Property;_OutlineWidthMultiplier;OutlineWidthMultiplier;13;0;Create;True;0;0;False;0;False;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.WireNode;11;-369.2941,13.32383;Inherit;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.FresnelNode;118;-678.7585,-440.8292;Inherit;False;Standard;WorldNormal;ViewDir;False;False;5;0;FLOAT3;0,0,1;False;4;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;5;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;102;-666.4099,796.6605;Inherit;False;Property;_OutlineWidth;OutlineWidth;12;0;Create;True;0;0;False;0;False;0;0.006310679;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;16;-1101.358,-51.7616;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;1,1,1,1;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;152;-722.8276,-202.1216;Inherit;False;149;PontLightInt;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;145;-433.8406,784.5049;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;9;-810.3584,167.2384;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;98;-806.9099,317.8604;Inherit;False;Property;_OutlineColorTint;OutlineColorTint;11;0;Create;True;0;0;False;0;False;1,1,1,1;0,0,0,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;99;-843.9098,582.2603;Inherit;True;Property;_OutlineColor;OutlineColor;10;0;Create;True;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.NormalVertexDataNode;112;-243.2478,1026.231;Inherit;False;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.WireNode;10;-606.3584,170.2384;Inherit;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;150;-510.2021,-239.57;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.VertexColorNode;106;-453.0108,895.5573;Inherit;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;110;-48.2478,947.231;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;105;-242.4099,787.6605;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.ClampOpNode;123;-367.0471,-308.3158;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;100;-525.6097,534.9604;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;2;-487.3584,155.2384;Inherit;False;3;3;0;FLOAT3;0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT3;0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;121;-303.7585,-89.82922;Inherit;False;Property;_FresnelColor;Fresnel Color;19;0;Create;True;0;0;False;0;False;0.4382885,1,0.4198113,1;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ToggleSwitchNode;125;-159.8027,-223.8195;Inherit;False;Property;_Fresnel;Fresnel;21;0;Create;True;0;0;False;0;False;0;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;114;-286.729,-549.4121;Inherit;True;Property;_EmisiveTexture;Emisive Texture;16;0;Create;True;0;0;False;0;False;-1;None;None;True;0;False;black;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;116;-236.729,-842.412;Inherit;False;Property;_EmisiveColor;EmisiveColor;17;1;[HDR];Create;True;0;0;False;0;False;1,1,1,1;1,1,1,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ClampOpNode;1;-195.1352,151.1288;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;1,1,1,1;False;1;COLOR;0
Node;AmplifyShaderEditor.ToggleSwitchNode;113;-48.2478,736.231;Inherit;False;Property;_VCOutline;VC Outline;15;0;Create;True;0;0;False;0;False;0;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ToggleSwitchNode;103;-140.7099,549.6604;Inherit;False;Property;_UseTexturedOutline;Textured Outline;14;0;Create;False;0;0;False;0;False;0;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;71;-2059.256,1542.523;Inherit;False;Constant;_Float5;Float 5;7;0;Create;True;0;0;False;0;False;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ConditionalIfNode;64;-1762.008,1408.274;Inherit;True;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;115;30.27102,-626.4122;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;72;-2060.256,1618.523;Inherit;False;Constant;_Float6;Float 6;7;0;Create;True;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.OutlineNode;96;204.1901,489.3604;Inherit;False;2;True;None;0;0;Front;3;0;FLOAT3;0,0,0;False;2;FLOAT;0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.LerpOp;120;25.24146,77.17078;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;620.7991,-74.8;Float;False;True;-1;2;AnimeCelShaderEditor;0;0;CustomLighting;VaxKun/AnimeShader/Diffuse;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Off;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;0.5;True;True;0;False;Opaque;;Geometry;ForwardOnly;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT3;0,0,0;False;4;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;93;0;94;0
WireConnection;135;0;129;0
WireConnection;79;0;38;0
WireConnection;130;0;67;0
WireConnection;130;1;135;0
WireConnection;130;2;68;0
WireConnection;74;0;76;0
WireConnection;74;1;93;0
WireConnection;75;0;78;0
WireConnection;75;1;79;2
WireConnection;128;0;129;0
WireConnection;144;0;143;0
WireConnection;65;1;68;0
WireConnection;65;0;130;0
WireConnection;69;0;74;0
WireConnection;69;1;75;0
WireConnection;127;0;128;0
WireConnection;127;1;70;0
WireConnection;141;0;142;2
WireConnection;141;1;144;0
WireConnection;126;0;69;0
WireConnection;126;1;70;0
WireConnection;126;2;127;0
WireConnection;63;0;65;0
WireConnection;62;0;126;0
WireConnection;62;1;63;0
WireConnection;140;0;39;0
WireConnection;140;1;141;0
WireConnection;138;0;140;0
WireConnection;138;1;139;0
WireConnection;60;0;62;0
WireConnection;90;0;95;0
WireConnection;31;0;38;0
WireConnection;53;0;54;0
WireConnection;53;1;90;0
WireConnection;61;0;60;0
WireConnection;136;1;31;2
WireConnection;136;0;137;0
WireConnection;32;0;138;0
WireConnection;32;1;40;0
WireConnection;57;0;38;0
WireConnection;52;0;53;0
WireConnection;52;1;57;1
WireConnection;30;0;136;0
WireConnection;30;1;32;0
WireConnection;50;0;52;0
WireConnection;35;0;30;0
WireConnection;35;1;36;0
WireConnection;49;0;50;0
WireConnection;49;1;81;0
WireConnection;49;2;82;0
WireConnection;49;3;83;0
WireConnection;49;4;83;0
WireConnection;58;0;57;3
WireConnection;37;0;35;0
WireConnection;45;0;49;0
WireConnection;45;1;58;0
WireConnection;26;0;28;0
WireConnection;25;0;30;0
WireConnection;25;1;34;0
WireConnection;25;2;80;0
WireConnection;41;0;43;0
WireConnection;41;1;44;0
WireConnection;42;0;45;0
WireConnection;42;1;46;0
WireConnection;42;2;47;0
WireConnection;42;3;48;0
WireConnection;42;4;48;0
WireConnection;147;1;30;0
WireConnection;147;2;36;0
WireConnection;24;0;25;0
WireConnection;24;1;26;0
WireConnection;22;0;41;0
WireConnection;22;1;42;0
WireConnection;13;0;14;1
WireConnection;8;0;13;0
WireConnection;8;1;15;0
WireConnection;8;2;60;0
WireConnection;19;0;21;0
WireConnection;19;1;22;0
WireConnection;18;0;147;0
WireConnection;18;1;24;0
WireConnection;149;0;141;0
WireConnection;17;0;18;0
WireConnection;17;1;19;0
WireConnection;3;0;13;0
WireConnection;3;1;8;0
WireConnection;3;2;63;0
WireConnection;11;0;3;0
WireConnection;118;1;124;0
WireConnection;118;3;119;0
WireConnection;16;0;17;0
WireConnection;145;0;102;0
WireConnection;145;1;146;0
WireConnection;9;0;16;0
WireConnection;9;1;24;0
WireConnection;9;2;63;0
WireConnection;10;0;11;0
WireConnection;150;0;118;0
WireConnection;150;1;152;0
WireConnection;110;0;112;0
WireConnection;110;1;145;0
WireConnection;105;0;106;0
WireConnection;105;1;145;0
WireConnection;123;0;150;0
WireConnection;100;0;98;0
WireConnection;100;1;99;0
WireConnection;2;0;8;0
WireConnection;2;1;9;0
WireConnection;2;2;10;0
WireConnection;125;1;123;0
WireConnection;1;0;2;0
WireConnection;113;0;110;0
WireConnection;113;1;105;0
WireConnection;103;0;98;0
WireConnection;103;1;100;0
WireConnection;64;0;69;0
WireConnection;64;1;70;0
WireConnection;64;2;71;0
WireConnection;64;3;72;0
WireConnection;64;4;72;0
WireConnection;115;0;116;0
WireConnection;115;1;114;0
WireConnection;96;0;103;0
WireConnection;96;1;113;0
WireConnection;120;0;1;0
WireConnection;120;1;121;0
WireConnection;120;2;125;0
WireConnection;0;2;115;0
WireConnection;0;13;120;0
WireConnection;0;11;96;0
ASEEND*/
//CHKSM=8A97802C4118E99F64B95A99CD112DFDA6C69EB5