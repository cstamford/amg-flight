// ==================================================================== \\
// File   : WaypointNode.cs                                             \\
// Author : Christopher Stamford, additions by Sean Vieira              \\
//                                                                      \\
// This class encapsulates a set of node in a path.                     \\
//                                                                      \\
// Operating in a similar fashion to a link list, the user can iterate  \\
// through the list (in the game's case, go from one node to the other  \\
// after using a base obelisk).                                         \\
// ==================================================================== \\

using System;
using cst.Flight;
using UnityEngine;

namespace cst.Waypoints
{
    public class WaypointNode : MonoBehaviour
    {
        private enum NodeMode
        {
            START,
            INTERMEDIARY,
            END,
            INFER
        }

        private enum MovementState
        {
            INACTIVE,
            ACTIVE
        }

        // If no previous node, respond to input
        // If no next node, wrong point

        [SerializeField] private NodeMode     m_nodeMode            = NodeMode.INFER;
        [SerializeField] private GameObject   m_previousNodeObject  = null;
        [SerializeField] private GameObject   m_nextNodeObject      = null;
        [SerializeField] private float        m_transitionTime      = 1.0f;

        private WaypointNode     m_previousNode;
        private WaypointNode     m_nextNode;
        private MovementState    m_movementState;
        private SeraphController m_controller;

        private float            m_interpTimer;
        private Vector3          m_position;
        private Vector3          m_initialSeraphPos;

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

            m_interpTimer = 0.0f;
            m_movementState = MovementState.INACTIVE;
        }

        public void Update()
        {
            m_position = transform.position;

            if (m_movementState == MovementState.ACTIVE && m_nodeMode != NodeMode.END)
                interpToNext();
        }

        // Use for now
        public void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.name.Contains("Seraph"))
                onInteract(other.gameObject.GetComponent<SeraphController>());
        }

        public void onArrive(SeraphController controller)
        {            
            m_controller       = controller;
            m_initialSeraphPos = controller.transform.position;

            if (m_nodeMode == NodeMode.END)
                m_controller.state = SeraphState.FALLING;
            else
                m_movementState = MovementState.ACTIVE;
        }

        public void onInteract(SeraphController controller)
        {
            m_controller = controller;

            if (m_nodeMode == NodeMode.START)
            {
                m_controller.state = SeraphState.WARPING;
                m_nextNode.onArrive(m_controller);
                resetNode();
            }
        }

        private void interpToNext()
        {
            m_interpTimer += Time.deltaTime;

            if (m_interpTimer > m_transitionTime)
            {
                m_nextNode.onArrive(m_controller);
                resetNode();
            }
            else
            {
                m_controller.transform.position = Vector3.Lerp(m_initialSeraphPos, m_position, m_interpTimer);
            }
        }

        private void resetNode()
        {
            m_movementState    = MovementState.INACTIVE;
            m_controller       = null;
            m_interpTimer      = 0.0f;
            m_initialSeraphPos = Vector3.zero;
        }

        private void invalidNode()
        {
            enabled = false;
            throw new Exception("This node has no previous node and no next node - it is invalid.");
        }
    }
}