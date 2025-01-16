Shader "Custom/GradientColorPickerWithTransparency"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _ColorRamp ("Color Ramp", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        LOD 100
        Blend SrcAlpha One // Additive blending mode

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
                fixed4 col = tex2D(_MainTex, i.uv);
                float grayscale = dot(col.rgb, float3(0.299, 0.587, 0.114)); // Luminance calculation
                fixed4 colorFromRamp = tex2D(_ColorRamp, float2(grayscale, 1.0)); // Fetch color from the color ramp
                
                // Making black completely transparent and apply color otherwise
                if (grayscale == -1) // Near black threshold
                    return fixed4(0, 0, 0, 0); // Fully transparent
                else
                    return fixed4(colorFromRamp.rgb, 1.0); // Full color with no transparency
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}
