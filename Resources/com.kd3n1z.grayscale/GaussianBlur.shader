Shader "Hidden/com.kd3n1z.grayscale/GaussianBlur"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _BlurRadius ("Blur Radius", Int) = 5
        _Sigma ("Sigma (Blur Spread)", Float) = 3.0
    }
    SubShader
    {
        Cull Off
        ZWrite Off
        ZTest Always
        Blend Off

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

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            sampler2D _MainTex;
            int _BlurRadius;
            float _Sigma;
            float4 _MainTex_TexelSize;

            float gaussian(float x)
            {
                const float INV_SQRT_2PI = 0.39894228040143267793994605993438186847585863116493465766592582967065792589930183850125233390730693643030255886263518268551099195455583724299621273062550770634527058272049931756451634580753059725364273;
                return INV_SQRT_2PI * exp(-(x * x) / (2.0 * _Sigma * _Sigma)) / _Sigma;
            }

            float4 frag(v2f i) : SV_Target
            {
                float4 color = float4(0, 0, 0, 0);
                float totalWeight = 0.0;

                for (int x = -_BlurRadius; x <= _BlurRadius; x++)
                {
                    for (int y = -_BlurRadius; y <= _BlurRadius; y++)
                    {
                        float2 offset = float2(x, y) * _MainTex_TexelSize.xy;
                        float weight = gaussian(length(float2(x, y)));

                        color += tex2D(_MainTex, clamp(i.uv + offset, 0.0, 1.0)) * weight;
                        totalWeight += weight;
                    }
                }

                return color / totalWeight;
            }
            ENDCG
        }
    }
}