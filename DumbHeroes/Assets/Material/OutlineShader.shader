Shader "Sprites/Outline"
{
	Properties
	{
		_MainTex("Sprite Texture", 2D) = "white" {}
		_Color("Colour", Color) = (1,1,1,1)
		_OutlineSize("Outline Size", int) = 1
		_Outline("Outline",float)=1
	}

		SubShader
		{
			
			Tags
			{
				"Queue" = "Transparent"
				"IgnoreProjector" = "True"
				"RenderType" = "Transparent"
				"PreviewType" = "Plane"
				"CanUseSpriteAtlas" = "True"
			}
			
			Cull Off

			//Lighting Off
			//ZWrite Off
			Blend One OneMinusSrcAlpha

			Pass
			{

			CGPROGRAM
				#pragma vertex SpriteVert
				#pragma fragment frag
			/*
				#pragma target 2.0
				#pragma multi_compile_instancing
				#pragma multi_compile _ PIXELSNAP_ON
				#pragma multi_compile _ ETC1_EXTERNAL_ALPHA
				*/
				#include "UnityCG.cginc"
			
				sampler2D _MainTex;
				
				struct v2f {
					float4 pos : SV_POSITION;
					half2 uv : TEXCOORD0;
					fixed4 color : COLOR;
				};
				

				v2f SpriteVert(appdata_full v)
				{
					v2f o;
					o.pos = UnityObjectToClipPos(v.vertex);
					o.uv = v.texcoord;
					o.color = v.color;

					return o;
				}

				fixed4 _Color;
				float4 _MainTex_TexelSize;
				int _OutlineSize;
				float _Outline;


				
				fixed4 frag(v2f IN) : COLOR
				{
				
					half4 c = tex2D(_MainTex, IN.uv);
					c*=IN.color;
					//c.rgb *= c.a;
					half4 outlineC = _Color;
					outlineC.a *= ceil(c.a);
					outlineC.rgb *= outlineC.a;
					int j= _OutlineSize;

					if (_Outline==0)
					{
						j=0;
					}
					
					float totalAlpha = 1.0;
					float nearestAlphaCount = -1;

					[unroll(64)]
					for (int i = 1; i < j + 1; i++) {
						fixed pixelUp = tex2D(_MainTex, IN.uv + fixed2(0, i*_MainTex_TexelSize.y)).a;
						fixed pixelDown = tex2D(_MainTex, IN.uv - fixed2(0, i*_MainTex_TexelSize.y)).a;
						fixed pixelRight = tex2D(_MainTex, IN.uv + fixed2(i * _MainTex_TexelSize.x, 0)).a;
						fixed pixelLeft = tex2D(_MainTex, IN.uv - fixed2(i * _MainTex_TexelSize.x, 0)).a;

						totalAlpha = totalAlpha * pixelUp * pixelDown * pixelRight * pixelLeft;

						if (totalAlpha == 0 && nearestAlphaCount == -1)
						{
							nearestAlphaCount= _OutlineSize-i;
						}
					}

					float finalAlpha= nearestAlphaCount / _OutlineSize;

					if (nearestAlphaCount == -1)
					{
						finalAlpha=1;
					}

					if (totalAlpha == 0) {
						c.rgba = lerp(  fixed4(c.r,c.g,c.b,1), _Color, finalAlpha);
					}

					c.rgb *= c.a;

					return c;
					
				}
			ENDCG
			}
		}
}
