using System.Collections.Generic;
using cst.Waypoints;
using cst.Common;
using UnityEngine;

namespace cst.Flight
{
    public class WarpingController : SharedGroundControls
    {
        private const float LANDING_TRANSITION_RETURN_ROLL_SPEED = 180.0f;
        private const float CURVE_SMOOTH_FACTOR = 1.0f;

        private readonly List<WaypointNode> m_nodes = new List<WaypointNode>(); 
        private readonly List<Vector3>      m_interpNodesList = new List<Vector3>();

        private WaypointRenderer m_renderer;

        private float m_interpTime;
        private int   m_currentNodeIndex;
        private int   m_currentNodeInterpStep;

        public WarpingController(SeraphController controller)
            : base(controller)
        { }

        public override void start(TransitionData data)
        {
            Debug.Log(GetType().Name + " received transition data: " + data);
            m_renderer = gameObject.GetComponent<WaypointRenderer>();
        }

        public override void update()
        {
            m_position = transform.position;
            m_rotation = transform.eulerAngles;

            handleFacing();
            handleWarpingTransitionRoll();
            handleWarpingMovement();

            transform.eulerAngles = m_rotation;
            transform.position    = m_position;
        }

        public override void triggerEnter(Collider other)
        {
            Debug.Log(GetType().Name + " triggerEnter()");
        }

        public override void triggerExit(Collider other)
        {
            Debug.Log(GetType().Name + " triggerExit()");
        }

        public override void collisionEnter(Collision other)
        {
            Debug.Log(GetType().Name + " collisionEnter()");
        }

        public override void collisionExit(Collision other)
        {
            Debug.Log(GetType().Name + " collisionExit()");
        }

        public override TransitionData transitionData()
        {
            return new TransitionData { direction = Vector3.zero, velocity = 0.0f };
        }

        public void setFirstNode(WaypointNode node)
        {
            resetWarping();

            while (node != null)
            {
                m_nodes.Add(node);
                node = node.nextNode;
            }

            generateInterpolatedNodeList();

            if (m_renderer != null)
                m_renderer.render(m_interpNodesList);
        }

        private void generateInterpolatedNodeList()
        {
            foreach (WaypointNode node in m_nodes)
            {
                m_interpNodesList.Add(node.transform.position);
            }
        }

        private void resetWarping()
        {
            m_nodes.Clear();
            m_interpNodesList.Clear();
            m_currentNodeIndex      = 0;
            m_currentNodeInterpStep = 0;
            m_interpTime            = 0.0f;
        }

        // TODO: Find a way to share code nicely between here and LandingController
        private void handleWarpingTransitionRoll()
        {
            float angleStep = LANDING_TRANSITION_RETURN_ROLL_SPEED *
                Time.deltaTime;

            float angle = m_rotation.z - 180.0f;

            if (angle > 0.0f)
            {
                m_rotation.z = Helpers.wrapAngle(m_rotation.z + angleStep);

                if (m_rotation.z - 180.0f < 0.0f)
                    m_rotation.z = 0.0f;
            }
            else if (angle < 0.0f)
            {
                m_rotation.z = Helpers.wrapAngle(m_rotation.z - angleStep);

                if (m_rotation.z - 180.0f > 0.0f)
                    m_rotation.z = 0.0f;
            }
        }

        private void handleWarpingMovement()
        {
            if (m_currentNodeIndex >= m_nodes.Count - 1)
            {
                if (m_renderer != null)
                    m_renderer.stopRendering();

                state = SeraphState.FALLING;
            }
            else
            {
                m_interpTime += Time.deltaTime;
                float normalised = m_interpTime / (m_nodes[m_currentNodeIndex].transitionTime / CURVE_SMOOTH_FACTOR);

                if (normalised > 1.0f)
                {
                    m_interpTime = 0.0f;

                    if (m_currentNodeInterpStep >= CURVE_SMOOTH_FACTOR - 1)
                    {
                        ++m_currentNodeIndex;
                        m_currentNodeInterpStep = 0;
                    }
                    else
                    {
                        ++m_currentNodeInterpStep;
                    }
                }
                else
                {
                    int index = m_currentNodeIndex + m_currentNodeInterpStep;
                    m_position = Vector3.Lerp(m_interpNodesList[index], m_interpNodesList[index + 1], normalised);
                }
            }
        }
    }
}
