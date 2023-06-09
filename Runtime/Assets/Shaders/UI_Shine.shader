Shader "Framework/UI/Shine"
{
     Properties{
         [PerRendererData] _MainTex("Sprite Texture", 2D) = "white" {}
         _BlikColor ("Blik color", Color) = (1,1,1,1)
         _Interval   ("Interval", float) = 1
         _Speed ("Speed", float) = 1
         _Size ("Size", float) = 1
         _Rotation("Rotation", Range(-1,1)) = 0.5
    
        _StencilComp ("Stencil Comparison", Float) = 8
        _Stencil ("Stencil ID", Float) = 0
        _StencilOp ("Stencil Operation", Float) = 0
        _StencilWriteMask ("Stencil Write Mask", Float) = 255
        _StencilReadMask ("Stencil Read Mask", Float) = 255

        _ColorMask ("Color Mask", Float) = 15

        [Toggle(UNITY_UI_ALPHACLIP)] _UseUIAlphaClip ("Use Alpha Clip", Float) = 0
 
     }
 
         SubShader{
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
 
         Pass{
         CGPROGRAM
 #pragma vertex vert
 #pragma fragment frag
            #pragma target 2.0
 #include "UnityCG.cginc"
    #include "UnityUI.cginc"
 
         struct appdata_t
     {
         float4 vertex   : POSITION;
         float4 color    : COLOR;
         float2 texcoord : TEXCOORD0;
     };
 
     struct v2f
     {
         float4 vertex   : SV_POSITION;
         fixed4 color : COLOR;
         half2 texcoord  : TEXCOORD0;
         float4 worldPosition : TEXCOORD1;
         float2 screenpos : TEXCOORD2;
     };
 
    fixed4 _BlikColor;
    float4 _ClipRect;
         float _Rotation;
         float _Interval;
         float _Speed;
         float _Size;
         float _UnscaledTime;
 
     v2f vert(appdata_t IN)
     {
         v2f OUT;
         OUT.vertex = UnityObjectToClipPos(IN.vertex);
         OUT.texcoord = IN.texcoord;
 #ifdef UNITY_HALF_TEXEL_OFFSET
         OUT.vertex.xy += (_ScreenParams.zw - 1.0)*float2(-1,1);
 #endif
        OUT.color = IN.color;
        OUT.worldPosition = IN.vertex;
        OUT.screenpos = OUT.vertex.xy / OUT.vertex.zw;
        return OUT;
     }
 
     sampler2D _MainTex;
 #define PI 3.14159265359
         
     fixed4 frag(v2f i) : COLOR{
         fixed4 c = tex2D(_MainTex, i.texcoord) * i.color;

         float t = _UnscaledTime * _Speed;

         float aspect = _ScreenParams.x / _ScreenParams.y;

         float p = i.screenpos.x * _Rotation + lerp(0,1/aspect,i.screenpos.y) * (1-_Rotation) * _ProjectionParams.x;

        float k = sin( (p + t)* PI /_Interval) ;

         _Size = max(0,_Size);
         
        float blikValue = -pow(k*_Interval/_Size, 4) + 1;

         blikValue = max(0,blikValue);
         blikValue = min(1,blikValue*1000);

         blikValue = lerp(0,_BlikColor.a,blikValue);
         
        c = fixed4((1-blikValue) * c.rgb + (blikValue) * _BlikColor.rgb, c.a);

        c.a *= UnityGet2DClipping(i.worldPosition.xy, _ClipRect);

        #ifdef UNITY_UI_ALPHACLIP
        clip (color.a - 0.001);
        #endif
         
         clip(c.a - 0.01);
         return c;
     }
         ENDCG
         
     }
     }
}
