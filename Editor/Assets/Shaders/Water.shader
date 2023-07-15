Shader "Custom/Water"
{
    Properties
    {
        _Glow("Glow", Float) = 0
        _Density("Density", Float) = 0
        _Fresnel("Fresnel", Float) = 0
        _Refraction("Refraction", Float) = 0
        _Reflection("Reflection", Float) = 0
        _TintSemantic("TintSemantic", Color) = (0, 0, 0, 0)

        _SunGlare("SunGlare", Float) = 0

        _TexScale1("TexScale1", Float) = 0
        _Bumpiness1("Bumpiness1", Float) = 0
        _TexScrollX1("TexScrollX1", Float) = 0
        _TexScrollZ1("TexScrollZ1", Float) = 0
        _BumpMap1("BumpMap1", 2D) = "white" {}

        _TexScale2("TexScale2", Float) = 0
        _Bumpiness2("Bumpiness2", Float) = 0
        _TexScrollX2("TexScrollX2", Float) = 0
        _TexScrollZ2("TexScrollZ2", Float) = 0
        _BumpMap2("BumpMap2", 2D) = "white" {}

        _TexScale3("TexScale3", Float) = 0
        _Bumpiness3("Bumpiness3", Float) = 0
        _TexScrollX3("TexScrollX3", Float) = 0
        _TexScrollZ3("TexScrollZ3", Float) = 0
        _BumpMap3("BumpMap3", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "Queue" = "Transparent" "RenderType" = "Transparent" }
        LOD 200
        Cull Off

        CGPROGRAM

        #pragma target 3.0
        #pragma surface surf Standard alpha

        float4 _TintSemantic;

        float _TexScale1;
        float _TexScrollX1;
        float _TexScrollZ1;
        sampler2D _BumpMap1;

        float _TexScale2;
        float _TexScrollX2;
        float _TexScrollZ2;
        sampler2D _BumpMap2;

        float _TexScale3;
        float _TexScrollX3;
        float _TexScrollZ3;
        sampler2D _BumpMap3;

        struct Input
        {
            float2 uv_BumpMap1;
        };

        void surf(Input IN, inout SurfaceOutputStandard o)
        {
            float2 texScroll1 = IN.uv_BumpMap1;
            float2 texScroll2 = IN.uv_BumpMap1;
            float2 texScroll3 = IN.uv_BumpMap1;

            float texScrollX1 = _TexScrollX1 * _Time * 5.0;
            float texScrollZ1 = _TexScrollZ1 * _Time * 5.0;

            float texScrollX2 = _TexScrollX2 * _Time * 5.0;
            float texScrollZ2 = _TexScrollZ2 * _Time * 5.0;

            float texScrollX3 = _TexScrollX3 * _Time * 5.0;
            float texScrollZ3 = _TexScrollZ3 * _Time * 5.0;

            texScroll1 += float2(texScrollX1, texScrollZ1);
            texScroll2 += float2(texScrollX2, texScrollZ2);
            texScroll3 += float2(texScrollX3, texScrollZ3);

            float4 n1 = tex2D(_BumpMap1, texScroll1);
            float4 n2 = tex2D(_BumpMap2, texScroll2);
            float4 n3 = tex2D(_BumpMap3, texScroll3);

            float3 b1 = UnpackNormal(n1);
            float3 b2 = UnpackNormal(n2);
            float3 b3 = UnpackNormal(n3);

            o.Normal = b1;

            o.Albedo = _TintSemantic.rgb;
            o.Alpha = 0.5;
        }

        ENDCG
    }
}