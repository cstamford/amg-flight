// ==================================================================== \\
// File   : WaypointRenderer.cs                                         \\
// Author : Christopher Stamford                                        \\
//                                                                      \\
// Draws the waypoint path using Unity's line renderer.                 \\
// ==================================================================== \\

using System;
using System.Collections.Generic;
using cst.Flight;
using UnityEngine;

namespace cst.Waypoints
{
    public class WaypointRenderer : MonoBehaviour
    {
        private const float BEAM_HEIGHT_OFFSET = 1.0f;

        [SerializeField] private GameObject m_seraph;
        [SerializeField] private Material   m_beamMaterial;

        private SeraphController m_seraphController;
        private LineRenderer     m_lineRenderer;
        private bool             m_drawn;

        public void Start()
        {
            if (m_seraph == null)
                throw new Exception("Seraph object was null");

            m_seraphController = m_seraph.GetComponent<SeraphController>();

            m_lineRenderer = gameObject.AddComponent<LineRenderer>();
            m_lineRenderer.SetWidth(1.5f, 1.5f);
            m_lineRenderer.material = m_beamMaterial;
            m_lineRenderer.material.shader = Shader.Find("Flight/Lightbeam/1.0");
            m_lineRenderer.SetColors(Color.white, Color.white);
        }

        public void Update()
        {
            if (m_seraphController.state == SeraphState.WARPING)
            {
                if (!m_drawn)
                {
                    WarpingController controller = m_seraphController.activeController as WarpingController;

                    if (controller != null)
                        render(controller.pathList);
                }
            }
            else
            {
                stopRendering();
            }
        }

        public void render(List<Vector3> nodes)
        {
            m_lineRenderer.SetVertexCount(nodes.Count);

            for (int i = 0; i < nodes.Count; ++i)
            {
                m_lineRenderer.SetPosition(i, new Vector3(nodes[i].x, nodes[i].y - BEAM_HEIGHT_OFFSET, nodes[i].z));
            }

            m_lineRenderer.enabled = true;
            m_drawn = true;
        }

        public void stopRendering()
        {
            m_lineRenderer.SetVertexCount(0);
            m_lineRenderer.enabled = false;
            m_drawn = false;
        }
    }
}
