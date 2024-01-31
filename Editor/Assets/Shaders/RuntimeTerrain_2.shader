Shader "Custom/RuntimeTerrain_2"
{
    Properties
    {
        _MainTex("MainTex", 2D) = "white" {}
        _DetailMaskMap("DetailMaskMap", 2D) = "white" {}
        _DetailRepeat0("DetailRepeat0", Float) = 0.0
        _DetailRepeat1("DetailRepeat1", Float) = 0.0
        _DetailColorMap0("DetailColorMap0", 2D) = "white" {}
        _DetailColorMap1("DetailColorMap1", 2D) = "white" {}
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
        float _DetailRepeat1;
        sampler2D _DetailColorMap0;
        sampler2D _DetailColorMap1;

        void surf(Input IN, inout SurfaceOutputStandard o)
        {
            float4 c = tex2D(_MainTex, IN.uv_MainTex);
            float4 mask = tex2D(_DetailMaskMap, IN.uv_MainTex);
            float4 detail0 = tex2D(_DetailColorMap0, IN.uv2_DetailMaskMap * _DetailRepeat0);
            float4 detail1 = tex2D(_DetailColorMap1, IN.uv2_DetailMaskMap * _DetailRepeat1);

            float4 r0 = lerp(detail0, detail1, mask.r);

            o.Albedo = r0.rgb;
        }

        ENDCG
    }
}
