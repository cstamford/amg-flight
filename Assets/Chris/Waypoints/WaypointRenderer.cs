using System.Collections.Generic;
using UnityEngine;

namespace cst.Waypoints
{
    public class WaypointRenderer : MonoBehaviour
    {
        private const float BEAM_HEIGHT_OFFSET = 2.5f;
        [SerializeField] private Material m_beamMaterial;
        private LineRenderer m_lineRenderer;

        public void Start()
        {
            m_lineRenderer = gameObject.AddComponent<LineRenderer>();

            m_lineRenderer.SetWidth(1.5f, 1.5f);
            m_lineRenderer.SetColors(Color.grey, Color.grey);

            Color colour = m_beamMaterial.color;
            colour.a = 0.25f;
            m_beamMaterial.color = colour;
            m_lineRenderer.material = m_beamMaterial;

        }

        public void render(List<Vector3> nodes)
        {
            m_lineRenderer.SetVertexCount(nodes.Count);

            for (int i = 0; i < nodes.Count; ++i)
            {
                m_lineRenderer.SetPosition(i, new Vector3(nodes[i].x, nodes[i].y - BEAM_HEIGHT_OFFSET, nodes[i].z));
            }

            m_lineRenderer.enabled = true;
        }

        public void stopRendering()
        {
            m_lineRenderer.SetVertexCount(0);
            m_lineRenderer.enabled = false;
        }
    }
}
