Shader "Hidden/Blindness"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
		_ViewRange("Fog Range", Range(1, 40)) = 10
		_FogColor("Fog Color", Color) = (0,0,0,0)
		_Fuzziness("Fog Hardness", Range(.1, 2)) = 1
		_SkyPeak("Sky Blend", Range(.1,5)) = 1
		_SkyRemap("Horizon Blur", Range(0,1)) = 0
    }
    SubShader
    {
        // No culling or depth
        Cull Off ZWrite Off ZTest Always

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

			uniform sampler2D _CameraDepthTexture;
			uniform float _ViewRange;
			uniform float _Fuzziness;
			uniform float _SkyPeak;
			uniform float _SkyRemap;
			uniform fixed4 _FogColor;

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
				float2 depth : TEXCOORD0;
				float4 ray : TEXCOORD1;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
				float4 interpolatedRay : TEXCOORD1;

            };

            v2f vert (appdata v)
            {
				v2f o;

				// Get vertex and uv coordinates
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv.xy;


				// Get the interpolated frustum ray
				// (generated the calling script custom Blit function)
				o.interpolatedRay = v.ray;

				return o;
            }

            sampler2D _MainTex;
			float3 normalize(float3 v)
			{
				return rsqrt(dot(v, v)) * v;
			}

			float map(float value, float from1, float to1, float from2, float to2) {
				return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
			}

			fixed4 frag(v2f i) : SV_Target
			{
				// Get the color from the texture
				half4 colTex = tex2D(_MainTex, i.uv);
				half4 black = (0, 0, 0, 0);
				half4 white = (1, 1, 1, 1);
				// flat depth value with high precision nearby and bad precision far away???
				float rawDepth = DecodeFloatRG(tex2D(_CameraDepthTexture, i.uv));

				// flat depth but with higher precision far away and lower precision nearby???
				float linearDepth = Linear01Depth(rawDepth);

				// Vector from camera position to the vertex in world space
				float4 wsDir = linearDepth * i.interpolatedRay;

				// Position of the vertex in world space
				float3 wsPos = _WorldSpaceCameraPos + wsDir;

				// Distance to a given point in world space coordinates
				// (in this case the camera position, so: dist = length(wsDir))
				float dist = distance(wsPos, _WorldSpaceCameraPos);
				half t = saturate(dist / _ViewRange);
				float4 c = lerp(black, white, pow(1-t, _Fuzziness));
				c = c * colTex;
				c = lerp(_FogColor, c, pow(1 - t, _Fuzziness));
				

				if(linearDepth >= 1)
				{
					float range = pow(normalize(wsPos - _WorldSpaceCameraPos), _SkyPeak).y;
					float remap = 1 - _SkyRemap;
					float4 skyC = range * colTex;
					range = map(range, 0, 1, 0, remap);
					skyC = lerp(_FogColor, skyC, range);
					return skyC;
				}

				return c;
			}
            ENDCG
        }
    }
}
