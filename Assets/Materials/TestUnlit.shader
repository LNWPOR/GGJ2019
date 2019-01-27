Shader "Unlit/TestUnlit"
{
    Properties
    {
        _WaveLength ("WaveLength", Float) = 4.57
        _PhaseOffset ("PhaseOffset", Float) = 3.64
        _Speed ("Speed", Float) = 0.3

        _BitShiftRadius ("BitShiftRadius", Range(0.0, 90.0)) = 10
        _BitShiftHeight ("BitShiftHeight", Range(0.0, 1.0)) = 0.1
        _minSurfaceHeight ("MinSurfaceHeight", Float) = 1
        _numSurfaceVerticies ("TotalSurfacePoint", Float) = 1
        _GroundHeight ("GroundHeight", Float) = 10
        _MainTex ("Texture", 2D) = "white" {}

        _TopGroundColor ("TopGroundColor", Color) = (1, 0, 0, 1)
        _BottomGroundColor ("BottomGroundColor", Color) = (0.2, 0.2, 0.2, 1)

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
            #define PI2 6.28318530717958647692
            #define RAD2DEG 57.29577951308232087680
            #define DEG2RAD 0.017453292519943295769

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
            float _WaveLength, _PhaseOffset, _Speed;

            float _BitShiftRadius;
            float _BitShiftHeight;

            float _GroundHeight;
            float4 _TopGroundColor;
            float4 _BottomGroundColor;

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
                
                float a = atan2(pos.x, pos.y);

                float r = length(pos);

                // float shiftedHeight = _WaveLength / 36.0;

                float BitShiftRadiusDeg = _BitShiftRadius * DEG2RAD;
                float toggleSwitch = sign(mod(a, BitShiftRadiusDeg*2) - BitShiftRadiusDeg);
                
                // r += shiftedHeight * toggleSwitch;
                r += _BitShiftHeight * toggleSwitch;
                r /= relativePercent;
                
                // float v = sin(  _WaveLength *((r) -_PhaseOffset * a -_Speed * _Time.y));
                
                // In this case period = 2Pi/_WaveLength
                // Phase shift = -_PhaseOffset * a -_Speed * Time.y

                float modStep = mod( r -_PhaseOffset * a -_Speed * _Time.y, _WaveLength );
                
                modStep /= _WaveLength;

                // v = mod( sqrt(r) -_Speed * _Time.y,_WaveLength * a);
                // return clamp(v,0.,1.);
                // return v;
                // return abs(v);

                return modStep;
            }
            
            float4 rgb2float4( float r, float g, float b )
            {
                return float4(r/255.0, g/255.0, b/255.0, 1);
            }

            float4 nyanCatColorStep( float val )
            {
                // Val in range 0, 1
                if(val <= 1/6.0)
                    return rgb2float4(103,52,255);
                else if(val <= 2/6.0)
                    return rgb2float4(0,153,255);
                else if(val<=3/6.0)
                    return rgb2float4(51,255,0);
                else if(val<=4/6.0)
                    return rgb2float4(255,255,0);
                else if(val<=5/6.0)
                    return rgb2float4(255,153,0);
                else
                    return rgb2float4(255,0,0);

            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col;

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
                
                // col = float4( val, val, val, val );

                float distGround = i.distToSurface - (selfMaxDist - _GroundHeight);
                if(distGround<0)
                    col = nyanCatColorStep(val);
                else
                {
                    //  Turn distGround into percentage
                    distGround = distGround / _GroundHeight;

                    if(distGround < 0.8)
                        col = _BottomGroundColor;
                    else
                        col = _TopGroundColor;
                    
                    // col = lerp( _BottomGroundColor, _TopGroundColor, distGround );
                    // col = float4( 0.8, 0.2, 0.2, 1);
                }

                // col = float4( i.distToSurfaceNorm, i.radius, 0, 1);
                return col;
            }
            ENDCG
        }
    }
}
