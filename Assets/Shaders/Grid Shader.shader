Shader "CustomRenderTexture/Grid Shader"
{
   Properties
    {
        _Line_Color ("Grid Color", Color) = (0.22, 0.22, 0.22, 1)
        _Tile_Color ("Tile Color", Color) = (0.137, 0.137, 0.137, 1)
        _GridSize ("Grid Size (Tiles per Unit)", Float) = 1.0
        _LineWidth ("Line Width", Float) = 0.05
        _CenterPos ("Center Position", Vector) = (0, 0, 0, 0)
        _Radius ("Visible Radius", Float) = 18
        _FadeWidth ("Fade Width", Float) = 1.0
    }

    SubShader
    {
        Tags { "Queue" = "Transparent" "RenderType" = "Transparent" }

        Pass
        {
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off
            Cull Off
            Lighting Off

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata_t
            {
                float4 vertex : POSITION;
                float3 worldPos : TEXCOORD0;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float3 worldPos : TEXCOORD0;
                UNITY_VERTEX_OUTPUT_STEREO
            };

            float4 _Line_Color;
            float4 _Tile_Color;
            float _GridSize;
            float _LineWidth;
            float4 _CenterPos;
            float _Radius;
            float _FadeWidth;

            v2f vert (appdata_t v)
            {
                UNITY_SETUP_INSTANCE_ID(v);
                v2f o;
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);

                o.vertex = UnityObjectToClipPos(v.vertex);
                o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(i);

                // --- Grid pattern ---
                float2 worldGrid = i.worldPos.xz * _GridSize;
                float2 grid = abs(frac(worldGrid) - 0.5);
                float gridLine = min(grid.x, grid.y);
                float lineMask = smoothstep(_LineWidth, 0.0, gridLine);

                // --- Circular mask ---
                float dist = distance(i.worldPos.xz, _CenterPos.xz);
                float insideCircle = 1.0 - smoothstep(_Radius - _FadeWidth, _Radius, dist);

                // --- Base colors ---
                float4 gridColor = lerp(_Tile_Color, _Line_Color, lineMask);
                float4 outsideColor = _Tile_Color;

                // --- Blend based on circle ---
                float4 finalColor = lerp(outsideColor, gridColor, insideCircle);

                return finalColor;
            }
            ENDCG
        }
    }
}
