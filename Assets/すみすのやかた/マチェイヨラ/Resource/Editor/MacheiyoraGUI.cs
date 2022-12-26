// ========== ========== ==========
//   マチェイヨラ       ver 3.1.0
//      Author : にしおかすみす！
//      Twitter : @nsokSMITHdayo
// ========== ========== ==========

using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Linq;
using System;

namespace Macheiyora
{
    public class MacheiyoraGUI : ShaderGUI
    {
        private const string shaderVer = "3.1.0";
        bool isOpenURL;
        private const string boothURL  = "https://smith-no-yakata.booth.pm/items/3767387";
        private const string gitHubURL = "https://github.com/nsoksmith/Macheiyora";
        private const string manualURL = "https://github.com/nsoksmith/Macheiyora/blob/main/usage.md";
        
        // UI -----------------------------
        public GUILayoutOption[] shortButtonStyle = new GUILayoutOption[]{ GUILayout.Width(130) }; 
        public GUILayoutOption[] middleButtonStyle = new GUILayoutOption[]{ GUILayout.Width(130) }; 
        
        void drawButton(string text, string URL)
        {
            if (GUILayout.Button(text))
            {
                Application.OpenURL(URL);
            }
        }

        static void drawLine()
        {
            EditorGUI.DrawRect(EditorGUI.IndentedRect(EditorGUILayout.GetControlRect(false, 1)), Color.gray);
        }

        static bool Foldout(bool display, string title)
        {
            var style = new GUIStyle("ShurikenModuleTitle");
            style.font = new GUIStyle(EditorStyles.boldLabel).font;
            style.border = new RectOffset(15, 7, 4, 4);
            style.fixedHeight = 22;
            style.contentOffset = new Vector2(20f, -2f);

            var rect = GUILayoutUtility.GetRect(16f, 22f, style);
            GUI.Box(rect, title, style);

            var e = Event.current;

            var toggleRect = new Rect(rect.x + 4f, rect.y + 2f, 13f, 13f);
            if (e.type == EventType.Repaint)
            {
                EditorStyles.foldout.Draw(toggleRect, false, false, display, false);
            }

            if (e.type == EventType.MouseDown && rect.Contains(e.mousePosition))
            {
                display = !display;
                e.Use();
            }

            return display;
        }
        
        public override void OnGUI(MaterialEditor ME, MaterialProperty[] Prop)
        {
            var mat = (Material)ME.target;

            String[] shaderName = new String[4]{ "すみすのやかた/マチェイヨラ/マチェイヨラ_Opaque",
                                                 "すみすのやかた/マチェイヨラ/マチェイヨラ_Cutout",
                                                 "すみすのやかた/マチェイヨラ/マチェイヨラ_Fade",
                                                 "すみすのやかた/マチェイヨラ/マチェイヨラ_Transparent" };

            bool isOpaque = (mat.shader.name == shaderName[0]);
            bool isCutout = (mat.shader.name == shaderName[1]);
            bool isFade   = (mat.shader.name == shaderName[2]);
            bool isTransp = (mat.shader.name == shaderName[3]);

            MaterialProperty Lang              = FindProperty("__Lang", Prop);
            MaterialProperty Albedo_Foldout    = FindProperty("__Albedo_Foldout", Prop);
            MaterialProperty Mask_Foldout      = FindProperty("__Mask_Foldout", Prop);
            MaterialProperty Detail_Foldout    = FindProperty("__Detail_Foldout", Prop);
            MaterialProperty ClearCoat_Foldout = FindProperty("__ClearCoat_Foldout", Prop);
            MaterialProperty Lame_Foldout      = FindProperty("__Lame_Foldout", Prop);
            MaterialProperty Carbon_Foldout    = FindProperty("__Carbon_Foldout", Prop);
            MaterialProperty Advanced_Foldout  = FindProperty("__Advanced_Foldout", Prop);

            MaterialProperty CullMode          = FindProperty("_CullMode", Prop);

            MaterialProperty Color               = FindProperty("_Color", Prop);
            MaterialProperty MainTex             = FindProperty("_MainTex", Prop);
            MaterialProperty Is_MatCap           = FindProperty("_Is_MatCap", Prop);
            MaterialProperty MainTex_Tiling_X    = FindProperty("_MainTex_Tiling_X", Prop);
            MaterialProperty MainTex_Tiling_Y    = FindProperty("_MainTex_Tiling_Y", Prop);
            MaterialProperty MainTex_Offset_X    = FindProperty("_MainTex_Offset_X", Prop);
            MaterialProperty MainTex_Offset_Y    = FindProperty("_MainTex_Offset_Y", Prop);
            MaterialProperty sync_MainTex_Tiling = FindProperty("__sync_MainTex_Tiling", Prop);
            MaterialProperty sync_MainTex_Offset = FindProperty("__sync_MainTex_Offset", Prop);
            Vector2          MainTex_Tiling;
            Vector2          MainTex_Offset;
            bool             bool_MainTex_Tiling;
            bool             bool_MainTex_Offset;
            
            MaterialProperty Cutoff     = FindProperty("_Cutoff", Prop, false);
            MaterialProperty LameCutoff = FindProperty("_LameCutoff", Prop, false);

            MaterialProperty blending   = FindProperty("__blending", Prop, false);
            MaterialProperty BlendSrc   = FindProperty("_BlendSrc", Prop, false);
            MaterialProperty BlendDst   = FindProperty("_BlendDst", Prop, false);

            if(isOpaque){
                mat.DisableKeyword("_ALPHATEST_ON");
                mat.DisableKeyword("_ALPHABLEND_ON");
                mat.DisableKeyword("_ALPHAPREMULTIPLY_ON");
            }
            if(isCutout){
                mat.EnableKeyword("_ALPHATEST_ON");
                mat.DisableKeyword("_ALPHABLEND_ON");
                mat.DisableKeyword("_ALPHAPREMULTIPLY_ON");
            }
            if(isFade){
                mat.DisableKeyword("_ALPHATEST_ON");
                mat.EnableKeyword("_ALPHABLEND_ON");
                mat.DisableKeyword("_ALPHAPREMULTIPLY_ON");
            }
            if(isTransp){
                mat.DisableKeyword("_ALPHATEST_ON");
                mat.DisableKeyword("_ALPHABLEND_ON");
                mat.EnableKeyword("_ALPHAPREMULTIPLY_ON");
            }

            MaterialProperty GradTex     = FindProperty("_GradTex", Prop);
            MaterialProperty InvertGrad  = FindProperty("_InvertGrad", Prop);
            MaterialProperty InvertGradFromDetailTex = FindProperty("_InvertGradFromDetailTex", Prop);
            MaterialProperty BlendMode   = FindProperty("_BlendMode", Prop);
            MaterialProperty BlendFactor = FindProperty("_BlendFactor", Prop);
            MaterialProperty GradPower   = FindProperty("_GradPower", Prop);
            MaterialProperty GradShift   = FindProperty("_GradShift", Prop);

            MaterialProperty ClearCoat       = FindProperty("_ClearCoat", Prop);
            MaterialProperty ClearCoatColor  = FindProperty("_ClearCoatColor", Prop);
            MaterialProperty ClearCoatCount  = FindProperty("_ClearCoatCount", Prop);
            MaterialProperty ClearCoatNormal          = FindProperty("_ClearCoatNormal", Prop);
            MaterialProperty ClearCoatNormal_Tiling_X = FindProperty("_ClearCoatNormal_Tiling_X", Prop);
            MaterialProperty ClearCoatNormal_Tiling_Y = FindProperty("_ClearCoatNormal_Tiling_Y", Prop);
            MaterialProperty ClearCoatNormal_Offset_X = FindProperty("_ClearCoatNormal_Offset_X", Prop);
            MaterialProperty ClearCoatNormal_Offset_Y = FindProperty("_ClearCoatNormal_Offset_Y", Prop);
            MaterialProperty sync_ClearCoatNormal_Tiling = FindProperty("__sync_ClearCoatNormal_Tiling", Prop);
            MaterialProperty sync_ClearCoatNormal_Offset = FindProperty("__sync_ClearCoatNormal_Offset", Prop);
            Vector2          ClearCoatNormal_Tiling;
            Vector2          ClearCoatNormal_Offset;
            bool             bool_ClearCoatNormal_Tiling;
            bool             bool_ClearCoatNormal_Offset;
            MaterialProperty ClearCoatNormalDirectX = FindProperty("_ClearCoatNormalDirectX", Prop);
            MaterialProperty ClearCoatNormalLevel   = FindProperty("_ClearCoatNormalLevel", Prop);

            MaterialProperty ClearCoatSmooth = FindProperty("_ClearCoatSmooth", Prop);
            MaterialProperty CCSmoothChannel = FindProperty("_CCSmoothChannel", Prop);
            MaterialProperty CCSCInvert      = FindProperty("_CCSCInvert", Prop);
            MaterialProperty CCSmoothAdjust  = FindProperty("_CCSmoothAdjust", Prop);
            MaterialProperty BSSmoothChannel = FindProperty("_BSSmoothChannel", Prop);
            MaterialProperty BSSCInvert      = FindProperty("_BSSCInvert", Prop);
            MaterialProperty BSSmoothAdjust  = FindProperty("_BSSmoothAdjust", Prop);
            MaterialProperty BaseSurfSmooth  = FindProperty("_BaseSurfSmooth", Prop);
            MaterialProperty MetallicChannel = FindProperty("_MetallicChannel", Prop);
            MaterialProperty MetallicAdjust  = FindProperty("_MetallicAdjust", Prop);
            MaterialProperty Metallic        = FindProperty("_Metallic", Prop);

            MaterialProperty NormalMap             = FindProperty("_NormalMap", Prop);
            MaterialProperty NormalMap_Tiling_X    = FindProperty("_NormalMap_Tiling_X", Prop);
            MaterialProperty NormalMap_Tiling_Y    = FindProperty("_NormalMap_Tiling_Y", Prop);
            MaterialProperty NormalMap_Offset_X    = FindProperty("_NormalMap_Offset_X", Prop);
            MaterialProperty NormalMap_Offset_Y    = FindProperty("_NormalMap_Offset_Y", Prop);
            MaterialProperty sync_NormalMap_Tiling = FindProperty("__sync_NormalMap_Tiling", Prop);
            MaterialProperty sync_NormalMap_Offset = FindProperty("__sync_NormalMap_Offset", Prop);
            Vector2          NormalMap_Tiling;
            Vector2          NormalMap_Offset;
            bool             bool_NormalMap_Tiling;
            bool             bool_NormalMap_Offset;
            MaterialProperty DirectX     = FindProperty("_DirectX", Prop);
            MaterialProperty NormalLevel = FindProperty("_NormalLevel", Prop);

            MaterialProperty NormalMap2             = FindProperty("_NormalMap2", Prop);
            MaterialProperty NormalMap2_Tiling_X    = FindProperty("_NormalMap2_Tiling_X", Prop);
            MaterialProperty NormalMap2_Tiling_Y    = FindProperty("_NormalMap2_Tiling_Y", Prop);
            MaterialProperty NormalMap2_Offset_X    = FindProperty("_NormalMap2_Offset_X", Prop);
            MaterialProperty NormalMap2_Offset_Y    = FindProperty("_NormalMap2_Offset_Y", Prop);
            MaterialProperty sync_NormalMap2_Tiling = FindProperty("__sync_NormalMap2_Tiling", Prop);
            MaterialProperty sync_NormalMap2_Offset = FindProperty("__sync_NormalMap2_Offset", Prop);
            Vector2          NormalMap2_Tiling;
            Vector2          NormalMap2_Offset;
            bool             bool_NormalMap2_Tiling;
            bool             bool_NormalMap2_Offset;
            MaterialProperty DirectX2     = FindProperty("_DirectX2", Prop);
            MaterialProperty NormalLevel2 = FindProperty("_NormalLevel2", Prop);

            MaterialProperty DetailTex             = FindProperty("_DetailTex", Prop);
            MaterialProperty DetailTex_Tiling_X    = FindProperty("_DetailTex_Tiling_X", Prop);
            MaterialProperty DetailTex_Tiling_Y    = FindProperty("_DetailTex_Tiling_Y", Prop);
            MaterialProperty DetailTex_Offset_X    = FindProperty("_DetailTex_Offset_X", Prop);
            MaterialProperty DetailTex_Offset_Y    = FindProperty("_DetailTex_Offset_Y", Prop);
            MaterialProperty sync_DetailTex_Tiling = FindProperty("__sync_DetailTex_Tiling", Prop);
            MaterialProperty sync_DetailTex_Offset = FindProperty("__sync_DetailTex_Offset", Prop);
            Vector2          DetailTex_Tiling;
            Vector2          DetailTex_Offset;
            bool             bool_DetailTex_Tiling;
            bool             bool_DetailTex_Offset;

            MaterialProperty Use_VertColorAsMask  = FindProperty("_Use_VertColorAsMask", Prop);
            MaterialProperty GradMask             = FindProperty("_GradMask", Prop);
            MaterialProperty GradMask_Tiling_X    = FindProperty("_GradMask_Tiling_X", Prop);
            MaterialProperty GradMask_Tiling_Y    = FindProperty("_GradMask_Tiling_Y", Prop);
            MaterialProperty GradMask_Offset_X    = FindProperty("_GradMask_Offset_X", Prop);
            MaterialProperty GradMask_Offset_Y    = FindProperty("_GradMask_Offset_Y", Prop);
            MaterialProperty sync_GradMask_Tiling = FindProperty("__sync_GradMask_Tiling", Prop);
            MaterialProperty sync_GradMask_Offset = FindProperty("__sync_GradMask_Offset", Prop);
            Vector2          GradMask_Tiling;
            Vector2          GradMask_Offset;
            bool             bool_GradMask_Tiling;
            bool             bool_GradMask_Offset;

            if(GradMask.textureValue!=null || mat.GetInt("_Use_VertColorAsMask")==1){
                mat.SetInt("__masked", 1);
            }else{
                mat.SetInt("__masked", 0);
            }

            MaterialProperty GradMaskChannel     = FindProperty("_GradMaskChannel", Prop);
            MaterialProperty InvertGradMask      = FindProperty("_InvertGradMask", Prop);
            MaterialProperty LameMaskChannel     = FindProperty("_LameMaskChannel", Prop);
            MaterialProperty InvertLameMask      = FindProperty("_InvertLameMask", Prop);
            MaterialProperty GlitterMaskChannel  = FindProperty("_GlitterMaskChannel", Prop);
            MaterialProperty InvertGlitterMask   = FindProperty("_InvertGlitterMask", Prop);
            MaterialProperty EmissionMaskChannel = FindProperty("_EmissionMaskChannel", Prop);
            MaterialProperty InvertEmissionMask  = FindProperty("_InvertEmissionMask", Prop);
            MaterialProperty CarbonMaskChannel   = FindProperty("_CarbonMaskChannel", Prop);
            MaterialProperty InvertCarbonMask    = FindProperty("_InvertCarbonMask", Prop);
            MaterialProperty CCMaskChannel       = FindProperty("_CCMaskChannel", Prop);
            MaterialProperty InvertCCMask        = FindProperty("_InvertCCMask", Prop);
  
            MaterialProperty FresnelLevel  = FindProperty("_FresnelLevel", Prop);
            MaterialProperty FresnelPower  = FindProperty("_FresnelPower", Prop);
            MaterialProperty InvertFresnel = FindProperty("_InvertFresnel", Prop);
            
            MaterialProperty EmissiveBoost        = FindProperty("_EmissiveBoost", Prop);
            MaterialProperty BaseAffectByLightDir = FindProperty("_BaseAffectByLightDir", Prop);
            
            MaterialProperty BaseGlitterTex        = FindProperty("_BaseGlitterTex", Prop);
            MaterialProperty BaseGlitterTex_Tiling = FindProperty("_BaseGlitterTex_Tiling", Prop);
            MaterialProperty BaseGlitterLevel      = FindProperty("_BaseGlitterLevel", Prop);

            MaterialProperty Lame            = FindProperty("_Lame", Prop);
            MaterialProperty LameBackFace    = FindProperty("_LameBackFace", Prop);
            MaterialProperty LameChroma      = FindProperty("_LameChroma", Prop);
            MaterialProperty LameHue         = FindProperty("_LameHue", Prop);
            MaterialProperty LameAspect      = FindProperty("_LameAspect", Prop);
            MaterialProperty LameVolume      = FindProperty("_LameVolume", Prop);
            MaterialProperty LameDistance    = FindProperty("_LameDistance", Prop);
            MaterialProperty LameThinOut     = FindProperty("_LameThinOut", Prop);
            MaterialProperty LameMetallic    = FindProperty("_LameMetallic", Prop);
            MaterialProperty LameSmooth      = FindProperty("_LameSmooth", Prop);

            MaterialProperty Use_Carbon         = FindProperty("_Use_Carbon", Prop);
            MaterialProperty Carbon_Color1      = FindProperty("_Carbon_Color1", Prop);
            MaterialProperty Use_Carbon_Color1  = FindProperty("_Use_Carbon_Color1", Prop);
            MaterialProperty Carbon_Color2      = FindProperty("_Carbon_Color2", Prop);
            MaterialProperty Use_Carbon_Color2  = FindProperty("_Use_Carbon_Color2", Prop);
            MaterialProperty Carbon_Tiling      = FindProperty("_Carbon_Tiling", Prop);
            MaterialProperty Carbon_Detail      = FindProperty("_Carbon_Detail", Prop);
            MaterialProperty Carbon_Tex         = FindProperty("_Carbon_Tex", Prop);
            MaterialProperty Carbon_Normal      = FindProperty("_Carbon_Normal", Prop);
            MaterialProperty Carbon_Aspect      = FindProperty("_Carbon_Aspect", Prop);
            MaterialProperty Carbon_StripeAngle = FindProperty("_Carbon_StripeAngle", Prop);
            MaterialProperty Carbon_BumpScale   = FindProperty("_Carbon_BumpScale", Prop);

            // Localize -----------------------------
            String Loc_SwitchLanguage       = "N/A";
            String Loc_Lang                 = "N/A";
            String Loc_Albedo               = "N/A";
            String Loc_Detail               = "N/A";
            String Loc_Adjust               = "N/A";
            String Loc_DirectX              = "DirectX";
            String Loc_Level                = "N/A";
            String Loc_Power                = "N/A";
            String Loc_Invert               = "N/A";
            String Loc_FromDetailTex        = "N/A";
            String Loc_CullMode             = "N/A";
            String Loc_CullHelpBox          = "N/A";
            String Loc_MainTex              = "N/A";
            String Loc_MatCap               = "MatCap";
            String Loc_Tiling               = "Tiling";
            String Loc_Offset               = "Offset";
            String Loc_SyncXY               = "Sync X - Y";
            String Loc_Cutoff               = "N/A";
            String Loc_LameCutoff           = "N/A";
            String Loc_blending             = "N/A";
            String Loc_BlendSrc             = "Src";
            String Loc_BlendDst             = "Dst";
            String Loc_Use_VertColorAsMask  = "N/A";
            String Loc_Mask                 = "N/A";
            String Loc_MaskChannel          = "N/A";
            String Loc_LameMaskChannel      = "N/A";
            String Loc_GlitterMaskChannel   = "N/A";
            String Loc_EmissionMaskChannel  = "N/A";
            String Loc_CarbonMaskChannel    = "N/A";
            String Loc_CCMaskChannel        = "N/A";
            String Loc_FresnelLevel         = "N/A";
            String Loc_FresnelPower         = "N/A";
            String Loc_GradTex              = "N/A";
            String Loc_BlendMode            = "N/A";
            String Loc_BlendFactor          = "N/A";
            String Loc_Shift                = "N/A";
            String Loc_DetailTex            = "N/A";
            String Loc_ClearCoat            = "N/A";
            String Loc_Color                = "N/A";
            String Loc_Count                = "N/A";
            String Loc_SmoothChannel        = "N/A";
            String Loc_MetallicChannel      = "N/A";
            String Loc_Metallic             = "N/A";
            String Loc_NormalMap            = "N/A";
            String Loc_NormalMap2           = "N/A";
            String Loc_EmissiveBoost        = "N/A";
            String Loc_BaseAffectByLightDir = "N/A";
            String Loc_BaseGlitterTex       = "N/A";
            String Loc_Lame                 = "N/A";
            String Loc_BackFace             = "N/A";
            String Loc_LameCount            = "N/A";
            String Loc_Chroma               = "N/A";
            String Loc_Hue                  = "N/A";
            String Loc_Aspect               = "N/A";
            String Loc_Volume               = "N/A";
            String Loc_Distance             = "N/A";
            String Loc_ThinOut              = "N/A";
            String Loc_Carbon               = "N/A";
            String Loc_Carbon_Color1        = "N/A";
            String Loc_Carbon_Color2        = "N/A";
            String Loc_Carbon_Detail        = "N/A";
            String Loc_Carbon_Tex           = "N/A";
            String Loc_Carbon_Normal        = "N/A";
            String Loc_Carbon_StripeAngle   = "N/A";
            String Loc_Smooth               = "N/A";
            String Loc_ShaderName           = "N/A";
            // Japanese
            if(mat.GetInt("__Lang")==0){
                Loc_SwitchLanguage       = "Switch Language";
                Loc_Lang                 = "English";
                Loc_Albedo               = "色";
                Loc_Detail               = "ディテール";
                Loc_Adjust               = "調整";
                Loc_Level                = "レベル";
                Loc_Power                = "パワー";
                Loc_Invert               = "反転";
                Loc_FromDetailTex        = "ディテールテクスチャから";
                Loc_CullMode             = "カリングモード";
                Loc_CullHelpBox          = "　　Back : 表面のみ描画\n　　Off : 　両面描画";
                Loc_MainTex              = "テクスチャ";
                Loc_Cutoff               = "カットオフ閾値";
                Loc_LameCutoff           = "ラメのカットオフ閾値";
                Loc_blending             = "ブレンディング";
                Loc_Use_VertColorAsMask  = "頂点カラーを使用";
                Loc_Mask                 = "マスク";
                Loc_MaskChannel          = "グラデーションマスク";
                Loc_LameMaskChannel      = "ラメマスク";
                Loc_GlitterMaskChannel   = "ザラザラマスク";
                Loc_EmissionMaskChannel  = "エミッションマスク";
                Loc_CarbonMaskChannel    = "カーボンマスク";
                Loc_CCMaskChannel        = "クリアコートマスク";
                Loc_FresnelLevel         = "フレネルレベル";
                Loc_FresnelPower         = "フレネルパワー";
                Loc_GradTex              = "グラデーション";
                Loc_BlendMode            = "ブレンドモード";
                Loc_BlendFactor          = "ブレンド率";
                Loc_Shift                = "グラデーションシフト";
                Loc_DetailTex            = "ディテールテクスチャ";
                Loc_ClearCoat            = "クリアコート";
                Loc_Color                = "カラー";
                Loc_Count                = "厚さ";
                Loc_SmoothChannel        = "光沢-チャンネル";
                Loc_MetallicChannel      = "メタリック-チャンネル";
                Loc_Metallic             = "メタリック";
                Loc_NormalMap            = "ノーマルマップ";
                Loc_NormalMap2           = "ノーマルマップ 2";
                Loc_EmissiveBoost        = "エミッションブースト";
                Loc_BaseAffectByLightDir = "光源の影響率";
                Loc_BaseGlitterTex       = "ザラザラのテクスチャ";
                Loc_Lame                 = "ラメ";
                Loc_BackFace             = "裏面";
                Loc_LameCount            = "ラメの量";
                Loc_Chroma               = "彩度";
                Loc_Hue                  = "色相";
                Loc_Aspect               = "縦横比";
                Loc_Volume               = "ボリューム";
                Loc_Distance             = "間隙";
                Loc_ThinOut              = "まばら";
                Loc_Carbon               = "カーボンファイバー";
                Loc_Carbon_Color1        = "カーボン色 1";
                Loc_Carbon_Color2        = "カーボン色 2";
                Loc_Carbon_Detail        = "カーボンディテール";
                Loc_Carbon_Tex           = "カーボンテクスチャ";
                Loc_Carbon_Normal        = "カーボンノーマルマップ";
                Loc_Carbon_StripeAngle   = "カーボンアングル";
                Loc_Smooth               = "光沢";
                Loc_ShaderName           = "マチェイヨラ";
            // English
            }else if(mat.GetInt("__Lang")==1){
                Loc_SwitchLanguage       = "言語を変更";
                Loc_Lang                 = "日本語";
                Loc_Albedo               = "Color";
                Loc_Detail               = "Detail";
                Loc_Adjust               = "Adjust";
                Loc_Level                = "Level";
                Loc_Power                = "Power";
                Loc_Invert               = "Invert";
                Loc_FromDetailTex        = "From Detail Texture";
                Loc_CullMode             = "Culling Mode";
                Loc_CullHelpBox          = "　　Back : Drow only front faces\n　　Off : 　Drow double sided";
                Loc_MainTex              = "Main Texture";
                Loc_Cutoff               = "Cutoff - Threshold";
                Loc_LameCutoff           = "Sparkle Cutoff - Threshold";
                Loc_blending             = "Blending";
                Loc_Use_VertColorAsMask  = "Use Vertex Color";
                Loc_Mask                 = "Mask";
                Loc_MaskChannel          = "Gradation Mask";
                Loc_LameMaskChannel      = "Lame Mask";
                Loc_GlitterMaskChannel   = "Glitter Mask";
                Loc_EmissionMaskChannel  = "Emission Mask";
                Loc_CarbonMaskChannel    = "Carbon Mask";
                Loc_CCMaskChannel        = "Clear Coat Mask";
                Loc_FresnelLevel         = "Fresnel Level";
                Loc_FresnelPower         = "Fresnel Power";
                Loc_GradTex              = "Gradation";
                Loc_BlendMode            = "Blend Mode";
                Loc_BlendFactor          = "Blend Ratio";
                Loc_Shift                = "Gradation Shift";
                Loc_DetailTex            = "Detail Texture";
                Loc_ClearCoat            = "Clear coat";
                Loc_Color                = "Color";
                Loc_Count                = "Count";
                Loc_SmoothChannel        = "Gloss-Channel";
                Loc_MetallicChannel      = "Metallic-Channel";
                Loc_Metallic             = "Metallic";
                Loc_NormalMap            = "Normal Map";
                Loc_NormalMap2           = "Normal Map 2";
                Loc_EmissiveBoost        = "Emissive Boost";
                Loc_BaseAffectByLightDir = "Light direction effect";
                Loc_BaseGlitterTex       = "Glitter Texture";
                Loc_Lame                 = "Sparkle";
                Loc_BackFace             = "Back";
                Loc_LameCount            = "Sparkle Count";
                Loc_Chroma               = "Chroma";
                Loc_Hue                  = "Hue";
                Loc_Aspect               = "Aspect";
                Loc_Volume               = "Volume";
                Loc_Distance             = "Distance";
                Loc_ThinOut              = "Thin Out";
                Loc_Carbon               = "Carbon Fiber";
                Loc_Carbon_Color1        = "Carbon Color 1";
                Loc_Carbon_Color2        = "Carbon Color 2";
                Loc_Carbon_Detail        = "Carbon Detail";
                Loc_Carbon_Tex           = "Carbon Texture";
                Loc_Carbon_Normal        = "Carbon NormalMap";
                Loc_Carbon_StripeAngle   = "Carbon Stripe Angle";
                Loc_Smooth               = "Gloss";
                Loc_ShaderName           = "Macheiyora ";
            }

            // var defaultGradTex = AssetDatabase.LoadAssetAtPath<Texture>("Assets/すみすのやかた/マチェイヨラ/Gradation/メフィスト.png");
            var glitterTex = AssetDatabase.LoadAssetAtPath<Texture>("Assets/すみすのやかた/マチェイヨラ/Resource/Map/Glitter.jpg");
            var carbonTex = AssetDatabase.LoadAssetAtPath<Texture>("Assets/すみすのやかた/マチェイヨラ/Resource/Map/carbon_t.png");
            var carbonNorm = AssetDatabase.LoadAssetAtPath<Texture>("Assets/すみすのやかた/マチェイヨラ/Resource/Map/carbon_n.png");
            
            
            // if(GradTex.textureValue == null){
            //     mat.SetTexture("_GradTex", defaultGradTex);
            // }
            if(BaseGlitterTex.textureValue == null){
                mat.SetTexture("_BaseGlitterTex", glitterTex);
            }
            if(Carbon_Tex.textureValue == null){
                mat.SetTexture("_Carbon_Tex", carbonTex);
            }
            if(Carbon_Normal.textureValue == null){
                mat.SetTexture("_Carbon_Normal", carbonNorm);
            }

            // Property -----------------------------

            EditorGUIUtility.fieldWidth = 0;

            bool bool_Lang = Convert.ToBoolean(mat.GetInt("__Lang"));
            using (new EditorGUILayout.HorizontalScope())
            {
                GUILayout.Label(Loc_SwitchLanguage, EditorStyles.boldLabel);
                if (GUILayout.Button(Loc_Lang)) bool_Lang = !bool_Lang;

            }

            // About Macheiyora -----------------------------
            EditorGUILayout.BeginHorizontal();
            {
                drawButton("Booth", boothURL);
                drawButton("GitHub", gitHubURL);
                if(mat.GetInt("__Lang")==0) drawButton("Manual", manualURL);
                mat.SetInt("__Lang", Convert.ToInt32(bool_Lang));
            }
            EditorGUILayout.EndHorizontal();

            EditorGUI.indentLevel ++;

            // Albedo -----------------------------
            bool bool_Albedo_Foldout = Convert.ToBoolean(mat.GetInt("__Albedo_Foldout"));
            bool_Albedo_Foldout = Foldout(bool_Albedo_Foldout, Loc_Albedo);
            mat.SetInt("__Albedo_Foldout", Convert.ToInt32(bool_Albedo_Foldout));
            if(bool_Albedo_Foldout)
            {
                EditorGUILayout.BeginHorizontal();
                {
                    ME.TexturePropertySingleLine (new GUIContent(Loc_MainTex) , MainTex , Color);
                    bool bool_Is_MatCap = Convert.ToBoolean(mat.GetInt("_Is_MatCap"));
                    bool_Is_MatCap = EditorGUILayout.ToggleLeft(Loc_MatCap, bool_Is_MatCap, GUILayout.MaxWidth(85));
                    mat.SetInt("_Is_MatCap", Convert.ToInt32(bool_Is_MatCap));
                    if(bool_Is_MatCap){
                        mat.EnableKeyword("_IS_MATCAP_ON");
                    }else{
                        mat.DisableKeyword("_IS_MATCAP_ON");
                    }
                }
                EditorGUILayout.EndHorizontal();
                if(MainTex.textureValue != null)
                {
                    if(mat.GetInt("_Is_MatCap")==0)
                    {
                        MainTex_Tiling = new Vector2(mat.GetFloat("_MainTex_Tiling_X"), mat.GetFloat("_MainTex_Tiling_Y"));
                        using (new EditorGUILayout.HorizontalScope()){
                            EditorGUILayout.PrefixLabel (Loc_Tiling);
                            bool_MainTex_Tiling = Convert.ToBoolean(mat.GetInt("__sync_MainTex_Tiling"));
                            bool_MainTex_Tiling = EditorGUILayout.ToggleLeft (Loc_SyncXY, bool_MainTex_Tiling);
                            mat.SetInt("__sync_MainTex_Tiling", Convert.ToInt32(bool_MainTex_Tiling));
                        }
                        EditorGUI.indentLevel ++;
                        if(mat.GetFloat("__sync_MainTex_Tiling")==1){
                            MainTex_Tiling.x = EditorGUILayout.FloatField ("", MainTex_Tiling.x);
                        }else{
                            MainTex_Tiling = EditorGUILayout.Vector2Field ("", MainTex_Tiling);
                        }
                        EditorGUI.indentLevel --;
                        if(mat.GetFloat("__sync_MainTex_Tiling")==1) MainTex_Tiling.y = MainTex_Tiling.x;
                        mat.SetFloat("_MainTex_Tiling_X", MainTex_Tiling.x);
                        mat.SetFloat("_MainTex_Tiling_Y", MainTex_Tiling.y);

                        MainTex_Offset = new Vector2(mat.GetFloat("_MainTex_Offset_X"), mat.GetFloat("_MainTex_Offset_Y"));
                        using (new EditorGUILayout.HorizontalScope()){
                            EditorGUILayout.PrefixLabel (Loc_Offset);
                            bool_MainTex_Offset = Convert.ToBoolean(mat.GetInt("__sync_MainTex_Offset"));
                            bool_MainTex_Offset = EditorGUILayout.ToggleLeft (Loc_SyncXY, bool_MainTex_Offset);
                            mat.SetInt("__sync_MainTex_Offset", Convert.ToInt32(bool_MainTex_Offset));
                        }
                        EditorGUI.indentLevel ++;
                        if(mat.GetFloat("__sync_MainTex_Offset")==1){
                            MainTex_Offset.x = EditorGUILayout.FloatField ("", MainTex_Offset.x);
                        }else{
                            MainTex_Offset = EditorGUILayout.Vector2Field ("", MainTex_Offset);
                        }
                        EditorGUI.indentLevel --;
                        if(mat.GetFloat("__sync_MainTex_Offset")==1) MainTex_Offset.y = MainTex_Offset.x;
                        mat.SetFloat("_MainTex_Offset_X", MainTex_Offset.x);
                        mat.SetFloat("_MainTex_Offset_Y", MainTex_Offset.y);
                    }

                    if(isCutout){
                        ME.ShaderProperty(Cutoff, new GUIContent(Loc_Cutoff));
                        if (mat.GetInt("_Lame") >= 1) ME.ShaderProperty(LameCutoff, new GUIContent(Loc_LameCutoff));
                    }
                }
                if(isFade){
                    ME.ShaderProperty(blending, new GUIContent(Loc_blending));
                    switch(mat.GetInt("__blending"))
                    {
                        case 0:
                            mat.SetInt("_BlendSrc", 5);
                            mat.SetInt("_BlendDst", 10);
                            break;
                        case 1:
                            mat.SetInt("_BlendSrc", 1);
                            mat.SetInt("_BlendDst", 1);
                            break;
                        case 2:
                            mat.SetInt("_BlendSrc", 4);
                            mat.SetInt("_BlendDst", 1);
                            break;
                        case 3:
                            mat.SetInt("_BlendSrc", 5);
                            mat.SetInt("_BlendDst", 1);
                            break;
                        case 4:
                            ME.ShaderProperty(BlendSrc, new GUIContent("    " + Loc_BlendSrc));
                            ME.ShaderProperty(BlendDst, new GUIContent("    " + Loc_BlendDst));
                            break;
                    }
                }

                drawLine();

                using (new EditorGUILayout.HorizontalScope())
                {
                    ME.TexturePropertySingleLine (new GUIContent(Loc_GradTex), GradTex);
                    
                    bool bool_InvertGrad = Convert.ToBoolean(mat.GetInt("_InvertGrad"));
                    EditorGUI.indentLevel --;
                    bool_InvertGrad = EditorGUILayout.ToggleLeft (Loc_Invert, bool_InvertGrad, GUILayout.MaxWidth(52));
                    EditorGUI.indentLevel ++;
                    mat.SetInt("_InvertGrad", Convert.ToInt32(bool_InvertGrad));
                }
                if(mat.GetInt("_InvertGrad")==1) ME.ShaderProperty(InvertGradFromDetailTex, new GUIContent(Loc_FromDetailTex));
                ME.ShaderProperty(BlendMode, new GUIContent(Loc_BlendMode));
                ME.ShaderProperty(BlendFactor, new GUIContent(Loc_BlendFactor));
                ME.ShaderProperty(GradPower, new GUIContent(Loc_Power));
                ME.ShaderProperty(GradShift, new GUIContent(Loc_Shift));

                using (new EditorGUILayout.HorizontalScope())
                {
                    ME.ShaderProperty(FresnelLevel, new GUIContent(Loc_FresnelLevel));
                    bool bool_InvertFresnel = Convert.ToBoolean(mat.GetInt("_InvertFresnel"));
                    EditorGUI.indentLevel --;
                    bool_InvertFresnel = EditorGUILayout.ToggleLeft (Loc_Invert, bool_InvertFresnel, GUILayout.MaxWidth(52));
                    EditorGUI.indentLevel ++;
                    mat.SetInt("_InvertFresnel", Convert.ToInt32(bool_InvertFresnel));
                }
                ME.ShaderProperty(FresnelPower, new GUIContent(Loc_FresnelPower));

                drawLine();

                ME.ShaderProperty(EmissiveBoost, new GUIContent(Loc_EmissiveBoost));
                ME.ShaderProperty(BaseAffectByLightDir, new GUIContent(Loc_BaseAffectByLightDir));
            }

            // Mask -----------------------------
            bool bool_Mask_Foldout = Convert.ToBoolean(mat.GetInt("__Mask_Foldout"));
            bool_Mask_Foldout = Foldout(bool_Mask_Foldout, Loc_Mask);
            mat.SetInt("__Mask_Foldout", Convert.ToInt32(bool_Mask_Foldout));
            if(bool_Mask_Foldout)
            {
                using (new EditorGUILayout.HorizontalScope()){
                    ME.TexturePropertySingleLine (new GUIContent(Loc_Mask), GradMask);
                    bool bool_Use_VertColorAsMask = Convert.ToBoolean(mat.GetInt("_Use_VertColorAsMask"));
                    bool_Use_VertColorAsMask = EditorGUILayout.ToggleLeft(Loc_Use_VertColorAsMask, bool_Use_VertColorAsMask);
                    mat.SetInt("_Use_VertColorAsMask", Convert.ToInt32(bool_Use_VertColorAsMask));
                }
                if(GradMask.textureValue != null){
                        GradMask_Tiling = new Vector2(mat.GetFloat("_GradMask_Tiling_X"), mat.GetFloat("_GradMask_Tiling_Y"));
                        using (new EditorGUILayout.HorizontalScope()){
                            EditorGUILayout.PrefixLabel (Loc_Tiling);
                            bool_GradMask_Tiling = Convert.ToBoolean(mat.GetInt("__sync_GradMask_Tiling"));
                            bool_GradMask_Tiling = EditorGUILayout.ToggleLeft (Loc_SyncXY, bool_GradMask_Tiling);
                            mat.SetInt("__sync_GradMask_Tiling", Convert.ToInt32(bool_GradMask_Tiling));
                        }
                        EditorGUI.indentLevel ++;
                        if(mat.GetFloat("__sync_GradMask_Tiling")==1){
                            GradMask_Tiling.x = EditorGUILayout.FloatField ("", GradMask_Tiling.x);
                        }else{
                            GradMask_Tiling = EditorGUILayout.Vector2Field ("", GradMask_Tiling);
                        }
                        EditorGUI.indentLevel --;
                        if(mat.GetFloat("__sync_GradMask_Tiling")==1) GradMask_Tiling.y = GradMask_Tiling.x;
                        mat.SetFloat("_GradMask_Tiling_X", GradMask_Tiling.x);
                        mat.SetFloat("_GradMask_Tiling_Y", GradMask_Tiling.y);

                        GradMask_Offset = new Vector2(mat.GetFloat("_GradMask_Offset_X"), mat.GetFloat("_GradMask_Offset_Y"));
                        using (new EditorGUILayout.HorizontalScope()){
                            EditorGUILayout.PrefixLabel (Loc_Offset);
                            bool_GradMask_Offset = Convert.ToBoolean(mat.GetInt("__sync_GradMask_Offset"));
                            bool_GradMask_Offset = EditorGUILayout.ToggleLeft (Loc_SyncXY, bool_GradMask_Offset);
                            mat.SetInt("__sync_GradMask_Offset", Convert.ToInt32(bool_GradMask_Offset));
                        }
                        EditorGUI.indentLevel ++;
                        if(mat.GetFloat("__sync_GradMask_Offset")==1){
                            GradMask_Offset.x = EditorGUILayout.FloatField ("", GradMask_Offset.x);
                        }else{
                            GradMask_Offset = EditorGUILayout.Vector2Field ("", GradMask_Offset);
                        }
                        EditorGUI.indentLevel --;
                        if(mat.GetFloat("__sync_GradMask_Offset")==1) GradMask_Offset.y = GradMask_Offset.x;
                        mat.SetFloat("_GradMask_Offset_X", GradMask_Offset.x);
                        mat.SetFloat("_GradMask_Offset_Y", GradMask_Offset.y);

                    drawLine();
                }
                if(GradMask.textureValue != null || mat.GetInt("_Use_VertColorAsMask")==1){
                    using (new EditorGUILayout.HorizontalScope()){
                        ME.ShaderProperty(GradMaskChannel, new GUIContent(Loc_MaskChannel));
                        bool bool_InvertGradMask = Convert.ToBoolean(mat.GetInt("_InvertGradMask"));
                        bool_InvertGradMask = EditorGUILayout.ToggleLeft (Loc_Invert, bool_InvertGradMask, GUILayout.MaxWidth(70));
                        mat.SetInt("_InvertGradMask", Convert.ToInt32(bool_InvertGradMask));
                    }
                    using (new EditorGUILayout.HorizontalScope()){
                    ME.ShaderProperty(EmissionMaskChannel, new GUIContent(Loc_EmissionMaskChannel));
                        bool bool_InvertEmissionMask = Convert.ToBoolean(mat.GetInt("_InvertEmissionMask"));
                        bool_InvertEmissionMask = EditorGUILayout.ToggleLeft (Loc_Invert, bool_InvertEmissionMask, GUILayout.MaxWidth(70));
                        mat.SetInt("_InvertEmissionMask", Convert.ToInt32(bool_InvertEmissionMask));
                    }
                    using (new EditorGUILayout.HorizontalScope()){
                    ME.ShaderProperty(GlitterMaskChannel, new GUIContent(Loc_GlitterMaskChannel));
                        bool bool_InvertGlitterMask = Convert.ToBoolean(mat.GetInt("_InvertGlitterMask"));
                        bool_InvertGlitterMask = EditorGUILayout.ToggleLeft (Loc_Invert, bool_InvertGlitterMask, GUILayout.MaxWidth(70));
                        mat.SetInt("_InvertGlitterMask", Convert.ToInt32(bool_InvertGlitterMask));
                    }
                    using (new EditorGUILayout.HorizontalScope()){
                    ME.ShaderProperty(LameMaskChannel, new GUIContent(Loc_LameMaskChannel));
                        bool bool_InvertLameMask = Convert.ToBoolean(mat.GetInt("_InvertLameMask"));
                        bool_InvertLameMask = EditorGUILayout.ToggleLeft (Loc_Invert, bool_InvertLameMask, GUILayout.MaxWidth(70));
                        mat.SetInt("_InvertLameMask", Convert.ToInt32(bool_InvertLameMask));
                    }
                    using (new EditorGUILayout.HorizontalScope()){
                    ME.ShaderProperty(CCMaskChannel, new GUIContent(Loc_CCMaskChannel));
                        bool bool_InvertCCMask = Convert.ToBoolean(mat.GetInt("_InvertCCMask"));
                        bool_InvertCCMask = EditorGUILayout.ToggleLeft (Loc_Invert, bool_InvertCCMask, GUILayout.MaxWidth(70));
                        mat.SetInt("_InvertCCMask", Convert.ToInt32(bool_InvertCCMask));
                    }
                    using (new EditorGUILayout.HorizontalScope()){
                    ME.ShaderProperty(CarbonMaskChannel, new GUIContent(Loc_CarbonMaskChannel));
                        bool bool_InvertCarbonMask = Convert.ToBoolean(mat.GetInt("_InvertCarbonMask"));
                        bool_InvertCarbonMask = EditorGUILayout.ToggleLeft (Loc_Invert, bool_InvertCarbonMask, GUILayout.MaxWidth(70));
                        mat.SetInt("_InvertCarbonMask", Convert.ToInt32(bool_InvertCarbonMask));
                    }
                }else{
                    mat.SetInt("_InvertMask", 0);
                }
            }

            // Detail -----------------------------
            bool bool_Detail_Foldout = Convert.ToBoolean(mat.GetInt("__Detail_Foldout"));
            bool_Detail_Foldout = Foldout(bool_Detail_Foldout, Loc_Detail);
            mat.SetInt("__Detail_Foldout", Convert.ToInt32(bool_Detail_Foldout));
            if(bool_Detail_Foldout)
            {
                ME.TexturePropertySingleLine (new GUIContent(Loc_DetailTex) , DetailTex);
                if (DetailTex.textureValue != null) {
                    DetailTex_Tiling = new Vector2(mat.GetFloat("_DetailTex_Tiling_X"), mat.GetFloat("_DetailTex_Tiling_Y"));
                    using (new EditorGUILayout.HorizontalScope()){
                        EditorGUILayout.PrefixLabel (Loc_Tiling);
                        bool_DetailTex_Tiling = Convert.ToBoolean(mat.GetInt("__sync_DetailTex_Tiling"));
                        bool_DetailTex_Tiling = EditorGUILayout.ToggleLeft (Loc_SyncXY, bool_DetailTex_Tiling);
                        mat.SetInt("__sync_DetailTex_Tiling", Convert.ToInt32(bool_DetailTex_Tiling));
                    }
                    EditorGUI.indentLevel ++;
                    if(mat.GetFloat("__sync_DetailTex_Tiling")==1){
                        DetailTex_Tiling.x = EditorGUILayout.FloatField ("", DetailTex_Tiling.x);
                    }else{
                        DetailTex_Tiling = EditorGUILayout.Vector2Field ("", DetailTex_Tiling);
                    }
                    EditorGUI.indentLevel --;
                    if(mat.GetFloat("__sync_DetailTex_Tiling")==1) DetailTex_Tiling.y = DetailTex_Tiling.x;
                    mat.SetFloat("_DetailTex_Tiling_X", DetailTex_Tiling.x);
                    mat.SetFloat("_DetailTex_Tiling_Y", DetailTex_Tiling.y);

                    DetailTex_Offset = new Vector2(mat.GetFloat("_DetailTex_Offset_X"), mat.GetFloat("_DetailTex_Offset_Y"));
                    using (new EditorGUILayout.HorizontalScope()){
                        EditorGUILayout.PrefixLabel (Loc_Offset);
                        bool_DetailTex_Offset = Convert.ToBoolean(mat.GetInt("__sync_DetailTex_Offset"));
                        bool_DetailTex_Offset = EditorGUILayout.ToggleLeft (Loc_SyncXY, bool_DetailTex_Offset);
                        mat.SetInt("__sync_DetailTex_Offset", Convert.ToInt32(bool_DetailTex_Offset));
                    }
                    EditorGUI.indentLevel ++;
                    if(mat.GetFloat("__sync_DetailTex_Offset")==1){
                        DetailTex_Offset.x = EditorGUILayout.FloatField ("", DetailTex_Offset.x);
                    }else{
                        DetailTex_Offset = EditorGUILayout.Vector2Field ("", DetailTex_Offset);
                    }
                    EditorGUI.indentLevel --;
                    if(mat.GetFloat("__sync_DetailTex_Offset")==1) DetailTex_Offset.y = DetailTex_Offset.x;
                    mat.SetFloat("_DetailTex_Offset_X", DetailTex_Offset.x);
                    mat.SetFloat("_DetailTex_Offset_Y", DetailTex_Offset.y);
                }

                drawLine();

                if (DetailTex.textureValue != null) {
                    using (new EditorGUILayout.HorizontalScope())
                    {
                        ME.ShaderProperty(BSSmoothChannel, new GUIContent(Loc_SmoothChannel));
                        if (mat.GetInt("_BSSmoothChannel")!=4){
                            bool bool_BSSCInvert = Convert.ToBoolean(mat.GetInt("_BSSCInvert"));
                            bool_BSSCInvert = EditorGUILayout.ToggleLeft (Loc_Invert, bool_BSSCInvert, GUILayout.MaxWidth(70));
                            mat.SetInt("_BSSCInvert", Convert.ToInt32(bool_BSSCInvert));
                        }
                    }
                    if (mat.GetInt("_BSSmoothChannel")==4){
                        ME.ShaderProperty(BaseSurfSmooth, new GUIContent(Loc_Smooth));
                    }else{
                        ME.ShaderProperty(BSSmoothAdjust, new GUIContent(Loc_Adjust));
                    }
                    using (new EditorGUILayout.HorizontalScope())
                    {
                        ME.ShaderProperty(MetallicChannel, new GUIContent(Loc_MetallicChannel));
                        if (mat.GetInt("_MetallicChannel")!=4){
                            bool bool_MetallicCInvert = Convert.ToBoolean(mat.GetInt("_MetallicCInvert"));
                            bool_MetallicCInvert = EditorGUILayout.ToggleLeft (Loc_Invert, bool_MetallicCInvert, GUILayout.MaxWidth(70));
                            mat.SetInt("_MetallicCInvert", Convert.ToInt32(bool_MetallicCInvert));
                        }
                    }
                    if (mat.GetInt("_MetallicChannel")==4){
                        ME.ShaderProperty(Metallic, new GUIContent(Loc_Metallic));
                    }else{
                        ME.ShaderProperty(MetallicAdjust, new GUIContent(Loc_Adjust));
                    }
                }else{
                    mat.SetInt("_BSSmoothChannel", 4);
                    mat.SetInt("_MetallicChannel", 4);
                    ME.ShaderProperty(BaseSurfSmooth, new GUIContent(Loc_Smooth));
                    ME.ShaderProperty(Metallic, new GUIContent(Loc_Metallic));
                }

                drawLine();
                
                ME.TexturePropertySingleLine (new GUIContent(Loc_NormalMap) , NormalMap);
                if (NormalMap.textureValue != null) {
                        NormalMap_Tiling = new Vector2(mat.GetFloat("_NormalMap_Tiling_X"), mat.GetFloat("_NormalMap_Tiling_Y"));
                        using (new EditorGUILayout.HorizontalScope()){
                            EditorGUILayout.PrefixLabel (Loc_Tiling);
                            bool_NormalMap_Tiling = Convert.ToBoolean(mat.GetInt("__sync_NormalMap_Tiling"));
                            bool_NormalMap_Tiling = EditorGUILayout.ToggleLeft (Loc_SyncXY, bool_NormalMap_Tiling);
                            mat.SetInt("__sync_NormalMap_Tiling", Convert.ToInt32(bool_NormalMap_Tiling));
                        }
                        EditorGUI.indentLevel ++;
                        if(mat.GetFloat("__sync_NormalMap_Tiling")==1){
                            NormalMap_Tiling.x = EditorGUILayout.FloatField ("", NormalMap_Tiling.x);
                        }else{
                            NormalMap_Tiling = EditorGUILayout.Vector2Field ("", NormalMap_Tiling);
                        }
                        EditorGUI.indentLevel --;
                        if(mat.GetFloat("__sync_NormalMap_Tiling")==1) NormalMap_Tiling.y = NormalMap_Tiling.x;
                        mat.SetFloat("_NormalMap_Tiling_X", NormalMap_Tiling.x);
                        mat.SetFloat("_NormalMap_Tiling_Y", NormalMap_Tiling.y);

                        NormalMap_Offset = new Vector2(mat.GetFloat("_NormalMap_Offset_X"), mat.GetFloat("_NormalMap_Offset_Y"));
                        using (new EditorGUILayout.HorizontalScope()){
                            EditorGUILayout.PrefixLabel (Loc_Offset);
                            bool_NormalMap_Offset = Convert.ToBoolean(mat.GetInt("__sync_NormalMap_Offset"));
                            bool_NormalMap_Offset = EditorGUILayout.ToggleLeft (Loc_SyncXY, bool_NormalMap_Offset);
                            mat.SetInt("__sync_NormalMap_Offset", Convert.ToInt32(bool_NormalMap_Offset));
                        }
                        EditorGUI.indentLevel ++;
                        if(mat.GetFloat("__sync_NormalMap_Offset")==1){
                            NormalMap_Offset.x = EditorGUILayout.FloatField ("", NormalMap_Offset.x);
                        }else{
                            NormalMap_Offset = EditorGUILayout.Vector2Field ("", NormalMap_Offset);
                        }
                        EditorGUI.indentLevel --;
                        if(mat.GetFloat("__sync_NormalMap_Offset")==1) NormalMap_Offset.y = NormalMap_Offset.x;
                        mat.SetFloat("_NormalMap_Offset_X", NormalMap_Offset.x);
                        mat.SetFloat("_NormalMap_Offset_Y", NormalMap_Offset.y);

                    ME.ShaderProperty(DirectX, new GUIContent(Loc_DirectX));
                    ME.ShaderProperty(NormalLevel, new GUIContent(Loc_Level));

                    drawLine();
                }
                ME.TexturePropertySingleLine (new GUIContent(Loc_NormalMap2) , NormalMap2);
                if (NormalMap2.textureValue != null) {
                    NormalMap2_Tiling = new Vector2(mat.GetFloat("_NormalMap2_Tiling_X"), mat.GetFloat("_NormalMap2_Tiling_Y"));
                    using (new EditorGUILayout.HorizontalScope()){
                        EditorGUILayout.PrefixLabel (Loc_Tiling);
                        bool_NormalMap2_Tiling = Convert.ToBoolean(mat.GetInt("__sync_NormalMap2_Tiling"));
                        bool_NormalMap2_Tiling = EditorGUILayout.ToggleLeft (Loc_SyncXY, bool_NormalMap2_Tiling);
                        mat.SetInt("__sync_NormalMap2_Tiling", Convert.ToInt32(bool_NormalMap2_Tiling));
                    }
                    EditorGUI.indentLevel ++;
                    if(mat.GetFloat("__sync_NormalMap2_Tiling")==1){
                        NormalMap2_Tiling.x = EditorGUILayout.FloatField ("", NormalMap2_Tiling.x);
                    }else{
                        NormalMap2_Tiling = EditorGUILayout.Vector2Field ("", NormalMap2_Tiling);
                    }
                    EditorGUI.indentLevel --;
                    if(mat.GetFloat("__sync_NormalMap2_Tiling")==1) NormalMap2_Tiling.y = NormalMap2_Tiling.x;
                    mat.SetFloat("_NormalMap2_Tiling_X", NormalMap2_Tiling.x);
                    mat.SetFloat("_NormalMap2_Tiling_Y", NormalMap2_Tiling.y);

                    NormalMap2_Offset = new Vector2(mat.GetFloat("_NormalMap2_Offset_X"), mat.GetFloat("_NormalMap2_Offset_Y"));
                    using (new EditorGUILayout.HorizontalScope()){
                        EditorGUILayout.PrefixLabel (Loc_Offset);
                        bool_NormalMap2_Offset = Convert.ToBoolean(mat.GetInt("__sync_NormalMap2_Offset"));
                        bool_NormalMap2_Offset = EditorGUILayout.ToggleLeft (Loc_SyncXY, bool_NormalMap2_Offset);
                        mat.SetInt("__sync_NormalMap2_Offset", Convert.ToInt32(bool_NormalMap2_Offset));
                    }
                    EditorGUI.indentLevel ++;
                    if(mat.GetFloat("__sync_NormalMap2_Offset")==1){
                        NormalMap2_Offset.x = EditorGUILayout.FloatField ("", NormalMap2_Offset.x);
                    }else{
                        NormalMap2_Offset = EditorGUILayout.Vector2Field ("", NormalMap2_Offset);
                    }
                    EditorGUI.indentLevel --;
                    if(mat.GetFloat("__sync_NormalMap2_Offset")==1) NormalMap2_Offset.y = NormalMap2_Offset.x;
                    mat.SetFloat("_NormalMap2_Offset_X", NormalMap2_Offset.x);
                    mat.SetFloat("_NormalMap2_Offset_Y", NormalMap2_Offset.y);

                    ME.ShaderProperty(DirectX2, new GUIContent(Loc_DirectX));
                    ME.ShaderProperty(NormalLevel2, new GUIContent(Loc_Level));
                }

                drawLine();

                ME.TexturePropertySingleLine (new GUIContent(Loc_BaseGlitterTex), BaseGlitterTex);
                if(BaseGlitterTex.textureValue != null){
                    ME.ShaderProperty(BaseGlitterTex_Tiling, new GUIContent(Loc_Tiling));
                    ME.ShaderProperty(BaseGlitterLevel, new GUIContent(Loc_Level));
                }

            }

            // Clear Coat -----------------------------
            bool bool_ClearCoat_Foldout = Convert.ToBoolean(mat.GetInt("__ClearCoat_Foldout"));
            bool bool_ClearCoat = Convert.ToBoolean(mat.GetInt("_ClearCoat"));
            EditorGUILayout.BeginHorizontal();
            {
                bool_ClearCoat_Foldout = Foldout(bool_ClearCoat_Foldout, Loc_ClearCoat);
                bool_ClearCoat = EditorGUILayout.ToggleLeft("", bool_ClearCoat, GUILayout.MaxWidth(45));
            }
            EditorGUILayout.EndHorizontal();
            mat.SetInt("__ClearCoat_Foldout", Convert.ToInt32(bool_ClearCoat_Foldout));
            mat.SetInt("_ClearCoat", Convert.ToInt32(bool_ClearCoat));
            if(bool_ClearCoat){
                mat.EnableKeyword("_CLEARCOAT_ON");
            }else{
                mat.DisableKeyword("_CLEARCOAT_ON");
            }
            if(bool_ClearCoat_Foldout)
            {
                ME.TexturePropertySingleLine (new GUIContent(Loc_NormalMap), ClearCoatNormal);
                if (ClearCoatNormal.textureValue != null){
                    ClearCoatNormal_Tiling = new Vector2(mat.GetFloat("_ClearCoatNormal_Tiling_X"), mat.GetFloat("_ClearCoatNormal_Tiling_Y"));
                    using (new EditorGUILayout.HorizontalScope()){
                        EditorGUILayout.PrefixLabel (Loc_Tiling);
                        bool_ClearCoatNormal_Tiling = Convert.ToBoolean(mat.GetInt("__sync_ClearCoatNormal_Tiling"));
                        bool_ClearCoatNormal_Tiling = EditorGUILayout.ToggleLeft (Loc_SyncXY, bool_ClearCoatNormal_Tiling);
                        mat.SetInt("__sync_ClearCoatNormal_Tiling", Convert.ToInt32(bool_ClearCoatNormal_Tiling));
                    }
                    EditorGUI.indentLevel ++;
                    if(mat.GetFloat("__sync_ClearCoatNormal_Tiling")==1){
                        ClearCoatNormal_Tiling.x = EditorGUILayout.FloatField ("", ClearCoatNormal_Tiling.x);
                    }else{
                        ClearCoatNormal_Tiling = EditorGUILayout.Vector2Field ("", ClearCoatNormal_Tiling);
                    }
                    EditorGUI.indentLevel --;
                    if(mat.GetFloat("__sync_ClearCoatNormal_Tiling")==1) ClearCoatNormal_Tiling.y = ClearCoatNormal_Tiling.x;
                    mat.SetFloat("_ClearCoatNormal_Tiling_X", ClearCoatNormal_Tiling.x);
                    mat.SetFloat("_ClearCoatNormal_Tiling_Y", ClearCoatNormal_Tiling.y);

                    ClearCoatNormal_Offset = new Vector2(mat.GetFloat("_ClearCoatNormal_Offset_X"), mat.GetFloat("_ClearCoatNormal_Offset_Y"));
                    using (new EditorGUILayout.HorizontalScope()){
                        EditorGUILayout.PrefixLabel (Loc_Offset);
                        bool_ClearCoatNormal_Offset = Convert.ToBoolean(mat.GetInt("__sync_ClearCoatNormal_Offset"));
                        bool_ClearCoatNormal_Offset = EditorGUILayout.ToggleLeft (Loc_SyncXY, bool_ClearCoatNormal_Offset);
                        mat.SetInt("__sync_ClearCoatNormal_Offset", Convert.ToInt32(bool_ClearCoatNormal_Offset));
                    }
                    EditorGUI.indentLevel ++;
                    if(mat.GetFloat("__sync_ClearCoatNormal_Offset")==1){
                        ClearCoatNormal_Offset.x = EditorGUILayout.FloatField ("", ClearCoatNormal_Offset.x);
                    }else{
                        ClearCoatNormal_Offset = EditorGUILayout.Vector2Field ("", ClearCoatNormal_Offset);
                    }
                    EditorGUI.indentLevel --;
                    if(mat.GetFloat("__sync_ClearCoatNormal_Offset")==1) ClearCoatNormal_Offset.y = ClearCoatNormal_Offset.x;
                    mat.SetFloat("_ClearCoatNormal_Offset_X", ClearCoatNormal_Offset.x);
                    mat.SetFloat("_ClearCoatNormal_Offset_Y", ClearCoatNormal_Offset.y);

                    ME.ShaderProperty(ClearCoatNormalDirectX, new GUIContent(Loc_DirectX));
                    ME.ShaderProperty(ClearCoatNormalLevel, new GUIContent(Loc_Level));
                }
                ME.ShaderProperty(ClearCoatColor, new GUIContent(Loc_Color));
                ME.ShaderProperty(ClearCoatCount, new GUIContent(Loc_Count));
                if (DetailTex.textureValue != null) {
                    using (new EditorGUILayout.HorizontalScope())
                    {
                        ME.ShaderProperty(CCSmoothChannel, new GUIContent(Loc_SmoothChannel));
                        if (mat.GetInt("_CCSmoothChannel")!=4){
                            bool bool_CCSCInvert = Convert.ToBoolean(mat.GetInt("_CCSCInvert"));
                            bool_CCSCInvert = EditorGUILayout.ToggleLeft (Loc_Invert, bool_CCSCInvert, GUILayout.MaxWidth(70));
                            mat.SetInt("_CCSCInvert", Convert.ToInt32(bool_CCSCInvert));
                        }
                    }
                    if (mat.GetInt("_CCSmoothChannel")==4){
                        ME.ShaderProperty(ClearCoatSmooth, new GUIContent(Loc_Smooth));
                    }else{
                        ME.ShaderProperty(CCSmoothAdjust, new GUIContent(Loc_Adjust));
                    }
                }else{
                    mat.SetInt("_CCSmoothChannel", 4);
                    ME.ShaderProperty(ClearCoatSmooth, new GUIContent(Loc_Smooth));
                }
            }

            // Lame -----------------------------
            bool bool_Lame_Foldout = Convert.ToBoolean(mat.GetInt("__Lame_Foldout"));
            bool_Lame_Foldout = Foldout(bool_Lame_Foldout, Loc_Lame);
            mat.SetInt("__Lame_Foldout", Convert.ToInt32(bool_Lame_Foldout));
            if(bool_Lame_Foldout)
            {
                using (new EditorGUILayout.HorizontalScope())
                {
                    ME.ShaderProperty(Lame, new GUIContent(Loc_LameCount));
                    bool bool_LameBackFace = Convert.ToBoolean(mat.GetInt("_LameBackFace"));
                    bool_LameBackFace = EditorGUILayout.ToggleLeft (Loc_BackFace, bool_LameBackFace, GUILayout.MaxWidth(70));
                    mat.SetInt("_LameBackFace", Convert.ToInt32(bool_LameBackFace));
                }
                if (mat.GetInt("_Lame")>=1)
                {
                    ME.ShaderProperty(LameChroma, new GUIContent(Loc_Chroma));
                    ME.ShaderProperty(LameHue, new GUIContent(Loc_Hue));
                    ME.ShaderProperty(LameAspect, new GUIContent(Loc_Aspect));
                    ME.ShaderProperty(LameVolume, new GUIContent(Loc_Volume));
                    ME.ShaderProperty(LameDistance, new GUIContent(Loc_Distance));
                    ME.ShaderProperty(LameThinOut, new GUIContent(Loc_ThinOut));
                    ME.ShaderProperty(LameMetallic, new GUIContent(Loc_Metallic));
                    ME.ShaderProperty(LameSmooth, new GUIContent(Loc_Smooth));
                }
            }

            // Carbon -----------------------------
            bool bool_Carbon_Foldout = Convert.ToBoolean(mat.GetInt("__Carbon_Foldout"));
            bool bool_Carbon = Convert.ToBoolean(mat.GetInt("_Use_Carbon"));
            EditorGUILayout.BeginHorizontal();
            {
                bool_Carbon_Foldout = Foldout(bool_Carbon_Foldout, Loc_Carbon);
                bool_Carbon = EditorGUILayout.ToggleLeft("", bool_Carbon, GUILayout.MaxWidth(45));
            }
            EditorGUILayout.EndHorizontal();
            mat.SetInt("__Carbon_Foldout", Convert.ToInt32(bool_Carbon_Foldout));
            mat.SetInt("_Use_Carbon", Convert.ToInt32(bool_Carbon));
            if(bool_Carbon){
                mat.EnableKeyword("_USE_CARBON_ON");
            }else{
                mat.DisableKeyword("_USE_CARBON_ON");
            }
            if(bool_Carbon_Foldout)
            {
                using (new EditorGUILayout.HorizontalScope())
                {
                    ME.ShaderProperty(Use_Carbon_Color1, new GUIContent(Loc_Carbon_Color1));
                    if(mat.GetInt("_Use_Carbon_Color1")==1){
                        ME.ShaderProperty(Carbon_Color1, new GUIContent(""));
                    }
                }
                using (new EditorGUILayout.HorizontalScope())
                {
                    ME.ShaderProperty(Use_Carbon_Color2, new GUIContent(Loc_Carbon_Color2));
                    if(mat.GetInt("_Use_Carbon_Color2")==1){
                        ME.ShaderProperty(Carbon_Color2, new GUIContent(""));
                    }
                }
                ME.ShaderProperty(Carbon_Tiling, new GUIContent(Loc_Tiling));
                ME.ShaderProperty(Carbon_Aspect, new GUIContent(Loc_Aspect));
                ME.ShaderProperty(Carbon_StripeAngle, new GUIContent(Loc_Carbon_StripeAngle));
                ME.ShaderProperty(Carbon_Detail, new GUIContent(Loc_Carbon_Detail));
                ME.ShaderProperty(Carbon_BumpScale, new GUIContent(Loc_Level));
                using (new EditorGUILayout.HorizontalScope())
                {
                    ME.TexturePropertySingleLine (new GUIContent(Loc_Carbon_Tex), Carbon_Tex);
                    ME.TexturePropertySingleLine (new GUIContent(Loc_Carbon_Normal), Carbon_Normal);
                }
            } 

            // Advanced -----------------------------
            bool bool_Advanced_Foldout = Convert.ToBoolean(mat.GetInt("__Advanced_Foldout"));
            bool_Advanced_Foldout = Foldout(bool_Advanced_Foldout, "Advanced Settings");
            mat.SetInt("__Advanced_Foldout", Convert.ToInt32(bool_Advanced_Foldout));
            if(bool_Advanced_Foldout)
            {
                ME.ShaderProperty(CullMode, new GUIContent(Loc_CullMode));
                EditorGUILayout.HelpBox(Loc_CullHelpBox, MessageType.Info);

                drawLine();

                ME.RenderQueueField();
            }

            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            GUILayout.Label(Loc_ShaderName+" "+shaderVer, EditorStyles.boldLabel);
            EditorGUILayout.EndHorizontal();
        }
    }
}