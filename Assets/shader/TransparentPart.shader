Shader "Custom/Transparent/part" {
	Properties{
		_MainTex("Base (RGB)", 2D) = "white" {}
		_HeightVal("height Value", Float) = 1
		_TransparentLen("transparent length", Range(0 , 100)) = 0.7 //控制在最后距离中不再计算渐变直接透明
		_TransitionVal("transition Value", Range(1, 100)) = 1.2
	}
	SubShader{
		Tags{ "RenderType" = "Opaque" "Queue" = "Transparent" }
		LOD 200

		Pass{ ColorMask 0 }  //只渲染深度

		CGPROGRAM
		#pragma surface surf Lambert alpha vertex:Myvert  
		sampler2D _MainTex;
		float _HeightVal;
		float _TransparentLen;
		float _TransitionVal;

		struct Input {
			float2 uv_MainTex;
			float3 vertex;
		};

		void Myvert(inout appdata_full v, out Input IN){
			UNITY_INITIALIZE_OUTPUT(Input, IN);
			IN.vertex = v.vertex.xyz;  //本地坐标
		}

		void surf(Input IN, inout SurfaceOutput o) {
			half4 c = tex2D(_MainTex, IN.uv_MainTex);
			o.Albedo = c.rgb;
			
			if (IN.vertex.z > _HeightVal) {
				o.Alpha = 1;
			}else {
				o.Alpha = (1 - abs(IN.vertex.z) / _TransitionVal) - _TransparentLen;
			}	
		}
		ENDCG
	}
	FallBack "Diffuse"
}