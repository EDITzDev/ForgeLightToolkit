Shader "Custom/DualTextureRigidAlpha"
{
    Properties
    {
        _Diffuse("Diffuse", 2D) = "white" {}
        _Diffuse2("Diffuse2", 2D) = "white" {}
        _Glow("Glow", Float) = 0.0
        _TextureClamp("TextureClamp", Integer) = 0
        _FadeStencil("FadeStencil", Integer) = 0
        _TintSemantic("TintSemantic", Color) = (0, 0, 0, 0)

        _Cutoff("Alpha Cutoff", Range(0, 1)) = 0.5
    }
    SubShader
    {
        Tags { "RenderType" = "TransparentCutout" }
        LOD 200
        Cull Front

        CGPROGRAM

        #pragma target 3.0
        #pragma surface surf Standard fullforwardshadows addshadow alphatest:_Cutoff

        sampler2D _Diffuse;
        sampler2D _Diffuse2;
        float4 _TintSemantic;

        struct Input
        {
            float2 uv_Diffuse;
            float2 uv_Diffuse2;
        };

        void surf(Input IN, inout SurfaceOutputStandard o)
        {
            float4 c = tex2D(_Diffuse, IN.uv_Diffuse);
            o.Albedo = c.rgb;
            o.Alpha = c.a;
        }

        ENDCG
    }
}