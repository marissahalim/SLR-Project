Shader "Custom/TestShader" {
    Properties {
        _MainTex ("Base Texture (TIFF)", 2D) = "white" {}
        _ColorRamp ("Color Ramp", 2D) = "white" {}
    }

    SubShader {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        LOD 100

        Blend SrcAlpha OneMinusSrcAlpha
        Cull Off
        ZWrite Off

        Pass {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata_t {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            sampler2D _ColorRamp;

            v2f vert (appdata_t v) {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target {
                fixed4 tiffColor = tex2D(_MainTex, i.uv);

                // Respect transparency in TIFF
                if (tiffColor.a == 0) {
                    return fixed4(0, 0, 0, 0);
                }

                // Map tiff grayscale intensity to a position in the ColorRamp
                float grayscale = dot(tiffColor.rgb, float3(0.2989, 0.5870, 0.1140)); 
                fixed4 rampColor = tex2D(_ColorRamp, float2(grayscale, 0.5));

                // Return the ramp color with original alpha
                return fixed4(rampColor.rgb, tiffColor.a);
            }
            ENDCG
        }
    }
}
