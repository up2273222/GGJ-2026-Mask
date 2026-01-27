Shader "Unlit/FogShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader
    {
        Cull Off
        ZTest Always
        ZWrite Off
        

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            
            #pragma multi_compile_fog
            
            #define FOG_DISTANCE

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float3 worldPos : TEXCOORD1;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float3 worldPos : TEXCOORD1;
                float3 ray : TEXCOORD2;
            };
            

            sampler2D _MainTex, _CameraDepthTexture;
            float4 _MainTex_ST;
            float3 _FrustumCorners[4];
            float _fogStart, _fogEnd;
            
            
            

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                o.worldPos = mul(unity_ObjectToWorld,v.vertex).xyz;
                o.ray = _FrustumCorners[v.uv.x + 2 * v.uv.y];
                return o;
            }

            float4 frag (v2f i) : SV_Target
            {
            
              
              float depth = SAMPLE_DEPTH_TEXTURE(_CameraDepthTexture, i.uv);
              depth = Linear01Depth(depth);
             // float viewDistance = depth * _ProjectionParams.z - _ProjectionParams.y;
              float viewDistance = length(i.ray * depth);
              float fogFactor =  1.0 -((viewDistance - _fogStart) / (_fogEnd - _fogStart));
              fogFactor = saturate(fogFactor);
              
              
              
              float3 col = tex2D(_MainTex, i.uv).xyz;
              float3 fogCol = lerp(unity_FogColor.xyz,col, fogFactor);
              return float4(fogCol,1);
            }
            ENDCG
        }
    }
}
