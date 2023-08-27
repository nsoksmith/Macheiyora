// ========== ========== ==========
//   Macheiyora.cginc
//      Author : にしおかすみす！
//      Twitter : @nsokSMITHdayo
// ========== ========== ==========

float2 aspect(float ratio)
{
    float X = lerp(0, 1, ratio * 2);
    float Y = lerp(1, 0, (ratio - 0.5) * 2);
    return lerp(float2(X, 1), float2(1, Y), step(0.5, ratio));
}

float4 multiLerp(float4 mainColor, float4 blendColor, float factor, int mode)
{
    switch(mode)
    {
        case 0: // Mix
        return lerp(mainColor, blendColor, factor);
        break;
        case 1: // Multiply
        return lerp(mainColor, mainColor * blendColor, factor);
        break;
        default: // Additive
        return mainColor + blendColor * factor;
    }
}

float selectMask(fixed4 tex, int channel, int inv, int masked)
{
    float mask = 1;
    if (channel < 4)
    {
        mask = tex[channel];
        // inversion
        mask = lerp(mask, 1 - mask, inv);
    }
    return lerp(1, mask, masked);
}

float selectChannel(fixed4 tex, float adjust, int channel, float val, int inv)
{
    float value = val;
    if (channel < 4)
    {
        value = tex[channel];
        // inversion
        value = lerp(value, 1 - value, inv);
        value *= adjust;
    }
    return value;
}

float sigmoid(float value, float factor)
{
    return smoothstep(factor * 0.5, (1 - factor) * 0.5 + 0.5, value);
}

float triWave(float x)
{
    return abs(2*frac(x)-1);
}