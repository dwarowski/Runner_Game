Shader "Custom/VHSEffect"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _NoiseTex ("Noise Texture", 2D) = "white" {}
        _ScanlineSpeed ("Scanline Speed", Float) = 10.0
        _ScanlineIntensity ("Scanline Intensity", Range(0,1)) = 0.02
        _NoiseIntensity ("Noise Intensity", Range(0,1)) = 0.002
    }
    SubShader
    {
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

            sampler2D _MainTex;
            sampler2D _NoiseTex;
            float _ScanlineSpeed;
            float _ScanlineIntensity;
            float _NoiseIntensity;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // Шум
                float noise = tex2D(_NoiseTex, i.uv + _Time.x).r * _NoiseIntensity;

                // В фрагментном шейдере (frag):
                float jitter = (frac(sin(_Time.y * 10.0) * 1000.0) - 0.5) * 0.001; // Случайное смещение
                float2 jitterUV = i.uv + float2(jitter, 0);
                

                // Полосы (scanlines)
                float scanlines = sin(i.uv.y * _ScreenParams.y * 0.7 + _Time.y * _ScanlineSpeed) * _ScanlineIntensity;

                // Итоговый цвет
                fixed4 col = tex2D(_MainTex, jitterUV);
                col.rgb += noise + scanlines;

                return col;
            }
            ENDCG
        }
    }
}
