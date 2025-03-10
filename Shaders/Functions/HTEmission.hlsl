#ifndef HT_EMISSION_INCLUDED
#define HT_EMISSION_INCLUDED

half3 HTCalcEmissionColor(float2 uv, half3 originalColor)
{
    // TODO:
    // ・Keywordの整理(URP標準のKeywordとの兼ね合い)
    // ・originalColorとemissionColorのブレンド方法
    // ・SkyLightが入るとFactorRGBが意味をなさなくなるので解決法を考える

    half3 emissionColor = _EmissionColor;
#if defined(_HT_USE_EMISSION_MAP)
    emissionColor *= SAMPLE_TEXTURE2D(_EmissionMap, sampler_BaseMap, uv).rgb;
#endif

    emissionColor *= originalColor * half3(_EmissionFactorR, _EmissionFactorG, _EmissionFactorB);

    return emissionColor * _EmissionIntensity;
}

half3 HTCalcEmissionColorIntensity(half3 emissionColor)
{
    return emissionColor * _EmissionIntensity;
}

half3 HTOverrideEmissionColor(half3 originalColor, half3 emissionColor)
{
    // NOTE: _EmissionIntensityにあわせてEmissionColorにしてしまう。
    // こうするとより意図した色で発光させることが可能になる。
    return lerp(originalColor, emissionColor, saturate(_EmissionIntensity));
}

#endif
