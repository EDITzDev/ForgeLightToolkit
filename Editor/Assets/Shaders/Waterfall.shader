Shader "Custom/Waterfall"
{
    Properties
    {
        _Glow("Glow", Float) = 0
        _EdgeFalloff("EdgeFalloff", Float) = 0
        _Divergence("Divergence", Float) = 0
        _FadeStencil("FadeStencil", Integer) = 0
        _ZRange("ZRange", Float) = 0

        _Texture1("Texture1", 2D) = "white" {}
        _Mask1("Mask1", 2D) = "white" {}
        _TexScale1("TexScale1", Float) = 0
        _MaskScale1("MaskScale1", Float) = 0
        _TexScroll1("TexScroll1", Float) = 0
        _MaskScroll1("MaskScroll1", Float) = 0

        _Texture2("Texture2", 2D) = "white" {}
        _Mask2("Mask2", 2D) = "white" {}
        _TexScale2("TexScale2", Float) = 0
        _MaskScale2("MaskScale2", Float) = 0
        _TexScroll2("TexScroll2", Float) = 0
        _MaskScroll2("MaskScroll2", Float) = 0
    }
    SubShader
    {
        Tags { "Queue" = "Transparent" "RenderType" = "Transparent" }
        LOD 200
        Cull Off

        CGPROGRAM

        #pragma target 3.0
        #pragma surface surf Standard fullforwardshadows alpha

            float _Glow;

        sampler2D _Texture1;
        sampler2D _Mask1;
        float _TexScale1;
        float _MaskScale1;
        float _TexScroll1;
        float _MaskScroll1;

        sampler2D _Texture2;
        sampler2D _Mask2;
        float _TexScale2;
        float _MaskScale2;
        float _TexScroll2;
        float _MaskScroll2;

        struct Input
        {
            float2 uv_Texture1;
        };

        void surf(Input IN, inout SurfaceOutputStandard o)
        {
            float2 texScroll1 = IN.uv_Texture1;
            float2 maskScroll1 = IN.uv_Texture1;

            float2 texScroll2 = IN.uv_Texture1;
            float2 maskScroll2 = IN.uv_Texture1;

            float texScrollY1 = _TexScroll1 * _Time * 5.0;
            float maskScrollY1 = _MaskScroll1 * _Time * 5.0;

            float texScrollY2 = _TexScroll2 * _Time * 5.0;
            float maskScrollY2 = _MaskScroll2 * _Time * 5.0;

            texScroll1.y -= texScrollY1;
            maskScroll1.y -= maskScrollY1;

            texScroll2.y -= texScrollY2;
            maskScroll2.y -= maskScrollY2;

            float4 c = tex2D(_Texture1, texScroll1 * _TexScale1);
            float4 c2 = tex2D(_Texture2, maskScroll1 * _TexScale2);

            float4 m = tex2D(_Mask1, texScroll2 * _MaskScale1);
            float4 m2 = tex2D(_Mask2, maskScroll2 * _MaskScale2);

            o.Albedo = lerp(c.rgb, c2.rgb, 0.5);
            o.Alpha = lerp(m.rgb, m.rgb, 0.5);
        }

        ENDCG
    }
}