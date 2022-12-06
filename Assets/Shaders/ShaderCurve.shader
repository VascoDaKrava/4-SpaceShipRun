Shader "Custom/UnlitTextureCurve"
{
    Properties
    {
        _curvePower("Curve Power", Float) = 10.0
    }

    SubShader{
        Pass {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            float _curvePower;

            // vertex input: position, UV
            struct appdata {
                float4 vertex : POSITION;
                float4 texcoord : TEXCOORD0;
            };

            struct v2f {
                float4 pos : SV_POSITION;
                float4 uv : TEXCOORD0;
            };

            // Vertex
            v2f vert(appdata_full v) {
                v2f o;
                v.vertex.y = -_curvePower * (0.5 - v.texcoord.x) * (0.5 - v.texcoord.x);
                //v.vertex.xyz += 5 * sin(v.normal * v.texcoord.x);
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = float4(v.texcoord.xy, 0, 0);
                return o;
            }

            // Color
            half4 frag(v2f i) : SV_Target {
                half4 c = frac(i.uv);
                if (any(saturate(i.uv) - i.uv))
                    c.b = 0.5;
                return c;
            }

            ENDCG
        }
    }
}