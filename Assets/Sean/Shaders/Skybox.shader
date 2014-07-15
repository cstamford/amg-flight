// Author: Sean Vieira
// Notes:	Based on Vertex color unlit 5 shader, 
//			except with depth buffer writing off and
//			queuing changed to background

Shader "Flight/Skybox/1.0" {

Properties {
    _Color ("Main Color", Color) = (1,1,1,1)
    _MainTex ("Texture", 2D) = "white" {}
}

Category {
	Tags { "Queue"="Background" }	
    Lighting Off
    ZWrite Off
    Fog {Mode Off}
    
    BindChannels {
        Bind "Color", color
        Bind "Vertex", vertex
        Bind "TexCoord", texcoord
    }
   
    SubShader {
        Pass { 
        
            SetTexture [_MainTex] {
                Combine texture * primary DOUBLE
            }
            
            SetTexture [_MainTex] {
            	constantColor [_Color]
				combine previous * constant
            }
        }
    }
}
}