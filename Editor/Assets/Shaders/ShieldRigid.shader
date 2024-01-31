Shader "Custom/ShieldRigid"
{
    Properties
    {
        _Diffuse("Diffuse", 2D) = "white" {}
        _BumpMap1("BumpMap1", 2D) = "white" {}
        _BumpMap2("BumpMap2", 2D) = "white" {}
        _Fade("Fade", Float) = 0.0
        _OuterColor("OuterColor", Color) = (0, 0, 0, 0)
        _InnerColor("InnerColor", Color) = (0, 0, 0, 0)
        _BaseColor("BaseColor", Color) = (0, 0, 0, 0)
        _Bumpiness1("Bumpiness1", Float) = 0.0
        _Bumpiness2("Bumpiness2", Float) = 0.0
        _TexScrollX0("TexScrollX0", Float) = 0.0
        _TexScrollZ0("TexScrollZ0", Float) = 0.0
        _TexScrollX1("TexScrollX1", Float) = 0.0
        _TexScrollZ1("TexScrollZ1", Float) = 0.0
        _TexScrollX2("TexScrollX2", Float) = 0.0
        _TexScrollZ2("TexScrollZ2", Float) = 0.0
        _TexScale1("TexScale1", Float) = 0.0
        _TexScale2("TexScale2", Float) = 0.0
        _ZRange("ZRange", Float) = 0.0
        _Refraction("Refraction", Float) = 0.0
        _OuterFresnel("OuterFresnel", Float) = 0.0
        _OuterScale("OuterScale", Float) = 0.0
        _DoubleSided("DoubleSided", Integer) = 0
    }
    SubShader
    {
        Tags { "RenderType" = "Opaque" }
        LOD 200
        Cull Off

        CGPROGRAM

        #pragma target 3.0
        #pragma surface surf Standard fullforwardshadows addshadow

        sampler2D _Diffuse;

        struct Input
        {
            float2 uv_Diffuse;
        };

        void surf(Input IN, inout SurfaceOutputStandard o)
        {
            float4 c = tex2D(_Diffuse, IN.uv_Diffuse);
            o.Albedo = c.rgb;
        }

        ENDCG
    }
}