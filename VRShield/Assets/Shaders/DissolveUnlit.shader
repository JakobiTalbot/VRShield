Shader "Unlit/DissolveUnlit"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
		// dissolve properties
		_DissolveTexture("Dissolve Texture", 2D) = "white" {}
		_Amount("Amount", Range(0, 1)) = 0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
			sampler2D _DissolveTexture;
            float4 _MainTex_ST;
			half _Amount;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col = fixed4(1, 0.29, 0, 1);
				// dissolve
				half dissolve_value = tex2D(_DissolveTexture, i.uv).r;
				clip(dissolve_value - _Amount);
				// dissolve outline
				if (step(dissolve_value - _Amount, 0.05f) == 1)
					col.rgb = fixed3(1, 1, 1);
                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);
                return col;
            }
            ENDCG
        }
    }
}
