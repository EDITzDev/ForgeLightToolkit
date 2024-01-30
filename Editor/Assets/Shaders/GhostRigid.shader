Shader "Custom/GhostRigid"
{
    Properties
    {
        _Diffuse("Diffuse", 2D) = "white" {}
        _TintSemantic("TintSemantic", Color) = (0, 0, 0, 0)
        _Opacity("Opacity", Float) = 0.0
        _FallOff("FallOff", Float) = 0.0
        _ZRange("ZRange", Float) = 0.0
        _Intensity("Intensity", Float) = 0.0
    }
    SubShader
    {
        Tags { "Queue" = "Transparent" "RenderType" = "Transparent" }
        LOD 200
        Cull Off

        CGPROGRAM

        #pragma target 3.0
        #pragma surface surf Standard fullforwardshadows alpha

        float _ZRange;
        float4 _TintSemantic;

        sampler2D _Diffuse;

        struct Input
        {
            float2 uv_Diffuse;
        };

        void surf(Input IN, inout SurfaceOutputStandard o)
        {
            float4 c = tex2D(_Diffuse, IN.uv_Diffuse);
            o.Albedo = c.rgb;
            o.Alpha = _ZRange;
        }

        ENDCG
    }
}