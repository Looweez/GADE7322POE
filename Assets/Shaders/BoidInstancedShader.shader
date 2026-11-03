Shader "Custom/BoidInstancedShader"
{
    Properties
    {
        _BaseColor ("Base Color", Color) = (0.2, 0.8, 1, 1)
        _HeightColor ("Height Color", Color) = (1, 0.2, 0.5, 1)
        _MinY ("Min Height Color", Float) = 0.0
        _MaxY ("Max Height Color", Float) = 15.0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        #pragma surface surf Standard addshadow fullforwardshadows
        #pragma instancing_options procedural:setup

        struct Boid
        {
            float3 position;
            float3 velocity;
        };

        #ifdef UNITY_PROCEDURAL_INSTANCING_ENABLED
            StructuredBuffer<Boid> boidsBuffer;
        #endif

        fixed4 _BaseColor;
        fixed4 _HeightColor;
        float _MinY;
        float _MaxY;

        void setup()
        {
            #ifdef UNITY_PROCEDURAL_INSTANCING_ENABLED
                Boid boid = boidsBuffer[unity_InstanceID];

                float3 position = boid.position;
                float3 forward = normalize(boid.velocity);
                if (length(forward) < 0.001) forward = float3(0, 0, 1);
                
                float3 up = float3(0, 1, 0);
                float3 right = normalize(cross(up, forward));
                up = cross(forward, right);

                // Construct transformation matrix explicitly
                unity_ObjectToWorld._11_21_31_41 = float4(right, 0.0);
                unity_ObjectToWorld._12_22_32_42 = float4(up, 0.0);
                unity_ObjectToWorld._13_23_33_43 = float4(forward, 0.0);
                unity_ObjectToWorld._14_24_34_44 = float4(position, 1.0);
            #endif
        }

        struct Input
        {
            float3 worldPos;
        };

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            // Interpolate color based on world Y height
            float heightLerp = saturate((IN.worldPos.y - _MinY) / (_MaxY - _MinY));
            fixed4 col = lerp(_BaseColor, _HeightColor, heightLerp);

            o.Albedo = col.rgb;
            o.Alpha = col.a;
        }
        ENDCG
    }
    FallBack "Diffuse"
}