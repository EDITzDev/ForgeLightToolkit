Shader "Custom/RuntimeTerrain_1"
{
    Properties
    {
        _MainTex("MainTex", 2D) = "white" {}
        _DetailMaskMap("DetailMaskMap", 2D) = "white" {}
        _DetailRepeat0("DetailRepeat0", Float) = 0.0
        _DetailColorMap0("DetailColorMap0", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "RenderType" = "Opaque" }
        LOD 200
        Cull Off

        CGPROGRAM

        #pragma target 3.0
        #pragma surface surf Standard fullforwardshadows addshadow

        struct Input
        {
            float2 uv_MainTex;
            float2 uv2_DetailMaskMap;
        };

        sampler2D _MainTex;
        sampler2D _DetailMaskMap;
        float _DetailRepeat0;
        sampler2D _DetailColorMap0;

        void surf(Input IN, inout SurfaceOutputStandard o)
        {
            float4 c = tex2D(_MainTex, IN.uv_MainTex);
            float4 mask = tex2D(_DetailMaskMap, IN.uv_MainTex);
            float4 r0 = tex2D(_DetailColorMap0, IN.uv2_DetailMaskMap * _DetailRepeat0);

            o.Albedo = r0.rgb;
        }

        ENDCG
    }
}
