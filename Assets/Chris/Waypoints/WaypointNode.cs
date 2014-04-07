// ==================================================================== \\
// File   : WaypointNode.cs                                             \\
// Author : Christopher Stamford                                        \\
//                                                                      \\
// This class encapsulates a node in a path.                            \\
// ==================================================================== \\

using System;
using cst.Flight;
using UnityEngine;

namespace cst.Waypoints
{
    public class WaypointNode : MonoBehaviour
    {
        public enum NodeMode
        {
            START,
            INTERMEDIARY,
            END,
            INFER
        }

        [SerializeField] private NodeMode   m_nodeMode           = NodeMode.INFER;
        [SerializeField] private GameObject m_previousNodeObject = null;
        [SerializeField] private GameObject m_nextNodeObject     = null;
        [SerializeField] private float      m_transitionTime     = 1.0f;

        private WaypointNode m_previousNode;
        private WaypointNode m_nextNode;

        public NodeMode mode
        {
            get { return m_nodeMode; }
        }

        public WaypointNode lastNode
        {
            get { return m_previousNode; }
        }

        public WaypointNode nextNode
        {
            get { return m_nextNode; }
        }

        public float transitionTime
        {
            get { return m_transitionTime; }
        }

        public void Start()
        {
            if (m_previousNodeObject != null)
                m_previousNode = m_previousNodeObject.GetComponent<WaypointNode>();

            if (m_nextNodeObject != null)
                m_nextNode = m_nextNodeObject.GetComponent<WaypointNode>();

            if (m_nodeMode == NodeMode.INFER)
            {
                if (m_previousNode != null && m_nextNode != null)
                    m_nodeMode = NodeMode.INTERMEDIARY;
                else if (m_previousNode != null && m_nextNode == null)
                    m_nodeMode = NodeMode.END;        
                else if (m_nextNode != null && m_previousNode == null)
                    m_nodeMode = NodeMode.START;            
                else
                    invalidNode();
            }
            else if (m_previousNode == null && m_nextNode == null)
            {
                invalidNode();
            }
        }

        public void OnTriggerEnter(Collider other)
        {
            if (!other.gameObject.name.Contains("Seraph")) 
                return;

            SeraphController controller = other.gameObject.GetComponent<SeraphController>();

            if (controller != null && m_nodeMode == NodeMode.START)
            {
                controller.state = SeraphState.WARPING;
                WarpingController warpController = controller.activeController as WarpingController;
                warpController.setFirstNode(this);
            }
        }

        private void invalidNode()
        {
            enabled = false;
            throw new Exception("This node has no previous node and no next node - it is invalid.");
        }
    }
}