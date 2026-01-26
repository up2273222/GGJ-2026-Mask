Shader "Unlit/PostProcessShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
    }

    CGINCLUDE
     #include "UnityCG.cginc"

            struct meshdata
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
            float4 _MainTex_ST;

            v2f vert (meshdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }
    ENDCG

    SubShader
    {
        Tags { "RenderType"="Opaque" }


        Pass //GRAYSCALE 0
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            
            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);
                
                float grayscale = dot(col.xyz, float3(0.3,0.59,0.11));
                     col.x = grayscale;
                     col.y = grayscale;
                     col.z = grayscale;
                     col.w = 1;
                     return col;
                     
               // return col;
                return col.r;
            }
            ENDCG
        }

        Pass //VIGNETTE 1
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            
            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);
                
                float2 remapUV = (i.uv * 2) - 1;
                float circle = length(remapUV);
                return fixed4(circle.xxx,1);
                
                
               
                     
               
            }
            ENDCG


        }

    }
}
