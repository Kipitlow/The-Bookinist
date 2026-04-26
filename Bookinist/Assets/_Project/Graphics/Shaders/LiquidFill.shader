Shader "UI/LiquidFill"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Fill ("Fill", Range(0,1)) = 1
        _WaveStrength ("Wave Strength", Range(0,0.1)) = 0.02
        _WaveSpeed ("Wave Speed", Range(0,10)) = 2
        _Color ("Color", Color) = (1,1,1,1)
        _GlobalIntensity ("Global Intensity", Range(0,1)) = 1
        _HeightReduction ("Heigth Reduction", Range(1, 100)) = 1.8
    }

    SubShader
    {
        Tags
        {
            "Queue"="Transparent"
            "IgnoreProjector"="True"
            "RenderType"="Transparent"
            "CanUseSpriteAtlas"="True"
        }

        Cull Off
        Lighting Off
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile __ UNITY_UI_CLIP_RECT

            #include "UnityCG.cginc"
            #include "UnityUI.cginc"

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _ClipRect;

            float _Fill;
            float _WaveStrength;
            float _WaveSpeed;
            float _GlobalIntensity;
            float _HeightReduction;
            float4 _Color;

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
                float4 worldPosition : TEXCOORD1;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.worldPosition = v.vertex;
                return o;
            }

            float wave(float x, float time)
            {
                float t = time * _WaveSpeed;

                // base coordinate
                float uv = x * 10;

                // noise de phase (IMPORTANT)
                float phaseNoise = sin(x * 3.0 + t * 0.7) * 0.8;

                // sin principal avec phase instable
                float baseWave = sin(uv + t + phaseNoise);

                // second layer inversé (crée les "haut bas haut bas")
                float altWave = sin(uv * 1.7 - t * 1.3 + phaseNoise * 2.0);

                // micro chaos rapide
                float jitter = sin(uv * 5.0 + t * 3.0) * 0.2;

                float result =
                    (baseWave * 0.6 +
                     altWave * 0.3 +
                     jitter * 0.1);

                result = result / _HeightReduction; // réduction globale (ajustable)

                result = clamp(result, -1, 1);

                return result * _WaveStrength * _GlobalIntensity;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float2 uv = i.uv;

                // petit mouvement liquide vertical (optionnel mais OK)
                uv.y += sin(_Time.y * 2.0 + uv.x * 5.0) * 0.005;

                // 1. vague
                float w = wave(uv.x, _Time.y);

                // 2. fill propre (IMPORTANT)
                float fillLine = saturate(_Fill + w);

                // 3. masque
                float alphaMask = step(uv.y, fillLine);

                fixed4 col = tex2D(_MainTex, uv) * _Color;

                col.a *= alphaMask;

                // UI mask scrollview safe
                col.a *= UnityGet2DClipping(i.worldPosition.xy, _ClipRect);

                return col;
            }
            ENDCG
        }
    }
}