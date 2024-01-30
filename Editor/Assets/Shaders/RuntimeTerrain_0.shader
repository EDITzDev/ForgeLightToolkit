Shader "Custom/RuntimeTerrain_0"
{
    Properties
    {
        _MainTex("MainTex", 2D) = "white" {}
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
        };

        sampler2D _MainTex;

        void surf(Input IN, inout SurfaceOutputStandard o)
        {
            float4 c = tex2D(_MainTex, IN.uv_MainTex);
            o.Albedo = c.rgb;
        }

        ENDCG
    }
}