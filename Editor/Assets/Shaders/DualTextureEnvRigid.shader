Shader "Custom/DualTextureEnvRigid"
{
    Properties
    {
        _Diffuse("Diffuse", 2D) = "white" {}
        _Diffuse2("Diffuse2", 2D) = "white" {}
        _ChromaAbsorption("ChromaAbsorption", Float) = 0.0
        _Fresnel("Fresnel", Float) = 0.0
        _Reflection("Reflection", Float) = 0.0
        _TextureClamp("TextureClamp", Integer) = 0
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
        sampler2D _Diffuse2;

        struct Input
        {
            float2 uv_Diffuse;
            float2 uv2_Diffuse2;
        };

        void surf(Input IN, inout SurfaceOutputStandard o)
        {
            float4 c = tex2D(_Diffuse, IN.uv_Diffuse);
            float4 c2 = tex2D(_Diffuse2, IN.uv2_Diffuse2);
            o.Albedo = lerp(c, c2, c2.a);
            o.Alpha = c;
        }

        ENDCG
    }
}