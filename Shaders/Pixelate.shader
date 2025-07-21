Shader "Grayscale/Sprite Pixelate"
{
    Properties
    {
        _MainTex("Texture", 2D) = "white" {}
        _Color("Tint", Color) = (1, 1, 1, 1)
        _PixelSize("Pixel Size", Float) = 10.0

        [HideInInspector] _RendererColor("RendererColor", Color) = (1,1,1,1)
        [HideInInspector] _Flip("Flip", Vector) = (1,1,1,1)
        [PerRendererData] _AlphaTex("External Alpha", 2D) = "white" {}
        [PerRendererData] _EnableExternalAlpha("Enable External Alpha", Float) = 0
    }

    SubShader
    {
        Tags
		{
			"Queue" = "Transparent"
			"IgnoreProjector" = "True"
			"RenderType" = "Transparent"
			"PreviewType" = "Plane"
			"CanUseSpriteAtlas" = "True"
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

            float4 _MainTex_TexelSize;
            float _PixelSize;

            fixed4 frag(v2f i) : SV_Target
            {
                float2 uv = floor(i.texcoord / (_PixelSize * _MainTex_TexelSize.xy)) * (_PixelSize * _MainTex_TexelSize.xy);
                return SampleSpriteTexture(uv) * i.color * _Color;
            }
            ENDCG
        }
    }
}
