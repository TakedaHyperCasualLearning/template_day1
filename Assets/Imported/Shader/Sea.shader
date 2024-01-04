Shader "Unlit/Sea"
{
    Properties
    {
        _DepthGradientShallow("Depth Gradient Shallow", Color) = (0.325, 0.807, 0.971, 0.725)
        _DepthGradientDeep("Depth Gradient Deep", Color) = (0.086, 0.407, 1, 0.749)
        _DepthMaxDistance("Depth Maximum Distance", Float) = 1
        _SurfaceNoise("Surface Noise", 2D) = "white" {}
        _SurfaceNoiseCutoff("Surface Noise Cutoff", Range(0, 1)) = 0.777
        _FoamDistance("Foam Distance", Float) = 0.4
        _FoamPower("Foam Power", Float) = 0.5
        _SurfaceNoiseScroll("Surface Noise Scroll Amount", Vector) = (0.03, 0.03, 0, 0)
        _SurfaceDistortion("Surface Distortion", 2D) = "white" {}
        _SurfaceDistortionAmount("Surface Distortion Amount", Range(0, 1)) = 0.27

        _FoamMaxDistance("Foam Maximum Distance", Float) = 0.4
        _FoamMinDistance("Foam Minimum Distance", Float) = 0.04
    }
    SubShader
    {
        Tags {"Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent"}
        
        Blend SrcAlpha OneMinusSrcAlpha
        Pass
        {
			CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float2 noiseUV : TEXCOORD0;
                float4 screenPosition : TEXCOORD1;
                float2 distortUV : TEXCOORD2;
                float3 viewNormal : NORMAL;
            };


            float4 _DepthGradientShallow;
            float4 _DepthGradientDeep;

            float _DepthMaxDistance;
            float4 uv : TEXCOORD0;
            sampler2D _CameraDepthTexture;
            sampler2D _SurfaceNoise;
            float4 _SurfaceNoise_ST;
            float _SurfaceNoiseCutoff;
            float _FoamDistance;
            float _FoamPower;
            float2 _SurfaceNoiseScroll;
            sampler2D _SurfaceDistortion;
            float4 _SurfaceDistortion_ST;

            float _SurfaceDistortionAmount;
            sampler2D _CameraNormalsTexture;
            float _FoamMaxDistance;
            float _FoamMinDistance;
            v2f vert (appdata v)
            {
                v2f o;

                o.vertex = UnityObjectToClipPos(v.vertex);
                o.screenPosition = ComputeScreenPos(o.vertex);
                float4 worldPos = mul (unity_ObjectToWorld, v.vertex);
                float2 uv = float2(worldPos.z, worldPos.x + worldPos.y);
                o.noiseUV = TRANSFORM_TEX(uv, _SurfaceNoise);
                o.distortUV = TRANSFORM_TEX(uv, _SurfaceDistortion);
                o.viewNormal = COMPUTE_VIEW_NORMAL;
                
                return o;
            }

            float4 frag (v2f i) : SV_Target
            {
                float existingDepth01 = tex2Dproj(_CameraDepthTexture, UNITY_PROJ_COORD(i.screenPosition)).r;
                float existingDepthLinear = LinearEyeDepth(existingDepth01);
                float depthDifference = existingDepthLinear - i.screenPosition.w;
                float waterDepthDifference01 = saturate(depthDifference / _DepthMaxDistance);
                float4 waterColor = lerp(_DepthGradientShallow, _DepthGradientDeep, waterDepthDifference01);

                float2 distortSample = tex2D(_SurfaceDistortion, i.distortUV).xy * _SurfaceDistortionAmount;
                float2 noiseUV = float2((i.noiseUV.x + _Time.y * _SurfaceNoiseScroll.x), (i.noiseUV.y + _Time.y * _SurfaceNoiseScroll.y) ) + distortSample;

                //float2 noiseUV = float2((i.noiseUV.x +  _SurfaceNoiseScroll.x) + distortSample.x, (i.noiseUV.y + _SurfaceNoiseScroll.y) + distortSample.y);

                float4 surfaceNoiseSample = tex2D(_SurfaceNoise, noiseUV);
                float3 existingNormal = tex2Dproj(_CameraNormalsTexture, UNITY_PROJ_COORD(i.screenPosition));
                float3 normalDot = saturate(dot(existingNormal, i.viewNormal));
                float foamDistance = lerp(_FoamMaxDistance, _FoamMinDistance, normalDot);
                float foamDepthDifference01 = saturate(depthDifference / foamDistance);
                float surfaceNoiseCutoff = foamDepthDifference01 * _SurfaceNoiseCutoff;
                //float noise = clamp((surfaceNoiseSample - surfaceNoiseCutoff)*_FoamPower, 0, 1) ;
                float noise = surfaceNoiseSample > surfaceNoiseCutoff ? 1 : 0;
                waterColor.a = _DepthGradientDeep.a;
                return waterColor  + float4(noise, noise, noise, 0) * _FoamPower;
            }
            ENDCG
        }
    }
}
