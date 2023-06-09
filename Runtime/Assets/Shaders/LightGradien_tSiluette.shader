Shader "Framework/LightGradient_Silluete"
{
    Properties
    {
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _Color ("Color", Color) = (1,1,1,1)
        _SpecularColor("Specular Color", Color) = (1,1,1,1)
        _ShadowStrength("Shadow Strength", Range(0.0,1.0)) = 0.0
        _Shininess ( "Glossiness", float) = 1.0
        _Reflectiveness ("Reflectiveness", Range(0.0,1.0)) = 1.0
        _RimLight ("Rim Light Strength", Range(0.0,1.0)) = 0.0
        _RimLightSZ ("Rim Light Size", Range(0.0,1.0)) = 0.0
        _LightGradient ("Light Gradient", 2D) = "white" {}
        _BlikGradient ("Blik Gradient", 2D) = "black" {}
        _SiluetteColor("Siluette Color", Color) = (1,1,1,0)
        _Flash("Flash", Range(0.0,1.0)) = 0.0

    }
    SubShader
    {
         Pass
		{
			
			Name "AlwaysVisible"
			Tags{ "RenderType"="Transparent" "Queue" = "AlphaTest"}

			Cull Front
			ZWrite Off
			ZTest Always
			
			Stencil {
                Ref 0
                Comp Equal
                Pass IncrSat
				Fail IncrSat
			}

            Blend SrcAlpha OneMinusSrcAlpha
			
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"

			struct v2f
			{
				float4 vertex : SV_POSITION;
			    fixed4 color    : COLOR0;
			};

			float4 _SiluetteColor;
			
			v2f vert(appdata_full v)
			{
				v2f o;
            	o.vertex = UnityObjectToClipPos(v.vertex);
				
				float _MaxDistance = 40;
				float _MinDistance = 20;
				
				o.color = float4(_SiluetteColor.r,_SiluetteColor.g,_SiluetteColor.b,1);
				
        		half3 viewDirW = _WorldSpaceCameraPos - mul((half4x4)unity_ObjectToWorld, v.vertex);
        		half viewDist = length(viewDirW);
        		half falloff = saturate((viewDist - _MinDistance) / (_MaxDistance - _MinDistance));
        		o.color.a *= ((1.0f - falloff) * _SiluetteColor.a);
				
				return o;
			};
			
			fixed4 frag(v2f i) : SV_Target
			{
				return i.color;
			};
			
			ENDCG
		}
        
         Tags { "RenderType"="Opaque"  }
        LOD 250

        CGPROGRAM
        
        
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf AddictumBlinnPhong addshadow exclude_path:prepass nolightmap noforwardadd halfasview interpolateview
        

        fixed4 _Color;
        float _ShadowStrength;
        float _Shininess;
        float _RimLight;
        float _RimLightSZ;
        fixed4 _SpecularColor;
        float _Reflectiveness;
        sampler2D _LightGradient;
        sampler2D _BlikGradient;
        float _Flash;

        
        struct SurfaceABP {
            half3 Albedo;
            half3 Normal;
            float Alpha;
            half3 Emission;
            float Shininess;
            float ShadowStrength;
            float Reflectiveness;
            float _Flash;

        };
        
        float circlestep_out(float a, float b, float v) { 
            v--;
            return b * sqrt(1-v*v)+a;
        }
        inline fixed4 LightingAddictumBlinnPhong(SurfaceABP s, fixed3 lightDir, fixed3 viewDir, fixed atten) {

            //atten = saturate(atten);
            atten = saturate(atten);

            float NdotL = dot(s.Normal, lightDir);

            float str = saturate(NdotL);

            str *= atten;


            viewDir = normalize(viewDir);

            float lightLevel = max(1 - _ShadowStrength, str);
            float lightStr = tex2D(_LightGradient, clamp(str, 0.01, 0.99));
            fixed4 lightMultiply = tex2D(_LightGradient, clamp(str, 0.01, 0.99));

            lightStr = lerp(0, lightStr, 1 - s.ShadowStrength);
            lightMultiply = lerp(lightMultiply, fixed4(1, 1, 1, 1), 1 - s.ShadowStrength);

            float attenuation = 1.0;
            float3 ambientLighting = s.Albedo.rgb;
            float3 diffuseReflection = attenuation * _LightColor0.rgb * smoothstep(_ShadowStrength, 1, str);



            diffuseReflection = s.Albedo * lightMultiply * attenuation;


            float3 specularReflection;
            {


                float reflectStr = dot(reflect(-lightDir, s.Normal), viewDir);
                reflectStr = (reflectStr + 1) / 2;

                float v = pow(reflectStr, s.Shininess) * str;

                v = smoothstep(0, 1, v);

                v = clamp(v, 0.01, 1);

                fixed4 csr = tex2D(_BlikGradient, v);

                v = csr.rgba;

                specularReflection = attenuation * _LightColor0.rgb * _SpecularColor.rgb * v;
            }

            float rimlight = 1 - saturate(dot(viewDir, s.Normal));
            rimlight = rimlight * _RimLight;
            rimlight = smoothstep(0.0, 1 - _RimLightSZ, rimlight);

            fixed4 c;
            c.rgb = (diffuseReflection + lerp(half3(0, 0, 0), specularReflection, _Reflectiveness)) + rimlight * s.Albedo;
            c.a = s.Alpha;

            UNITY_OPAQUE_ALPHA(c.a);
            return c;
        }

        sampler2D _MainTex;

        struct Input {
            float2 uv_MainTex;
            fixed3 normal;
        };
            

        void surf (Input IN, inout SurfaceABP o)
        {
            // Albedo comes from a texture tinted by color
            //Flash color
            float4 color5 = float4(1, 1, 1, 1);
            fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
            o.Albedo =  c.rgb;
            o.Alpha = c.a;
            
            o.Normal = UnpackNormal (fixed4(0.5,0.5,1,1));
            o.Shininess = _Shininess;
            o.ShadowStrength = _ShadowStrength;
            o.Reflectiveness = _Reflectiveness;

            //Flash 
            float4 lerpResult6 = lerp(float4(0, 0, 0, 0), color5, _Flash);
            o.Emission = lerpResult6;
        }
        ENDCG
    }
    FallBack "CustomLight/PhongSimplified"
    //FallBack "Unlit/Texture"
}
