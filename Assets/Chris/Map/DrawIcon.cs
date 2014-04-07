// ==================================================================== \\
// File   : DrawIcon.cs                                                 \\
// Author : Christopher Stamford									    \\
//                                                                      \\
// The DrawIcon script will draw an icon that is visible only in the    \\
// Map Only layer. The icon is drawn as a textured quad using the tex   \\
// and shader provided by the user. The quad will always face directly  \\
// in the air, and will follow the location of the attached parent.     \\
// ==================================================================== \\

using cst.Common;
using UnityEngine;

namespace cst.Map
{
    public class DrawIcon : MonoBehaviour
    {
        [SerializeField] private Texture     m_texture;
        [SerializeField] private Shader      m_shader;
        [SerializeField] private float       m_width      = 5.0f;
        [SerializeField] private float       m_height     = 5.0f;
        [SerializeField] private float       m_gameHeight = 200.0f;
        private                  GameObject  m_iconQuad;
        private                  Vector3     m_rotation;

        public void Start() 
        {
            if (m_texture == null)
	        {
	            Debug.Log("Provided icon texture was null. Disabling icon script.");
	            enabled = false;
	        }

	        if (m_shader == null)
	        {
	            Debug.Log("No shader provided. Using default shader.");
	            m_shader = Shader.Find("Unlit/Transparent");
	        }

            m_iconQuad = new GameObject("Map Icon", typeof(MeshRenderer), typeof(MeshFilter));
            m_iconQuad.layer = LayerMask.NameToLayer("Map Only");
            m_iconQuad.GetComponent<MeshFilter>().mesh = MeshFactory.buildQuad();
	        m_iconQuad.transform.localScale          = new Vector3(m_width, m_height, m_iconQuad.transform.localScale.z);   
            m_iconQuad.renderer.material.mainTexture = m_texture;
	        m_iconQuad.renderer.material.shader      = m_shader;

            m_rotation = new Vector3(90.0f, 0.0f, 0.0f);
	        m_iconQuad.transform.position    = transform.position;
	        m_iconQuad.transform.eulerAngles = m_rotation;
        }

        public void Update()
        {
            m_iconQuad.transform.position = new Vector3(transform.position.x, m_gameHeight, transform.position.z);
            m_iconQuad.transform.eulerAngles = m_rotation;
	    }
    }
}
