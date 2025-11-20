Shader "UI/GrayscaleMaskedChild"
{
    Properties
    {
        _MainTex("Texture", 2D) = "white" {}
        _Color("Tint", Color) = (1,1,1,1)
        
        [IntRange] _StencilComp ("Stencil Comparison", Range(0, 8)) = 8
        [Int] _Stencil ("Stencil ID", Int) = 0
        [Int] _StencilOp ("Stencil Operation", Int) = 0
        [Int] _StencilWriteMask ("Stencil Write Mask", Int) = 255
        [Int] _StencilReadMask ("Stencil Read Mask", Int) = 255
        [Color] _ColorMask ("Color Mask", Float) = 15 
    }

    SubShader
    {
        Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" }
        
        Stencil
        {
            Ref [_Stencil]
            Comp [_StencilComp]
            Pass Keep           
            Fail Keep
            ZFail Keep
            ReadMask [_StencilReadMask]
            WriteMask [_StencilWriteMask]
        }
        
        ColorMask [_ColorMask]
        
        
        Blend SrcAlpha OneMinusSrcAlpha
        Cull Off
        ZWrite Off
        Lighting Off

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata_t
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            sampler2D _MainTex;
            float4 _Color;

            v2f vert(appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv) * _Color;
                
                float gray = dot(col.rgb, float3(0.299, 0.587, 0.114));
                col.rgb = fixed3(gray, gray, gray);
                
                return col; 
            }
            ENDCG
        }
    }
}