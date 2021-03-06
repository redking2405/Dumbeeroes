﻿Shader "Custom/ShockWave_Shader"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
		_Amplitude("Amplitude", Range(0,1)) = 0
	}
		SubShader
		{
			Tags { "RenderType" = "Opaque" }
			LOD 100

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
				};

				struct v2f
				{
					float2 uv : TEXCOORD0;
					float4 vertex : SV_POSITION;


				};

				sampler2D _MainTex;
				float4  _MainTex_ST;
				float   _Radius;
				float   _Amplitude;
				float2  _ShockwavePos;

				v2f vert(appdata v)
				{
					v2f o;
					o.vertex = v.vertex;
					o.uv = o.vertex * 0.5 + 0.5;

					float2 diff = float2(o.uv.x - _ShockwavePos.x,o.uv.y - _ShockwavePos.y);
					float dist = sqrt(diff.x*diff.x + diff.y*diff.y);
					float2 uv_displaced = float2(o.uv.x,o.uv.y);
					float wavesize = 0.2f;
					if (dist > _Radius) {
						if (dist < _Radius + wavesize) {
							float angle = (dist - _Radius) * 2 * 3.141592654 / wavesize;
							float cossin = (1 - cos(angle))*0.5;
							uv_displaced.x -= cossin * diff.x* _Amplitude / dist;
							uv_displaced.y -= cossin * diff.y* _Amplitude / dist;
						}
					}

					o.uv = uv_displaced;

					return o;
				}

				fixed4 frag(v2f i) : SV_Target
				{
					fixed4 col = tex2D(_MainTex, i.uv);
					return col;
				}
				ENDCG
			}
		}
}

//Ragdoll: Disjoint quand attrapé/lancé le corps puis si collision et raycast vers le bas OK avec la distance=>désactiver le ragdoll


/*Gestion déplacement caisse

Pile d'objets=> En cas de collision, ajouter l'objet du dessus à une liste d'objet pour celui du dessous
Chaque objet ajoute le poigt de tous les objets de sa propre liste au sien
Le poigt total d'un objet determine le nombre de personne necessaire pour le déplacer
Le poigt total est recalculer à chaque frame
Quand un objet est ajouter à une liste d'objet stocké, c'est son poigt total qui est pris en compte
OnTriggerExit pour detecter le sortie d'un objet de la liste

*/
