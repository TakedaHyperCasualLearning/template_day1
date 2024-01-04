Shader "Unlit/AnimatedShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _PosTex("position texture", 2D) = "black"{}
		_NmlTex("normal texture", 2D) = "white"{}
        _Color("Color", color) = (1, 0, 0, 0)
		_Ambien("Ambien", color) = (1, 0, 0, 1)
        _DT("delta time", float) = 0
        _Length("animation length", Float) = 1
		_PositionScaling("Scale Rate", float) = 1
		_Ramp ("Toon Ramp (RGB)", 2D) = "gray" {}
		[Range(0.0, 1.0)]_LastFrameWhenNotLooped("LastFrame Rate", float) = 1
        [Toggle(ANIM_LOOP)] _Loop("loop", Float) = 0
    }
    SubShader
    {
        Pass
        {
			Tags { "RenderType"="Opaque" "LightMode"="ForwardBase"}
            CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_instancing
            #pragma multi_compile_fog
			#include "UnityCG.cginc"

			#pragma multi_compile ___ ANIM_LOOP
			#pragma target 3.0

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct v2f
			{
				float2 uv : TEXCOOR0;
				fixed4 color : TEXCOORD2;
				float4 vertex : SV_POSITION;
				
				UNITY_FOG_COORDS(3)
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			sampler2D _MainTex, _PosTex, _NmlTex;
			float4 _MainTex_ST, _PosTex_TexelSize, _NmlTex_ST;
			float4 _Color;
			float4 _Ambien;
			float _PositionScaling;
			float _LastFrameWhenNotLooped;
			sampler2D _Ramp;
			UNITY_INSTANCING_BUFFER_START(Props)
				UNITY_DEFINE_INSTANCED_PROP(float,_Length)
				UNITY_DEFINE_INSTANCED_PROP(float, _DT)
			UNITY_INSTANCING_BUFFER_END(Props)

			
			v2f vert (appdata v, uint vid : SV_VertexID)
			{
				v2f o;

				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);

				float t = (_Time.y - UNITY_ACCESS_INSTANCED_PROP(Props, _DT)) / UNITY_ACCESS_INSTANCED_PROP(Props, _Length);
#if ANIM_LOOP
				t = fmod(t, 1.0);
#else
				t = min(t, _LastFrameWhenNotLooped);
#endif			
				float x = (vid + 0.5) * _PosTex_TexelSize.x;
				float y = t;
				float4 pos = tex2Dlod(_PosTex, float4(x, y, 0, 0)) * _PositionScaling;
				float3 normal = UnityObjectToWorldNormal(tex2Dlod(_NmlTex, float4(x, y, 0, 0)));
				fixed ndl = max(0, dot(normal, _WorldSpaceLightPos0.xyz)*0.5 + 0.5);
				fixed3 ramp = tex2Dlod(_Ramp, fixed4(ndl,ndl, 0, 0));
				
				ramp = lerp(_Ambien.rgb, _Color.rgb, ramp) ;
				o.color = fixed4(ramp, 1);
				o.uv = v.uv;
				o.vertex = UnityObjectToClipPos(pos);
				
				UNITY_TRANSFER_FOG(o, o.vertex);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID(i);
				fixed4 texCol = tex2D(_MainTex, i.uv);
				fixed4 col = texCol * i.color;
				UNITY_APPLY_FOG(i.fogCoord, col);
				return col;
			}
			ENDCG
        }
		Pass
        {
            Tags {"LightMode"="ShadowCaster"}
			Fog {Mode Off}
            ZWrite On ZTest Less Cull Off
            Offset [_ShadowBias], [_ShadowBiasSlope]
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_shadowcaster
            #pragma multi_compile SHADOWS_NATIVE SHADOWS_CUBE
            #pragma fragmentoption ARB_precision_hint_fastest
			#pragma multi_compile ___ ANIM_LOOP
            #include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float3 normal : NORMAL;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};
            struct v2f {
                V2F_SHADOW_CASTER;
				UNITY_VERTEX_INPUT_INSTANCE_ID
            };

			sampler2D  _PosTex, _NmlTex;
			float4  _PosTex_TexelSize, _NmlTex_ST;
			float _PositionScaling;
			float _LastFrameWhenNotLooped;
			UNITY_INSTANCING_BUFFER_START(Props)
				UNITY_DEFINE_INSTANCED_PROP(float,_Length)
				UNITY_DEFINE_INSTANCED_PROP(float, _DT)
			UNITY_INSTANCING_BUFFER_END(Props)
 
            v2f vert(appdata v, uint vid : SV_VertexID)
            {
                v2f o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				float t = (_Time.y - UNITY_ACCESS_INSTANCED_PROP(Props, _DT)) / UNITY_ACCESS_INSTANCED_PROP(Props, _Length);
#if ANIM_LOOP
				t = fmod(t, 1.0);
#else
				t = min(t, _LastFrameWhenNotLooped);
#endif			
				float x = (vid + 0.5) * _PosTex_TexelSize.x;
				float y = t;
				float4 pos = tex2Dlod(_PosTex, float4(x, y, 0, 0)) * _PositionScaling;
				v.vertex = pos;
				
                TRANSFER_SHADOW_CASTER_NORMALOFFSET(o)
                return o;
            }
 
            float4 frag(v2f i) : SV_Target
            {
                SHADOW_CASTER_FRAGMENT(i)
            }
            ENDCG
        }
    }
}
