//
// Filename : WaypointRenderer.cs
// Author : Louis Dimmock
// Date : 9th April 2014
// 
// Version : 1.0
// Version Info :
//		Script that draws an interpolated line between waypoint nodes using the LineRenderer component
//		Based on Christopher Stamford's scripts
//

using System;
using System.Collections.Generic;
using System.Linq;
using cst.Flight;
using cst.Common;
using cst.Waypoints;
using UnityEngine;

namespace Louis.Waypoint
{
    public class WaypointRenderer : MonoBehaviour
    {
		// Define how much to smooth the interpolation
		private const int CURVE_SMOOTH_FACTOR = 6;
	
		// Define the beam height
        private const float BEAM_HEIGHT_OFFSET = 1.0f;

		// Define the beam material
        public Material m_beamMaterial;

		// Define the shader
		public Shader m_shader;

		// Define our seraph game object
        private GameObject m_seraph;
		
		// Define the line renderer to use
        private LineRenderer m_lineRenderer;
		
		// Define our node
		private WaypointNode m_currentNode;
		
		// Define a list to store the waypoints nodes
		private List<WaypointNode> m_waypointNodes = new List<WaypointNode>(); 
		
		// Define a list to store the interpolated nodes
        private List<Vector3> m_interpolatedNodes = new List<Vector3>();

        void Start()
        {
			// Find the seraph object
			m_seraph = GameObject.FindWithTag("Player");
            if (m_seraph == null)
                throw new Exception("Seraph object was null");
			
			// Get the waypoint node from the object
			m_currentNode = this.gameObject.GetComponent<WaypointNode>();
			if(m_currentNode == null)
				throw new Exception("Current node was null");

			if( m_beamMaterial == null)
				throw new Exception("Material was not set");
			if( m_shader == null)
				throw new Exception("Shader was not set");

			// Add a line renderer to the node
            m_lineRenderer = gameObject.AddComponent<LineRenderer>();

			// Define its settings
            m_lineRenderer.SetWidth(1.5f, 1.5f);
            m_lineRenderer.material = m_beamMaterial;
			m_lineRenderer.material.shader = m_shader;
			
			// Loop through nodes and add them to the list
			RetrieveNodes();
			
			// Loop through nodes and create an interpolated list
			InterpolateNodes();

			// Create the line renderer
			CreateLineRender();
        }

        void Update()
        {
			// Nothing to do here
        }

		private void CreateLineRender()
        {
			// Set the number of vertices to use
			m_lineRenderer.SetVertexCount(m_interpolatedNodes.Count);

			// Loop through the vertices and set their position
			for (int i = 0; i < m_interpolatedNodes.Count; ++i)
            {
				m_lineRenderer.SetPosition(i, new Vector3(m_interpolatedNodes[i].x, 
				                                          m_interpolatedNodes[i].y - BEAM_HEIGHT_OFFSET, 
				                                          m_interpolatedNodes[i].z));
            }

			// Enable the line renderer
            m_lineRenderer.enabled = true;
        }
		
		private void RetrieveNodes()
		{
			// Loop through the nodes and add them to the list
			while (m_currentNode != null)
            {
				// Add the node to the list
				m_waypointNodes.Add(m_currentNode);

				// Get the next node
				m_currentNode = m_currentNode.nextNode;
            }
		}
		
		private void InterpolateNodes()
		{
			m_interpolatedNodes.AddRange(Helpers.smoothCurve( m_waypointNodes.Select(node => node.transform.position).ToList(), 
										CURVE_SMOOTH_FACTOR));
		}
    }
}
