Shader "Custom/PlanetShader"
{
	Properties
	{
		_MainTexture("Albedo (RGB)", 2D) = "white" {}
		_Height("Height", Range(0,1)) = 0.3
		_Seed("Seed", Range(0,10)) = 5
	}

	SubShader
	{
		Tags { "RenderType" = "Opaque" }
		LOD 200
	
		CGPROGRAM

		#pragma surface surf Lambert noforwardadd noshadow vertex:vert
		#pragma target 3.0
		
		sampler2D _MainTexture;
			
		struct Input
		{
			float2 uv_MainTexture;
			float4 color: COLOR;
		};

		float _Height;
		float _Seed;

		float hash(float2 st)
		{
			return frac(sin(dot(st.xy, float2(12.9898, 78.233))) * 43758.5453123);
		}

		float noise(float2 p, float size)
		{
			float result = 0;
			p *= size;
			float2 i = floor(p + _Seed);
			float2 f = frac(p + _Seed / 739);
			float2 e = float2(0, 1);
			float z0 = hash((i + e.xx) % size);
			float z1 = hash((i + e.yx) % size);
			float z2 = hash((i + e.xy) % size);
			float z3 = hash((i + e.yy) % size);
			float2 u = smoothstep(0, 1, f);

			result = lerp(z0, z1, u.x) + (z2 - z0) * u.y * (1.0 - u.x) + (z3 - z1) * u.x * u.y;
		
			return result;
		}

		void vert(inout appdata_full v)
		{
			float height = noise(v.texcoord, 5) * 0.75 + noise(v.texcoord, 30) * 0.125 + noise(v.texcoord, 50) * 0.125;
			v.color.a = height + _Height;
		}

		void surf(Input IN, inout SurfaceOutput o)
		{
			fixed4 color = tex2D(_MainTexture, IN.uv_MainTexture);
			float height = IN.color.a;

			o.Albedo = color.rgb * height;
			o.Alpha = 1.0;
		}
		ENDCG
	}
	FallBack "Diffuse"
}