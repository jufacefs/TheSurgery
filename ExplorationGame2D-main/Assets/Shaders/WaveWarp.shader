Shader "Custom/HeatWaveShader"
{
    Properties
    {
        _MainTex ("Main Texture", 2D) = "white" {}
        _DistortionTex ("Distortion Texture", 2D) = "white" {}
        _DistortionStrength ("Distortion Strength", Range(0, 0.1)) = 0.05
        _Speed ("Distortion Speed", Float) = 1.0
    }
    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
        LOD 100
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata_t {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            sampler2D _DistortionTex;
            float _DistortionStrength;
            float _Speed;

            v2f vert (appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            float2 GetDistortion(float2 uv)
            {
                // 使用 fmod 让纹理UV循环滚动
                float2 distortionUV = fmod(uv + _Speed * _Time.y, 1.0);
                float2 distortion = tex2D(_DistortionTex, distortionUV).rg * 2.0 - 1.0;
                return distortion * _DistortionStrength;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // 根据扰动纹理对 UV 偏移
                float2 distortedUV = i.uv + GetDistortion(i.uv);
                return tex2D(_MainTex, distortedUV);
            }
            ENDCG
        }
    }
}
