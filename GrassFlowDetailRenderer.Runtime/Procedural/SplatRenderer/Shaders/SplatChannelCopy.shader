Shader "Hidden/ProceduralTerrainMap/SplatChannelCopy"
{
    Properties
    {
        [NoScaleOffset] _MainTex("Texture", 2D) = "white" {}
        [NoScaleOffset] _SplatTex("Splat Texture", 2D) = "white" {}
        _SourceChannel("Source Channel", Int) = 0
        _TargetChannel("Target Channel", Int) = 0
        _Operation("Operation", Int) = 0
        _Range("Range", Vector) = (0,1,0,0)
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
            sampler2D _SplatTex;
            int _SourceChannel;
            int _TargetChannel;
            int _Operation;
            float2 _Range;

            fixed4 frag(v2f i) : SV_Target
            {
                float4 out_color = tex2D(_MainTex, i.uv);
                const float4 splat_color = tex2D(_SplatTex, i.uv);
                float channel_data = splat_color[_SourceChannel];

                // Remap to range
                channel_data = clamp(
                    0, 1, _Range.x + (_Range.y - _Range.x) * channel_data);

                if (_Operation == 0) // INCLUDE
                {
                    if (_TargetChannel == 0) // R
                        out_color.r *= channel_data;
                    if (_TargetChannel == 1) // G
                        out_color.g *= channel_data;
                    if (_TargetChannel == 2) // B
                        out_color.b *= channel_data;
                    if (_TargetChannel == 3) // A
                        out_color.a *= channel_data;
                }
                if (_Operation == 1) // EXCLUDE
                {
                    if (_TargetChannel == 0) // R
                        out_color.r -= channel_data;
                    if (_TargetChannel == 1) // G
                        out_color.g -= channel_data;
                    if (_TargetChannel == 2) // B
                        out_color.b -= channel_data;
                    if (_TargetChannel == 3) // A
                        out_color.a -= channel_data;
                }


                return out_color;
            }
            ENDCG
        }
    }
}
