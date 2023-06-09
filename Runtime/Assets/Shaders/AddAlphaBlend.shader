Shader "Mobile/Particles/AddAlphaBlend_NoCurve" {
    Properties {
        _Color ("Multiplier", Color) = (1,1,1,1)
        _MainTex ("MainTex", 2D) = "white" {}
        _ColorMul ("Color Multiplier", float) = 1
    }
    SubShader {
        Tags {
            "IgnoreProjector"="True"
            "Queue"="Transparent"
            "RenderType"="Transparent"
            "ForceNoShadowCasting"="True"
        }
        Pass {
            Name "FORWARD"
            Tags {
                "Queue"="Transparent"
            }
            ZWrite Off
            Cull Back
            Blend One OneMinusSrcAlpha
            
            CGPROGRAM
            
            #pragma vertex vert
            #pragma fragment frag
            #pragma fragmentoption ARB_precision_hint_fastest
            
            #pragma target 2.0
            
            #include "UnityCG.cginc"
            
            uniform fixed4 _Color;
            uniform sampler2D _MainTex; 
            uniform float4 _MainTex_ST;
            uniform fixed _ColorMul;

            struct appdata {
                float4 vertex : POSITION;
                float2 texcoord : TEXCOORD0;
                fixed4 color : COLOR;
            };

            struct v2f {
                float4 pos : POSITION;
                float2 texcoord : TEXCOORD0;
                fixed4 color : COLOR;
            };

            v2f vert(appdata v)
            {
                v2f o;
            	UNITY_SETUP_INSTANCE_ID(v);
            	UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);

                v.color *= _Color;
                o.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);
                fixed alphaBlendFactor = 1.0f - saturate(max(max(v.color.r, v.color.g), v.color.b) * 2.0f - 1.0f); // 1 = alphablend, 0 = additive blend
                
            	v.vertex.w = 1;
            	o.pos = UnityObjectToClipPos(v.vertex);
            	
                o.color.rgb = v.color.rgb * v.color.a * lerp(_ColorMul, 2.0f, alphaBlendFactor);
                o.color.a = v.color.a * alphaBlendFactor;
            	UNITY_TRANSFER_FOG(o,o.vertex);
                return o;

            }
            
            fixed4 frag(v2f i) : COLOR
            {
                fixed4 color;
                fixed4 tex = tex2D(_MainTex, i.texcoord);
                color = tex * i.color;
                return color;

            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}