Shader "Custom/EdgeDetection"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

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

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float3x3 sobelX = float3x3(-1, 0, 1, -2, 0, 2, -1, 0, 1);
                float3x3 sobelY = float3x3(-1, -2, -1, 0, 0, 0, 1, 2, 1);
                float dx = 0.0;
                float dy = 0.0;
                float3 sample;

                for (int x = -1; x <= 1; x++)
                {
                    for (int y = -1; y <= 1; y++)
                    {
                        sample = tex2D(_MainTex, i.uv + float2(x, y) * 0.01).rgb;  // Adjust texture coordinate scaling as needed
                        dx += dot(sample, sobelX[x + 1][y + 1]);
                        dy += dot(sample, sobelY[x + 1][y + 1]);
                    }
                }

                float edge = length(float2(dx, dy));
                return fixed4(edge, edge, edge, 1 - step(0.1, edge));  // Highlight edges, make non-edges transparent
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}
