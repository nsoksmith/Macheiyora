// ========== ========== ==========
//   ClearCoat.cginc
//      Author : にしおかすみす！
//      Twitter : @nsokSMITHdayo
// ========== ========== ==========

#include "./Macheiyora.cginc"

sampler2D _DetailTex;
float     _DetailTex_Tiling_X;
float     _DetailTex_Tiling_Y;
float     _DetailTex_Offset_X;
float     _DetailTex_Offset_Y;

sampler2D _ClearCoatNormal;
float     _ClearCoatNormal_Tiling_X;
float     _ClearCoatNormal_Tiling_Y;
float     _ClearCoatNormal_Offset_X;
float     _ClearCoatNormal_Offset_Y;

sampler2D _GradMask;
float     _GradMask_Tiling_X;
float     _GradMask_Tiling_Y;
float     _GradMask_Offset_X;
float     _GradMask_Offset_Y;

int       _ClearCoat;
fixed4    _ClearCoatColor;
int       _ClearCoatCount;
int       _CCSmoothChannel;
int       _CCSCInvert;
float     _CCSmoothAdjust;
float     _ClearCoatSmooth;
int       _ClearCoatNormalDirectX;
float     _ClearCoatNormalLevel;
float     _ClearCoatNormalTiling;

int       __masked;
int       _Use_VertColorAsMask;
int       _InvertCCMask;
int       _CCMaskChannel;

struct Input
{
    float2  custom_uv;
    nointerpolation fixed4 vColor : COLOR;
};

void vert (inout appdata_full v, out Input o)
{
    UNITY_INITIALIZE_OUTPUT(Input,o);
    o.custom_uv = v.texcoord.xy;
}

void surf (Input i, inout SurfaceOutputStandard o)
{
    #ifdef _CLEARCOAT_ON

        // Texture Scale Offset
        float2 custom_uv_DetailTex  = i.custom_uv;
               custom_uv_DetailTex *= float2(_DetailTex_Tiling_X, _DetailTex_Tiling_Y);
               custom_uv_DetailTex += float2(_DetailTex_Offset_X, _DetailTex_Offset_Y);
               
        float2 custom_uv_ClearCoatNormal  = i.custom_uv;
               custom_uv_ClearCoatNormal *= float2(_ClearCoatNormal_Tiling_X, _ClearCoatNormal_Tiling_Y);
               custom_uv_ClearCoatNormal += float2(_ClearCoatNormal_Offset_X, _ClearCoatNormal_Offset_Y);

        float2 custom_uv_GradMask        = i.custom_uv;
               custom_uv_GradMask       *= float2(_GradMask_Tiling_X      , _GradMask_Tiling_Y);
               custom_uv_GradMask       += float2(_GradMask_Offset_X      , _GradMask_Offset_Y);
               
        // Mask
        fixed4 maskVec  = lerp(tex2D(_GradMask, custom_uv_GradMask), i.vColor, _Use_VertColorAsMask);
        float  CCMask = selectMask(maskVec, _CCMaskChannel, _InvertCCMask, __masked);
        if(CCMask < 0.5) discard;

        o.Albedo      = _ClearCoatColor;
        fixed4 detail = tex2D(_DetailTex, custom_uv_DetailTex);
        o.Smoothness  = selectChannel(detail, _CCSmoothAdjust, _CCSmoothChannel, _ClearCoatSmooth, _CCSCInvert);
        o.Metallic    = (_ClearCoatCount - 1) * 0.1;

        fixed4 n = tex2D(_ClearCoatNormal, custom_uv_ClearCoatNormal);
        if(_ClearCoatNormalDirectX==1) n.g = 1 - n.g;
        o.Normal = UnpackScaleNormal(n, _ClearCoatNormalLevel);
    #else
        discard;
    #endif
}