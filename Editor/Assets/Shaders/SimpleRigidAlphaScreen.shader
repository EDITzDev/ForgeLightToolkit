Shader "Custom/SimpleRigidAlphaScreen"
{
    Properties
    {
        _Diffuse("Diffuse", 2D) = "white" {}
        _Fade("ScrollV", Float) = 0.0
        _ScrollV("ScrollV", Float) = 0.0
        _ScrollU("ScrollU", Float) = 0.0
        _Glow("Glow", Float) = 0.0
        _DoubleSidedDefaultFalse("DoubleSidedDefaultFalse", Integer) = 0
    }
    SubShader
    {
        Tags { "Queue" = "AlphaTest" "RenderType" = "TransparentCutout" }
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

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            float4 c = tex2D (_Diffuse, IN.uv_Diffuse);
            o.Albedo = c.rgb;
            o.Alpha = c.a;
        }

        ENDCG
    }
}