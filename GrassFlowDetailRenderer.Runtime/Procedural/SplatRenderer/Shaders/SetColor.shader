Shader "Hidden/ProceduralTerrainMap/SetColor"
{
    Properties
    {
        [NoScaleOffset] _MainTex("Texture", 2D) = "white" {}
		_Color("Color", Color) = (0,0,0,0)
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
            float4 _Color;

            fixed4 frag(v2f i) : SV_Target
            {
                float4 out_color = tex2D(_MainTex, i.uv);

                out_color = _Color;

                return out_color;
            }
            ENDCG
        }
    }
}
