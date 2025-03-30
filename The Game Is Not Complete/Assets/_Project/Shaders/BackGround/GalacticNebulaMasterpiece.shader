//Shader "Custom/GalacticNebulaMasterpiece" {
Shader "Custom/GalacticNebulaMasterpiece" {
    Properties {
        _MainTex ("Texture", 2D) = "white" {}
        _CoreColor ("Core Color", Color) = (0, 0.8, 1, 1) // Cyan core
        _RingColor ("Ring Color", Color) = (1, 0.2, 0.8, 1) // Pink rings
        _PulseSpeed ("Pulse Speed", Float) = 1.0
        _RingDensity ("Ring Density", Float) = 10.0
        _WaveAmplitude ("Wave Amplitude", Range(0, 0.5)) = 0.1
        _GlowIntensity ("Glow Intensity", Range(0, 5)) = 2.0
        _RotationSpeed ("Rotation Speed", Float) = 0.3
        _RingFadeDistance ("Ring Fade Distance", Range(0, 1)) = 0.7 // 70% radius limit
    }
    SubShader {
        Tags { "Queue"="Geometry" "RenderType"="Opaque" }
        LOD 100

        Pass {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _CoreColor;
            float4 _RingColor;
            float _PulseSpeed;
            float _RingDensity;
            float _WaveAmplitude;
            float _GlowIntensity;
            float _RotationSpeed;
            float _RingFadeDistance;

            // Rotate UVs around center
            float2 rotate(float2 uv, float angle) {
                float2 center = float2(0.5, 0.5);
                float2 delta = uv - center;
                float s = sin(angle);
                float c = cos(angle);
                return float2(c * delta.x - s * delta.y, s * delta.x + c * delta.y) + center;
            }

            v2f vert (appdata v) {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target {
                // Rotate UVs for orbiting effect
                float2 rotatedUV = rotate(i.uv, _Time.y * _RotationSpeed);

                // Distance from center
                float2 center = float2(0.5, 0.5);
                float dist = length(rotatedUV - center);

                // Radial waves with pulsing
                float wave = sin(dist * _RingDensity - _Time.y * _PulseSpeed) * _WaveAmplitude;
                float ringPattern = abs(wave) + (1.0 - dist);

                // Core glow (unaffected by fade)
                float coreGlow = 1.0 - smoothstep(0.0, 0.3, dist);
                fixed4 core = _CoreColor * coreGlow * _GlowIntensity;

                // Ring color with fade-out at 70% radius
                float ringMask = smoothstep(_RingFadeDistance, _RingFadeDistance - 0.1, dist); // Fade starts at 70%, ends at 60%
                float ringStrength = saturate(ringPattern * (sin(_Time.y * _PulseSpeed) * 0.5 + 0.5)) * ringMask;
                fixed4 rings = _RingColor * ringStrength * _GlowIntensity;

                // Combine
                fixed4 col = tex2D(_MainTex, i.uv);
                col.rgb = lerp(core.rgb, rings.rgb, ringStrength);

                // Fade edges to black beyond ring limit
                float edgeFade = smoothstep(_RingFadeDistance + 0.1, 1.0, dist);
                col.rgb *= (1.0 - edgeFade);

                return col;
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}