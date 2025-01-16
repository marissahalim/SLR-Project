Shader "Custom/RippleEffect"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _RippleOrigin ("Ripple Origin", Vector) = (0.5, 0.5, 0, 0)
        _RippleMagnitude ("Magnitude", Float) = 0.1
        _RippleFrequency ("Frequency", Float) = 10.0
        _CustomTime ("Custom Time", Float) = 0.0  // Renamed from _Time to _CustomTime
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
            float4 _RippleOrigin;
            float _RippleMagnitude;
            float _RippleFrequency;
            float _CustomTime;  // Use the new variable name here

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float dist = distance(i.uv, _RippleOrigin.xy);
                float effect = sin(dist * _RippleFrequency - _CustomTime) * _RippleMagnitude / (dist + 1);  // Updated to use _CustomTime
                fixed4 col = tex2D(_MainTex, i.uv + effect);
                return col;
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}
