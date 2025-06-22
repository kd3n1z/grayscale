Shader "Hidden/com.kd3n1z.grayscale/Blur"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _BlurRadius ("Blur Radius", Int) = 10
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
            float4 _MainTex_TexelSize;

            float4 frag(v2f i) : SV_Target
            {
                float4 color = float4(0, 0, 0, 0);
                int count = 0;

                for (int x = -_BlurRadius; x <= _BlurRadius; x++)
                {
                    for (int y = -_BlurRadius; y <= _BlurRadius; y++)
                    {
                        color += tex2D(_MainTex, clamp(i.uv + float2(x, y) * _MainTex_TexelSize.xy, 0.0, 1.0));
                        count++;
                    }
                }

                return color / count;
            }
            ENDCG
        }
    }
}