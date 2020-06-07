// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Unlit/BezierMeshDiyLight"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _thinkness ("thinkness", Float) = 0.5
        _height ("height", Float) = 4
        _P0  ("P0", Vector) = (1,0,0,0)
        _P1  ("P1", Vector) = (1,0,0,0)
        _P2  ("P2", Vector) = (1,0,0,0)
        _P3  ("P3", Vector) = (1,0,0,0)
        _helpV ("helpV", Vector) = (1,0,0,0)
         _color ("color", Color) = (0,1,0,0)
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
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float3 worldNormal : TEXCOORD1;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            float3 bezier(float t, float3 P0, float3 P1, float3 P2, float3 P3)
            {
                float a = (1 - t);
                return P0 * a * a * a + P1 * 3 * t * a * a + P2 * 3 * t * t * a + P3 * t * t * t;
            }

            float3 bezierTangent(float t,  float3 P0,  float3 P1,  float3 P2,  float3 P3)
            {
                float t2 = t * t;
                return P0 * (-3 * t2 + 6 * t - 3) + P1 * (9 * t2 - 12 * t + 3) + P2 * (-9 * t2 + 6 * t) + P3 * (3 * t2);
            }

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _height;
            float _thinkness;
            float4 _P0,_P1,_P2,_P3;
            float4 _helpV;
            float4 _color;

            v2f vert (appdata v)
            {
                float3 vertex =v.vertex;
                float t=vertex.y/_height;

                //在curve上的位置
                float3 pos_on_curve=bezier(t,_P0,_P1,_P2,_P3);
                
                // 計算3軸 
                float3 Y_axis=normalize(bezierTangent(t,_P0,_P1,_P2,_P3));
                float3 helpV=_helpV;
                float3 Z_axis =normalize(cross(helpV,Y_axis));
                float3 X_axis =cross(Y_axis,Z_axis);

                //從線變成有厚度的
                float3 world_pos=pos_on_curve+
                                    _thinkness*vertex.x*X_axis+
                                    _thinkness*vertex.z*Z_axis;
                // float3 world_pos=vertex;

                //轉換normal
                float3 n=v.normal;
                float3 normal_along_curve=n.x*X_axis+n.y*Y_axis+n.z*Z_axis;

                v2f o;
                o.vertex =  UnityObjectToClipPos(float4(world_pos, 1.0));
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.worldNormal=UnityObjectToWorldDir(normal_along_curve);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv);
                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);


                float3 light_dir=float3(0,1,0);
                float s= clamp(dot(i.worldNormal,light_dir),0,1);

                return s*_color;
            }
            ENDCG
        }
    }
}
