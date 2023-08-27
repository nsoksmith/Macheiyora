// ========== ========== ==========
//   Lame_surf.cginc
//      Author : にしおかすみす！
//      Twitter : @nsokSMITHdayo
// ========== ========== ==========
if(_LameBackFace==0) if(!(i.IsFacing>0)) discard;

// Texture Scale Offset
float2 custom_uv_MainTex   = i.custom_uv;
       custom_uv_MainTex  *= float2(_MainTex_Tiling_X , _MainTex_Tiling_Y);
       custom_uv_MainTex  += float2(_MainTex_Offset_X , _MainTex_Offset_Y);

float2 custom_uv_GradMask  = i.custom_uv;
       custom_uv_GradMask *= float2(_GradMask_Tiling_X, _GradMask_Tiling_Y);
       custom_uv_GradMask += float2(_GradMask_Offset_X, _GradMask_Offset_Y);

float2 custom_uv_Lame  = i.custom_uv;
       custom_uv_Lame *= aspect(_LameAspect);

// Mask
fixed4 maskVec  = lerp(tex2D(_GradMask, custom_uv_GradMask), i.vColor, _Use_VertColorAsMask);
float  lameMask = selectMask(maskVec, _LameMaskChannel, _InvertLameMask, __masked);

if(lameMask < 0.5) discard;

float2 value     = (custom_uv_Lame + uv_offset) * _LameVolume;
float3 noise     = voronoiNoise(value);
float  isBorder  = step(noise.z, _LameDistance*0.6);
float  isThinOut = step(rand1dTo1d(noise.y, 0.123), _LameThinOut-0.000000001);
if(isBorder)  discard;
if(isThinOut) discard;

#ifndef USING_DIRECTIONAL_LIGHT
    fixed3 lightDir = normalize(UnityWorldSpaceLightDir(i.worldPos));
#else
    fixed3 lightDir = _WorldSpaceLightPos0.xyz;
#endif

fixed4 n  = fixed4(0.5, 0.5, 1, 1);
       n += (fixed4(rand1dTo3d(noise.y),1)-0.5)*0.3;
o.Normal  = UnpackNormal(n);

// float3 wNorm = WorldNormalVector(i, o.Normal);
// half   NdotL = dot(wNorm, lightDir);
// float3 ref   = normalize(-lightDir + 2 * wNorm * NdotL);
// float3 toEye = normalize(_WorldSpaceCameraPos - i.worldPos);
// float  phong = saturate((dot(ref, toEye)+1)*0.5);

// float  fresnel = abs(dot(i.viewDir, o.Normal));
// float  reflHue = triWave(fresnel+(noise.y-0.5)*_LameRandomness);

// fixed3 c = HSVtoRGB(float3(lerp(_LameHue1,_LameHue2,reflHue),
//                            lerp(_LameChroma1,_LameChroma2,reflHue),
//                            1))*(1 + _LameGlow);

fixed3 c = HSVtoRGB(float3(lerp(_LameHue1,_LameHue2,noise.y),
                           lerp(_LameChroma1,_LameChroma2,noise.y),
                           1))*(1 + _LameGlow);

o.Albedo     = c;
o.Metallic   = _LameMetallic;
o.Smoothness = _LameSmooth;

o.Alpha      = tex2D(_MainTex, custom_uv_MainTex).a;