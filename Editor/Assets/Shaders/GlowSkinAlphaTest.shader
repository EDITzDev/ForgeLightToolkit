Shader "Custom/GlowSkinAlphaTest"
{
    Properties
    {
        _Diffuse("Diffuse", 2D) = "white" {}
        _GlowMap("GlowMap", 2D) = "white" {}
        _ScrollU("ScrollU", Float) = 0.0
        _ScrollV("ScrollV", Float) = 0.0
        _Bias("Bias", Integer) = 0
        _DoubleSided("DoubleSided", Integer) = 0
        _FadeStencil("FadeStencil", Integer) = 0
        _TintSemantic("TintSemantic", Color) = (0, 0, 0, 0)
        _Glow("Glow", Float) = 0.0

        _Cutoff("Alpha Cutoff", Range(0, 1)) = 0.5
    }
    SubShader
    {
        Tags { "Queue" = "AlphaTest" "RenderType" = "Opaque" }
        LOD 200
        Cull Off

        CGPROGRAM

        #pragma target 3.0
        #pragma surface surf Standard fullforwardshadows addshadow

        sampler2D _Diffuse;
        sampler2D _GlowMap;

        float _ScrollU;
        float _ScrollV;
        float _Glow;

        struct Input
        {
            float2 uv_Diffuse;
        };

        void surf(Input IN, inout SurfaceOutputStandard o)
        {
            float2 scroll = IN.uv_Diffuse;

            float scrollU = _ScrollU * _Time * 5.0;
            float scrollV = _ScrollV * _Time * 5.0;

            scroll += float2(scrollU, scrollV);

            float4 c = tex2D(_Diffuse, scroll);

            o.Albedo = c.rgb;
            o.Alpha = c.a;

            if (_Glow > 0)
                o.Emission = c.rgb * tex2D(_GlowMap, scroll);
        }

        ENDCG
    }
}