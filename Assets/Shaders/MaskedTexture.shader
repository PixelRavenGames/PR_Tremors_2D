// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "MaskedTexture"
{
   Properties
   {
      _MainTex ("Base (RGB)", 2D) = "white" {}
      _Mask ("Culling Mask", 2D) = "white" {}
      _Color ("Tint", Color) = (1,1,1,1)
      _Cutoff ("Alpha cutoff", Range (0,1)) = 0.1
   }
   SubShader
   {
      Tags
      {
            "Queue"="Transparent"
			"RenderType"="Transparent" 
			"PreviewType"="Plane"
      }
      Lighting Off
      ZWrite Off
      Blend SrcAlpha OneMinusSrcAlpha
      AlphaTest GEqual [_Cutoff]
      Pass
      {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 3.0
            
			fixed4 _Color;

            // note: no SV_POSITION in this struct
            struct v2f {
                float2 uv : TEXCOORD0;
				fixed4 color : COLOR;
            };

            v2f vert (
                float4 vertex : POSITION, // vertex position input
                float2 uv : TEXCOORD0, // texture coordinate input
				float4 color : COLOR,
                out float4 outpos : SV_POSITION // clip space position output
                )
            {
                v2f o;
                o.uv = uv;
                o.color = color * _Color;
                outpos = UnityObjectToClipPos(vertex);
                return o;
            }

            sampler2D _MainTex;
            sampler2D _Mask;

			fixed4 SampleSpriteTexture (float2 uv)
			{
				fixed4 color = tex2D (_MainTex, uv);

#if UNITY_TEXTURE_ALPHASPLIT_ALLOWED
				if (_AlphaSplitEnabled)
					color.a = tex2D (_AlphaTex, uv).r;
#endif //UNITY_TEXTURE_ALPHASPLIT_ALLOWED

				return color;
			}

            fixed4 frag (v2f i, UNITY_VPOS_TYPE screenPos : VPOS) : SV_Target
            {
                // screenPos.xy will contain pixel integer coordinates.
                // use them to implement a checkerboard pattern that skips rendering
                // 4x4 blocks of pixels

                // checker value will be negative for 4x4 blocks of pixels
                // in a checkerboard pattern
                screenPos.xy = floor(screenPos.xy * 0.25) * 0.5;
                float checker = -frac(screenPos.r + screenPos.g);
        
                // clip HLSL instruction stops rendering a pixel if value is negative
                clip(tex2D(_Mask, i.uv).r - 0.5);

                // for pixels that were kept, read the texture and output it
                fixed4 c = SampleSpriteTexture (i.uv) * i.color;
				c.rgb *= c.a;
				return c;
            }
            ENDCG
 
      }
   }
}
