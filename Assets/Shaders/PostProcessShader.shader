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


        Pass //TEMPERATURE 0
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            
            float _temperature;
            
            
            float3 KtoRGB(float _temperatureK)
                {
                float red,green,blue;
                float3 _outCol;
                
                float t = _temperatureK / 100.0;
                
                //RED
                if (t <= 66)
                {
                 red = 255;
                }
                else
                {
                    red = (329.698727446 * (pow(t - 60, -0.1332047592)));
			        red = clamp(red,0,255);
                }
                
                //GREEN
                if (t <= 66)
                {
                   green = (99.4708025861 * log(t) - 161.1195681661);
                }
                else
                {
                    green = (288.1221695283 * (pow(t - 60, -0.0755148492)));
                }
                green = clamp(green,0,255);
                
                
                //BLUE
                if (t >= 66)
                {
                    blue = 255;
                }
                else if (t <= 19)
                {
                    blue = 0;
                }
                else
                {
                    blue = (138.5177312231 * log(t - 10) - 305.0447927307);
			        blue = clamp(blue, 0, 255);
                }
                _outCol.r = red;
                _outCol.g = green;
                _outCol.b = blue;
                _outCol = _outCol / 255.0;
                _outCol = pow(_outCol, 2.2);
                return _outCol;
            }
            
            float4 frag (v2f i) : SV_Target
            {
                float4 col = tex2D(_MainTex, i.uv);
                
                
                
               return float4(lerp(col,col * KtoRGB(_temperature) ,1),1);
            }
            ENDCG
        }

        Pass //VIGNETTE 1
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            
            float _vignetteRadius, _vignetteFeather;
            
            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);
                
                float2 remapUV = (i.uv * 2) - 1;
                float circle = length(remapUV);
                float mask = 1-smoothstep(_vignetteRadius, _vignetteRadius + _vignetteFeather, circle);
                
                return fixed4(col.xyz * mask,1);
                
          
            }
            ENDCG


        }

    }
}
