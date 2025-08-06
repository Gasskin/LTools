Shader "Spine/GPUSkeleton"
{
    Properties
    {
        _MainTex ("Main Texture", 2D) = "white" {}
        _AnimationTex ("Animation Texture", 2D) = "white" {}
        _FrameIndex ("Current Frame Index", Float) = 0
        _MaxX ("Max X", Float) = 1
        _MinX ("Min X", Float) = -1
        _MaxY ("Max Y", Float) = 1
        _MinY ("Min Y", Float) = -1
        _AnimationTexWidth("Animation Texture Width",Float) = 0
        _AnimationTexHeight("Animation Texture Height",Float) = 0
    }

    SubShader
    {
        Tags
        {
            "Queue"="Transparent" "RenderType"="Transparent"
        }
        Blend One OneMinusSrcAlpha
        ZWrite Off
        Cull Off

        Pass
        {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _MainTex;
            sampler2D _AnimationTex;

            float _FrameIndex;
            float _MinX, _MaxX, _MinY, _MaxY;
            float4 _MainTex_ST;
            float _AnimationTexWidth;
            float _AnimationTexHeight;

            struct VertexInput
            {
                float4 vertex : POSITION;
                uint vertexID : SV_VertexID;
                float2 uv : TEXCOORD0;
                float4 color : COLOR;
            };

            struct VertexOutput
            {
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
                float4 color : COLOR;
            };


            float UnpackBit88ToFloat(float2 packed, float minVal, float maxVal)
            {
                uint x = (uint)(packed.x * 255.0 + 0.5);
                uint y = (uint)(packed.y * 255.0 + 0.5);
                float unpack = (x << 8 | y) / 65535.0f;
                return unpack * (maxVal - minVal) + minVal;
            }

            float2 SampleVertexPos(uint vertexIndex)
            {
                float u = (vertexIndex + 0.5) / _AnimationTexWidth;
                float v = (_FrameIndex + 0.5) / _AnimationTexHeight;
                float4 color32 = tex2Dlod(_AnimationTex, float4(u, v, 0, 0));

                float2 packedX = float2(color32.r, color32.g);
                float2 packedY = float2(color32.b, color32.a);

                float x = UnpackBit88ToFloat(packedX, _MinX, _MaxX);
                float y = UnpackBit88ToFloat(packedY, _MinY, _MaxY);

                return float2(x, y);
            }

            VertexOutput vert(VertexInput input)
            {
                VertexOutput o;
                float2 pos = SampleVertexPos(input.vertexID);
                o.pos = UnityObjectToClipPos(float4(pos, 0, 1));
                o.uv = TRANSFORM_TEX(input.uv, _MainTex);
                o.color = input.color;
                return o;
            }

            fixed4 frag(VertexOutput i) : SV_Target
            {
                float4 texColor = tex2D(_MainTex, i.uv);
                return (texColor * i.color);
            }
            ENDHLSL
        }
    }
}