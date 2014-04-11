//================================================================================================================
//
// Filename: CharacterShader.shader
// Author: Sean Vieira
//
// Version: 1.0
//
// Description: A simple shader that renders only a shadow
// Ref: http://answers.unity3d.com/questions/269292/having-an-invisible-object-that-casts-shadows.html 
//			answer by manutoo
//
//======================================================================================================

Shader "Flight/CharacterShadowCaster/1.0" 
{	
	Subshader
    {
       UsePass "VertexLit/SHADOWCOLLECTOR"    
       UsePass "VertexLit/SHADOWCASTER"
    }
 
    Fallback off				
}
