Shader "Custom/ThermalVision"
{
    Properties
    {
        _Temperature("Temperature", Range(0, 100)) = 20
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        #pragma surface surf Standard fullforwardshadows

        struct Input
        {
            float3 worldPos;
        };

        float _Temperature;

        void surf(Input IN, inout SurfaceOutputStandard o)
        {
            float heat = _Temperature / 100.0;
            o.Albedo = lerp(float3(0, 0, 1), float3(1, 0, 0), heat);
        }
        ENDCG
    }
    FallBack "Diffuse"
}