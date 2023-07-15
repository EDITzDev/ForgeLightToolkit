Shader "Custom/GlassRigid"
{
    Properties
    {
       _Diffuse("Diffuse", 2D) = "white" {}
       _Fresnel("Fresnel", Float) = 0.0
       _Reflection("Reflection", Float) = 0.0
       _ChromaAbsorption("ChromaAbsorption", Float) = 0.0
       _TintSemantic("TintSemantic", Color) = (0, 0, 0, 0)
    }
    SubShader
    {
        Tags { "RenderType" = "Transparent" }
        LOD 200
        Cull Off

        CGPROGRAM

        #pragma target 3.0
        #pragma surface surf Standard fullforwardshadows addshadow alpha

        sampler2D _Diffuse;

        struct Input
        {
            float2 uv_Diffuse;
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