// ========== ========== ==========
//   マチェイヨラ_Cutout
//      Author : にしおかすみす！
//      Twitter : @nsokSMITHdayo
// ========== ========== ==========

Shader "すみすのやかた/マチェイヨラ/マチェイヨラ_Cutout"
{
    Properties
    {
        [HideInInspector]
        [Enum(Japanese,0,English,1)] __Lang   ("__Lang"              , Int) = 0
        [HideInInspector] __Albedo_Foldout    ("__Albedo_Foldout"    , Int) = 0
        [HideInInspector] __Mask_Foldout      ("__Mask_Foldout"      , Int) = 0
        [HideInInspector] __Detail_Foldout    ("__Detail_Foldout"    , Int) = 0
        [HideInInspector] __ClearCoat_Foldout ("__ClearCoat_Foldout" , Int) = 0
        [HideInInspector] __Lame_Foldout      ("__Lame_Foldout"      , Int) = 0
        [HideInInspector] __Carbon_Foldout    ("__Carbon_Foldout"    , Int) = 0
        [HideInInspector] __Advanced_Foldout  ("__Advanced_Foldout"  , Int) = 0
        
        [Enum(Back,2,Off,0)]
                        _CullMode         ("Cull Mode" , Int) = 2

                        _Color            ("Color"     , Color) = (1,1,1,1)
        [NoScaleOffset] _MainTex          ("MainTex"   , 2D) = "white" {}
        [Toggle(IS_MATCAP)] _Is_MatCap    ("Is MatCap" , Int) = 0
        [HideInInspector] __sync_MainTex_Tiling ("__sync", Int) = 1
                        _MainTex_Tiling_X ("Tiling X"  , float) = 1
                        _MainTex_Tiling_Y ("Tiling Y"  , float) = 1
        [HideInInspector] __sync_MainTex_Offset ("__sync", Int) = 0
                        _MainTex_Offset_X ("Offset X"  , float) = 0
                        _MainTex_Offset_Y ("Offset Y"  , float) = 0
        
                        _Cutoff         ("Cut off"          , range(0, 1)) = 0.5
                        _LameCutoff     ("Lame Cut off"     , range(0, 1)) = 0

        [NoScaleOffset] _GradTex        ("Gradation Tex"   , 2D) = "white" {}
        [Toggle]        _InvertGrad     ("Invert Grad"     , Int) = 0
        [Enum(Mix,0,Multiply,1,Additive,2)]
                        _BlendMode        ("Blend Mode"      , Int) = 1
                        _BlendFactor    ("Blend Factor"    , range(0, 1)) = 1
                        _GradPower      ("Gradation Power" , float) = 1.2
                        _GradShift      ("Gradation Shift" , float) = 0

        [NoScaleOffset] _DetailTex          ("Detail Tex" , 2D) = "white" {}
        [HideInInspector] __sync_DetailTex_Tiling ("__sync", Int) = 1
                        _DetailTex_Tiling_X ("Tiling X"   , float) = 1
                        _DetailTex_Tiling_Y ("Tiling Y"   , float) = 1
        [HideInInspector] __sync_DetailTex_Offset ("__sync", Int) = 0
                        _DetailTex_Offset_X ("Offset X"   , float) = 0
                        _DetailTex_Offset_Y ("Offset Y"   , float) = 0

        [Toggle]    _ClearCoat              ("Clear Coat"                   , Int) = 1
                    _ClearCoatColor         ("Clear Coat Color"             , Color) = (1, 1, 1, 1)
        [IntRange]  _ClearCoatCount         ("Cleat Coat Count"             , range(1, 10)) = 1
        [NoScaleOffset]
        [Normal]   _ClearCoatNormal          ("Clear Coat Normal" , 2D) = "bump" {}
        [HideInInspector] __sync_ClearCoatNormal_Tiling ("__sync", Int) = 1
                   _ClearCoatNormal_Tiling_X ("Tiling X"          , float) = 1
                   _ClearCoatNormal_Tiling_Y ("Tiling Y"          , float) = 1
        [HideInInspector] __sync_ClearCoatNormal_Offset ("__sync", Int) = 0
                   _ClearCoatNormal_Offset_X ("Offset X"          , float) = 0
                   _ClearCoatNormal_Offset_Y ("Offset Y"          , float) = 0
        [Toggle]   _ClearCoatNormalDirectX   ("DirectX"           , Int) = 0
                   _ClearCoatNormalLevel     ("Level"             , float) = 0.1
                   
        [Enum(R,0,G,1,B,2,A,3,Manual,4)]
                   _CCSmoothChannel   ("Clear Coat Smooth Channel" , Int) = 4
        [Toggle]   _CCSCInvert        ("Invert"                    , int) = 0
                   _CCSmoothAdjust    ("Adjust"                    , float) = 1
                   _ClearCoatSmooth   ("Clear Coat Smooth"         , range(0, 1)) = 0.95
        [Enum(R,0,G,1,B,2,A,3,Manual,4)]
                   _BSSmoothChannel ("Base Surface Smooth Channel" , Int) = 4
        [Toggle]   _BSSCInvert        ("Invert"                    , int) = 0
                   _BSSmoothAdjust    ("Adjust"                    , float) = 1
                   _BaseSurfSmooth    ("Base Surface Smooth"       , Range(0,1)) = 0.7
        [Enum(R,0,G,1,B,2,A,3,Manual,4)]
                   _MetallicChannel   ("Metallic Channel"          , Int) = 4
        [Toggle]   _MetallicCInvert   ("Invert"                    , int) = 0
                   _MetallicAdjust    ("Adjust"                    , float) = 1
                   _Metallic          ("Metallic"                  , Range(0,1)) = 0.95

        [HideInInspector]               __masked             ("__masked"               , Int) = 0
        [Toggle]                        _Use_VertColorAsMask ("UseVertColorAsMask"     , Int) = 0
        [NoScaleOffset]                 _GradMask            ("Macheiyora Mask"        , 2D) = "white" {}
        [HideInInspector] __sync_GradMask_Tiling ("__sync", Int) = 1
                                        _GradMask_Tiling_X   ("Tiling X"               , float) = 1
                                        _GradMask_Tiling_Y   ("Tiling Y"               , float) = 1
        [HideInInspector] __sync_GradMask_Offset ("__sync", Int) = 0
                                        _GradMask_Offset_X   ("Offset X"               , float) = 0
                                        _GradMask_Offset_Y   ("Offset Y"               , float) = 0
        [Toggle]                        _InvertGradMask      ("Invert Gradation Mask"  , Int) = 0
        [Enum(R,0,G,1,B,2,A,3,None,4)]  _GradMaskChannel     ("Gradation Mask Channel" , Int) = 4
        [Toggle]                        _InvertLameMask      ("Invert Lame Mask"       , Int) = 0
        [Enum(R,0,G,1,B,2,A,3,None,4)]  _LameMaskChannel     ("Lame Mask Channel"      , Int) = 4
        [Toggle]                        _InvertGlitterMask   ("Invert Glitter Mask"    , Int) = 0
        [Enum(R,0,G,1,B,2,A,3,None,4)]  _GlitterMaskChannel  ("Glitter Mask Channel"   , Int) = 4
        [Toggle]                        _InvertEmissionMask  ("Invert Emission Mask"   , Int) = 0
        [Enum(R,0,G,1,B,2,A,3,None,4)]  _EmissionMaskChannel ("Emission Mask Channel"  , Int) = 4
        [Toggle]                        _InvertCarbonMask    ("Invert Carbon Mask"     , Int) = 0
        [Enum(R,0,G,1,B,2,A,3,None,4)]  _CarbonMaskChannel   ("Carbon Mask Channel"    , Int) = 4
        [Toggle]                        _InvertCCMask        ("Invert CC Mask"         , Int) = 0
        [Enum(R,0,G,1,B,2,A,3,None,4)]  _CCMaskChannel       ("CC Mask Channel"        , Int) = 4

                 _FresnelLevel   ("Fresnel Level"    , range(0, 1)) = 0
                 _FresnelPower   ("Fresnel Power"    , range(0, 1)) = 0.5
        [Toggle] _InvertFresnel  ("Invert Fresnel"   , int) = 0

        [Space(15)]
        [NoScaleOffset]
        [Normal]    _NormalMap          ("Normal Map 1" , 2D) = "bump" {}
        [HideInInspector] __sync_NormalMap_Tiling ("__sync", Int) = 1
                    _NormalMap_Tiling_X ("Tiling X"     , float) = 1
                    _NormalMap_Tiling_Y ("Tiling Y"     , float) = 1
        [HideInInspector] __sync_NormalMap_Offset ("__sync", Int) = 0
                    _NormalMap_Offset_X ("Offset X"     , float) = 0
                    _NormalMap_Offset_Y ("Offset Y"     , float) = 0
        [Toggle]    _DirectX            ("DirectX"      , Int) = 0
                    _NormalLevel        ("Level"        , float) = 0.5

        [NoScaleOffset]
        [Normal]    _NormalMap2          ("Normal Map 2" , 2D) = "bump" {}
        [HideInInspector] __sync_NormalMap2_Tiling ("__sync", Int) = 1
                    _NormalMap2_Tiling_X ("Tiling X"     , float) = 1
                    _NormalMap2_Tiling_Y ("Tiling Y"     , float) = 1
        [HideInInspector] __sync_NormalMap2_Offset ("__sync", Int) = 0
                    _NormalMap2_Offset_X ("Offset X"     , float) = 0
                    _NormalMap2_Offset_Y ("Offset Y"     , float) = 0
        [Toggle]    _DirectX2            ("DirectX"      , Int) = 0
                    _NormalLevel2        ("Level"        , float) = 0.5

                    _EmissiveBoost          ("Emissive Boost"           , range(0, 2)) = 0
                    _BaseAffectByLightDir   ("Affect by Light Direction", range(0, 1)) = 1

        [NoScaleOffset]
        [Normal]    _BaseGlitterTex        ("Base Glitter Texture" , 2D) = "bump" {}
                    _BaseGlitterTex_Tiling ("Tiling"               , float) = 40
                    _BaseGlitterLevel      ("Base Glitter Level"   , range(0, 1)) = 0.5

        [KeywordEnum(None,Low,High)]
                    _Lame           ("Lame Glitter"    , Int) = 1
        [Toggle]    _LameBackFace   ("Lame BackFace"   , Int) = 1
                    _LameChroma     ("Lame Chroma"     , range(0, 1)) = 0.8
                    _LameHue        ("Lame Hue"        , range(0, 1)) = 0
                    _LameAspect     ("Lame Aspect"     , range(0.01, 0.99)) = 0.5
                    _LameVolume     ("Lame Volume"     , float) = 300
                    _LameDistance   ("Lame Distance"   , range(0, 1)) = 0.85
                    _LameThinOut    ("Lame Thin Out"   , range(0, 1)) = 0
                    _LameMetallic   ("Lame Metallic"   , range(0, 1)) = 0.9
                    _LameSmooth     ("Lame Smooth"     , range(0, 1)) = 0.9

        [Toggle]    _Use_Carbon         ("Use Carbon" , Int) = 0
                    _Carbon_Color1      ("Color1", Color) = (0.2,0.2,0.2,1)
        [Toggle]    _Use_Carbon_Color1  ("use Color1", Int) = 0
                    _Carbon_Color2      ("Color2", Color) = (0.2,0.2,0.2,1)
        [Toggle]    _Use_Carbon_Color2  ("use Color1", Int) = 0
                    _Carbon_Tiling      ("Tiling", float) = 10
                    _Carbon_Detail      ("Detail", range(0, 1)) = 0.8
        [NoScaleOffset]
                    _Carbon_Tex         ("Carbon Texture", 2D) = "white" {}
        [NoScaleOffset][Normal]
                    _Carbon_Normal      ("CarbonNormalMap", 2D) = "bump" {}
                    _Carbon_Aspect      ("Carbon Aspect"  , range(0.01, 0.99)) = 0.5
        [Toggle]    _Carbon_StripeAngle ("Carbon Stripe Angle", int) = 0
                    _Carbon_BumpScale   ("Bump Scale", range(0, 1)) = 0.3
    }

	CGINCLUDE
		#define UNITY_SETUP_BRDF_INPUT MetallicSetup
	ENDCG
    
    SubShader
    {
        Tags
        {
            "RenderType"="TransparentCutout"
            "Queue"="AlphaTest"
        }
        LOD 200
        Cull[_CullMode]

        //Maziora
        CGPROGRAM
        #pragma surface surf Standard fullforwardshadows alphatest:_Cutoff vertex:vert
        #pragma target 4.0

        #pragma shader_feature _ _IS_MATCAP_ON
        #pragma shader_feature _ _USE_CARBON_ON
        
        #include "./Maziora.cginc"
        
        ENDCG

        //Lame Flake
        CGPROGRAM
        #pragma target 3.0
        #pragma surface surf Standard fullforwardshadows alphatest:_LameCutoff vertex:vert

        #pragma shader_feature _LAME_NONE _LAME_LOW _LAME_HIGH

        #include "./Lame_input.cginc"

        void surf (Input i, inout SurfaceOutputStandard o)
        {
            int uv_offset = 0;
            #ifdef _LAME_NONE
                discard;
            #else
                #include "./Lame_surf.cginc"
            #endif
        }

        ENDCG

        //More Lame Flake
        CGPROGRAM
        #pragma target 3.0
        #pragma surface surf Standard fullforwardshadows alphatest:_LameCutoff vertex:vert

        #pragma shader_feature _LAME_NONE _LAME_LOW _LAME_HIGH

        #include "./Lame_input.cginc"

        void surf (Input i, inout SurfaceOutputStandard o)
        {
            fixed uv_offset = 0.5;
            #ifdef _LAME_HIGH
                #include "./Lame_surf.cginc"
            #else
                discard;
            #endif
        }

        ENDCG

        //Clear
        CGPROGRAM
        #pragma target 3.0
        #pragma surface surf Standard alpha vertex:vert

        #pragma shader_feature _ _CLEARCOAT_ON

        #include "./ClearCoat.cginc"
        
        ENDCG
    
		// ------------------------------------------------------------------
		//  Shadow rendering pass
		Pass {
			Name "ShadowCaster"
			Tags { "LightMode" = "ShadowCaster" }

			ZWrite On ZTest LEqual

			CGPROGRAM
			#pragma target 3.0
            
			#pragma shader_feature _ _ALPHATEST_ON _ALPHABLEND_ON _ALPHAPREMULTIPLY_ON
			#pragma multi_compile_shadowcaster
			#pragma multi_compile_instancing

			#pragma vertex vertShadowCaster
			#pragma fragment fragShadowCaster

			#include "UnityStandardShadow.cginc"

			ENDCG
		}
    }
    // FallBack "Standard"
    CustomEditor "Macheiyora.MacheiyoraGUI"
}
