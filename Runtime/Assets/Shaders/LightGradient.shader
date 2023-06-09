Shader "Framework/LightGradient"
{
    Properties
    {
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        [PerRendererData]_Color ("Color", Color) = (1,1,1,1)
        _SpecularColor("Specular Color", Color) = (1,1,1,1)
        _ShadowStrength("Shadow Strength", Range(0.0,1.0)) = 0.0
        _Shininess ( "Glossiness", float) = 1.0
        _Reflectiveness ("Reflectiveness", Range(0.0,1.0)) = 1.0
        _RimLight ("Rim Light Strength", Range(0.0,1.0)) = 0.0
        _RimLightSZ ("Rim Light Size", Range(0.0,1.0)) = 0.0
        _LightGradient ("Light Gradient", 2D) = "white" {}
        _BlikGradient ("Blik Gradient", 2D) = "black" {}
        [PerRendererData]_Flash("Flash", Range(0.0,1.0)) = 0.0
        [PerRendererData]_FlashColor("Flash Color", Color) = (1,1,1,1)

    }
    SubShader
    {
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
        fixed4 _FlashColor;

        
        
        struct SurfaceABP {
            half3 Albedo;
            half3 Normal;
            float Alpha;
            half3 Emission;
            float Shininess;
            float ShadowStrength;
            float Reflectiveness;
            float _Flash;
            fixed4 _FlashColor;

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
            float4 color5 = _FlashColor;
            
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
