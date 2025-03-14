Shader "UI/GlowShader"
{
    Properties
    {
        _Color ("Color", Color) = (0,0,0,1) // Black circle
        _GlowColor ("Glow Color", Color) = (1,1,1,1) // White glow
        _GlowIntensity ("Glow Intensity", Float) = 5.0
        _GlowRange ("Glow Range", Range(0, 1)) = 0.2
    }
    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off
        Cull Off

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

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

            float4 _Color;
            float4 _GlowColor;
            float _GlowIntensity;
            float _GlowRange;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // Distance from center
                float2 center = float2(0.5, 0.5);
                float dist = distance(i.uv, center);

                // Black circle
                float circle = step(_GlowRange, dist); // 0 inside, 1 outside
                fixed4 baseCol = _Color * (1.0 - circle); // Black inside, transparent outside

                // Glow
                float glow = 1.0 - smoothstep(_GlowRange, _GlowRange * 2.0, dist);
                glow *= _GlowIntensity;
                fixed4 glowCol = _GlowColor * glow;

                // Combine
                fixed4 finalCol = baseCol + glowCol;
                finalCol.a = saturate(baseCol.a + glow);
                return finalCol;
            }
            ENDCG
        }
    }
}