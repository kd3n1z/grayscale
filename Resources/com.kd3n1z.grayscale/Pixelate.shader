Shader "Hidden/com.kd3n1z.grayscale/Pixelate"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _PixelSize ("Blur Radius", Int) = 10
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
            float4 _MainTex_TexelSize;
            int _PixelSize;

            float4 frag(v2f i) : SV_Target
            {
                float2 texSize = 1.0 / float2(_MainTex_TexelSize.x, _MainTex_TexelSize.y);
                fixed4 col = tex2D(_MainTex, floor(i.uv * texSize / _PixelSize) * _PixelSize / texSize);
                return col;
            }
            ENDCG
        }
    }
}