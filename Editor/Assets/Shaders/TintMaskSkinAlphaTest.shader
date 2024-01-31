Shader "Custom/TintMaskSkinAlphaTest"
{
    Properties
    {
        _Diffuse("Diffuse", 2D) = "white" {}
        _TintMask("TintMask", 2D) = "white" {}
        _ScrollV("ScrollV", Float) = 0.0
        _ScrollU("ScrollU", Float) = 0.0
        _Bias("Bias", Integer) = 0
        _DoubleSided("DoubleSided", Integer) = 0
        _FadeStencil("FadeStencil", Integer) = 0
        _TintSemantic("TintSemantic", Color) = (0, 0, 0, 0)
        _Glow("Glow", Float) = 0.0

        _Cutoff("Alpha Cutoff", Range(0, 1)) = 0.5
    }
    SubShader
    {
        Tags { "Queue" = "AlphaTest" "RenderType" = "TransparentCutout" }
        LOD 200
        Cull Off

        CGPROGRAM

        #pragma target 3.0
        #pragma surface surf Standard fullforwardshadows addshadow alphatest:_Cutoff

        sampler2D _Diffuse;
        sampler2D _TintMask;
        float4 _TintSemantic;

        struct Input
        {
            float2 uv_Diffuse;
        };

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            float4 c = tex2D(_Diffuse, IN.uv_Diffuse);
            o.Albedo = c.rgb;
            o.Alpha = c.a;
        }

        ENDCG
    }
}