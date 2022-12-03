// ========== ========== ==========
//   Maziora.cginc
//      Author : にしおかすみす！
//      Twitter : @nsokSMITHdayo
// ========== ========== ==========

#pragma shader_feature _ _IS_MATCAP_ON
#pragma shader_feature _ _USE_CARBON_ON

#include "Lighting.cginc"
#include "./Macheiyora.cginc"

sampler2D   _MainTex;
int         _Is_MatCap;
float           _MainTex_Tiling_X;
float           _MainTex_Tiling_Y;
float           _MainTex_Offset_X;
float           _MainTex_Offset_Y;
sampler2D   _GradMask;
float           _GradMask_Tiling_X;
float           _GradMask_Tiling_Y;
float           _GradMask_Offset_X;
float           _GradMask_Offset_Y;
sampler2D   _GradTex;
sampler2D   _DetailTex;
float           _DetailTex_Tiling_X;
float           _DetailTex_Tiling_Y;
float           _DetailTex_Offset_X;
float           _DetailTex_Offset_Y;
sampler2D   _NormalMap;
float           _NormalMap_Tiling_X;
float           _NormalMap_Tiling_Y;
float           _NormalMap_Offset_X;
float           _NormalMap_Offset_Y;
sampler2D   _NormalMap2;
float           _NormalMap2_Tiling_X;
float           _NormalMap2_Tiling_Y;
float           _NormalMap2_Offset_X;
float           _NormalMap2_Offset_Y;
sampler2D   _BaseGlitterTex;
float           _BaseGlitterTex_Tiling;

sampler2D   _Carbon_Tex;
sampler2D   _Carbon_Normal;
float       _Carbon_Tiling;

struct Input
{
                    float3 worldPos;
    nointerpolation float3 viewDir;
                    float2 custom_uv;
                    float3 worldNormal; INTERNAL_DATA
    nointerpolation float  IsFacing:VFACE;
    nointerpolation fixed4 vColor : COLOR;

    #ifdef _IS_MATCAP_ON
        nointerpolation float3 campos;
                        half3  normal;
                        half3  tangent;
                        half3  bitan;
        nointerpolation float3 vfront;
                        float  mirrorFlag;
                        float2 matcap_uv;
    #endif
};

fixed4 _Color;

int    _InvertGrad;
int    _BlendMode;
float  _BlendFactor;
float  _GradPower;
float  _GradShift;
int    _BSSmoothChannel;
int    _BSSCInvert;
float  _BSSmoothAdjust;
half   _BaseSurfSmooth;
int    _MetallicChannel;
int    _MetallicCInvert;
float  _MetallicAdjust;
half   _Metallic;
float  _NormalLevel;
int    _DirectX;
float  _NormalLevel2;
int    _DirectX2;

int    __masked;
int    _Use_VertColorAsMask;
int    _InvertGradMask;
int    _GradMaskChannel;
int    _InvertGlitterMask;
int    _GlitterMaskChannel;
int    _InvertEmissionMask;
int    _EmissionMaskChannel;
int    _InvertCarbonMask;
int    _CarbonMaskChannel;

float  _FresnelLevel;
float  _FresnelPower;
int    _InvertFresnel;

float  _EmissiveBoost;
float  _BaseAffectByLightDir;

float  _BaseGlitterLevel;

fixed4 _Carbon_Color1;
int    _Use_Carbon_Color1;
fixed4 _Carbon_Color2;
int    _Use_Carbon_Color2;
float  _Carbon_Detail;
float  _Carbon_Aspect;
int    _Carbon_StripeAngle;
float  _Carbon_BumpScale;

void vert (inout appdata_full v, out Input o)
{
    UNITY_INITIALIZE_OUTPUT(Input,o);
    o.custom_uv = v.texcoord.xy;
    
    #ifdef _IS_MATCAP_ON
        #ifdef USING_STEREO_MATRICES
            o.campos = (unity_StereoWorldSpaceCameraPos[0] + unity_StereoWorldSpaceCameraPos[1]) * 0.5f;
        #else
            o.campos = _WorldSpaceCameraPos;
        #endif

        o.normal = v.normal;
        o.tangent = v.tangent;
        o.bitan = cross(UnityObjectToWorldNormal(v.normal) , UnityObjectToWorldDir(v.tangent.xyz)) * v.tangent.w * unity_WorldTransformParams.w;

        float3 Matrix_V = UNITY_MATRIX_V[1].xyz;
        float3 CamRotY  = abs(UNITY_MATRIX_V._m21);
               CamRotY *= CamRotY * CamRotY;
               Matrix_V = lerp(float3(0.0f , 1.0f , 0.0f) , Matrix_V , CamRotY);
        o.vfront        = normalize(Matrix_V);

        // 鏡の中判定（右手座標系か、左手座標系かの判定）o.mirrorFlag = -1 なら鏡の中.
        float3 crossFwd = cross(UNITY_MATRIX_V[0], UNITY_MATRIX_V[1]);
        o.mirrorFlag    = dot(crossFwd, UNITY_MATRIX_V[2]) < 0 ? 1 : -1;
    #endif
}

void surf (Input i, inout SurfaceOutputStandard o)
{
    // Texture Scale Offset
    float2 custom_uv_MainTex         = i.custom_uv;
           custom_uv_MainTex        *= float2(_MainTex_Tiling_X       , _MainTex_Tiling_Y);
           custom_uv_MainTex        += float2(_MainTex_Offset_X       , _MainTex_Offset_Y);
    float2 custom_uv_GradMask        = i.custom_uv;
           custom_uv_GradMask       *= float2(_GradMask_Tiling_X      , _GradMask_Tiling_Y);
           custom_uv_GradMask       += float2(_GradMask_Offset_X      , _GradMask_Offset_Y);
    float2 custom_uv_DetailTex       = i.custom_uv;
           custom_uv_DetailTex      *= float2(_DetailTex_Tiling_X     , _DetailTex_Tiling_Y);
           custom_uv_DetailTex      += float2(_DetailTex_Offset_X     , _DetailTex_Offset_Y);
    float2 custom_uv_NormalMap       = i.custom_uv;
           custom_uv_NormalMap      *= float2(_NormalMap_Tiling_X     , _NormalMap_Tiling_Y);
           custom_uv_NormalMap      += float2(_NormalMap_Offset_X     , _NormalMap_Offset_Y);
    float2 custom_uv_NormalMap2      = i.custom_uv;
           custom_uv_NormalMap2     *= float2(_NormalMap2_Tiling_X    , _NormalMap2_Tiling_Y);
           custom_uv_NormalMap2     += float2(_NormalMap2_Offset_X    , _NormalMap2_Offset_Y);
    float2 custom_uv_BaseGlitterTex  = i.custom_uv * _BaseGlitterTex_Tiling;
    #ifdef _USE_CARBON_ON
        float2 custom_uv_CarbonFiber  = i.custom_uv * _Carbon_Tiling * 5;
               custom_uv_CarbonFiber *= aspect(_Carbon_Aspect);
        if(_Carbon_StripeAngle==1) custom_uv_CarbonFiber.y = 1 - custom_uv_CarbonFiber.y;
    #endif

    float3 lightDir = normalize(UnityWorldSpaceLightDir(i.worldPos));
    
    // Mask
    fixed4 maskVec  = lerp(tex2D(_GradMask, custom_uv_GradMask), i.vColor, _Use_VertColorAsMask);
    float  gradMask = selectMask(maskVec, _GradMaskChannel, _InvertGradMask, __masked);
    
    fixed4 n    = tex2D(_NormalMap, custom_uv_NormalMap);
           n.g  = lerp(n.g, 1 - n.g, _DirectX);
    fixed3 uN   = UnpackScaleNormal(n, _NormalLevel);

    fixed4 n2   = tex2D(_NormalMap2, custom_uv_NormalMap2);
           n2.g = lerp(n2.g, 1 - n2.g, _DirectX2);

    uN = BlendNormals(uN, UnpackScaleNormal(n2, _NormalLevel2));

    fixed carbon_mask = 0;
    #ifdef _USE_CARBON_ON
        carbon_mask = selectMask(maskVec, _CarbonMaskChannel, _InvertCarbonMask, __masked);
        fixed4 carbon_normal = tex2D(_Carbon_Normal, custom_uv_CarbonFiber);
        if(_Carbon_StripeAngle==1) carbon_normal.g = (carbon_normal.g -0.5) * -1 + 0.5;
        uN = BlendNormals(uN, UnpackScaleNormal(carbon_normal, _Carbon_BumpScale * carbon_mask));
    #endif

    fixed4 glitterNorm = tex2D(_BaseGlitterTex, custom_uv_BaseGlitterTex);
    uN = BlendNormals(uN, UnpackScaleNormal(glitterNorm, _BaseGlitterLevel * selectMask(maskVec, _GlitterMaskChannel, _InvertGlitterMask, __masked) * (1 - carbon_mask)));

    o.Normal = uN;

    float3 wNorm  = WorldNormalVector(i, o.Normal);
           wNorm  = (i.IsFacing>0) ? wNorm : -wNorm;
    half   NdotL  = dot(wNorm, lightDir);
    float3 ref    = normalize(-lightDir + 2 * wNorm * NdotL);
    float3 toEye  = normalize(_WorldSpaceCameraPos - i.worldPos);
    float  phong  = saturate((dot(ref, toEye)+1)*0.5);

    float fresnel = abs(dot(i.viewDir, o.Normal));

    float grad  = pow(saturate(fresnel), _GradPower) + _GradShift;
    float grad2 = pow(phong, _GradPower) + _GradShift;

    float2 uv  = float2(1 - grad, 0.5);
    float2 uv2 = float2(1 - grad2, 0.5);

    if(_InvertGrad == 1){
        uv  = float2(grad, 0.5);
        uv2 = float2(grad2, 0.5);
    }

    // MatCap
    #ifdef _IS_MATCAP_ON
        float3 View       = normalize(i.campos - i.worldPos);
        float3 MatCapV    = normalize(i.vfront - View * dot(View, i.vfront));
        float3 MatCapH    = normalize(cross(View , MatCapV));
        custom_uv_MainTex = float2(dot(MatCapH , wNorm), dot(MatCapV , wNorm)) * 0.5f + 0.5f;
    #endif
    
    fixed4 mainColor  = tex2D(_MainTex, custom_uv_MainTex) * _Color;
    fixed4 gradColor  = tex2D(_GradTex, uv);
    fixed4 gradColor2 = tex2D(_GradTex, uv2);
           gradColor2 = lerp(gradColor, gradColor2, phong);
    fixed4 finalGrad  = lerp(gradColor, gradColor2, _BaseAffectByLightDir);

    float blendFactor = _BlendFactor * gradMask;

    fresnel   = saturate(abs(fresnel) + (_FresnelLevel - 0.5)*2);
    fresnel   = sigmoid(fresnel, _FresnelPower);
    finalGrad = lerp(finalGrad, mainColor, lerp(fresnel, 1 - fresnel, _InvertFresnel));

    // fixed4 c = Tlerp(mainColor*finalGrad, mainColor, finalGrad, blendFactor);
    fixed4 c = multiLerp(mainColor, finalGrad, blendFactor, _BlendMode);

    #ifdef _USE_CARBON_ON
        fixed2 carbon               = tex2D(_Carbon_Tex, custom_uv_CarbonFiber).rg;
               carbon.r             = 1 - (1 - carbon.r) * _Carbon_Detail * carbon_mask;
        fixed4 carbon_color         = lerp(_Carbon_Color1, _Carbon_Color2, carbon.g) * carbon.r;
        fixed  carbon_color1_factor = (1 - carbon.g) * _Use_Carbon_Color1;
        fixed  carbon_color2_factor = carbon.g * _Use_Carbon_Color2;
        fixed  carbon_color_factor  = saturate(carbon_color1_factor + carbon_color2_factor);
               carbon_color_factor *= carbon_mask;

        c.rgb = lerp(c.rgb, carbon_color, carbon_color_factor) * carbon.r;
    #endif

    o.Albedo = c.rgb;

    float emissionMask = selectMask(maskVec, _EmissionMaskChannel, _InvertEmissionMask, __masked);
    o.Emission         = c.rgb * _EmissiveBoost * emissionMask;

    fixed4 detail = tex2D(_DetailTex, custom_uv_DetailTex);
    o.Smoothness  = selectChannel(detail, _BSSmoothAdjust, _BSSmoothChannel, _BaseSurfSmooth, _BSSCInvert);
    o.Metallic    = selectChannel(detail, _MetallicAdjust, _MetallicChannel, _Metallic, _MetallicCInvert);
    o.Alpha = mainColor.a;
}