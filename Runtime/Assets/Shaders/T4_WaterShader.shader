// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "T4_WaterShader"
{
	Properties
	{
		_Distance("Distance", Range( 0 , 5)) = 0.7162623
		_DeepWaterColor("Deep Water Color", Color) = (0,0,0,0)
		_Shallowwatercolor("Shallow water color", Color) = (0,0,0,0)
		_FoamAmount("Foam Amount", Range( 0 , 5)) = 5
		_FloamCutoff("Floam Cutoff", Range( 0 , 5)) = 1.927378
		_FoamSize("Foam Size", Float) = 0
		_FoamSpeed("Foam Speed", Float) = 0
		_WaveSize("Wave Size", Float) = 20
		_WaveSpeed("Wave Speed", Float) = 0
		_WaterShineColor("Water Shine Color", Color) = (0,0,0,0)
		_WaveHeight("Wave Height", Range( 0 , 1)) = 0
		_WaveShineSpeed1("Wave Shine Speed 1", Vector) = (0,0,0,0)
		_WaveShineSpeed2("Wave Shine Speed 2", Vector) = (0,0,0,0)
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Back
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#include "UnityCG.cginc"
		#pragma target 3.0
		#pragma surface surf Standard alpha:fade keepalpha addshadow fullforwardshadows vertex:vertexDataFunc 
		struct Input
		{
			float2 uv_texcoord;
			float4 screenPos;
		};

		uniform float _WaveSpeed;
		uniform float _WaveSize;
		uniform float _WaveHeight;
		uniform float2 _WaveShineSpeed1;
		uniform float2 _WaveShineSpeed2;
		uniform float4 _WaterShineColor;
		uniform float4 _DeepWaterColor;
		uniform float4 _Shallowwatercolor;
		UNITY_DECLARE_DEPTH_TEXTURE( _CameraDepthTexture );
		uniform float4 _CameraDepthTexture_TexelSize;
		uniform float _Distance;
		uniform float _FoamAmount;
		uniform float _FloamCutoff;
		uniform float _FoamSpeed;
		uniform float _FoamSize;


		float3 mod2D289( float3 x ) { return x - floor( x * ( 1.0 / 289.0 ) ) * 289.0; }

		float2 mod2D289( float2 x ) { return x - floor( x * ( 1.0 / 289.0 ) ) * 289.0; }

		float3 permute( float3 x ) { return mod2D289( ( ( x * 34.0 ) + 1.0 ) * x ); }

		float snoise( float2 v )
		{
			const float4 C = float4( 0.211324865405187, 0.366025403784439, -0.577350269189626, 0.024390243902439 );
			float2 i = floor( v + dot( v, C.yy ) );
			float2 x0 = v - i + dot( i, C.xx );
			float2 i1;
			i1 = ( x0.x > x0.y ) ? float2( 1.0, 0.0 ) : float2( 0.0, 1.0 );
			float4 x12 = x0.xyxy + C.xxzz;
			x12.xy -= i1;
			i = mod2D289( i );
			float3 p = permute( permute( i.y + float3( 0.0, i1.y, 1.0 ) ) + i.x + float3( 0.0, i1.x, 1.0 ) );
			float3 m = max( 0.5 - float3( dot( x0, x0 ), dot( x12.xy, x12.xy ), dot( x12.zw, x12.zw ) ), 0.0 );
			m = m * m;
			m = m * m;
			float3 x = 2.0 * frac( p * C.www ) - 1.0;
			float3 h = abs( x ) - 0.5;
			float3 ox = floor( x + 0.5 );
			float3 a0 = x - ox;
			m *= 1.79284291400159 - 0.85373472095314 * ( a0 * a0 + h * h );
			float3 g;
			g.x = a0.x * x0.x + h.x * x0.y;
			g.yz = a0.yz * x12.xz + h.yz * x12.yw;
			return 130.0 * dot( m, g );
		}


		void vertexDataFunc( inout appdata_full v, out Input o )
		{
			UNITY_INITIALIZE_OUTPUT( Input, o );
			float2 temp_cast_0 = (_WaveSpeed).xx;
			float2 temp_cast_1 = (_WaveSize).xx;
			float2 uv_TexCoord45 = v.texcoord.xy * temp_cast_1;
			float2 panner46 = ( 1.0 * _Time.y * temp_cast_0 + uv_TexCoord45);
			float simplePerlin2D47 = snoise( panner46 );
			simplePerlin2D47 = simplePerlin2D47*0.5 + 0.5;
			float3 temp_cast_2 = (( simplePerlin2D47 * _WaveHeight )).xxx;
			v.vertex.xyz += temp_cast_2;
			v.vertex.w = 1;
		}

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 panner64 = ( 1.0 * _Time.y * _WaveShineSpeed1 + float2( 0,0 ));
			float2 uv_TexCoord65 = i.uv_texcoord + panner64;
			float simplePerlin2D66 = snoise( uv_TexCoord65*2.5 );
			simplePerlin2D66 = simplePerlin2D66*0.5 + 0.5;
			float2 panner62 = ( 1.0 * _Time.y * _WaveShineSpeed2 + float2( 0,0 ));
			float2 uv_TexCoord63 = i.uv_texcoord + panner62;
			float simplePerlin2D54 = snoise( uv_TexCoord63*2.0 );
			simplePerlin2D54 = simplePerlin2D54*0.5 + 0.5;
			float4 ase_screenPos = float4( i.screenPos.xyz , i.screenPos.w + 0.00000000001 );
			float4 ase_screenPosNorm = ase_screenPos / ase_screenPos.w;
			ase_screenPosNorm.z = ( UNITY_NEAR_CLIP_VALUE >= 0 ) ? ase_screenPosNorm.z : ase_screenPosNorm.z * 0.5 + 0.5;
			float eyeDepth4 = LinearEyeDepth(SAMPLE_DEPTH_TEXTURE( _CameraDepthTexture, ase_screenPosNorm.xy ));
			float4 lerpResult15 = lerp( _DeepWaterColor , _Shallowwatercolor , saturate( ( ( eyeDepth4 - ase_screenPos.w ) / _Distance ) ));
			float eyeDepth23 = LinearEyeDepth(SAMPLE_DEPTH_TEXTURE( _CameraDepthTexture, ase_screenPosNorm.xy ));
			float2 temp_cast_0 = (_FoamSpeed).xx;
			float2 temp_cast_1 = (_FoamSize).xx;
			float2 uv_TexCoord36 = i.uv_texcoord * temp_cast_1;
			float2 panner40 = ( 1.0 * _Time.y * temp_cast_0 + uv_TexCoord36);
			float simplePerlin2D33 = snoise( panner40 );
			simplePerlin2D33 = simplePerlin2D33*0.5 + 0.5;
			o.Emission = ( ( ( simplePerlin2D66 * simplePerlin2D54 ) * _WaterShineColor ) + ( lerpResult15 + step( ( saturate( ( ( eyeDepth23 - ase_screenPos.w ) / _FoamAmount ) ) * _FloamCutoff ) , simplePerlin2D33 ) ) ).rgb;
			o.Alpha = 1;
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=18712
0;485;1954;514;3179.427;-164.5608;3.552077;True;True
Node;AmplifyShaderEditor.ScreenPosInputsNode;25;-812.8434,394.7706;Float;False;1;False;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ScreenDepthNode;23;-817.2241,311.082;Inherit;False;0;True;1;0;FLOAT4;0,0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;26;-559.1316,322.3841;Inherit;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ScreenDepthNode;4;-132.3962,29.36669;Inherit;False;0;True;1;0;FLOAT4;0,0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ScreenPosInputsNode;2;-128.0155,113.0551;Float;False;1;False;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;39;-196.7163,720.6124;Inherit;False;Property;_FoamSize;Foam Size;5;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.Vector2Node;69;-961.834,-1127.706;Inherit;False;Property;_WaveShineSpeed1;Wave Shine Speed 1;11;0;Create;True;0;0;0;False;0;False;0,0;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.Vector2Node;73;-973.8828,-886.7933;Inherit;False;Property;_WaveShineSpeed2;Wave Shine Speed 2;12;0;Create;True;0;0;0;False;0;False;0,0;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.RangedFloatNode;27;-621.3134,566.7774;Inherit;False;Property;_FoamAmount;Foam Amount;3;0;Create;True;0;0;0;False;0;False;5;0;0;5;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;6;39.53879,290.3899;Inherit;False;Property;_Distance;Distance;0;0;Create;True;0;0;0;False;0;False;0.7162623;0;0;5;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;41;62.77106,826.8502;Inherit;False;Property;_FoamSpeed;Foam Speed;6;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;36;22.48376,698.4802;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleSubtractOpNode;1;101.7208,45.99664;Inherit;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;28;-323.937,404.8495;Inherit;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PannerNode;62;-512.0403,-866.2314;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0.4,0.2;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.PannerNode;64;-496.9623,-1242.14;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0.4,-0.4;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;65;-238.4716,-1171.826;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SaturateNode;29;-77.06154,405.6217;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;63;-240.7242,-866.174;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;31;96.61981,574.8562;Inherit;False;Property;_FloamCutoff;Floam Cutoff;4;0;Create;True;0;0;0;False;0;False;1.927378;0;0;5;0;1;FLOAT;0
Node;AmplifyShaderEditor.PannerNode;40;320.3573,705.9485;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;43;225.8223,1059.6;Inherit;False;Property;_WaveSize;Wave Size;7;0;Create;True;0;0;0;False;0;False;20;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;5;379.5385,133.7898;Inherit;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;16;585.9725,-207.7459;Inherit;False;Property;_DeepWaterColor;Deep Water Color;1;0;Create;True;0;0;0;False;0;False;0,0,0,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SaturateNode;7;602.4385,139.8898;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;45;423.5782,1029.033;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;17;583.3725,-44.24588;Inherit;False;Property;_Shallowwatercolor;Shallow water color;2;0;Create;True;0;0;0;False;0;False;0,0,0,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;44;437.0891,1155.516;Inherit;False;Property;_WaveSpeed;Wave Speed;8;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.NoiseGeneratorNode;66;144.0404,-1306.063;Inherit;True;Simplex2D;True;False;2;0;FLOAT2;0,0;False;1;FLOAT;2.5;False;1;FLOAT;0
Node;AmplifyShaderEditor.NoiseGeneratorNode;54;151.2137,-1003.553;Inherit;True;Simplex2D;True;False;2;0;FLOAT2;0,0;False;1;FLOAT;2;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;30;379.317,429.6146;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.NoiseGeneratorNode;33;567.4431,514.8027;Inherit;True;Simplex2D;True;False;2;0;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;15;904.6255,63.32729;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;68;582.1613,-667.2988;Inherit;False;Property;_WaterShineColor;Water Shine Color;9;0;Create;True;0;0;0;False;0;False;0,0,0,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;67;625.9623,-1077.737;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PannerNode;46;726.7789,1063.141;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.StepOpNode;32;827.1161,388.2011;Inherit;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;74;1252.402,1011.675;Inherit;False;Property;_WaveHeight;Wave Height;10;0;Create;True;0;0;0;False;0;False;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;58;941.8815,-942.4982;Inherit;True;2;2;0;FLOAT;0;False;1;COLOR;0.7075472,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.NoiseGeneratorNode;47;973.8644,869.3317;Inherit;True;Simplex2D;True;False;2;0;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;37;1087.17,363.6821;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;56;1364.779,235.3875;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;61;1358.056,762.3716;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0.1;False;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;1594.94,408.452;Float;False;True;-1;2;ASEMaterialInspector;0;0;Standard;T4_WaterShader;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Transparent;0.5;True;True;0;False;Transparent;;Transparent;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;2;5;False;-1;10;False;-1;5;4;False;-1;1;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;False;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;26;0;23;0
WireConnection;26;1;25;4
WireConnection;36;0;39;0
WireConnection;1;0;4;0
WireConnection;1;1;2;4
WireConnection;28;0;26;0
WireConnection;28;1;27;0
WireConnection;62;2;73;0
WireConnection;64;2;69;0
WireConnection;65;1;64;0
WireConnection;29;0;28;0
WireConnection;63;1;62;0
WireConnection;40;0;36;0
WireConnection;40;2;41;0
WireConnection;5;0;1;0
WireConnection;5;1;6;0
WireConnection;7;0;5;0
WireConnection;45;0;43;0
WireConnection;66;0;65;0
WireConnection;54;0;63;0
WireConnection;30;0;29;0
WireConnection;30;1;31;0
WireConnection;33;0;40;0
WireConnection;15;0;16;0
WireConnection;15;1;17;0
WireConnection;15;2;7;0
WireConnection;67;0;66;0
WireConnection;67;1;54;0
WireConnection;46;0;45;0
WireConnection;46;2;44;0
WireConnection;32;0;30;0
WireConnection;32;1;33;0
WireConnection;58;0;67;0
WireConnection;58;1;68;0
WireConnection;47;0;46;0
WireConnection;37;0;15;0
WireConnection;37;1;32;0
WireConnection;56;0;58;0
WireConnection;56;1;37;0
WireConnection;61;0;47;0
WireConnection;61;1;74;0
WireConnection;0;2;56;0
WireConnection;0;11;61;0
ASEEND*/
//CHKSM=FFDC99B1072104C765941D1DABD9B20EFECD4F6E