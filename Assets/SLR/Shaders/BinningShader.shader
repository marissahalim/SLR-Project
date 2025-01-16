Shader "Custom/BinningShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _ColorRamp ("Color Ramp", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" }
        LOD 100

        Pass
        {
            Blend SrcAlpha OneMinusSrcAlpha
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
            sampler2D _ColorRamp;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // Read the discrete value from the texture
                float value = tex2D(_MainTex, i.uv).r * 7.0 - 1.0; // Map [0,1] to [-1,6]

                // Transparency for values -1 and 0
                if (value == -1.0 || value == 0.0)
                {
                    return fixed4(0, 0, 0, 0); // Fully transparent
                }

                // Map discrete values to the color ramp
                float rampUV = (value + 1.0) / 7.0; // Normalize [-1,6] to [0,1]
                fixed4 colorFromRamp = tex2D(_ColorRamp, float2(rampUV, 0.5));
                colorFromRamp.a = 1.0; // Fully opaque for other values

                return colorFromRamp;
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}
