// Upgrade NOTE: replaced 'SeperateSpecular' with 'SeparateSpecular'

Shader "Trans_Diffuse_2sided" {

    Properties {
       _Color ("Main Color", Color) = (1,1,1,1)
       _SpecColor ("Spec Color", Color) = (1,1,1,0)
       _Emission ("Emmisive Color", Color) = (0,0,0,0)
       _Shininess ("Shininess", Range (0.1, 1)) = 0.7
       _MainTex ("Base (RGB) Trans. (Alpha)", 2D) = "white" { }
    }
 
    Category {
	   //ZWrite Off
       Alphatest Greater 0
       cull off
       Tags {Queue=Transparent}
       Blend SrcAlpha OneMinusSrcAlpha
       SubShader {
          Material {
             Diffuse [_Color]
             Ambient [_Color]
             Shininess [_Shininess]
             Specular [_SpecColor]
             Emission [_Emission]   
          }
          Pass {
             Lighting On
             SeparateSpecular On
             SetTexture [_MainTex] {
                constantColor [_Color]
                Combine texture * primary DOUBLE, texture * constant
             }
          }
       }
    }
 }



