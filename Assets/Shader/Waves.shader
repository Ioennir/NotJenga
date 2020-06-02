Shader "Custom/Waves" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_Glossiness ("Smoothness", Range(0,1)) = 0.5
		_Metallic ("Metallic", Range(0,1)) = 0.0
		_WaterFogColor ("Water Fog Color", Color) = (0, 0, 0, 0)
		_WaterFogDensity ("Water Fog Density", Range(0, 2)) = 0.1
		// _Amplitude ("Amplitude", Float) = 1
		// _Wavelength ("Wavelength", Float) = 10
		// _Steepness ("Steepness", Range(0, 1)) = 0.5
		// _Direction("Direction (2D)", Vector) = (1, 0, 0, 0)
		
		_WaveA ("Wave A (dir, steepness, wavelength)", Vector) = (1, 0, 0.5, 10)
		_WaveB ("Wave B", Vector) = (0,1,0.25,20)
		_WaveC ("Wave C", Vector) = (1,1,0.15,10)
		_RefractionStrength ("Refraction Strength", Range(0, 1)) = 0.25
	}
	SubShader {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
		LOD 200
        
        GrabPass { "_WaterBackground" }
        
		CGPROGRAM
        #pragma surface surf Standard vertex:vert addshadow alpha finalcolor:ResetAlpha
		#pragma target 3.0

		sampler2D _MainTex;
		sampler2D _CameraDepthTexture, _WaterBackground;
		float4 _CameraDepthTexture_TexelSize;

		 struct Input {
			float2 uv_MainTex;
			float4 screenPos;
		};

		half _Glossiness;
		half _Metallic;
		// Parameters of the wave
		float4 _WaveA, _WaveB, _WaveC;
		float3 _WaterFogColor;
        float _WaterFogDensity;

		fixed4 _Color;
		void ResetAlpha (Input IN, SurfaceOutputStandard o, inout fixed4 color) {
			color.a = 1;
		}
		
		float _RefractionStrength;
		
		float3 GerstnerWave (
			float4 wave, float3 p, inout float3 tangent, inout float3 binormal
		) {
		    float steepness = wave.z;
		    float wavelength = wave.w;
		    float k = 2 * UNITY_PI / wavelength;
			float c = sqrt(9.8 / k);
			float2 d = normalize(wave.xy);
			float f = k * (dot(d, p.xz) - c * _Time.y);
			float a = steepness / k;

			tangent += float3(
				-d.x * d.x * (steepness * sin(f)),
				d.x * (steepness * cos(f)),
				-d.x * d.y * (steepness * sin(f))
			);
			binormal += float3(
				-d.x * d.y * (steepness * sin(f)),
				d.y * (steepness * cos(f)),
				-d.y * d.y * (steepness * sin(f))
			);
			return float3(
				d.x * (a * cos(f)),
				a * sin(f),
				d.y * (a * cos(f))
			);
		}
		
		 float2 AlignWithGrabTexel (float2 uv) {
            #if UNITY_UV_STARTS_AT_TOP
                if (_CameraDepthTexture_TexelSize.y < 0) {
                    uv.y = 1 - uv.y;
                }
            #endif
        
            return
                (floor(uv * _CameraDepthTexture_TexelSize.zw) + 0.5) *
                abs(_CameraDepthTexture_TexelSize.xy);
        }
		
		float3 ColorBelowWater (float4 screenPos, float3 tangentSpaceNormal) {
	        float2 uvOffset = tangentSpaceNormal.xy * _RefractionStrength;
	        uvOffset.y *=
		        _CameraDepthTexture_TexelSize.z * _CameraDepthTexture_TexelSize.y;
	        float2 uv = AlignWithGrabTexel((screenPos.xy + uvOffset) / screenPos.w);
	        #if UNITY_UV_STARTS_AT_TOP
            if (_CameraDepthTexture_TexelSize.y < 0) {
                uv.y = 1 - uv.y;
            }
	        #endif
	        float backgroundDepth =
		        LinearEyeDepth(SAMPLE_DEPTH_TEXTURE(_CameraDepthTexture, uv));
            float surfaceDepth = UNITY_Z_0_FAR_FROM_CLIPSPACE(screenPos.z);
            float depthDifference = backgroundDepth - surfaceDepth;
            uvOffset *= saturate(depthDifference);              
            uv = AlignWithGrabTexel(screenPos.xy / screenPos.w);
            backgroundDepth =
                LinearEyeDepth(SAMPLE_DEPTH_TEXTURE(_CameraDepthTexture, uv));
            depthDifference = backgroundDepth - surfaceDepth;
        
	        float3 backgroundColor = tex2D(_WaterBackground, uv).rgb;
	        float fogFactor = exp2(-_WaterFogDensity * depthDifference);
	        return lerp(_WaterFogColor, backgroundColor, fogFactor);
        }
        
      
       

		void vert(inout appdata_full vertexData) {
			float3 gridPoint = vertexData.vertex.xyz;
			float3 tangent = float3(1, 0, 0);
			float3 binormal = float3(0, 0, 1);
			float3 p = gridPoint;
			p += GerstnerWave(_WaveA, gridPoint, tangent, binormal);
			p += GerstnerWave(_WaveB, gridPoint, tangent, binormal);
			p += GerstnerWave(_WaveC, gridPoint, tangent, binormal);
			float3 normal = normalize(cross(binormal, tangent));
			vertexData.vertex.xyz = p;
			vertexData.normal = normal;
		}

		void surf (Input IN, inout SurfaceOutputStandard o) {
			fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
			o.Albedo = c.rgb;
			o.Metallic = _Metallic;
			o.Smoothness = _Glossiness;
			o.Emission = ColorBelowWater(IN.screenPos, o.Normal) * (1 - c.a);
			o.Albedo = ColorBelowWater(IN.screenPos, o.Normal);
			o.Alpha = c.a;
		}
		ENDCG
	}
	FallBack "Diffuse"
}