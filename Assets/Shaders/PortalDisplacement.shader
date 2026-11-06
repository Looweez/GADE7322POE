Shader "Custom/PortalUnlitDisplacement"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Color ("Base Color", Color) = (0.5, 0, 1, 1)
        _RimColor ("Glow Color", Color) = (0, 1, 1, 1)
        _Frequency ("Wave Frequency", Float) = 4.0
        _Amplitude ("Wave Amplitude", Float) = 0.2
        _Speed ("Wave Speed", Float) = 3.0
    }

    SubShader
    {
        Tags { "RenderType"="Opaque" "Queue"="Geometry" }
        LOD 100
        Cull Off //visible from both sides

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
                float rim : TEXCOORD1;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            fixed4 _Color;
            fixed4 _RimColor;
            float _Frequency;
            float _Amplitude;
            float _Speed;

            v2f vert (appdata v)
            {
                v2f o;

                //changes vertices to world psace
                float4 worldPos = mul(unity_ObjectToWorld, v.vertex);
                
                //calculates wave displacement
                float wave = sin((_Time.y * _Speed) + (worldPos.x * _Frequency) + (worldPos.z * _Frequency));
                
                //applies the displacemet then converts it back
                float3 worldDisplacement = v.normal * (wave * _Amplitude);
                v.vertex.xyz += mul((float3x3)unity_WorldToObject, worldDisplacement);

                
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);

                //calculates rim lighting in the world
                float3 worldNormal = UnityObjectToWorldNormal(v.normal);
                float3 viewDir = normalize(_WorldSpaceCameraPos - worldPos.xyz);
                o.rim = 1.0 - saturate(dot(worldNormal, float3(0, 1, 0)));

                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 tex = tex2D(_MainTex, i.uv);
                fixed4 col = _Color * tex;

                //adds a glow
                float3 emission = _RimColor.rgb * i.rim * 2.0;
                col.rgb += emission;

                return col;
            }
            ENDCG
        }
    }
    FallBack "Unlit/Texture"
}