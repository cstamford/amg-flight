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

    [SerializeField] private Vector3    m_mapOffset = Vector3.zero;
    [SerializeField] private GameObject m_inputManagerObject;
    [SerializeField] private Mesh       m_scrollMesh;
    [SerializeField] private Material   m_scrollMaterial;

    private InputManager m_inputManager;
    private MeshRenderer m_mapRenderer;
    private MapState     m_mapState = MapState.HIDDEN;
    private GameObject   m_mapObject;

	// Use this for initialization
	public void Start () 
    {
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

        m_mapObject = new GameObject("Map", typeof(MeshRenderer), typeof(MeshFilter));
        m_mapObject.layer = LayerMask.NameToLayer("Seraph Only");
	    m_mapObject.GetComponent<MeshFilter>().mesh = m_scrollMesh;
	    m_mapObject.renderer.material = m_scrollMaterial;
        m_mapObject.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);
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

        if (!m_mapObject.renderer.enabled)
            m_mapObject.renderer.enabled = true;

        Vector3 offset = m_mapOffset == Vector3.zero ? transform.forward * 0.5f : m_mapOffset;
        m_mapObject.transform.position = transform.position + offset - (Vector3.up / 5.0f);
        m_mapObject.transform.LookAt(transform);
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

        if (m_mapObject.renderer.enabled)
            m_mapObject.renderer.enabled = false;
    }
}
