Shader "Grayscale/Sprite Gaussian Blur"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Color("Tint", Color) = (1, 1,1 , 1)

        _BlurRadius ("Blur Radius", Int) = 5
        _Sigma ("Sigma (Blur Spread)", Float) = 3.0

        [HideInInspector] _Flip("Flip", Vector) = (1,1,1,1)
    }
    SubShader
    {
        Tags
        {
            "Queue"="Transparent"
            "IgnoreProjector"="True"
            "RenderType"="Transparent"
        }

        Lighting Off
        Cull Off
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex SpriteVert
            #pragma fragment frag
            #include "UnitySprites.cginc"

            int _BlurRadius;
            float _Sigma;
            float4 _MainTex_TexelSize;

            float gaussian(float x)
            {
                const float INV_SQRT_2PI =
                    0.39894228040143267793994605993438186847585863116493465766592582967065792589930183850125233390730693643030255886263518268551099195455583724299621273062550770634527058272049931756451634580753059725364273;
                return INV_SQRT_2PI * exp(-(x * x) / (2.0 * _Sigma * _Sigma)) / _Sigma;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                fixed4 color = fixed4(0, 0, 0, 0);
                float totalWeight = 0.0;

                for (int x = -_BlurRadius; x <= _BlurRadius; x++)
                {
                    for (int y = -_BlurRadius; y <= _BlurRadius; y++)
                    {
                        float2 offset = float2(x, y) * _MainTex_TexelSize.xy;
                        float weight = gaussian(length(float2(x, y)));

                        color += SampleSpriteTexture(clamp(i.texcoord + offset, 0.0, 1.0)) * i.color * weight;
                        totalWeight += weight;
                    }
                }

                return color / totalWeight;
            }
            ENDCG
        }
    }
}