Shader "Custom/SimpleRigidNoShadow"
{
    Properties
    {
        _Diffuse("Diffuse", 2D) = "white" {}
        _ScrollV("ScrollV", Float) = 0.0
        _ScrollU("ScrollU", Float) = 0.0
        _Glow("Glow", Float) = 0.0
        _TintSemantic("TintSemantic", Color) = (0, 0, 0, 0)
        _FadeStencil("FadeStencil", Integer) = 0
        _DoubleSidedDefaultFalse("DoubleSidedDefaultFalse", Integer) = 0
    }
    SubShader
    {
        Tags { "RenderType" = "Opaque" }
        LOD 200
        Cull Off

        CGPROGRAM

        #pragma target 3.0
        #pragma surface surf Standard

        sampler2D _Diffuse;

        struct Input
        {
            float2 uv_Diffuse;
        };

        void surf(Input IN, inout SurfaceOutputStandard o)
        {
            float4 c = tex2D(_Diffuse, IN.uv_Diffuse);
            o.Albedo = c.rgb;

            o.Emission = c.rgb * tex2D(_Diffuse, IN.uv_Diffuse).a;
        }

        ENDCG
    }
}