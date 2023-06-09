Shader "Framework/UI/Jump"
{
    Properties
    {
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
        
        _JumpBy("JumpBy Y", Float) = 0
        _SpeedMul("Speed Multiplier", Float) = 1 
    
        _StencilComp ("Stencil Comparison", Float) = 8
        _Stencil ("Stencil ID", Float) = 0
        _StencilOp ("Stencil Operation", Float) = 0
        _StencilWriteMask ("Stencil Write Mask", Float) = 255
        _StencilReadMask ("Stencil Read Mask", Float) = 255

        _ColorMask ("Color Mask", Float) = 15

        [Toggle(UNITY_UI_ALPHACLIP)] _UseUIAlphaClip ("Use Alpha Clip", Float) = 0
    }

    SubShader
    {
        Tags
        {
            "Queue"="Transparent"
            "IgnoreProjector"="True"
            "RenderType"="Transparent"
            "PreviewType"="Plane"
            "CanUseSpriteAtlas"="True"
        }

        Stencil
        {
            Ref [_Stencil]
            Comp [_StencilComp]
            Pass [_StencilOp]
            ReadMask [_StencilReadMask]
            WriteMask [_StencilWriteMask]
        }

        Cull Off
        Lighting Off
        ZWrite Off
        ZTest [unity_GUIZTestMode]
        Blend SrcAlpha OneMinusSrcAlpha
        ColorMask [_ColorMask]

        Pass
        {
        CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 2.0

            #include "UnityCG.cginc"
            #include "UnityUI.cginc"

            #pragma multi_compile __ UNITY_UI_CLIP_RECT
            #pragma multi_compile __ UNITY_UI_ALPHACLIP

            struct appdata_t
            {
                float4 vertex   : POSITION;
                float4 color    : COLOR;
                float2 uv : TEXCOORD0;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct v2f
            {
                 float4 vertex   : SV_POSITION;
                 fixed4 color : COLOR;
                 float2 uv : TEXCOORD0;
                 float4 worldPosition : TEXCOORD1;
            };
            float4 _ClipRect;
            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _JumpBy;
            float _SpeedMul;
            float _UnscaledTime;
            
            v2f vert(appdata_t IN) {
                 v2f OUT;
                 OUT.vertex = UnityObjectToClipPos(IN.vertex);
                 
                 if ( _ProjectionParams.x < 0 )
                    OUT.vertex.y += -_JumpBy * abs(sin(_UnscaledTime*_SpeedMul));
                 else
                    OUT.vertex.y += _JumpBy * abs(sin(_UnscaledTime*_SpeedMul));
                 
                 OUT.uv = IN.uv;
                 
                 OUT.color =  IN.color;
                 OUT.worldPosition = IN.vertex;
                 return OUT;
            }

            fixed4 frag(v2f IN) : SV_Target {
                fixed4 color = tex2D(_MainTex, IN.uv) * IN.color;

                #ifdef UNITY_UI_CLIP_RECT
                color.a *= UnityGet2DClipping(IN.worldPosition.xy, _ClipRect);
                #endif

                #ifdef UNITY_UI_ALPHACLIP
                clip (color.a - 0.001);
                #endif

                return color;
            }
        ENDCG
        }
    }
}
