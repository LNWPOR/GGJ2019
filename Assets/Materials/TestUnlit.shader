Shader "Unlit/TestUnlit"
{
    Properties
    {
        _A ("A", Float) = 100
        _B ("B", Float) = 0.02
        _C ("C", Float) = 0.3
        _minSurfaceHeight ("MinSurfaceHeight", Float) = 1
        _numSurfaceVerticies ("TotalSurfacePoint", Float) = 1
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

            #define PI 3.141592653589793238462
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
                float radius : TEXCOORD3;
                float2 position : TEXCOORD4;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _numSurfaceVerticies;
            float _minSurfaceHeight;
            float _A, _B, _C;

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
                    o.radius = 0;
                }
                else
                {
                    o.distToSurface = length(v.vertex.xy);
                    o.distToSurfaceNorm = 1;
                    o.radius = v.vertexId / _numSurfaceVerticies;
                }

                o.position = v.vertex.xy;
                return o;
            }

            float mod( float x, float y )
            {
                return x-y*floor(x/y);
            }

            float spiral2( float2 pos, float relativePercent ) {
                float r = length(pos) * relativePercent;
                float a = atan2(pos.x, pos.y);

                // float v = sin(  _A *((r) -_B * a -_C * _Time.y));
                
                // In this case period = 2Pi/_A
                // Phase shift = -_B * a -_C * Time.y

                // float PIPI = 3.141592653589793238462;
                float modStep = mod( r -_B * a -_C * _Time.y, _A );

                modStep /= _A;
                
                // v = mod( sqrt(r) -_C * _Time.y,_A * a);
                // return clamp(v,0.,1.);
                // return v;
                // return abs(v);

                return modStep;
            }


            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv);

                float val;

                float selfMaxDist = i.distToSurface / i.distToSurfaceNorm;

                float relativePercent = selfMaxDist / _minSurfaceHeight;

                //val = i.uv.y;
                // val = spiral( i.uv.y, i.uv.x );
                //val = spiral( i.uv.x, i.uv.y );

                val = spiral2( i.position, relativePercent );
                // val = spiral(i.distToSurface, i.radius);
                // val = spiral( i.distToSurfaceNorm, i.uv.y );
                // val = spiral( i.uv.x/i.uv.y, i.distToSurfaceNorm );

                // val = spiral( i.distToSurface, i.radius );
                // val = spiral( i.radius, i.distToSurface );
                // val = spiral( i.distToSurfaceNorm, i.radius );
                // val = spiral( i.radius, i.distToSurface );
                
                col = float4( val, val, val, val );

                // col = float4( i.distToSurfaceNorm, i.radius, 0, 1);
                return col;
            }
            ENDCG
        }
    }
}
