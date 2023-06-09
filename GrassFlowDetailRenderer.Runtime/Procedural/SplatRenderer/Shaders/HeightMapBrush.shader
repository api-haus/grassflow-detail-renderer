Shader "Hidden/ProceduralTerrainMap/HeightmapBrush"
{
    Properties
    {
        // Intermediate texture
        [NoScaleOffset] _MainTex("Texture", 2D) = "white" {}
        // Terrain HeightMap
        [NoScaleOffset] _HeightMap("HeightMap", 2D) = "red" {}
        // Target channel (R=density,G=height,etc..)
        _TargetChannel("Target Channel", Int) = 0
        // Terrain max height
        _HeightValue("Height value", Float) = 1500
        // Height rule
        _HeightRule("Height rule (min, max, rangeFrom, rangeTo)", Vector) = (100,200,0,1)
    }

    SubShader
    {
        Pass
        {
            Blend One OneMinusSrcAlpha

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            sampler2D _MainTex;
            sampler2D _HeightMap;
            float _HeightValue;
            int _TargetChannel;
            float4 _HeightRule;

            float inverse_lerp(float A, float B, float T)
            {
                return (T - A) / (B - A);
            }


            fixed4 frag(v2f i) : SV_Target
            {
                float4 out_color = tex2D(_MainTex, i.uv);

                // Read value from heightmap
                const float height_value = tex2D(_HeightMap, i.uv).r *
                    _HeightValue;

                // Apply min/max height
                const float min_height = _HeightRule.x;
                const float max_height = _HeightRule.y;
                const float height_rule_value = inverse_lerp(
                    min_height, max_height, height_value);


                // Remap to range
                const float value_min = _HeightRule.z;
                const float value_max = _HeightRule.w;
                float channel_value = lerp(
                    value_min, value_max, height_rule_value);

                if (height_rule_value < .00001)
                {
                    channel_value = 0;
                }
                if (height_rule_value > 1.0001)
                {
                    channel_value = 1;
                }

                // Set channel to color
                if (_TargetChannel == 0) // R
                    out_color.r *= channel_value;
                if (_TargetChannel == 1) // G
                    out_color.g *= channel_value;
                if (_TargetChannel == 2) // B
                    out_color.b *= channel_value;
                if (_TargetChannel == 3) // A
                    out_color.a *= channel_value;

                return out_color;
            }
            ENDCG
        }
    }
}
