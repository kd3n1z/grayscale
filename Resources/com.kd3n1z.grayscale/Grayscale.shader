Shader "Hidden/com.kd3n1z.grayscale/Grayscale"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _RedWeight ("Red Weight", Float) = 0.299
        _GreenWeight ("Green Weight", Float) = 0.587
        _BlueWeight ("Blue Weight", Float) = 0.114
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
            float _RedWeight;
            float _GreenWeight;
            float _BlueWeight;

            float4 frag(v2f i) : SV_Target
            {
                float4 color = tex2D(_MainTex, i.uv);
                float gray = dot(color.rgb, float3(_RedWeight, _GreenWeight, _BlueWeight));
                return float4(gray, gray, gray, color.a);
            }
            ENDCG
        }
    }
}