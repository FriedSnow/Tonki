Shader "Custom/BlurGlassShader"
{
    Properties
    {
        _MainTex ("Base (RGB)", 2D) = "white" {}
        _BlurAmount ("Blur Amount", Range(0, 1)) = 0.5
    }
    SubShader
    {
        Tags { "Queue" = "Transparent" }
        LOD 200

        Pass
        {
            Cull Off
            ZWrite Off
            Blend SrcAlpha OneMinusSrcAlpha

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _MainTex;
            float _BlurAmount;
            float4 _MainTex_ST;

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            float4 frag (v2f i) : SV_Target
            {
                float2 uv = i.uv;
                float4 color = tex2D(_MainTex, uv);

                float3 result = color.rgb;
                float2 offsets[9] = {
                    float2(-1.0,  1.0), float2( 0.0,  1.0), float2( 1.0,  1.0),
                    float2(-1.0,  0.0), float2( 0.0,  0.0), float2( 1.0,  0.0),
                    float2(-1.0, -1.0), float2( 0.0, -1.0), float2( 1.0, -1.0)
                };

                for (int j = 0; j < 9; j++)
                {
                    result += tex2D(_MainTex, uv + offsets[j] * _BlurAmount).rgb;
                }

                result /= 10.0;

                return float4(result, color.a);
            }
            ENDCG
        }
    }
    Fallback "Diffuse"
}
