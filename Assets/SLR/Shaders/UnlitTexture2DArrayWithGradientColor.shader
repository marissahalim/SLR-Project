Shader "Custom/Unlit Texture2D Array with Gradient Color" {
    Properties {
        _MainTex ("Texture (RGB)", 2DArray) = "" {}
        _GradientTex ("Gradient", 2D) = "white" {}
        _Slice ("Texture Slice", Int) = 0
    }
    
    SubShader {
        Tags { "RenderType"="Opaque" }
        LOD 100
    
        Pass {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 3.5
            #include "UnityCG.cginc"
    
            struct appdata_t {
                float4 vertex : POSITION;
                float2 texcoord : TEXCOORD0;
            };
    
            struct v2f {
                float4 vertex : SV_POSITION;
                float3 texcoord : TEXCOORD0;  // Modified to accommodate the z-component
            };
    
            UNITY_DECLARE_TEX2DARRAY(_MainTex);
            sampler2D _GradientTex;  // Declare this correctly for a 2D texture
            int _Slice;
    
            v2f vert (appdata_t v) {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.texcoord.xy = v.texcoord;
                o.texcoord.z = float(_Slice);  // Ensure the slice index is a float in the texture coordinates
                return o;
            }
    
            fixed4 frag (v2f i) : SV_Target {
                fixed4 texCol = UNITY_SAMPLE_TEX2DARRAY(_MainTex, i.texcoord);
                /*float grayscale = dot(texCol.rgb, float3(0.299, 0.587, 0.114));  // Convert RGB to grayscale
                fixed4 color = tex2D(_GradientTex, float2(grayscale, 0.5));  // Sample the gradient texture using grayscale value
    
                if (grayscale < 0.05) color.a = 0;  // Make nearly black pixels transparent
    
                return color;*/
                return texCol.g;

                //float value = clamp(texCol.r, 0.0, 365.0) / 365.0;
                //return value;

                //return texCol.r == -99;

                //return fixed4(1, 0, 0, 1);
            }
            ENDCG
        }
    }
    }
    