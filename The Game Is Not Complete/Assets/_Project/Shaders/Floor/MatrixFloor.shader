Shader "Custom/MatrixFloor"
{
    Properties
    {
        _MainColor ("Main Color", Color) = (0, 1, 0, 1) // Default green Matrix color
        _GridSize ("Grid Size", Range(1, 50)) = 10 // Controls grid density
        _GlowIntensity ("Glow Intensity", Range(0, 5)) = 1 // Glow strength
        _Speed ("Scroll Speed", Range(0, 10)) = 2 // Speed of the "rain" effect
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        #pragma surface surf Standard fullforwardshadows
        #pragma target 3.0

        struct Input
        {
            float2 uv_MainTex; // We won't use a texture, but Unity expects UVs
            float3 worldPos;   // For world-space effects
        };

        fixed4 _MainColor;
        float _GridSize;
        float _GlowIntensity;
        float _Speed;

        // Simple hash function for pseudo-randomness
        float hash(float2 p)
        {
            return frac(sin(dot(p, float2(127.1, 311.7))) * 43758.5453);
        }

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            // Create a grid pattern
            float2 grid = frac(IN.worldPos.xz * _GridSize);
            float2 gridID = floor(IN.worldPos.xz * _GridSize);

            // Randomize "rain" effect per grid cell
            float random = hash(gridID);
            float rain = frac(random + _Time.y * _Speed); // Scrolling effect over time

            // Define grid lines (sharp edges)
            float gridLines = step(0.95, grid.x) + step(0.95, grid.y);
            gridLines = saturate(gridLines);

            // Create the "Matrix" glow effect
            float glow = smoothstep(0.0, 0.5, rain) * (1.0 - smoothstep(0.5, 1.0, rain));
            glow *= _GlowIntensity;

            // Base color (dark floor)
            fixed4 baseColor = fixed4(0, 0.1, 0, 1); // Dark greenish tint

            // Combine effects
            fixed4 col = baseColor;
            col += _MainColor * glow; // Add glowing "rain"
            col += _MainColor * gridLines * 0.2; // Faint grid lines

            o.Albedo = col.rgb;
            o.Metallic = 0.0;
            o.Smoothness = 0.5;
            o.Emission = col.rgb * 0.5; // Slight emissive glow
            o.Alpha = 1.0;
        }
        ENDCG
    }
    FallBack "Diffuse"
}