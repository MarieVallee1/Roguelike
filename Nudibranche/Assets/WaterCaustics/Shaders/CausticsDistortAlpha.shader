Shader "Projector/CausticsDistortAlpha" {
	Properties {
		[Header(Color)]
		_Color ("Color (Opacity)", Color) = (1,1,1,0)
		_Multiply ("Multiply", Range (1, 10)) = 1.5
		_Diffraction ("Diffraction", Range (0.0, 1.0)) = 0.1
		[Header(Textures)]
		_Blend ("Dual texture blend", Range (0, 1)) = 0.9
		_MainTex ("Caustic Texture", 2D) = "black" { }
		_NoiseTex ("Distortion Texture", 2D) = "black" { }
		_AlphaTex ("Alpha/Cut-out", 2D) = "white" {}
		[Header(Height)]
		_Height ("Water Height", Float) = 1.0
		_EdgeBlend ("Edge Blend", Range (0.1, 100)) = 1.0
		_DepthBlend ("Depth Blend", Float) = 10.0
		_DepthFade ("Depth Fade", Float) = 10.0
		[Header(Movement)]
		_CausticSpeed("Caustic Speed", Float) = 5.0
		_DistortionSpeed ("Distortion Speed", Float) = 1.0
		[Header(Distance)]
		_LOD ("LOD Distance", Range (1, 100)) = 25.0
		_Distance ("Distance", Float) = 100.0
		_DistanceBlend ("Distance Blend", Range (0.1, 500)) = 200.0
	 }
	 
	 Subshader {
		Tags { "RenderType"="Transparent" "Queue"="Transparent+100" }
		Pass {
			ZWrite Off
			Offset -1, -1
			//Blend OneMinusDstColor One //- Soft Additive
			//Blend One One //- Linear Dodge
			Blend DstColor One

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_fog
			#include "UnityCG.cginc"
		 
			struct v2f {
				float4 pos : SV_POSITION;
				float2 uv : TEXCOORD0;
				float2 uvNoise : TEXCOORD1;
				float2 uvAlpha : TEXCOORD2;
				float3 wPos : TEXCOORD3; // added for height/distance comparisons.
				float3 worldNormal : TEXCOORD4;
				UNITY_FOG_COORDS(5)
			};

			sampler2D _MainTex;
			sampler2D _NoiseTex;
			sampler2D _AlphaTex;
			float4 _MainTex_ST;
			float4 _NoiseTex_ST;
			float4 _AlphaTex_ST;
			float4 _Color;
			float _Diffraction;
			float4x4 unity_Projector;
			float _Distortion;
			float _Height;
			float _Blend;
			float _DepthBlend;
			float _DepthFade;
			float _EdgeBlend;
			float _Multiply;
			float _LOD;
			float _Distance;
			float _DistanceBlend;
			float _CausticSpeed;
			float _DistortionSpeed;

			v2f vert (appdata_tan v) {
				v2f o;
				o.pos = UnityObjectToClipPos (v.vertex);
				o.wPos = mul(unity_ObjectToWorld, v.vertex).xyz;
				o.uv = TRANSFORM_TEX (mul (unity_Projector, v.vertex).xy, _MainTex);
				o.uvNoise = TRANSFORM_TEX (mul (unity_Projector, v.vertex).xy, _NoiseTex);
				o.uvAlpha = TRANSFORM_TEX (mul (unity_Projector, v.vertex).xy, _AlphaTex);
				o.worldNormal = normalize(mul(float4(v.normal, 0.0), unity_ObjectToWorld).xyz);
				UNITY_TRANSFER_FOG(o,o.pos);
				return o;
			}
			fixed4 triplanarUV(float3 blendNormal, float3 wpos)
			{					
				float2 finaluv = wpos.xy;
				float2 x = wpos.yz;
				float2 y = wpos.xz;
				finaluv = lerp(finaluv, x, blendNormal.x);
				finaluv = lerp(finaluv, y, blendNormal.y);
				finaluv *= 0.005; // added to preserve scaling values when upgrading from previous version.
				return half4(finaluv.x,finaluv.y,0,0);
			}
			fixed4 frag (v2f i) : COLOR {
				float3 blending = abs(i.worldNormal);
				float3 blendNormal = normalize(max(blending, 0.00001));
				float b = (blendNormal.x + blendNormal.y + blendNormal.z);
				blendNormal/=blendNormal*2;
				
				float dist = distance(_WorldSpaceCameraPos, i.wPos);
				fixed4 noise = tex2D(_NoiseTex, i.uvNoise-frac(_Time.y*_DistortionSpeed/100));
				noise = noise + (_Time.y*_CausticSpeed/100);
				float distLOD = dist/_LOD;
				float height = i.wPos.y-_Height;
				float heightBlend = height/-_DepthBlend;
				
				float2 uvTotal = triplanarUV(blendNormal,i.wPos) * _MainTex_ST;
				
				fixed cAr = tex2Dlod (_MainTex, float4(uvTotal + frac(fixed2(_Diffraction, _Diffraction)+noise),0,heightBlend*100+distLOD));
				fixed cAg = tex2Dlod (_MainTex, float4(uvTotal + frac(fixed2(_Diffraction, -_Diffraction)+noise),0,heightBlend*100+distLOD));
				fixed cAb = tex2Dlod (_MainTex, float4(uvTotal + frac(fixed2(-_Diffraction, -_Diffraction)+noise),0,heightBlend*100+distLOD));
				fixed4 cA = fixed4(cAr, cAg, cAb,1);
				fixed cBr = tex2Dlod (_MainTex, float4(uvTotal - frac(fixed2(_Diffraction, _Diffraction)+noise),0,heightBlend*100+distLOD));
				fixed cBg = tex2Dlod (_MainTex, float4(uvTotal - frac(fixed2(_Diffraction, -_Diffraction)+noise),0,heightBlend*100+distLOD));
				fixed cBb = tex2Dlod (_MainTex, float4(uvTotal - frac(fixed2(-_Diffraction, -_Diffraction)+noise),0,heightBlend*100+distLOD));
				fixed4 cB = fixed4(cBr, cBg, cBb,1);
				float4 cAlpha = tex2D (_AlphaTex, i.uvAlpha);
				cA = lerp(cA,cB,_Blend*0.5);
				
				if (i.wPos.y<=_Height) 
					cA = clamp(lerp(cA,fixed4(0,0,0,0),heightBlend*_DepthFade),0,1);
				else 
					cA = lerp(cA,fixed4(0,0,0,0),height/_EdgeBlend);

				float distDiff = (dist-_Distance)/_DistanceBlend;
				if (dist>_Distance)
					cA = lerp(saturate(cA-distDiff),0,distDiff);
				cA = saturate(cA * _Color * _Multiply * cAlpha);
				UNITY_APPLY_FOG_COLOR(i.fogCoord, cA, fixed4(0,0,0,0));				
				return cA;
			}
			ENDCG
		}
	}
}