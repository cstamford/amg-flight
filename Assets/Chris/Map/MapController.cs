// ==================================================================== \\
// File   : InputManager.cs                                             \\
// Author : Christopher Stamford, additions by Sean Vieira              \\
//                                                                      \\
// Handles drawing the dynamic map and reacts to user input.            \\
// ==================================================================== \\

using System;
using cst.Common;
using UnityEngine;
using Action = cst.Common.Action;

public class MapController : MonoBehaviour
{
    private enum MapState
    {
        VISIBLE,
        TRANSITION_TO_VISIBLE,
        TRANSITION_TO_HIDDEN,
        HIDDEN
    }

    [SerializeField] private RenderTexture m_mapTexture;
    [SerializeField] private Vector3       m_mapScale  = new Vector3(1.0f, 1.0f, 1.0f);
    [SerializeField] private Vector3       m_mapOffset = Vector3.zero;
    [SerializeField] private GameObject    m_inputManagerObject;

    private InputManager m_inputManager;
    private GameObject   m_mapQuad;
    private MeshRenderer m_mapRenderer;
    private MapState     m_mapState = MapState.HIDDEN;

	// Use this for initialization
	public void Start () 
    {
	    if (m_mapTexture == null)
	    {
            enabled = false;
            throw new Exception("Provided render texture was null.");
	    }

        if (m_inputManagerObject == null)
        {
            enabled = false;
            throw new Exception("No input manager added to the Seraph. Please define one in the inspector.");
        }

        m_inputManager = m_inputManagerObject.GetComponent<InputManager>();

        if (m_inputManager == null)
        {
            enabled = false;
            throw new Exception("No InputManager script detected on the provided InputManagerObject.");
        }

        m_mapQuad = new GameObject("Map", typeof(MeshRenderer), typeof(MeshFilter));
        m_mapQuad.layer = LayerMask.NameToLayer("Seraph Only");
        m_mapQuad.GetComponent<MeshFilter>().mesh = MeshFactory.buildQuad();
	    m_mapQuad.transform.localScale = m_mapScale;
	    m_mapQuad.renderer.material.mainTexture = m_mapTexture;
	    m_mapQuad.renderer.material.shader = Shader.Find("Unlit/Transparent");
    }
	
	// Update is called once per frame
    public void Update()
	{
	    switch (m_mapState)
	    {
	        case MapState.VISIBLE:
	            handleVisible();
	            break;

            case MapState.TRANSITION_TO_VISIBLE:
                handleTransitionToVisible();
	            break;

            case MapState.TRANSITION_TO_HIDDEN:
	            handleTransitiontoHidden();
	            break;

	        case MapState.HIDDEN:
	            handleHidden();
	            break;
	    }
	}

    private void handleVisible()
    {
        if (m_inputManager.actionFired(Action.SHOW_MAP))
        {
            m_mapState = MapState.TRANSITION_TO_HIDDEN;
            return;
        }

        if (!m_mapQuad.renderer.enabled)
            m_mapQuad.renderer.enabled = true;

        Vector3 offset = m_mapOffset == Vector3.zero ? transform.forward * 2.5f : m_mapOffset;
        m_mapQuad.transform.position = transform.position + offset;
        m_mapQuad.transform.LookAt(transform);

        // Flip the quad so it can be rendered correctly
        m_mapQuad.transform.Rotate(Vector3.up, 180.0f);
    }

    private void handleTransitionToVisible()
    {
        m_mapState = MapState.VISIBLE;
    }

    private void handleTransitiontoHidden()
    {
        m_mapState = MapState.HIDDEN;
    }

    private void handleHidden()
    {
        if (m_inputManager.actionFired(Action.SHOW_MAP))
        {
            m_mapState = MapState.TRANSITION_TO_VISIBLE;
            return;
        }

        if (m_mapQuad.renderer.enabled)
            m_mapQuad.renderer.enabled = false;
    }
}
