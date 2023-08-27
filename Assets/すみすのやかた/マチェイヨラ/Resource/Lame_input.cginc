// ========== ========== ==========
//   Lame_input.cginc
//      Author : にしおかすみす！
//      Twitter : @nsokSMITHdayo
// ========== ========== ==========

#include "Lighting.cginc"
#include "./Macheiyora.cginc"
#include "./Sparkle.cginc"

sampler2D _MainTex;
float     _MainTex_Tiling_X;
float     _MainTex_Tiling_Y;
float     _MainTex_Offset_X;
float     _MainTex_Offset_Y;

sampler2D _GradMask;
float     _GradMask_Tiling_X;
float     _GradMask_Tiling_Y;
float     _GradMask_Offset_X;
float     _GradMask_Offset_Y;
struct Input
{
    float2 custom_uv;
    float3 worldPos;
    float3 viewDir;
    float3 worldNormal; INTERNAL_DATA
    
    nointerpolation float  IsFacing:VFACE;
    nointerpolation fixed4 vColor : COLOR;
};
int   __masked;
int   _Use_VertColorAsMask;
int   _InvertLameMask;
int   _LameMaskChannel;
int   _LameBackFace;
float _LameChroma1;
float _LameChroma2;
float _LameHue1;
float _LameHue2;
float _LameAspect;
float _LameVolume;
float _LameDistance;
float _LameThinOut;
// float _LameRandomness;
float _LameMetallic;
float _LameSmooth;
float _LameGlow;
void vert (inout appdata_full v, out Input o)
{
    UNITY_INITIALIZE_OUTPUT(Input,o);
    o.custom_uv = v.texcoord.xy;
}