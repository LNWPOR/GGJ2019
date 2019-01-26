Shader "Unlit/TestUnlit"
{
    Properties
    {
        _CoreCenter("Core Center", Vector) = (0, 0, 0, 0)
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
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                uint vertexId : SV_VertexID;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
                float distToSurface : TEXCOORD1;
                float distToSurfaceNorm : TEXCOORD2;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            float4 _CoreCenter;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);

                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                
                o.distToSurface = length( v.vertex.xy );

                if( v.vertexId == 0 )
                {
                    o.distToSurface = 0;
                    o.distToSurfaceNorm =0;
                }
                else
                {
                    o.distToSurface = length(v.vertex.xy);
                    o.distToSurfaceNorm = 1;
                }

                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv);

                float val = i.distToSurface / 23;

                val = i.uv.x;

                col = float4( val, val, val, val );

                return col;
            }
            ENDCG
        }
    }
}
