Shader "Custom/EnvSkin"
{
    Properties
    {
        _Diffuse("Diffuse", 2D) = "white" {}
        _Fresnel("Fresnel", Float) = 0.0
        _ChromaAbsorption("ChromaAbsorption", Float) = 0.0
        _Reflection("Reflection", Float) = 0.0
        _Bias("Bias", Integer) = 0
        _RimLighting("RimLighting", Float) = 0.0
        _TintSemantic("TintSemantic", Color) = (0, 0, 0, 0)
        _FadeStencil("FadeStencil", Integer) = 0
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
        float4 _TintSemantic;

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