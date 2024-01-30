Shader "Custom/HologramRigid"
{
    Properties
    {
        _Diffuse("Diffuse", 2D) = "white" {}
        _Fade("Fade", Float) = 0.0
        _HorizontalPower("HorizontalPower", Float) = 0.0
        _HorizontalShift("HorizontalShift", Float) = 0.0
        _VTraceColor("VTraceColor", Color) = (0, 0, 0, 0)
        _VTraceSpeed("VTraceSpeed", Float) = 0.0
        _VTraceFrequency("VTraceFrequency", Float) = 0.0
        _ScanlineColor("ScanlineColor", Color) = (0, 0, 0, 0)
        _ScanlineSpeed("ScanlineSpeed", Float) = 0.0
        _ScanlineResolution("ScanlineResolution", Float) = 0.0
        _FallOffScale("FallOffScale", Float) = 0.0
        _FallOffPower("FallOffPower", Float) = 0.0
        _FallOff("FallOff", Float) = 0.0
        _TextureIntensityBoost("TextureIntensityBoost", Float) = 0.0
        _Opacity("Opacity", Float) = 0.0
        _TintSemantic("TintSemantic", Color) = (0, 0, 0, 0)
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

        struct Input
        {
            float2 uv_Diffuse;
        };

        void surf(Input IN, inout SurfaceOutputStandard o)
        {
            float4 c = tex2D(_Diffuse, IN.uv_Diffuse);
            o.Albedo = c.rgb;

            // TODO: Specular
        }

        ENDCG
    }
}