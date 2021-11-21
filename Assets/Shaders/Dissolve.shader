Shader "Effects/Invisibility/Dissolve" {
	Properties{
		[HDR]
		_Emission("Emission Color", Color) = (1, 1, 1, 1)
		_MainTex("Albedo (RGB)", 2D) = "white" {}
		_BumpMap("Normal Map", 2D) = "bump" {}
		_DataMap("Data Map", 2D) = "gray" {}
		_CamoTex("Camouflage (RGB)", 2D) = "white" {}
		_CamoScale("Camouflage Scale", Range(1, 15)) = 5
		_Glossiness("Smoothness", Range(0,1)) = 0.5
		_DissolveTex("Camouflage (RGB)", 2D) = "white" {}
		_Cutoff("Alpha cutoff", Range(0,1)) = 0.5
	}
		SubShader{
			Tags { "RenderType" = "Opaque" }
			LOD 200

			CGPROGRAM
			#pragma surface surf Standard fullforwardshadows keepalpha

			#pragma target 3.0

			sampler2D _MainTex;
			sampler2D _CamoTex;
			sampler2D _BumpMap;
			sampler2D _DataMap;
			sampler2D _DissolveTex;

			half3 _Emission;
			half _CamoScale;
			half _Glossiness;
			half _Cutoff;

			struct Input {
				float2 uv_MainTex;
				float2 uv2_CamoTex;
				float4 color : COLOR;
			};

			void surf(Input IN, inout SurfaceOutputStandard o) {
				fixed4 base = tex2D(_MainTex, IN.uv_MainTex);
				fixed4 camo = tex2D(_CamoTex, IN.uv2_CamoTex * _CamoScale);
				fixed4 data = tex2D(_DataMap, IN.uv_MainTex);
				fixed dissolve = (tex2D(_DissolveTex, IN.uv2_CamoTex * 2) + IN.uv2_CamoTex.r * 0.25) * 0.8;
				fixed dissolveOutline = max(0, _Cutoff - dissolve + 0.05) * 100;
				clip(dissolve - _Cutoff);
				o.Normal = UnpackNormal(tex2D(_BumpMap, IN.uv_MainTex));
				o.Albedo = lerp(lerp(base, camo, data.g), 0.5, data.a);
				o.Metallic = data.r;
				o.Smoothness = data.b * _Glossiness;
				o.Emission = data.a * _Emission + dissolveOutline;
				o.Alpha = 1;// dissolve;
			}
			ENDCG
		}
			FallBack "Diffuse"
}
